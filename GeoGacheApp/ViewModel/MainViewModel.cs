using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;

namespace Geocache.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            MessengerInstance.Register<Type>(this, "ChangePage", ViewModel => { ChangeContent(ViewModel); });
        }

        #region variables
        dynamic _currentContent;
        #endregion

        #region parameters

        public dynamic CurrentContent
        {
            get
            {
                if (_currentContent == null)
                {
                    _currentContent = new LoginPageVM();
                }
                return _currentContent;
            }
            set
            {
                if (_currentContent == value)
                    return;
                _currentContent = value;
                RaisePropertyChanged("CurrentContent");
            }
        }

        #endregion
        

        #region commands
        
        #endregion

        #region methods
        // basically the old way of doing it, ViewModelLocator is not used at alll
        void ChangeContent(Type ViewModel)
        {
            CurrentContent = SimpleIoc.Default.GetInstance(ViewModel,ViewModel.GUID.ToString());
        }

        #endregion

    }
}