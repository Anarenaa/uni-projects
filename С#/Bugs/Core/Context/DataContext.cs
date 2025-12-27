using Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Core.Context
{
    public class DataContext : DbContext
    {
        public DbSet<Record> Records => Set<Record>();
        public DbSet<Tag> Tags => Set<Tag>();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=local_database.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<RecordTag>().HasKey(x => new { x.RecordId, x.TagId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
