using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Restaurant_POS.Messages;
using Restaurant_POS.Models;
using Restaurant_POS.Services;
using Restaurant_POS.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Restaurant_POS.ViewModels
{

    public partial class SettingsMenuVM : ObservableObject
    {
        private readonly UsersRepository _usersRepository;
        private readonly CategoriesRepository _categoriesRepository;
        private readonly ProductsRepository _productsRepository;

        private Window _userWindowView;
        private Window _productWindowView;
        private Window _categoryWindowView;

        private User _currentUser;
        [ObservableProperty]
        public ObservableCollection<User> users;
        [ObservableProperty]
        public List<Category> categories;
        [ObservableProperty]
        public List<Product> products;
        
        public SettingsMenuVM()
        {
            _usersRepository = new UsersRepository();
            _categoriesRepository = new CategoriesRepository();
            _productsRepository = new ProductsRepository();

            Categories = _categoriesRepository.GetAllCategories();
            Products = _productsRepository.getAllProducts();

            WeakReferenceMessenger.Default.Register<UserLoginorUpdatedMessage>(this, OnUserLoginorUpdated);
            WeakReferenceMessenger.Default.Register<ViewUpdated>(this, OnViewUpdated);
        }
        private void OnViewUpdated(object recipient, ViewUpdated message)
        {
            switch (message.Value)
            {
                case "userWindowClosed":
                    {
                        _userWindowView.Close();
                        break;
                    }
                case "user":
                    {
                        _userWindowView.Close();
                        Users =new ObservableCollection<User>( _usersRepository.GetAllUsers(_currentUser.Id));
                        break;
                    }
                case "categoryWindowClosed":
                    {
                        _categoryWindowView.Close();
                        break;
                    }
                case "category":
                    {
                        _categoryWindowView.Close();
                        Categories = _categoriesRepository.GetAllCategories();
                        Products = _productsRepository.getAllProducts();
                        break;
                    }
                case "productWindowClosed":
                    {
                        _productWindowView.Close();
                        break;
                    }
                case "product":
                    {
                        _productWindowView.Close();
                        Products = _productsRepository.getAllProducts();
                        break;
                    }
            }
        }
        private void OnUserLoginorUpdated(object recipient, UserLoginorUpdatedMessage message)
        {
            _currentUser = _usersRepository.GetUserById(message.Value);
            Users =  new ObservableCollection<User>( _usersRepository.GetAllUsers(_currentUser.Id));
        }

        [RelayCommand]
        public void AddNewItem(string type)
        {
            switch (type)
            {
                case "user":
                    {
                        _userWindowView = new UserWindowView();
                        _userWindowView.ShowDialog();
                        break;
                    }
                case "category":
                    {
                        _categoryWindowView = new CategoryWindowView();
                        _categoryWindowView.ShowDialog();
                        break;
                    }
                case "product":
                    {
                        //TODO: check the category count when equal to zero
                        if (Categories.Count > 0)
                        {
                            _productWindowView = new ProductWindowView();
                            _productWindowView.ShowDialog();
                        }
                        else
                        {
                            MessageBox.Show("Category Collection is Empty. You should add Product Category first!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                            _categoryWindowView = new CategoryWindowView();
                            _categoryWindowView.ShowDialog();
                        }
                        break;
                    }
            }
        }
        [RelayCommand]
        public void CustomizedSelectedUser(int id) 
        {
            _userWindowView = new UserWindowView();
            WeakReferenceMessenger.Default.Send<UserCustomizedMessage>(new UserCustomizedMessage(id));
            _userWindowView.ShowDialog();

        }
        [RelayCommand]
        public void CustomizedSelectedCategory(int id)
        {
            _categoryWindowView = new CategoryWindowView();
            WeakReferenceMessenger.Default.Send<CategoryCustomizedMessage>(new CategoryCustomizedMessage(id));
            _categoryWindowView.ShowDialog();
        }
        [RelayCommand]
        public void CustomizedSelectedProduct(int id)
        {
            _productWindowView = new ProductWindowView();
            WeakReferenceMessenger.Default.Send<ProductCustomizedMessage>(new ProductCustomizedMessage(id));
            _productWindowView.ShowDialog();
        }
    }
}
