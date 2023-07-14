using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_POS.Models
{
    [PrimaryKey(nameof(InvoiceId))]
    public class InvoiceBill
    {
        public int InvoiceId { get; set; }
        public DateTime DateTime { get; set; }
        public User User { get; set; }
        public List<Order> OrderList { get; set; }
        public double SubTotal 
        { get 
            { 
                if(OrderList != null)
                {
                    return OrderList.Sum(o=>o.SubTotal);
                }else { return 0; }
            }  
        }
        public double TotalDiscount { get 
            {
                if (OrderList != null)
                {
                    return OrderList.Sum(o => o.TotalDiscount);
                }
                else { return 0; }

            }
        }
        public double Cash { get; set; }
        public double Balance { get 
            {
                if ((OrderList != null) && (Cash >= SubTotal))
                {
                    return Cash - SubTotal;
                }
                else { return 0; }
            } 
        }
    }
}
