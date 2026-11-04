using System.IO;
using Microsoft.EntityFrameworkCore;

namespace GardenCenter.Data;

public static class Db
{
    public static GardenCenterContext Create()
    {
        // Читаем строку подключения из файла connection.txt
        string path = Path.Combine(System.AppContext.BaseDirectory, "connection.txt");
        string connectionString = File.ReadAllText(path).Trim();

        var optionsBuilder = new DbContextOptionsBuilder<GardenCenterContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new GardenCenterContext(optionsBuilder.Options);
    }
}
