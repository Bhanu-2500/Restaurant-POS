using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Restaurant_POS.DBContext;
using Restaurant_POS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_POS.Services
{
    public class InvoiceBillsRepository
    {
        public void AddNewInvoice(int Id, List<Order> orders, double cash)
        {
            
            using (var db = new PoSDbContext())
            {
                List<Order> orderList = new List<Order>();
                foreach (var order in orders)
                {
                   Order tempOrder= new Order(){
                                                    Amount = order.Amount,
                                                    Product = db.Products.Where(p => p.Id == order.Product.Id).First(),
                                                };
                    db.Orders.Add(tempOrder);
                    orderList.Add(tempOrder);
                }
                InvoiceBill invoiceBill = new InvoiceBill()
                {
                    
                    DateTime = DateTime.Now,
                    User = db.Users.Where(u => u.Id == Id).First(),
                    OrderList = orderList,
                    Cash = cash
                };
                db.InvoiceBills.Add(invoiceBill);
                db.SaveChanges();
            }
        }
        public List<InvoiceBill> GetAllInvoiceBills(int Id) 
        {
            List<InvoiceBill> invoiceBills = new List<InvoiceBill>();
            using (var db = new PoSDbContext())
            {
                if (!db.Users.Where(u => u.Id == Id).FirstOrDefault().IsAdmin)
                {
                    invoiceBills = db.InvoiceBills.Where(ib => ib.User.Id == Id).Include(ib => ib.OrderList).ToList();
                    if (invoiceBills.Count > 0)
                    {
                        foreach (var invoice in invoiceBills)
                        {
                            foreach (var order in invoice.OrderList)
                            {
                                Order tempOrder = db.Orders.Where(o => o.Id == order.Id).Include(o => o.Product).FirstOrDefault();
                                if (tempOrder != null)
                                {
                                    order.Product = tempOrder.Product;
                                }
                            }
                        }
                    }

                }
                else
                {
                    invoiceBills = db.InvoiceBills.Include(ib => ib.OrderList).Include(ib => ib.User).ToList();
                    if (invoiceBills.Count > 0)
                    {
                        foreach (var invoice in invoiceBills)
                        {
                            foreach (var order in invoice.OrderList)
                            {
                                Order tempOrder = db.Orders.Where(o => o.Id == order.Id).Include(o => o.Product).FirstOrDefault();
                                if (tempOrder != null)
                                {
                                    order.Product = tempOrder.Product;
                                }
                            }
                        }
                    }

                }
                invoiceBills = invoiceBills.OrderByDescending(ib => ib.DateTime).ToList();
                return invoiceBills;
            }
        }
    }
}
