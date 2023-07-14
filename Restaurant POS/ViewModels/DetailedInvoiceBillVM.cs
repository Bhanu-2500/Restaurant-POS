using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Restaurant_POS.Messages;
using Restaurant_POS.Models;
using Restaurant_POS.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace Restaurant_POS.ViewModels
{
    public partial class DetailedInvoiceBillVM : ObservableObject
    {
        private readonly UsersRepository _usersRepository;
        private readonly InvoiceBillsRepository _invoiceBillsRepository;

        private User CurrentUser { get; set; }
        private List<InvoiceBill> Invoices { get; set; }
        [ObservableProperty]
        public string searchId;
        [ObservableProperty]
        public ObservableCollection<InvoiceBill> fillteredInvoiceBills;
        [ObservableProperty]
        public Visibility noItemFoundTextVisibility;
        [ObservableProperty]
        public DateTime fillteredDate;
        [ObservableProperty]
        public int invoiceCount;
        [ObservableProperty]
        public double totalSales;

        public DetailedInvoiceBillVM()
        {
            _usersRepository = new UsersRepository();
            _invoiceBillsRepository = new InvoiceBillsRepository();
            WeakReferenceMessenger.Default.Register<UserLoginorUpdatedMessage>(this, OnUserLoginOrUpdated);
            WeakReferenceMessenger.Default.Register<ViewUpdated>(this, OnviewUpdated);
            Invoices = new List<InvoiceBill>();
            FillteredDate = DateTime.MinValue.Date;
        }

        //When order confirmed and get paid, Updates the Invoice bill list
        private void OnviewUpdated(object recipient, ViewUpdated message)
        {
                Reset();
        }

        //According to the user type, which is admin or normal user, Invoicebill list changes
        private void OnUserLoginOrUpdated(object recipient, UserLoginorUpdatedMessage message)
        {
            CurrentUser = _usersRepository.GetUserById(message.Value);
            Reset();
        }
        private void Reset()
        {
            Invoices = _invoiceBillsRepository.GetAllInvoiceBills(CurrentUser.Id);
            FillteredInvoiceBills = new ObservableCollection<InvoiceBill>(Invoices.ToList());
            SearchId = string.Empty;
            FillteredDate = DateTime.MinValue.Date;
            NoItemFoundTextVisibility = Visibility.Hidden;
            CalculateInvoiceListDetails();
        }

        //Show themore details about the invoices in the list
        public void CalculateInvoiceListDetails()
        {
            InvoiceCount = FillteredInvoiceBills.Count;
            TotalSales = FillteredInvoiceBills.Sum(ib => ib.SubTotal);
        }

        //Clear the search bar and Date picker
        [RelayCommand]
        public void ClearSearch()
        {
            Reset();
        }

        //When search Id changing filter out that particular invoice bill from the available list
        partial void OnSearchIdChanged(string searchId)
        {
            if (string.IsNullOrEmpty(searchId) && FillteredDate.Date == DateTime.MinValue.Date)
            {
                FillteredInvoiceBills = new ObservableCollection<InvoiceBill>(Invoices.ToList());
                NoItemFoundTextVisibility = Visibility.Hidden;
            }
            else if(string.IsNullOrEmpty(searchId) && FillteredDate.Date != DateTime.MinValue.Date)
            {
                FillteredInvoiceBills = new ObservableCollection<InvoiceBill>(Invoices.Where(ib => ib.DateTime.Date == FillteredDate.Date).ToList());
                SearchId = string.Empty;
            }
            else if(!string.IsNullOrEmpty(searchId) && FillteredDate.Date == DateTime.MinValue.Date)
            {
                try
                {
                    if (Invoices.Any(ib => ib.InvoiceId == int.Parse(SearchId)))
                    {
                        FillteredInvoiceBills = new ObservableCollection<InvoiceBill>(Invoices.Where(ib => ib.InvoiceId == int.Parse(SearchId)).ToList());
                    }
                    else
                    {
                        FillteredInvoiceBills.Clear();
                        NoItemFoundTextVisibility = Visibility.Visible;
                    }
                }
                catch (Exception ex)
                {
                    FillteredInvoiceBills.Clear();
                    NoItemFoundTextVisibility = Visibility.Visible;
                }

            }
            else
            {
                try
                {
                    if(Invoices.Any(ib=>ib.InvoiceId == int.Parse(searchId) && ib.DateTime.Date== FillteredDate.Date))
                    {
                        FillteredInvoiceBills = new ObservableCollection<InvoiceBill>(Invoices.Where(ib => ib.InvoiceId == int.Parse(searchId) && ib.DateTime.Date== FillteredDate.Date).ToList());
                    }
                    else
                    {
                        FillteredInvoiceBills.Clear();
                        NoItemFoundTextVisibility = Visibility.Visible;
                    }
                }catch(Exception ex)
                {
                    FillteredInvoiceBills.Clear();
                    NoItemFoundTextVisibility = Visibility.Visible;
                }
            }
            CalculateInvoiceListDetails();
        }

        //When filltered date selected, fillter out the invoices which created that date
        partial void OnFillteredDateChanged(DateTime fillteredDate)
        {
            if(fillteredDate != DateTime.MinValue.Date)
            {
                FillteredInvoiceBills = new ObservableCollection<InvoiceBill>(Invoices.Where(ib=>ib.DateTime.Date == fillteredDate.Date).ToList());
                SearchId = string.Empty;
            }
            CalculateInvoiceListDetails();
        }


    }
}
