using CommunityToolkit.Mvvm.ComponentModel;
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

namespace Restaurant_POS.ViewModels
{
    public partial class MainWindowVM : ObservableObject
    {
        private readonly UsersRepository _usersRepository;

        [ObservableProperty]
        public Object currentView;
        [ObservableProperty]
        public User loginUser;

        public LoginViewVM LoginViewVM { get; set; }
        public ShopViewVM ShopViewVM { get; set; }
        public MainWindowVM()
        {
            _usersRepository = new UsersRepository();

            LoginViewVM = new LoginViewVM();
            ShopViewVM = new ShopViewVM();
            CurrentView = LoginViewVM;

            WeakReferenceMessenger.Default.Register<UserLoginorUpdatedMessage>(this, OnUserLogin);
            WeakReferenceMessenger.Default.Register<UserLogoutMessage>(this, OnUserLogout);
        }


        private void OnUserLogout(Object o, UserLogoutMessage message)
        {
            _usersRepository.UserLogout(LoginUser.Id);
            CurrentView = LoginViewVM;
        }

        private void OnUserLogin(Object o,UserLoginorUpdatedMessage message)
        {
            LoginUser = _usersRepository.GetUserById(message.Value);
            CurrentView = ShopViewVM;
        }
    }
}
