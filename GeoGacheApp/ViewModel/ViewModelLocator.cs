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
            
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<LoginPageVM>();
            SimpleIoc.Default.Register<HomePageBrowserVM>();
            SimpleIoc.Default.Register<HomePageVM>();
            SimpleIoc.Default.Register<HideTreasurePageVM>();
            SimpleIoc.Default.Register<UserPageVM>();
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }
        
        public LoginPageVM LoginPageVM => SimpleIoc.Default.GetInstance<LoginPageVM>(Guid.NewGuid().ToString());
        public MainViewModel MainViewModel => SimpleIoc.Default.GetInstance<MainViewModel>(Guid.NewGuid().ToString());
        public HomePageVM HomePageVM => SimpleIoc.Default.GetInstance<HomePageVM>(Guid.NewGuid().ToString());
        public RegisterPageVM RegisterPageVM => SimpleIoc.Default.GetInstance<RegisterPageVM>(Guid.NewGuid().ToString());
        public HomePageBrowserVM HomePageBrowserVM => SimpleIoc.Default.GetInstance<HomePageBrowserVM>(Guid.NewGuid().ToString());
        public UserPageVM UserPageVM => SimpleIoc.Default.GetInstance<UserPageVM>(Guid.NewGuid().ToString());
        public HideTreasurePageVM HideTreasurePageVM => SimpleIoc.Default.GetInstance<HideTreasurePageVM>(Guid.NewGuid().ToString());
        public FindTreasureVM FindTreasureVM => SimpleIoc.Default.GetInstance<FindTreasureVM>(Guid.NewGuid().ToString());
        public TreasureFoundVM TreasureFoundVM => SimpleIoc.Default.GetInstance<TreasureFoundVM>(Guid.NewGuid().ToString());
    }
}