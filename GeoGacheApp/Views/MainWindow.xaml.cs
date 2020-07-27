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
using GeoGacheApp.Models;
using GeoGacheApp.Views;

namespace GeoGacheApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Page
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginBtnClick(object sender, RoutedEventArgs e)
        {
            LoginValidation.ActionOnError act = ShowError;
            User user = new User
            {
                Username = userNametxtBox.Text,
                Password = passWordBox.Password
            };
            LoginValidation lv = new LoginValidation(userNametxtBox.Text,passWordBox.Password,act);
            if(lv.ValidateUserInput(ref user))
            {
                //HomepageView hp = new HomepageView();
                //hp.DataContext = user;
                //hp.greetingTxtBlock.Text = "welcome back, " + user.FirstName + "!";
                //hp.Currentuser = user;
                //foreach (UIElement b in hp.buttonPanel.Children)
                //{
                //    if (b is Button)
                //        b.IsEnabled = false;
                //}
                //switch (user.Role)
                //{
                //    case UserRoles.USER:
                //        {
                //            hp.findTreasureBttn.IsEnabled = true;
                //            hp.hideTreasureBttn.IsEnabled = true;
                //            hp.leaderboardsBttn.IsEnabled = true;
                //            hp.myTreasuresBttn.IsEnabled = true;
                //            break;
                //        }
                //    case UserRoles.MOD:
                //        {
                //            hp.moderateBttn.IsEnabled = true;
                //            break;
                //        }
                //    case UserRoles.ADMIN:
                //        {
                //            hp.deleteBttn.IsEnabled = true;
                //            hp.changeRoleBttn.IsEnabled = true;
                //            break;
                //        }
                //}

                //hp.Show();
                //this.Close();


                HomePage hp = new HomePage(user)
                {
                    DataContext = user
                };
                hp.greetingTxtBlock.Text = "welcome back, " + user.FirstName + "!";
                hp.Currentuser = user;
                foreach (UIElement b in hp.buttonPanel.Children)
                {
                    if (b is Button)
                        b.IsEnabled = false;
                }
                switch (user.Role)
                {
                    case UserRoles.USER:
                        {
                            hp.findTreasureBttn.IsEnabled = true;
                            hp.hideTreasureBttn.IsEnabled = true;
                            hp.leaderboardsBttn.IsEnabled = true;
                            hp.myTreasuresBttn.IsEnabled = true;
                            break;
                        }
                    case UserRoles.MOD:
                        {
                            hp.moderateBttn.IsEnabled = true;
                            break;
                        }
                    case UserRoles.ADMIN:
                        {
                            hp.deleteBttn.IsEnabled = true;
                            hp.changeRoleBttn.IsEnabled = true;
                            break;
                        }
                }
                //this.Close();
                this.NavigationService.Navigate(hp);
               
                

            }
            else { }
            
        }

        private void ShowError(string msg)
        {
            this.errLabel.Text = ("*" + msg + ".");
        }

        private void RegisterOptionClick(object sender, RoutedEventArgs e)
        {
            RegistrationScreen sc = new RegistrationScreen();
            this.NavigationService.Navigate(sc);
            
        }
        
    }
}
