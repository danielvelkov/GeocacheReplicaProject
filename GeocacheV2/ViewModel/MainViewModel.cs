using GalaSoft.MvvmLight;

namespace GeocacheV2.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            MessengerInstance.Register<ViewModelBase>(this, "ChangePage", ViewModel => { ChangeContent(ViewModel); });
        }

        #region variables
        ViewModelBase _currentContent;
        #endregion

        #region parameters

        public ViewModelBase CurrentContent
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
        void ChangeContent(ViewModelBase ViewModel)
        {
            CurrentContent = ViewModel;
        }

        #endregion

    }
}