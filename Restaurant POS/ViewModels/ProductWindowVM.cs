using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Win32;
using Restaurant_POS.Messages;
using Restaurant_POS.Models;
using Restaurant_POS.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Restaurant_POS.ViewModels
{
    public partial class ProductWindowVM : ObservableObject
    {
        private readonly ProductsRepository _productsRepository;
        private readonly CategoriesRepository _categoriesRepository;
        private bool _isCustomizedMode;

        public bool IsCustomizedMode { get { return _isCustomizedMode; } }

        [ObservableProperty]
        public Product newProduct;
        [ObservableProperty]
        public List<Category> categories;
        [ObservableProperty]
        public int selectedCategoryIndex;
        [ObservableProperty]
        public Visibility removeButtonVisibility;
        public ProductWindowVM()
        {
            _productsRepository = new ProductsRepository();
            _categoriesRepository = new CategoriesRepository();

            Categories = _categoriesRepository.GetAllCategories();
            NewProduct = new Product();
            _isCustomizedMode = false;
            RemoveButtonVisibility = Visibility.Collapsed;
            SelectedCategoryIndex = -1;

            WeakReferenceMessenger.Default.Register<ProductCustomizedMessage>(this, OnProductCustomized);
        }
        private void OnProductCustomized(object recipient, ProductCustomizedMessage message)
        {
            RemoveButtonVisibility = Visibility.Visible;
            _isCustomizedMode = true;
            NewProduct = _productsRepository.getProductById(message.Value);
            SelectedCategoryIndex = Categories.IndexOf(Categories.Where(c=>c.Id== NewProduct.Category.Id).First());
        }
        [RelayCommand]
        public void AddNewProductConfirm()
        {
            if (string.IsNullOrWhiteSpace(NewProduct.Name) || string.IsNullOrEmpty(NewProduct.Name) || string.IsNullOrWhiteSpace(NewProduct.ImagePath) || string.IsNullOrEmpty(NewProduct.ImagePath) ||
                string.IsNullOrWhiteSpace(NewProduct.Description) || string.IsNullOrEmpty(NewProduct.Description)|| NewProduct.Category==null )
            {
                MessageBox.Show("Must fill all the fields", "Update Error!", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            else if (NewProduct.Discount > 100 || NewProduct.Discount < 0)
            {
                //Error-Message
                MessageBox.Show("Discount value should be in 0-100.", "Value Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (NewProduct.MentionedPrice <= 0)
            {
                //Error-Message
                MessageBox.Show("Mention Price should be greater than 0.", "Value Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Product product = new Product()
                {
                    Name = NewProduct.Name,
                    ImagePath = NewProduct.ImagePath,
                    MentionedPrice = NewProduct.MentionedPrice,
                    Description = NewProduct.Description,
                    Discount = NewProduct.Discount,
                    Category = NewProduct.Category,
                };
                if (_isCustomizedMode)
                {
                    product.Id = NewProduct.Id;
                    _productsRepository.UpdateProduct(product);
                }
                else
                {
                    _productsRepository.AddNewProduct(product);
                }
                WeakReferenceMessenger.Default.Send<ViewUpdated>(new ViewUpdated("product"));
            }

        }
        [RelayCommand]
        public void CloseProductWindowView()
        {
            WeakReferenceMessenger.Default.Send<ViewUpdated>(new ViewUpdated("productWindowClosed"));
        }
        [RelayCommand]
        public void AddImage(string type)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != null)
            {
                NewProduct.ImagePath = openFileDialog.FileName.ToString();
            }
        }
        [RelayCommand]
        public void RemoveSelectedProduct()
        {
            MessageBoxResult result = new MessageBoxResult();
            result = MessageBox.Show("Do you want to remove this product!", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _productsRepository.RemoveProductById(NewProduct.Id);
                WeakReferenceMessenger.Default.Send<ViewUpdated>(new ViewUpdated("product"));
            }

        }
    }
}
