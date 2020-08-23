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
            SimpleIoc.Default.Register<RegisterPageVM>();
            ReRegisterInstances();
        }
        //when we log out
        public static void Cleanup()
        {
            // Clear the ViewModels

            SimpleIoc.Default.Unregister<UserDataService>();
            SimpleIoc.Default.Unregister<HomePageBrowserVM>();
            SimpleIoc.Default.Unregister<HomePageVM>();
            SimpleIoc.Default.Unregister<UserPageVM>();
            SimpleIoc.Default.Unregister<HideTreasurePageVM>();
            SimpleIoc.Default.Unregister<FindTreasureVM>();
            SimpleIoc.Default.Unregister<TreasureFoundVM>();
            SimpleIoc.Default.Unregister<UserTreasuresVM>();
            SimpleIoc.Default.Unregister<LeaderboardVM>();

            SimpleIoc.Default.Unregister<ModerateTreasuresVM>();
            SimpleIoc.Default.Unregister<ModerateAccountsVM>();

            SimpleIoc.Default.Unregister<ChangeUserRolesVM>();
        }
        // when we log in
        public static void ReRegisterInstances()
        {
            SimpleIoc.Default.Register<HomePageBrowserVM>();
            SimpleIoc.Default.Register<HomePageVM>();
            SimpleIoc.Default.Register<UserPageVM>();
            SimpleIoc.Default.Register<HideTreasurePageVM>();
            SimpleIoc.Default.Register<UserTreasuresVM>();
            SimpleIoc.Default.Register<LeaderboardVM>();
            SimpleIoc.Default.Register<FindTreasureVM>();
            SimpleIoc.Default.Register<TreasureFoundVM>();

            SimpleIoc.Default.Register<ModerateTreasuresVM>();
            SimpleIoc.Default.Register<ModerateAccountsVM>();

            SimpleIoc.Default.Register<ChangeUserRolesVM>();
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
        
        public ModerateTreasuresVM ModerateTreasuresVM => SimpleIoc.Default.GetInstance<ModerateTreasuresVM>();
        public ModerateAccountsVM ModerateAccountsVM => SimpleIoc.Default.GetInstance<ModerateAccountsVM>();

        public ChangeUserRolesVM UsersRoleVM => SimpleIoc.Default.GetInstance<ChangeUserRolesVM>();

    }
}