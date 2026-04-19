using Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Core.Context
{
    public class DataContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public DataContext(DbContextOptions<DataContext> options,
            IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }
        public DbSet<Feedback> Feedbacks => Set<Feedback>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<FeedbackCategory> FeedbackCategories => Set<FeedbackCategory>();
        public DbSet<User> Users => Set<User>();
        public DbSet<Log> Logs => Set<Log>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<FeedbackCategory>().HasKey(x => new { x.FeedbackId, x.CategoryId });

            // Configure encryption 
            var key = _configuration["DbEncryptionKey"];
            if (string.IsNullOrEmpty(key) || key.Length != 32)
                throw new Exception("The encryption key must be exactly 32 characters long!");

            var encryptionService = new EncryptionService(key);

            // Default every user creation with encrypted password
            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(u => u.Password)
                      .HasConversion(
                          p => encryptionService.Encrypt(p),
                          p => encryptionService.Decrypt(p)
                      );
            });

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Role = Role.Admin,
                    FullName = "Default Admin",
                    PhoneNumber = "000-000-0000",
                    Login = "admin",
                    Password = "admin",
                    CreatedAt = new DateTime(2026, 1, 29, 16, 13, 57, 796).AddTicks(7725),
                }
            );
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 2,
                    Role = Role.Analyst,
                    FullName = "Тетяна Шевченко",
                    PhoneNumber = "+380679876543",
                    Login = "tetyana_shevchenko",
                    Password = "8b12f5a3",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 01, 15, 10, 30, 0)
                },
                new User
                {
                    Id = 3,
                    Role = Role.Boss,
                    FullName = "Андрій Бондаренко",
                    PhoneNumber = "+380931112233",
                    Login = "andriy_bondarenko",
                    Password = "a4d9c2b1",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 01, 05, 14, 00, 0)
                },
                new User
                {
                    Id = 4,
                    Role = Role.Analyst,
                    FullName = "Марина Лисенко",
                    PhoneNumber = "+380665554433",
                    Login = "maryna_lysenko",
                    Password = "7e2c1b4d",
                    IsActive = true,
                    CreatedAt = new DateTime(2026, 01, 20, 09, 15, 0)
                },
                new User
                {
                    Id = 5,
                    Role = Role.Boss,
                    FullName = "Сергій Мельник",
                    PhoneNumber = "+380970001122",
                    Login = "serhiy_melnyk",
                    Password = "c9a8b7d6",
                    IsActive = false,
                    CreatedAt = new DateTime(2025, 12, 28, 18, 45, 0)
                }
            );
        }
    }
}
