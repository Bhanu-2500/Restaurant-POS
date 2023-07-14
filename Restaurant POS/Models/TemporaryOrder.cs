using CommunityToolkit.Mvvm.ComponentModel;


namespace Restaurant_POS.Models
{
    public partial class TemporaryOrder : ObservableObject
    {
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(SubTotal))]
        [NotifyPropertyChangedFor(nameof(TotalDiscount))]
        public int amount;
        public Product Product { get; set; }
        public double SubTotal
        {
            get
            {
                return Amount * Product.SellingPrice;
            }
            set
            {
                value = Product.SellingPrice * Amount;
            }
        }
        public double TotalDiscount
        {
            get
            {
                return Amount * Product.DiscountPrice;
            }
            set
            {
                value = Product.DiscountPrice * Amount;
            }
        }
        public TemporaryOrder(Product product)
        {
            this.Product = product;
            Amount = 1;
        }
    }

}
