/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:GeocacheV2"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using GeocacheV2.ViewModel.BrowserVM;

namespace GeocacheV2.ViewModel
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
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
            //SimpleIoc.Default.Register<RegisterPageVM>();
            //SimpleIoc.Default.Register<HideTreasurePageVM>();
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
        public static LoginPageVM LoginPageVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<LoginPageVM>();
            }
        }
        public static MainViewModel MainViewModel
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainViewModel>();
            }
        }
        public static HomePageVM HomePageVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomePageVM>();
            }
        }
        public static RegisterPageVM RegisterPageVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<RegisterPageVM>();
            }
        }
        public static HomePageBrowserVM HomePageBrowserVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HomePageBrowserVM>();
            }
        }
        public static UserPageVM UserPageVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<UserPageVM>();
            }
        }
        public static HideTreasurePageVM HideTreasurePageVM
        {
            get
            {
                return ServiceLocator.Current.GetInstance<HideTreasurePageVM>();
            }
        }
    }
}