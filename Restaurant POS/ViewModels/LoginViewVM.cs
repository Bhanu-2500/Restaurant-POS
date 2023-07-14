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

    public partial class LoginViewVM : ObservableObject
    {
        private readonly UsersRepository _usersRepository;
        [ObservableProperty]
        public string userName;
        [ObservableProperty]
        public string password;
        private string _password;
        public LoginViewVM()
        {
            _usersRepository = new UsersRepository();
        }
        [RelayCommand]
        public void Login()
        {
            if(!string.IsNullOrEmpty(UserName) || !string.IsNullOrEmpty(Password)|| !string.IsNullOrWhiteSpace(UserName) || !string.IsNullOrWhiteSpace(Password)) 
            {
                //Verified user details with database
                int userId = _usersRepository.CheckAuthentication(UserName, Password);
                if (userId != -1)
                {
                    UserName = String.Empty;
                    Password = String.Empty;
                    _usersRepository.SetLastLogin(userId);
                    WeakReferenceMessenger.Default.Send(new UserLoginorUpdatedMessage(userId));
                }
                else
                {
                    MessageBox.Show("The UserName or Password you entered is incorrect.", "Error!", MessageBoxButton.OK,MessageBoxImage.Warning);
                }

            }
            else
            {
                MessageBox.Show("The UserName or Password you entered is Empty.", "Error!", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        [RelayCommand]
        public void AppShutdown()
        {
            App.Current.Shutdown();
        }
        [RelayCommand]
        public void AppWindowMinimize()
        {
            App.Current.MainWindow.WindowState = WindowState.Minimized;
        }

    }
}
