using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Win32;
using Restaurant_POS.Messages;
using Restaurant_POS.Models;
using Restaurant_POS.Services;
using System;
using System.Windows;

namespace Restaurant_POS.ViewModels
{
    public partial class UserWindowVM : ObservableObject
    {
        private readonly UsersRepository _usersRepository;
        private bool _isCustomizedMode { get;set; }

        public bool IsCustomizedMode { get { return _isCustomizedMode; } }
        [ObservableProperty]
        public User newUser;
        [ObservableProperty]
        public Visibility removeButtonVisibility;
        public UserWindowVM()
        {
            NewUser = new User();
            _usersRepository = new UsersRepository();
            RemoveButtonVisibility = Visibility.Collapsed;
            _isCustomizedMode = false;
            WeakReferenceMessenger.Default.Register<UserCustomizedMessage>(this, OnUserCustomized);
        }

        private void OnUserCustomized(object recipient, UserCustomizedMessage message)
        {
            _isCustomizedMode = true;
            NewUser = _usersRepository.GetUserById(message.Value);
            RemoveButtonVisibility = Visibility.Visible;
        }

        [RelayCommand]
        public void AddNewUserConfirm()
        {
            if ((string.IsNullOrWhiteSpace(NewUser.Name) || string.IsNullOrEmpty(NewUser.Name)) || (string.IsNullOrWhiteSpace(NewUser.Password) || string.IsNullOrEmpty(NewUser.Password))
                 || (string.IsNullOrWhiteSpace(NewUser.Email) || string.IsNullOrEmpty(NewUser.Email)) || (string.IsNullOrWhiteSpace(NewUser.PhoneNumber) || string.IsNullOrEmpty(NewUser.PhoneNumber))
                 || (string.IsNullOrWhiteSpace(NewUser.ImagePath) || string.IsNullOrEmpty(NewUser.ImagePath)))
                {
                    MessageBox.Show("Must fill all the fields", "Update Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                }
             else
                {
                    User user = new User()
                    {   
                        Name = NewUser.Name,
                        Email = NewUser.Email,
                        PhoneNumber = NewUser.PhoneNumber,
                        ImagePath = NewUser.ImagePath,
                        Password = NewUser.Password,
                        IsAdmin = NewUser.IsAdmin,
                    };
                if (_isCustomizedMode)
                {   
                    user.Id = NewUser.Id;
                    _usersRepository.UpdateUserDetails(user);
                }
                else
                {
                    _usersRepository.AddNewUser(user);
                }
                WeakReferenceMessenger.Default.Send<ViewUpdated>(new ViewUpdated("user"));
            }
        }
        [RelayCommand]
        public void RemoveSelectedUser()
        {
            MessageBoxResult result = new MessageBoxResult();
            result = MessageBox.Show("Do you want to remove this User!", "Warning!", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                _usersRepository.RemoveUserById(NewUser.Id);
                WeakReferenceMessenger.Default.Send<ViewUpdated>(new ViewUpdated("user"));
            }
        }
        [RelayCommand]
        public void CloseUserWindowView()
        {
            WeakReferenceMessenger.Default.Send<ViewUpdated>(new ViewUpdated("userWindowClosed"));
        }
        [RelayCommand]
        public void AddImage(string type)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            if (openFileDialog.FileName != null)
            {
               NewUser.ImagePath = openFileDialog.FileName.ToString();
            }
        }


    }
}
