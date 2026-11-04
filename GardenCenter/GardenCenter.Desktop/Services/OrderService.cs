using System;
using System.Linq;
using System.Threading.Tasks;
using GardenCenter.Data;
using GardenCenter.Models;

namespace GardenCenter.Services;

public static class OrderService
{
    public static async Task CreateOrderAsync(int productId, int qty)
    {
        // Проверяем, выбран ли пользователь
        if (Session.CurrentUser == null)
            throw new Exception("Сначала выберите пользователя в списке!");

        using var db = Db.Create();

        // Находим товар по ID
        var product = await db.Products.FindAsync(productId);
        if (product == null) throw new Exception("Товар не найден.");

        // Находим первый вариант товара (для привязки заказа)
        var variant = db.ProductVariants.FirstOrDefault(v => v.ProductId == productId);

        // Создаем сам заказ
        var order = new Order
        {
            OrderDate = DateOnly.FromDateTime(DateTime.Today),
            ClientId = Session.CurrentUser.UserId
        };

        // Добавляем позицию заказа (сам товар и его цена)
        order.OrderItems.Add(new OrderItem
        {
            VariantId = variant?.VariantId ?? 1,
            Quantity = qty,
            UnitPrice = product.BasePrice
        });

        // Сохраняем в базу данных
        db.Orders.Add(order);
        await db.SaveChangesAsync();
    }
}