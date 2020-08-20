using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Geocache.Database;
using Geocache.Helper;
using Geocache.Models;
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
    public class ModerateAccountsVM: ViewModelBase
    {
        public ModerateAccountsVM(UserDataService userdata)
        {
            UserData = userdata;
            using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
            {
                Users = new ObservableCollection<User>(unitOfWork.Users.GetUsers(UserData.CurrentUser.ID));
            }
        }

        public UserDataService UserData { get; set; }
        private ObservableCollection<User> users;
        public ObservableCollection<User> Users { get => users; set => users = value; }

        #region Commands
        private ICommand banUser;
        private ICommand unbanUser;
        private ICommand deleteUser;
        private ICommand closeWindow;
        public ICommand BanUser
        {
            get
            {
                return banUser ?? (banUser = new RelayCommand<User>(x =>
                {
                    using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                    {
                        var user = unitOfWork.Users.Get(x.ID);
                        user.isBanned = true;
                        unitOfWork.Complete();
                        Users[Users.IndexOf(x)].isBanned = true;
                    }
                }));
            }
        }
        public ICommand UnbanUser
        {
            get
            {
                return unbanUser ?? (unbanUser = new RelayCommand<User>(x =>
                {
                    using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                    {
                        var user = unitOfWork.Users.Get(x.ID);
                        user.isBanned = false;
                        unitOfWork.Complete();
                        Users[Users.IndexOf(x)].isBanned = false;
                    }
                }));
            }
        }
        public ICommand DeleteUser
        {
            get
            {
                return deleteUser ?? (deleteUser = new RelayCommand<User>(x =>
                {
                    using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                    {
                        //delete all found treasures
                        unitOfWork.FoundTreasures.RemoveRange(unitOfWork.FoundTreasures.Find(ft => ft.UserID == x.ID));
                        //delete all user comments
                        foreach (Treasures_Comments tc in x.Treasures_Comments.ToList())
                        {
                           
                            var comment = unitOfWork.TreasureComments.Get(tc.ID);
                            unitOfWork.TreasureComments.Remove_Quicker(comment); //dont get it but EF demands it
                            unitOfWork.Complete();
                            
                        }
                        //delete all user treasures
                        foreach(Treasure treasure in unitOfWork.Treasures.GetUserTreasures(x.ID))
                        {
                            if (treasure.Chained_Treasure1.Count > 0)
                            {
                                Chained_Treasures ct1 = treasure.Chained_Treasure1.First();
                                if ((ct1 = unitOfWork.ChainedTreasures.SingleOrDefault(ct => ct.Treasure1_ID == treasure.ID)) != null)
                                {
                                    unitOfWork.ChainedTreasures.Remove_Quicker(ct1);

                                    unitOfWork.Complete();
                                }
                            }
                            if (treasure.Chained_Treasure2.Count > 0)
                            {
                                foreach(var tc in treasure.Chained_Treasure2)
                                {
                                    Chained_Treasures ct2;
                                    if ((ct2 = unitOfWork.ChainedTreasures.SingleOrDefault(ct => ct.Id == tc.Id)) != null)
                                    {
                                        int fixId = (int)ct2.Treasure1_ID;
                                        unitOfWork.ChainedTreasures.Remove_Quicker(ct2);
                                        unitOfWork.ChainedTreasures.UnchainTreasure(fixId); //unchain the connected treasure
                                        unitOfWork.Complete();
                                    }
                                }
                            }
                            unitOfWork.Markers.Remove_Quicker(treasure.MarkerInfo);
                            unitOfWork.Complete();
                            unitOfWork.Treasures.Remove_Quicker(treasure);
                            unitOfWork.Complete();
                        }

                        var user = unitOfWork.Users.Get(x.ID);
                        unitOfWork.Users.Remove_Quicker(user);
                        unitOfWork.Complete();

                        Users.Remove(x);
                        RaisePropertyChanged("Users");
                        MessageBox.Show(string.Format("User [{0}] has been removed.", x.Username));
                        MessengerInstance.Send<object>(new object(), "Refresh"); //update leaderboard and user treasures
                    }
                }));
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
        #endregion
    }
}
