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
    public partial class CategoryWindowVM : ObservableObject
    {
        private readonly CategoriesRepository _categoriesRepository;
        private bool _isCustomizedMode;

        public bool IsCustomizedMode { get { return _isCustomizedMode; } }
        [ObservableProperty]
        public Category newCategory;
        [ObservableProperty]
        public Visibility removeButtonVisibility;
        public CategoryWindowVM()
        {
            _categoriesRepository = new CategoriesRepository();
            NewCategory = new Category();
            RemoveButtonVisibility = Visibility.Collapsed;
            _isCustomizedMode = false;

            WeakReferenceMessenger.Default.Register<CategoryCustomizedMessage>(this, OnCategoryCustomized);
        }

        private void OnCategoryCustomized(object recipient, CategoryCustomizedMessage message)
        {
            NewCategory = _categoriesRepository.GetCategoryById(message.Value);
            RemoveButtonVisibility = Visibility.Visible;
            _isCustomizedMode = true;
        }

        [RelayCommand]
        public void AddImage()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != null)
                NewCategory.ImagePath = openFileDialog.FileName.ToString();
        }
    
        [RelayCommand]
        public void AddNewCategoryConfirm()
        {
            if (string.IsNullOrWhiteSpace(NewCategory.Name) || string.IsNullOrEmpty(NewCategory.Name) || string.IsNullOrWhiteSpace(NewCategory.ImagePath) || string.IsNullOrEmpty(NewCategory.ImagePath))
            {
                MessageBox.Show("Must fill all the fields", "Update Error!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                Category category = new Category()
                {
                    Name = NewCategory.Name,
                    ImagePath = NewCategory.ImagePath,
                };
                if (_isCustomizedMode)
                {
                    category.Id = NewCategory.Id;
                    _categoriesRepository.UpdateCategory(category);
                    WeakReferenceMessenger.Default.Send<ViewUpdated>(new ViewUpdated("category"));
                }
                else
                {
                    _categoriesRepository.AddNewCategory(category);
                }
                WeakReferenceMessenger.Default.Send<ViewUpdated>(new ViewUpdated("category"));
            }

        }
        [RelayCommand]
        public void RemoveSelectedCategory()
        {
            MessageBoxResult result = new MessageBoxResult();
            result = MessageBox.Show("Do you want to remove this category!", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (_categoriesRepository.RemoveCategoryById(NewCategory.Id))
                {
                    WeakReferenceMessenger.Default.Send<ViewUpdated>(new ViewUpdated("category"));
                }
                else
                {
                    MessageBox.Show("Products are registerd under this category. Cannot delete this category!", "Error!", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }

        }
        [RelayCommand]
        public void CloseCategoryWindowView()
        {
            WeakReferenceMessenger.Default.Send<ViewUpdated>(new ViewUpdated("categoryWindowClosed"));
        }
    }

}

