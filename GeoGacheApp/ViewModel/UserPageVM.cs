using GalaSoft.MvvmLight;
using Geocache.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.ViewModel
{
    public class UserPageVM : ViewModelBase
    {
        public UserPageVM(UserDataService userData)
        {
            UserData = userData;
        }

        public UserDataService UserData { get; }
    }
}
