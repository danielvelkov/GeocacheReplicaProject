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
using System.Windows.Shapes;

namespace GeoGacheApp.Views
{
    /// <summary>
    /// Interaction logic for RegistrationScreen.xaml
    /// </summary>
    public partial class RegistrationScreen : Page
    {
        public RegistrationScreen()
        {
            InitializeComponent();
        }

        private void ReturnToLoginViewBttn(object sender, RoutedEventArgs e)
        {
            MainWindow nmw = new MainWindow();
            this.NavigationService.Navigate(nmw);
            
        }

        private void RegisterBttn(object sender, RoutedEventArgs e)
        {
            //TO DO: make a are you sure button
            // make a double check if the username is taken
            // check if the password is the right one as the confirm password
            // 
            RegisterValidation.ActionOnError act = ShowError;
            RegisterValidation rv = new RegisterValidation(
                userNameTxtBox.Text, 
                passWordBox.Password, 
                confirmPassWordBox.Password,
                act);
            if (rv.ValidateRegisterData())
            {
                if(MessageBox.Show("are you sure everything is correct?", "confirm changes", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    UserContext ctx = new  UserContext();
                    ctx.Users.Add(
                        new User(
                            userNameTxtBox.Text,
                            passWordBox.Password,
                            UserRoles.USER,
                            false,
                            0,
                            DateTime.Now,
                            firstNameTxtBox.Text,
                            lastNameTxtBox.Text,
                            countryTxtBox.Text,
                            cityTxtBox.Text,
                            adressTxtBox.Text));
                    ctx.SaveChanges();

                    MainWindow mw = new MainWindow();
                    this.NavigationService.Navigate(mw);
                }
                else { }
            }
        }

        private void ShowError(string msg)
        {
            MessageBox.Show(msg,"error", MessageBoxButton.OK);
        }

    }
}
