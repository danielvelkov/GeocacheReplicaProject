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

namespace GeoGacheApp.Views
{
    /// <summary>
    /// Interaction logic for UserData.xaml
    /// </summary>
    public partial class ManageUserData : Page
    {
        private User currentuser;

        public ManageUserData(Object usr)
        {
            this.DataContext = usr;
            InitializeComponent();
        }

        public User Currentuser { get => currentuser; set => currentuser = value; }
        

        private void SaveChanges(object sender, RoutedEventArgs e)
        {
            Currentuser = this.DataContext as User;
            UserContext context = new UserContext();
            User usr = (from user in context.Users where user.ID == currentuser.ID select user).First();
            usr.FirstName = firstNameTxtBox.Text;
            usr.LastName = lastNameTxtBox.Text;
            usr.Adress = adressTxtBox.Text;
            usr.City = cityTxtBox.Text;
            usr.Country = countryTxtBox.Text;
            context.SaveChanges();
            this.NavigationService.Navigate(new HomePage(usr));

        }
    
}
}
