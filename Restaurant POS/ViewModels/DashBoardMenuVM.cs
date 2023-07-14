using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using LiveCharts;
using LiveCharts.Wpf;
using Microsoft.Win32;
using Restaurant_POS.Messages;
using Restaurant_POS.Models;
using Restaurant_POS.Services;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Restaurant_POS.ViewModels
{
    public partial class DashBoardMenuVM : ObservableObject
    {
        private readonly UsersRepository _usersRepository;

        [ObservableProperty]
        public User currentUser;
        [ObservableProperty]
        public bool isEditable;
        [ObservableProperty]
        public int invoiceCount;
        [ObservableProperty]
        public int invoiceCountForToday;

        //return a list of days upto today in this month
        //x-values for graphs
        public List<string> XLabels {
            get
            {
                List<string> list = new List<string>();
                for(int i = 1;  i <= DateTime.DaysInMonth(DateTime.Now.Year,DateTime.Now.Month); i++)
                {
                    list.Add(i.ToString());
                }
                return list;
            }
        }

        [ObservableProperty]
        public SeriesCollection seriesCollectionForWorkHours;
        [ObservableProperty]
        public SeriesCollection seriesCollectionForSales;

        //Constructor
        public DashBoardMenuVM()
        {
            _usersRepository = new UsersRepository();
            WeakReferenceMessenger.Default.Register<UserLoginorUpdatedMessage>(this, OnUserLoginOrUpdated);
            WeakReferenceMessenger.Default.Register<ViewUpdated>(this, OnNewOrderCreated);
            IsEditable = false;
        }

        private void OnNewOrderCreated(object recipient, ViewUpdated message)
        {
                InvoiceCount = _usersRepository.GetTotalInvoicesCount(CurrentUser.Id);
                InvoiceCountForToday = _usersRepository.GetTotalInvoicesCountForToday(CurrentUser.Id);
            if (CurrentUser.IsAdmin != true)
            {
                List<double> values = new List<double>((List<double>)_usersRepository.GetWorkHoursofThisMonth(CurrentUser.Id));
                SeriesCollectionForWorkHours = new SeriesCollection(){
                    new LineSeries
                    {
                        Title = CurrentUser.Name,
                        Values =new ChartValues<double>( (List<double>)_usersRepository.GetWorkHoursofThisMonth(CurrentUser.Id))
                    }
                        };
                InvoiceCount = _usersRepository.GetTotalInvoicesCount(CurrentUser.Id);
                InvoiceCountForToday = _usersRepository.GetTotalInvoicesCountForToday(CurrentUser.Id);
            }
            else
            {
                SeriesCollectionForWorkHours = new SeriesCollection();
                SeriesCollectionForSales = new SeriesCollection();
                List<(string, List<double>)> values = (List<(string, List<double>)>)_usersRepository.GetWorkHoursofThisMonth(CurrentUser.Id);
                foreach (var value in values)
                {
                    SeriesCollectionForWorkHours.Add(new LineSeries
                    {
                        Title = value.Item1,
                        Values = new ChartValues<double>(value.Item2)
                    });
                }
                List<(string, List<double>)> sales = (List<(string, List<double>)>)_usersRepository.GetTotalSalesoftheMonth(CurrentUser.Id);
                foreach (var value in sales)
                {
                    SeriesCollectionForSales.Add(new LineSeries
                    {
                        Title = value.Item1,
                        Values = new ChartValues<double>(value.Item2),
                    });
                }
            }


        }

        //Allow user to edit 
        [RelayCommand]
        public void EditUserDetails()
        {
            IsEditable = true;
        }

        //Save Edited User Details
        [RelayCommand]
        public void SaveEditedDetails()
        {
            if( string.IsNullOrEmpty(CurrentUser.Name)|| string.IsNullOrWhiteSpace(CurrentUser.Name)||
                string.IsNullOrEmpty(CurrentUser.Password)|| string.IsNullOrWhiteSpace(CurrentUser.Password)||
                string.IsNullOrEmpty(CurrentUser.PhoneNumber)|| string.IsNullOrWhiteSpace(CurrentUser.PhoneNumber)||
                string.IsNullOrEmpty(CurrentUser.ImagePath)|| string.IsNullOrWhiteSpace(CurrentUser.ImagePath)||
                string.IsNullOrEmpty(CurrentUser.Email)|| string.IsNullOrWhiteSpace(CurrentUser.Email))
            {
                MessageBox.Show("Any editable field cannot be enpty. Please! fill all the fields","Erro",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else
            {
                IsEditable = false;
                _usersRepository.UpdateUserDetails(CurrentUser);
                WeakReferenceMessenger.Default.Send<UserLoginorUpdatedMessage>(new UserLoginorUpdatedMessage(CurrentUser.Id));
            }
        }

        //Allow user to select profile picture
        [RelayCommand]
        public void UserProfilePictureSelection()
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            if (op.ShowDialog() == true)
            {
                CurrentUser.ImagePath = op.FileName.ToString();
            }
        }

        //Shut-down app
        [RelayCommand]
        public void ShutDownApp()
        {
            _usersRepository.UserLogout(CurrentUser.Id);
            MessageBoxResult messageBoxResult = new MessageBoxResult();
            messageBoxResult= MessageBox.Show("Do you want to Shut down the app","Warning",MessageBoxButton.YesNo,MessageBoxImage.Question);
            if (messageBoxResult == MessageBoxResult.Yes)
            {
                App.Current.Shutdown();
            }
        }

        //get the user when user logging 
        public void OnUserLoginOrUpdated(Object o, UserLoginorUpdatedMessage message)
        {
            CurrentUser = _usersRepository.GetUserById(message.Value);
            if(CurrentUser.IsAdmin !=true)
            {
                List<double> values = new List<double>((List<double>)_usersRepository.GetWorkHoursofThisMonth(CurrentUser.Id));
                SeriesCollectionForWorkHours = new SeriesCollection(){
                    new LineSeries
                    {
                        Title = CurrentUser.Name,
                        Values =new ChartValues<double>( (List<double>)_usersRepository.GetWorkHoursofThisMonth(CurrentUser.Id))
                    }
                        };
                InvoiceCount = _usersRepository.GetTotalInvoicesCount(CurrentUser.Id);
                InvoiceCountForToday = _usersRepository.GetTotalInvoicesCountForToday(CurrentUser.Id);
            }
            else
            {
                SeriesCollectionForWorkHours = new SeriesCollection();
                SeriesCollectionForSales = new SeriesCollection();
                List<(string, List<double>)> values = (List<(string, List<double>)>)_usersRepository.GetWorkHoursofThisMonth(CurrentUser.Id);
                foreach (var value in values)
                {
                    SeriesCollectionForWorkHours.Add(new LineSeries
                    {
                        Title = value.Item1,
                        Values = new ChartValues<double>(value.Item2)
                    });
                }
                List<(string,List<double>)> sales = (List<(string, List<double>)>)_usersRepository.GetTotalSalesoftheMonth(CurrentUser.Id);
                foreach(var value in sales)
                {
                    SeriesCollectionForSales.Add(new LineSeries
                    {
                        Title = value.Item1,
                        Values = new ChartValues<double>(value.Item2),
                    });
                }
            }
        }
    }
}
