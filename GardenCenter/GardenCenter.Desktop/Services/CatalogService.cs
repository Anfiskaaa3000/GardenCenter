using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using GardenCenter.Data;
using GardenCenter.Models;

namespace GardenCenter.Services;

public static class CatalogService
{
    public static async Task<List<Product>> GetProductsAsync()
    {
        // Создаем подключение к БД
        using var db = Db.Create();

        // Загружаем товары вместе с категориями
        return await db.Products.Include(p => p.Category).ToListAsync();
    }
}