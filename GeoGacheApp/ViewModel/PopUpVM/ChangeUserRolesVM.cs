using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Geocache.Database;
using Geocache.Enums;
using Geocache.Helper;
using Geocache.Models;
using Geocache.Models.WrappedModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Geocache.ViewModel.PopUpVM
{
    public class ChangeUserRolesVM: ViewModelBase
    {
        public ChangeUserRolesVM(UserDataService userdata)
        {
            Userdata = userdata;
            RefreshUserList();
            MessengerInstance.Register<object>(this, "Refresh", obj => { RefreshUserList(); });
        }

        public UserDataService Userdata { get;  set; }
        private ObservableCollection<UserChangedRole> users;
        public ObservableCollection<UserChangedRole> Users
        {
            get
            {
                return users;
            }
            set
            {
                users = value;
                RaisePropertyChanged("Users");
            }
        }

        #region Commands

        private ICommand closeWindow;
        private ICommand saveChanges;
        private ICommand findUser;
        private ICommand getUsers;

        public ICommand FindUser
        {
            get
            {
                if (findUser == null)
                    findUser = new RelayCommand<string>(username =>
                    {
                        RefreshUserList();
                        Users = new ObservableCollection<UserChangedRole>(Users.Where
                             (i => i.User.Username.Contains(username, StringComparison.OrdinalIgnoreCase) == true));
                    });
                return findUser;
            }
        }

        public ICommand GetUsers
        {
            get
            {
                if (getUsers == null)
                    getUsers = new RelayCommand(RefreshUserList);
                return getUsers;
            }
        }

        public ICommand CloseWindow
        {
            get
            {
                if (closeWindow == null)
                    closeWindow = new RelayCommand(() =>
                    {
                        Messenger.Default.Send(new CloseWindowEventArgs());
                    });
                return closeWindow;
            }
        }

        public ICommand SaveChanges
        {
            get
            {
                if (saveChanges == null)
                    saveChanges = new RelayCommand<UserChangedRole>(x =>
                    {
                        using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                        {
                            var user=unitOfWork.Users.Get(x.User.ID);
                            user.Role = (Roles)Convert.ChangeType(x.UserRole, x.UserRole.GetTypeCode());
                            unitOfWork.Complete();
                            MessageBox.Show(string.Format("User promoted to {0}.", x.UserRole));
                        }
                    });
                return saveChanges;
            }
        }
        #endregion
        private void RefreshUserList()
        {
            using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
            {
                Users = new ObservableCollection<UserChangedRole>();
                foreach (User user in unitOfWork.Users.Find(u => u.ID != Userdata.CurrentUser.ID).ToList())
                {
                    user.Points = unitOfWork.FoundTreasures.GetUserPoints(user.ID);
                    Users.Add(new UserChangedRole
                    {
                        User = user,
                        UserRole = (UserRoles)Convert.ChangeType(user.Role, user.Role.GetTypeCode())
                    });
                };
            }
        }
    }
}