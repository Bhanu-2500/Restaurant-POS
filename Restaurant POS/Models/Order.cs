using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_POS.Models
{
    public class Order
    {
        public int Id { get; set; }
        private int _amount { get; set; }=1;
        public int Amount { 
            get { return _amount; }
            set {
                if (value >0)
                { 
                    _amount = value;
                }
                else
                {   
                    _amount = 1;
                    throw new ArgumentOutOfRangeException(nameof(Amount),$"{nameof(Amount)} cannot be negative");
                }
            }
        }
        public Product Product { get; set; }
        public double SubTotal { get 
            {
                if (Product != null)
                {
                    return Product.SellingPrice * _amount;
                }
                else
                {
                    return 0;
                }
            } 
        }
        public double TotalDiscount { get 
            {
                if (Product != null)
                {
                    return Product.DiscountPrice * _amount;
                }
                else
                {
                    return 0;
                }
            } 
        }

    }
}
