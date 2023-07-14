using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant_POS.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public String ImagePath { get; set; }
        public Category Category { get; set; }
        private double _mentionedPrice;
        public double MentionedPrice 
        {
            get { return _mentionedPrice; }
            set 
            { 
                if(value>=0)
                {
                    _mentionedPrice = value;
                }
                else
                {
                    _mentionedPrice = 0;
                    throw new ArgumentOutOfRangeException(nameof(MentionedPrice),$"{nameof(MentionedPrice)} Cannot be negative");
                }
            }
        }
        private double _discount { get; set; }
        public double Discount {
            get
            {
                return _discount;
            }
            set
            {
                if(value >= 0 || value<=100)
                {
                  _discount = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(Discount), $"{nameof(Discount)} must be in range of 0 and 100");
                }
            }
        }
        public double SellingPrice
        {
            get
            {
                return MentionedPrice - (MentionedPrice * (_discount/100));
            }
        }
        public bool IsDiscountAvailable { get 
            { 
                if(_discount>0 &&  _discount <= 100)
                {
                    return true;
                }
                return false;
            }
        }
        public double DiscountPrice { get
            {
                return MentionedPrice - SellingPrice;
            } 
        }

    }
}
