using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Restaurant_POS.Messages;
using Restaurant_POS.Models;
using Restaurant_POS.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Restaurant_POS.ViewModels
{
    public partial class FoodsAndDrinksMenuVM : ObservableObject
    {
        private readonly ProductsRepository _productsRepository;
        private readonly InvoiceBillsRepository _invoiceBillsRepository;
        private readonly UsersRepository _usersRepository;
        private readonly CategoriesRepository _categoriesRepository;
        public User CurrentUser { get; set; }


        [ObservableProperty]
        public ObservableCollection<Product> fillteredProducts;
        [ObservableProperty]
        public ObservableCollection<TemporaryOrder> temporyOrderListItems;
        [ObservableProperty]
        public ObservableCollection<TemporaryCategory> tempCategoryListItems;
        [ObservableProperty]
        public Visibility checkoutViewVisibity;
        [ObservableProperty]
        public Visibility productListingViewVisibility;
        [ObservableProperty]
        public Visibility temporaryListCheckoutVisibility;
        [ObservableProperty]
        public Category selectedItemCategory;
        [ObservableProperty]
        public double totalAmount;
        [ObservableProperty]
        public double totalDiscountPrice;
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsSubmitButtonEnable))]
        [NotifyPropertyChangedFor(nameof(Balance))]
        public double cash;

        public List<Product> AllProducts;
        public List<Order> Orders { get; set; }
        public bool IsSubmitButtonEnable
        {
            get
            {
                if (TotalAmount <= Cash && Cash !=0)
                {
                    return true;
                }
                return false;
            }
        }
        public double Balance
        {
            get
            {
                if (IsSubmitButtonEnable)
                {
                    return Cash - TotalAmount;
                }
                return 0;
            }
        }

        //Constructor
        public FoodsAndDrinksMenuVM()
        {
            _usersRepository  = new UsersRepository();
            _invoiceBillsRepository = new InvoiceBillsRepository();
            _productsRepository = new ProductsRepository();
            _categoriesRepository = new CategoriesRepository();

            AllProducts = _productsRepository.getAllProducts();

            TempCategoryListItems = new ObservableCollection<TemporaryCategory>();
            TemporyOrderListItems = new ObservableCollection<TemporaryOrder>();
            FillteredProducts = new ObservableCollection<Product>(AllProducts);

            WeakReferenceMessenger.Default.Register<UserLoginorUpdatedMessage>(this, OnUserLogin);
            WeakReferenceMessenger.Default.Register<ViewUpdated>(this, OnViewUpdated);

            UpdateTempCategoryListItems();
            ResetView();

        }

        [RelayCommand]
        public void SetCategory(string categoryName)
        {
            foreach(TemporaryCategory tempCategory in TempCategoryListItems)
            {
                if(tempCategory.Name != categoryName)
                {
                    tempCategory.IsEnabled = false;
                }
               
                else
                {
                    tempCategory.IsEnabled = true;
                    FillteredProducts =new ObservableCollection<Product>(AllProducts.Where(p => p.Category.Name == categoryName).ToList());
                }
            }
            if (categoryName == "All")
            {
                FillteredProducts = new ObservableCollection<Product>(AllProducts);
            }
        }
        [RelayCommand]
        public void CashByNotes(string notes)
        {
            switch (notes)
            {
                case "5000":
                    {
                        Cash += 5000.0; break;
                    }
                case "1000":
                    {
                        Cash += 1000.0; break;
                    }
                case "500":
                    {
                        Cash += 500.0; break;
                    }
                case "100":
                    {
                        Cash += 100.0; break;
                    }
                case "50":
                    {
                        Cash += 50.0; break;
                    }
                case "0":
                    {
                        Cash = 0; break;
                    }
            }
        }
        [RelayCommand]
        public void AddToCart(int productId)
        {
            if (CurrentUser.IsAdmin)
            {
                MessageBox.Show("Admin User cannot make orders. Only Normal Users can make orders!","Warning!",MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                TemporaryListCheckoutVisibility = Visibility.Visible;
                CheckoutViewVisibity = Visibility.Hidden;
                TemporaryOrder temporaryOrder = new TemporaryOrder(_productsRepository.getProductById(productId));
                if (TemporyOrderListItems.Count() == 0)
                {
                    TemporyOrderListItems.Add(temporaryOrder);
                }
                if (!TemporyOrderListItems.Any(i => i.Product.Name == temporaryOrder.Product.Name))
                {
                    TemporyOrderListItems.Add(temporaryOrder);
                }
                UpdateTotalAmount();
            }
        }
        [RelayCommand]
        public void RemoveItemFromTempList( string productName )
        {
            TemporaryOrder temporaryOrder = TemporyOrderListItems.Where(i=>i.Product.Name == productName).FirstOrDefault();
            TemporyOrderListItems.Remove(temporaryOrder);
            UpdateTotalAmount();
            if (TemporyOrderListItems.Count() == 0)
            {
                ResetView();
            }
        }
        [RelayCommand]
        public void IncreaseAmount(Product product)
        {
            if (TemporyOrderListItems.Where(t => t.Product.Id == product.Id).FirstOrDefault().Amount < 20)
            {
                TemporyOrderListItems.Where(t => t.Product.Id == product.Id).FirstOrDefault().Amount++;
                UpdateTotalAmount();
            }
        }
        [RelayCommand]
        public void DecreaseAmount(Product product)
        {
            if (TemporyOrderListItems.Where(t => t.Product.Id == product.Id).FirstOrDefault().Amount > 1)
            {
                TemporyOrderListItems.Where(t => t.Product.Id == product.Id).FirstOrDefault().Amount--;
                UpdateTotalAmount();
            }
        }
        [RelayCommand]
        public void Checkout() 
        {
            CheckoutViewVisibity = Visibility.Visible;
            ProductListingViewVisibility = Visibility.Hidden;
            TemporaryListCheckoutVisibility = Visibility.Collapsed;
        }
        [RelayCommand]
        public void OderConformation()
        {
            List<Order> orders = new List<Order>();
            foreach(var item in TemporyOrderListItems.ToList())
            {
                orders.Add(new Order()
                {
                    Amount = item.Amount,
                    Product = item.Product,
                });
            }
            _invoiceBillsRepository.AddNewInvoice(CurrentUser.Id, orders, Cash);
            WeakReferenceMessenger.Default.Send<ViewUpdated>(new ViewUpdated("neworder"));
            ResetView();
        }
        [RelayCommand]
        public void CancelOrder()
        {
            ResetView();
        }
        public void ResetView()
        {
            TemporyOrderListItems.Clear();
            CheckoutViewVisibity = Visibility.Hidden;
            ProductListingViewVisibility = Visibility.Visible;
            TemporaryListCheckoutVisibility = Visibility.Visible;
            UpdateTotalAmount();
            Cash = 0;
        }
        public void UpdateTotalAmount()
        { 
            TotalDiscountPrice = 0;
            TotalAmount = 0;
            foreach (var item in TemporyOrderListItems.ToList())
            {
                TotalAmount += item.SubTotal;
                TotalDiscountPrice += item.TotalDiscount;
            }
        }

        // Get the user's Details when user login the system
        private void OnUserLogin(object o,UserLoginorUpdatedMessage message)
        {
            CurrentUser = _usersRepository.GetUserById(message.Value);
            ResetView();
            UpdateTempCategoryListItems();
            FillteredProducts = new ObservableCollection<Product>(AllProducts);
        }

        // Filltering out the product in the product list when user select the category
        private void UpdateTempCategoryListItems()
        {
            TempCategoryListItems.Clear();
            List<Category> categories = _categoriesRepository.GetAllCategories();
            foreach (Category category in categories)
            {
                TemporaryCategory tempCategory = new TemporaryCategory()
                {
                    Name = category.Name,
                    ImagePath = category.ImagePath,
                    IsEnabled = false
                };
                if (tempCategory.Name == "All")
                {
                    tempCategory.IsEnabled = true;
                }
                TempCategoryListItems.Add(tempCategory);
            }
        }

        //Update category or product list, When admin update,delete or create product or category 
        private void OnViewUpdated(object recipient, ViewUpdated message)
        {
            switch (message.Value)
            {
                case "product":
                    {
                        AllProducts = _productsRepository.getAllProducts();
                        FillteredProducts = new ObservableCollection<Product>(AllProducts);
                        break;
                    }
                case "category":
                    {
                        UpdateTempCategoryListItems();
                        break;
                    }
            }
        }
    }
}
