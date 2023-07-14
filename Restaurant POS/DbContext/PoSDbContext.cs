using Microsoft.EntityFrameworkCore;
using Restaurant_POS.Models;

namespace Restaurant_POS.DBContext
{
    public class PoSDbContext : DbContext
    {
        private readonly string _path = @"C:\Users\bhanu\Desktop\Restaurant POS\PoSDataBase.db";
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source = {_path}");
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<InvoiceBill> InvoiceBills { get; set; }
        public DbSet<WorkHistory> WorkHistory { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
