using Microsoft.EntityFrameworkCore;
using Restaurant_POS.DBContext;
using Restaurant_POS.Migrations;
using Restaurant_POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_POS.Services
{
    public class ProductsRepository
    {
        public List<Product> getAllProducts()
        {
            using(var db = new PoSDbContext())
            {
                var products = db.Products.Include(p=>p.Category).ToList();
                return products;
            }
        }
        public Product getProductById(int id)
        {
            using( var db = new PoSDbContext())
            {
                
                return db.Products.Where(p => p.Id == id).Include(p=>p.Category).FirstOrDefault();
            }
        }
        public void AddNewProduct(Product product)
        {
            using(var db = new PoSDbContext())
            {
                Category category = db.Categories.FirstOrDefault(c=>c.Id==product.Category.Id);
                if (category != null)
                {
                    product.Category = category;
                }
                db.Products.Add(product);
                db.SaveChanges();
            }
        }
        public void UpdateProduct(Product product)
        {
            using(var db =new PoSDbContext())
            {
                db.Products.Update(product);
                db.SaveChanges();
            }
        }
        public void RemoveProductById(int id) 
        {
            using(var db = new PoSDbContext())
            {
                Product product = db.Products.FirstOrDefault(p => p.Id == id);
                if(product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                }
            }
        }
    }
}
