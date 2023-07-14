using Restaurant_POS.DBContext;
using Restaurant_POS.Models;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Restaurant_POS.Services
{
    public class CategoriesRepository
    {
        public void AddNewCategory(Category category)
        {
            
            using(var db = new PoSDbContext())
            {
                if(db.Categories.Any(c=>c.Name == category.Name))
                {
                    MessageBox.Show("This category is already exist. Try another name.","Duplication Error",MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                db.Categories.Add(category);
                db.SaveChanges();
            }
        }
        public void UpdateCategory(Category category)
        {
            using( var db = new PoSDbContext())
            {
                if (db.Categories.Any(c => c.Name == category.Name && c.ImagePath==category.ImagePath))
                {
                    MessageBox.Show("This category is already exist. Cannot update, try another name.", "Duplication Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                db.Categories.Update(category);
                db.SaveChanges();
            }
        }
        public bool RemoveCategoryById(int Id)
        {
            using(var db = new PoSDbContext())
            {
                
                if(db.Products.Any(p=>p.Category.Id == Id))
                {
                    return false;
                }
                Category category = db.Categories.FirstOrDefault(c => c.Id == Id);
                db.Categories.Remove(category);
                db.SaveChanges();
                return true;
            }
        }
        public List<Category> GetAllCategories()
        {
            using(var db = new PoSDbContext())
            {
               List<Category> categories = db.Categories.ToList();
                if(categories != null)
                {
                    return categories;
                }
                return new List<Category>();
            }
        }
        public Category GetCategoryById(int Id)
        {
            using (var db = new PoSDbContext())
            {
                Category category = db.Categories.FirstOrDefault(x => x.Id == Id);
                if (category != null)
                {
                    return category;
                }
                return null;
            }
        }
    }
}
