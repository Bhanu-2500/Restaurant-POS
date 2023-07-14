using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Restaurant_POS.Messages;
using Restaurant_POS.Models;
using Restaurant_POS.Services;
using Restaurant_POS.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Restaurant_POS.ViewModels
{
    public partial class ShopViewVM : ObservableObject
    {
        private readonly UsersRepository _usersRepository;

        [ObservableProperty]
        public User currentUser;
        [ObservableProperty]
        public object selectedMenuItemView;
        [ObservableProperty]
        public Visibility temporyOrdersListViewVisibility;

        public FoodsAndDrinksMenuVM FoodsAndDrinksMenuVM { get; set; }
        public DashBoardMenuVM DashBoardMenuVM { get; set; }
        public SettingsMenuVM SettingsMenuVM { get; set; }
        public DetailedInvoiceBillVM DetailedInvoiceBillVM { get; set; }


        public ShopViewVM()
        {
            _usersRepository = new UsersRepository();

            FoodsAndDrinksMenuVM = new FoodsAndDrinksMenuVM();
            SettingsMenuVM = new SettingsMenuVM();
            DetailedInvoiceBillVM = new DetailedInvoiceBillVM();
            DashBoardMenuVM = new DashBoardMenuVM();
            WeakReferenceMessenger.Default.Register<UserLoginorUpdatedMessage>(this, OnUserLiginOrUpdated);
        }
        [RelayCommand]
        public void Logout()
        {
            WeakReferenceMessenger.Default.Send(new UserLogoutMessage(CurrentUser.Id));
            SelectedMenuItemView = FoodsAndDrinksMenuVM;
        }
        [RelayCommand]
        public void BillingListView()
        {
            SelectedMenuItemView = DetailedInvoiceBillVM;
        }
        [RelayCommand]
        public void ProductsView()
        {
            SelectedMenuItemView = FoodsAndDrinksMenuVM;
        }
        [RelayCommand]
        public void DashBoardView()
        {
            SelectedMenuItemView = DashBoardMenuVM;
        }
        [RelayCommand]
        public void SettingsView()
        { 

            SelectedMenuItemView = SettingsMenuVM;
        }

        //Get the logged user details
        private void OnUserLiginOrUpdated(Object sender, UserLoginorUpdatedMessage message)
        {
            CurrentUser = _usersRepository.GetUserById(message.Value);
            if (SelectedMenuItemView != DashBoardMenuVM)
            {

                SelectedMenuItemView = FoodsAndDrinksMenuVM;
            }
        }


    }
}
