using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using GardenCenter.Data;
using GardenCenter.Models;
using GardenCenter.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GardenCenter;

public partial class MainWindow : Window
{
    private List<Product> _allProducts = new();

    public MainWindow()
    {
        InitializeComponent();

        // Загружаем данные при старте
        Loaded += async (s, e) => await LoadDataAsync();

        // Обработка выбора пользователя
        UserBox.SelectionChanged += (s, e) =>
        {
            Session.CurrentUser = UserBox.SelectedItem as User;
        };
    }

    // Метод для загрузки товаров и картинок
    private async Task LoadDataAsync()
    {
        using var db = Db.Create();

        // Загружаем пользователей (только если список пуст)
        if (UserBox.ItemsSource == null)
            UserBox.ItemsSource = await db.Users.ToListAsync();

        // Загружаем товары
        _allProducts = await CatalogService.GetProductsAsync();

        // Загружаем картинки для каждого товара
        string baseDir = AppDomain.CurrentDomain.BaseDirectory;
        foreach (var p in _allProducts)
        {
            string imgName = string.IsNullOrWhiteSpace(p.ImagePath) ? "picture.png" : p.ImagePath;
            string fullPath = Path.Combine(baseDir, "Assets", imgName);
            if (File.Exists(fullPath))
            {
                p.FullImagePath = new Bitmap(fullPath);
            }
        }

        // Показываем на экране
        ProductsList.ItemsSource = _allProducts;
    }

    // Метод для поиска
    private void SearchChanged(object? sender, TextChangedEventArgs e)
    {
        if (SearchBox == null || _allProducts == null) return;

        string searchText = SearchBox.Text?.ToLower() ?? "";

        if (string.IsNullOrWhiteSpace(searchText))
        {
            ProductsList.ItemsSource = _allProducts;
        }
        else
        {
            var filtered = _allProducts.Where(p =>
                p.ProductName != null && p.ProductName.ToLower().Contains(searchText)).ToList();

            ProductsList.ItemsSource = filtered;
        }
    }

    // Обработчик кнопки "Купить"
    private async void BuyClick(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.CommandParameter is int productId)
        {
            try
            {
                // Вызываем сервис оформления заказа
                await OrderService.CreateOrderAsync(productId, 1);

                // Обновляем данные
                await LoadDataAsync();
                ProductsList.SelectedItem = null;

                // ПОКАЗЫВАЕМ ОКНО УСПЕХА
                var window = new Window
                {
                    Title = "Успех",
                    Width = 300,
                    Height = 150,
                    Content = new TextBlock { Text = "Заказ успешно оформлен!", Margin = new Avalonia.Thickness(20) }
                };
                await window.ShowDialog(this);
            }
            catch (Exception ex)
            {
                // ПОКАЗЫВАЕМ ОКНО С ОШИБКОЙ
                var window = new Window
                {
                    Title = "Ошибка",
                    Width = 400,
                    Height = 150,
                    Content = new TextBlock { Text = ex.Message, Margin = new Avalonia.Thickness(20) }
                };
                await window.ShowDialog(this);
            }
        }
        else
        {
            // Если команда не передала ID
            var window = new Window
            {
                Title = "Ошибка",
                Width = 300,
                Height = 150,
                Content = new TextBlock { Text = "Не удалось получить ID товара.", Margin = new Avalonia.Thickness(20) }
            };
            await window.ShowDialog(this);
        }
    }
}