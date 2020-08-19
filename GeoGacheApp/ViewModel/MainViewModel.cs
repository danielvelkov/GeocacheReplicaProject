using CommonServiceLocator;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using System;
using System.Diagnostics;

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
                    _currentContent = SimpleIoc.Default.GetInstance<LoginPageVM>();
                }
                return _currentContent;
            }
            set
            {
                _currentContent = value;
                RaisePropertyChanged("CurrentContent");
            }
        }

        #endregion

        #region methods

        void ChangeContent(Type ViewModelPage)
        {
            CurrentContent = SimpleIoc.Default.GetInstance(ViewModelPage);
        }

        #endregion

    }
}