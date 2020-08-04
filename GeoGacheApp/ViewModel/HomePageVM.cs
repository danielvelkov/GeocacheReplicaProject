using GalaSoft.MvvmLight;
using Geocache.Enums;
using Geocache.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.ViewModel
{
    public class HomePageVM : ViewModelBase
    {
        public HomePageVM(UserDataService userData)
        {
            UserData = userData;
        }

        #region fields

        #endregion

        #region Parameters
        public UserDataService UserData { get; }

        //welcome message
        public string Welcome
        {
            get
            {
                return "Welcome " + UserData.GetUser().FirstName;
            }
        }

        

        #endregion

        #region Commands

        #endregion

    }
}
