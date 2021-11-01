using HotelLibrary.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HotelLibrary.Models;
using Microsoft.Extensions.DependencyInjection; 


namespace HotelApp.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IDatabaseData db;
        
        
        public MainWindow(IDatabaseData db)
        {
            InitializeComponent();
            this.db = db;
        }

        private void SearchLastName_Click(object sender, RoutedEventArgs e)
        {
            List<BookingFullModel> bookings = db.SearchBookings(lastNameBox.Text);
            resultsList.ItemsSource = bookings; 
            lastNameBox.Text = ""; 
        }

        private void CheckInButton_Click(object sender, RoutedEventArgs e)
        {
            var checkInForm = App.serviceProvider.GetService<ConfirmationWindow>();
            var model = (BookingFullModel)((Button)e.Source).DataContext;
            checkInForm.PopulateCheckInInfo(model); 
            checkInForm.Show(); 
        }
    }
}

