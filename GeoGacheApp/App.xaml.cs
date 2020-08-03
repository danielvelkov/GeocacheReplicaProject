using GalaSoft.MvvmLight.Threading;
using Geocache.ViewModel;
using Geocache.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Geocache
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //protected override void OnStartup(StartupEventArgs e)
        //{

        //    // set it to the main window
        //    MainWindow view = new Views.MainWindow();
        //    //MainViewModel viewModel = new MainViewModel();
        //    //view.DataContext = viewModel;
        //    //view.Show();
        //    App.Current.MainWindow = view;
        //    view.Show();
        //    base.OnStartup(e);
        //}
        static App()
        {
            DispatcherHelper.Initialize();
        }
    }
}
