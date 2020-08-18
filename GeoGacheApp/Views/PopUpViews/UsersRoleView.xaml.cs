using GalaSoft.MvvmLight.Messaging;
using Geocache.Helper;
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
    /// Interaction logic for UsersRoleView.xaml
    /// </summary>
    public partial class UsersRoleView : Window
    {
        public UsersRoleView()
        {
            InitializeComponent();
            Unloaded += UsersRoleView_Unloaded;
            Messenger.Default.Register<CloseWindowEventArgs>(this, CloseWindow);
        }

        private void UsersRoleView_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<CloseWindowEventArgs>(this);
        }

        private void CloseWindow(CloseWindowEventArgs obj)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
