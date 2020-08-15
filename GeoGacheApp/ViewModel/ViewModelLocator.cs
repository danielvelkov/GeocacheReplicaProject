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
        }
        
        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }

        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
        }

        /// <summary>
        /// Gets the LoginPageVM property.
        /// </summary>
        //public static LoginPageVM LoginPageVM
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<LoginPageVM>();
        //    }
        //}
        //public static MainViewModel MainViewModel
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<MainViewModel>();
        //    }
        //}
        //public static HomePageVM HomePageVM
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<HomePageVM>();
        //    }
        //}
        //public static RegisterPageVM RegisterPageVM
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<RegisterPageVM>();
        //    }
        //}
        //public static HomePageBrowserVM HomePageBrowserVM
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<HomePageBrowserVM>();
        //    }
        //}
        //public static UserPageVM UserPageVM
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<UserPageVM>();
        //    }
        //}
        //public static HideTreasurePageVM HideTreasurePageVM
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<HideTreasurePageVM>();
        //    }
        //}
        //public static FindTreasureVM FindTreasureVM
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<FindTreasureVM>();
        //    }
        //}
        //public static TreasureFoundVM TreasureFoundVM
        //{
        //    get
        //    {
        //        return ServiceLocator.Current.GetInstance<TreasureFoundVM>();
        //    }
        //}
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