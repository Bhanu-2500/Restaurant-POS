using Microsoft.EntityFrameworkCore;
using Restaurant_POS.DBContext;
using Restaurant_POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Restaurant_POS.Services
{
    public class UsersRepository
    {
        public int CheckAuthentication(string  username, string password)
        {
            using(var db = new PoSDbContext())
            {
                User? user = db.Users.Where(u=>u.Name==username && u.Password==password).FirstOrDefault();
                if (user != null)
                {
                    user.LastLogin = DateTime.Now;
                    db.Users.Update(user);
                    db.SaveChanges();
                    return user.Id;
                }
                return -1;
            }
        }
        public User GetUserById(int id)
        {
            using( var db = new PoSDbContext())
            {
                return db.Users.Where(u => u.Id == id).Include(u=>u.PersonalWorkHistory).FirstOrDefault();
            }
           
        }
        public void SetLastLogin(int  id)
        {
            using(var db = new PoSDbContext())
            {
                db.Users.Where(u => u.Id == id).First().LastLogin = DateTime.Now;
                db.SaveChanges();
            }
        }
        public void UserLogout(int id)
        {
            using (var db = new PoSDbContext())
            {
                if(db.Users.Where(u => u.Id == id).Include(u => u.PersonalWorkHistory).FirstOrDefault().PersonalWorkHistory != null)
                {
                    User user = db.Users.Where(u => u.Id == id).Include(u => u.PersonalWorkHistory).FirstOrDefault();
                    if (user.PersonalWorkHistory.Any(d => d.Date.Day.Equals(DateTime.Now.Date.Day)))
                    {
                        TimeSpan totalWorkForLastLogin = DateTime.Now.Subtract(user.LastLogin);
                        user.PersonalWorkHistory.Where(d => d.Date.Day.Equals(DateTime.Now.Date.Day)).FirstOrDefault().TotalHoursForTheDay += totalWorkForLastLogin;
                        db.Users.Update(user);
                        db.SaveChanges();
                    }
                    else
                    {
                        WorkHistory workHistory = new WorkHistory() 
                        {
                            Date = DateTime.Now,
                            TotalHoursForTheDay = DateTime.Now.Subtract(user.LastLogin)
                        };
                        user.PersonalWorkHistory.Add(workHistory);
                        db.Users.Update(user);
                        db.SaveChanges();
                    }
                }
                else
                {
                    User user = db.Users.Where(u => u.Id == id).FirstOrDefault();
                    List<WorkHistory> workHistories = new List<WorkHistory>();
                    WorkHistory workHistory = new WorkHistory()
                    {
                        Date = DateTime.Now,
                        TotalHoursForTheDay = DateTime.Now.Subtract(user.LastLogin)
                    };
                    workHistories.Add(workHistory);
                    user.PersonalWorkHistory = workHistories;
                    db.Users.Update(user);
                    db.SaveChanges();
                }
            }
        }
        public Object GetWorkHoursofThisMonth(int id)
        {
            if (GetUserById(id).IsAdmin)
            {
                List<(string,List<double>)> workingHoursOfAllUsers = new List<(string,List<double>)>();
                using(var db = new PoSDbContext())
                {
                    List<User> users = db.Users.Where(u=>u.IsAdmin == false).Include(u=>u.PersonalWorkHistory.Where(w => w.Date.Month == DateTime.Now.Month)).ToList();
                    foreach(var user  in users)
                    {
                        List<double> workingHourForEachUser = new List<double>();
                        if (user.PersonalWorkHistory != null)
                        {   
                            int i = 1;
                            if (user.PersonalWorkHistory.Count > 0)
                            {
                                foreach (var workHistory in user.PersonalWorkHistory)
                                {
                                    while (i <= DateTime.Now.Date.Day)
                                    {
                                        if (i == workHistory.Date.Day)
                                        {
                                            workingHourForEachUser.Add(workHistory.TotalHoursForTheDay.TotalHours);
                                            i++;
                                            break;
                                        }
                                        else
                                        {
                                            workingHourForEachUser.Add(0);
                                        }
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 1; j < DateTime.Now.Date.Date.Day; j++)
                                {
                                    workingHourForEachUser.Add(0);
                                }
                            }
                        }
                        workingHoursOfAllUsers.Add((user.Name, workingHourForEachUser));
                    }
                }
                return workingHoursOfAllUsers;
            }
            else
            {
                List<double> workingHours = new List<double>();
                using (var db = new PoSDbContext())
                {
                    if (db.Users.Any(u => u.Id == id))
                    {
                        User user = db.Users.Where(u => u.Id == id).Include(u => u.PersonalWorkHistory.Where(w => w.Date.Month == DateTime.Now.Month)).FirstOrDefault();
                        if (user.PersonalWorkHistory != null)
                        {
                            int i = 1;
                            if (user.PersonalWorkHistory.Count > 0)
                            {
                                foreach (var workHistory in user.PersonalWorkHistory)
                                {
                                    while (i <= DateTime.Now.Date.Day)
                                    {
                                        if (i == workHistory.Date.Date.Day)
                                        {
                                            workingHours.Add(workHistory.TotalHoursForTheDay.TotalHours);
                                            i++;
                                            break;
                                        }
                                        else
                                        {
                                            workingHours.Add(0);
                                        }
                                        i++;
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 1; j < DateTime.Now.Date.Date.Day; j++)
                                {
                                    workingHours.Add(0);
                                }
                            }
                        }
                    }
                }
                return workingHours;
            }
        }
        public List<(string,List<double>)> GetTotalSalesoftheMonth(int id)
        {
            List<(string, List<double>)> values = new List<(string, List<double>)>();
            using(var db = new PoSDbContext())
            {
                List<User> users = db.Users.Where(u=>!u.IsAdmin).ToList();
                if(users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        List<double> salesForUser = new List<double>();
                        List<InvoiceBill> invoiceBills=  db.InvoiceBills.Where(ib=>ib.User.Id == user.Id).Include(ib=>ib.OrderList).OrderBy(ib=>ib.DateTime.Date).ToList();
                        foreach (InvoiceBill invoice in invoiceBills)
                        {
                            foreach (Order order in invoice.OrderList)
                            {
                                Order temporder = db.Orders.Where(o => o.Id == order.Id).Include(o => o.Product).First();
                                if (temporder != null)
                                {
                                    invoice.OrderList.Where(o => o.Id == order.Id).FirstOrDefault().Product = temporder.Product;
                                }
                            }
                        }
                        int i = 1;
                        while (i <= DateTime.Now.Date.Day)
                        {
                            if(invoiceBills.Any(ib=>ib.DateTime.Date.Day == i))
                            {
                               salesForUser.Add(invoiceBills.Where(ib=>ib.DateTime.Date.Day==i).ToList().Sum(ib=>ib.SubTotal));
                            }
                            else
                            {
                                salesForUser.Add(0);
                            }
                            i++;
                        }
                        values.Add((user.Name, salesForUser));
                    }
                }
            }
            return values;
        }
        public void UpdateUserDetails(User user)
        {
            if (user == null)
            {
                return;
            }
            using(var db = new PoSDbContext())
            {
                db.Update(user);
                db.SaveChanges();
            }
        }
        public List<User> GetAllUsers(int Id)
        {
            using(var db = new PoSDbContext())
            {
                List <User> users = db.Users.Where(u=>u.Id != Id).ToList();
                if (users != null)
                {
                    return users;
                }
                return new List<User>();
            }
        }
        public void AddNewUser(User user)
        {
            if (user == null)
            {
                return;
            }
            using(var db =new PoSDbContext())
            {
                db.Users.Add(user);
                db.SaveChanges();
            }
        }
        public void RemoveUserById(int id) 
        {
            using(var db = new PoSDbContext())
            {
                User user = db.Users.Where(u => u.Id == id).Include(u=>u.PersonalWorkHistory).First();

                if (user != null)
                {  
                    List<InvoiceBill> invoiceBills = db.InvoiceBills.Where(ib => ib.User.Id == user.Id).Include(ib=>ib.OrderList).ToList();
                    foreach(var invoiceBill in invoiceBills)
                    {
                        db.InvoiceBills.Remove(invoiceBill);
                    }
                    db.Users.Remove(user);
                    db.SaveChanges();
                }

            }
        }
        public int GetTotalInvoicesCount(int userId)
        {
            using(var db = new PoSDbContext())
            {
                return db.InvoiceBills.Where(ib=>ib.User.Id == userId).Count();
            }
        }
        public int GetTotalInvoicesCountForToday(int userId)
        {
            using(var db = new PoSDbContext())
            {
                return db.InvoiceBills.Where(ib=>ib.User.Id == userId && ib.DateTime.Date==DateTime.Now.Date).Count();
            }
        }
    }
}
