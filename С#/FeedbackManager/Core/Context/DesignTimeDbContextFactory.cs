using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Core.Context
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataContext>
    {
        //Без нього, EF Core не зможе знайти конструктор DataContext з IConfiguration
        //Внутрішнє використання лише під час розробки для використання засекреченого ключа шифрування,
        //застосунок не використовує цей клас
        public DataContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddUserSecrets<DataContext>() // Підвантажуємо секрети
                .Build();

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString); // для SQL Server

            return new DataContext(optionsBuilder.Options, configuration);
        }
    }
}
