/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:GeoGacheApp"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using Geocache.Helper;
using Geocache.ViewModel;
using Geocache.ViewModel.BrowserVM;
using Geocache.ViewModel.PopUpVM;
using System;

namespace Geocache.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application 
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<PopUpWindowController>();
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginPageVM>();
            SimpleIoc.Default.Register <RegisterPageVM>();
            SimpleIoc.Default.Register<HomePageBrowserVM>();
            SimpleIoc.Default.Register<HomePageVM>();
            SimpleIoc.Default.Register<HideTreasurePageVM>();
            SimpleIoc.Default.Register<UserPageVM>();
            SimpleIoc.Default.Register<LeaderboardVM>();
            SimpleIoc.Default.Register<UserTreasuresVM>();
            SimpleIoc.Default.Register<UsersRoleVM>();
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }
        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>();
        public HomePageBrowserVM HomePageBrowserVM => SimpleIoc.Default.GetInstance<HomePageBrowserVM>();
        public TreasureFoundVM TreasureFoundVM => SimpleIoc.Default.GetInstance<TreasureFoundVM>();
        public LeaderboardVM LeaderboardVM => SimpleIoc.Default.GetInstance<LeaderboardVM>();
        public UserTreasuresVM UserTreasuresVM => SimpleIoc.Default.GetInstance<UserTreasuresVM>();
        public UsersRoleVM UsersRoleVM => SimpleIoc.Default.GetInstance<UsersRoleVM>();
    }
}