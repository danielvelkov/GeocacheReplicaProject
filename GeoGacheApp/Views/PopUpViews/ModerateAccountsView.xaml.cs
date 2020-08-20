using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Geocache.Helper;
using Geocache.ViewModel.PopUpVM;
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

namespace Geocache.Views.PopUpViews
{
    /// <summary>
    /// Interaction logic for ModerateAccountsView.xaml
    /// </summary>
    public partial class ModerateAccountsView : Window
    {
        public ModerateAccountsView()
        {
            InitializeComponent();
            Unloaded += ModerateAccountsView_Unloaded; ; ;
            Messenger.Default.Register<CloseWindowEventArgs>(this, CloseWindow);
        }

        private void ModerateAccountsView_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<CloseWindowEventArgs>(this);
        }
        
        private void CloseWindow(CloseWindowEventArgs obj)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ChangeButton(object sender, RoutedEventArgs e)
        {
            Button b = e.Source as Button;
            
            if (b.Content.ToString() == "Ban")
            {
                b.Command = (this.DataContext as ModerateAccountsVM).BanUser;
                b.Command.Execute((e.Source as Button).CommandParameter);
                b.Content = "Unban";
            }
            else if (b.Content.ToString() == "Unban")
            {
                b.Command = (this.DataContext as ModerateAccountsVM).UnbanUser;
                b.Command.Execute((e.Source as Button).CommandParameter);
                b.Content = "Ban";
            }
            e.Handled=true;
        }
    }
}
