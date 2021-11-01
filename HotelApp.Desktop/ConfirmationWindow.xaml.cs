using HotelLibrary.Data;
using HotelLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace HotelApp.Desktop
{
    /// <summary>
    /// Interaction logic for ConfirmationWindow.xaml
    /// </summary>
    public partial class ConfirmationWindow : Window
    {
        private readonly IDatabaseData db;
        private BookingFullModel data = null; 

        public ConfirmationWindow(IDatabaseData db) 
        {
            InitializeComponent();
            this.db = db; 
        }

        public void PopulateCheckInInfo(BookingFullModel data)
        {
            this.data = data;
            firstNameText.Text = data.FirstName;
            lastNameText.Text = data.LastName;
            titleText.Text = data.Title;
            roomNumberText.Text = data.RoomNumber;
            totalCostText.Text = string.Format("{0:C}", data.TotalCost); 
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            db.CheckInGuest(data.Id);
            this.Close(); 
        }
    }
}
