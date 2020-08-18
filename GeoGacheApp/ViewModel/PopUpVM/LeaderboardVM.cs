using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using Geocache.Database;
using Geocache.Helper;
using Geocache.Models;
using Geocache.Models.WrappedModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Geocache.ViewModel.PopUpVM
{
    public class LeaderboardVM: ViewModelBase
    {
        ObservableCollection<UserRanking> leaderboard;

        public UserDataService UserData { get; private set; }
        public ObservableCollection<UserRanking> Leaderboard { get => leaderboard; set => leaderboard = value; }

        public LeaderboardVM(UserDataService userData)
        {
            UserData = userData;
            Leaderboard = new ObservableCollection<UserRanking>();
            using(var unitOfWork= new UnitOfWork(new GeocachingContext()))
            {
                var users = unitOfWork.Users.GetAll();
                foreach(User user in users)
                {
                    int points=unitOfWork.FoundTreasures.GetUserPoints(user.ID);
                    int hiddenTreasures = unitOfWork.Treasures.GetUserHiddenTreasuresCount(user.ID);
                    int foundTreasures = unitOfWork.FoundTreasures.GetUserFoundTreasuresCount(user.ID);
                    
                    Leaderboard.Add(new UserRanking { Points = points,
                        UserName = user.Username,
                        FoundTreasures=foundTreasures,
                        HiddenTreasures=hiddenTreasures});
                }
                Leaderboard = new ObservableCollection<UserRanking>
                    (Leaderboard.OrderByDescending<UserRanking,int>(t=>t.Points).AsEnumerable());

                int rank = 1;
                foreach (UserRanking ur in Leaderboard)
                {
                    ur.Rank = rank++;
                }
            }
        }

        private ICommand closeWindow;
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
    }
}
