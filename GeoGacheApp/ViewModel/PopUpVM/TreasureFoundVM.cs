using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Geocache.Database;
using Geocache.Enums;
using Geocache.Helper;
using Geocache.Models;
using Geocache.Models.WrappedModels;
using Geocache.Views.PopUpViews;
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
    public class TreasureFoundVM: ViewModelBase
    {
        public TreasureFoundVM(UserDataService userdata,SearchedTreasureArgs treasureArgs)
        {
            Userdata = userdata;
            using(var unitOfWork= new UnitOfWork(new GeocachingContext()))
            {
                Treasure = unitOfWork.Treasures.Get(treasureArgs.SearchedTreasureID);
            }
            TreasureLocation = treasureArgs.SearchedTreasureLocation;
        }

        public UserDataService Userdata { get;  set; }
        public Treasure Treasure { get;  set; }
        public Location TreasureLocation { get;  set; }

        #region Commands

        private ICommand submitKey;
        private ICommand closeWindow;
        private ICommand reportWrongKey;
        public ICommand SubmitKey
        {
            get
            {
                if (submitKey == null)
                    submitKey = new RelayCommand<string>(x =>
                    {
                        if (Treasure.Key == x)
                        {
                            using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
                            {
                                if(Userdata.CurrentUser.ID==Treasure.UserId)
                                {
                                    MessageBox.Show(String.Format("Congrats! you found the treasure \n Points:9999"));
                                    MessageBox.Show(String.Format("just kidding. you hid this treasure :)"));
                                    return;
                                }
                                // checks if user found the treasure
                                if (!unitOfWork.FoundTreasures.HasUserFoundTreasure(Userdata.CurrentUser.ID,
                                    Treasure.ID))
                                {
                                    int points = CalculatePoints(Treasure.TreasureSize, Treasure.TreasureType);
                                    unitOfWork.FoundTreasures.Add(new Found_Treasures(
                                        Userdata.CurrentUser.ID,
                                        Treasure.ID,
                                        points,
                                        DateTime.Now));
                                    unitOfWork.Complete();
                                    MessageBox.Show(String.Format("Congrats! you found the treasure \n Points:{0}", points));
                                    MessengerInstance.Send(new object(), "Refresh");
                                    if (Treasure.IsChained)
                                    {
                                        var result = MessageBox.Show("This treasure is connected to another one! \n" +
                                            "Would you like to go on another adventure?", "New Treasure ", MessageBoxButton.YesNo);
                                        if (result == MessageBoxResult.Yes)
                                        {
                                            var NextTreasure = unitOfWork.FoundTreasures.GetNextTreasure(Treasure.ID);
                                            if (!unitOfWork.FoundTreasures.HasUserFoundTreasure(Userdata.CurrentUser.ID, NextTreasure.ID))
                                            {
                                                //set the location of the user to the location of the previous treasure
                                                SimpleIoc.Default.GetInstance<FindTreasureVM>().UserData.UserLocation = TreasureLocation;

                                                SimpleIoc.Default.GetInstance<FindTreasureVM>().TreasureArgs.SearchedTreasureID = NextTreasure.ID;
                                                SimpleIoc.Default.GetInstance<FindTreasureVM>().TreasureArgs.SearchedTreasureLocation = NextTreasure.GetLatLng();
                                                SimpleIoc.Default.GetInstance<FindTreasureVM>().TreasureArgs.Description = NextTreasure.Description;
                                                SimpleIoc.Default.GetInstance<FindTreasureVM>().TreasureArgs.Name = NextTreasure.Name;
                                                SimpleIoc.Default.GetInstance<FindTreasureVM>().TreasureComments = new ObservableCollection<Treasures_Comments>
                                                (
                                                    unitOfWork.TreasureComments.Find(c => c.TreasureID == NextTreasure.ID)
                                                );
                                                //set the treasure to this one
                                                Treasure = NextTreasure;
                                                SimpleIoc.Default.GetInstance<FindTreasureVM>().ShowRoute.Execute(null);
                                                this.CloseWindow.Execute(null);
                                                return;
                                            }
                                            else
                                            {
                                                MessageBox.Show("You have already found that one.");

                                            }
                                        }
                                    }
                                    this.CloseWindow.Execute(null);
                                }
                                else MessageBox.Show("You have already found that one");
                            }
                        }
                        else MessageBox.Show("Provided key is wrong.");
                    });
                return submitKey;
            }
        }
        public ICommand ReportWrongKey
        {
            get
            {
                if (reportWrongKey == null)
                    reportWrongKey = new RelayCommand<string>(x =>
                    {
                        using(var unitOfWork=new UnitOfWork(new GeocachingContext()))
                        {
                            if(!unitOfWork.TreasureComments.HasUserReportedTreasure(
                                Userdata.CurrentUser.ID, Treasure.ID))
                            {
                                var report = new Treasures_Comments(
                                    Treasure.ID,
                                    Userdata.CurrentUser.ID,
                                    "User provided key at the geocache is wrong",
                                    DateTime.Now,
                                    CommentType.REPORT,
                                    0);
                                unitOfWork.TreasureComments.Add(report);
                                unitOfWork.Complete();
                                MessageBox.Show(string.Format("Report for geocache ({0}) filed.\nMods will check it out.", Treasure.Name));
                            }
                            else
                            {
                                MessageBox.Show("you have already reported it");
                                
                            }
                            this.CloseWindow.Execute(null);
                        }
                    });
                return reportWrongKey;
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

        int CalculatePoints(TreasureSizes size,TreasureType type)
        {
            const int PointsMultiplier = 100;
            if (TreasureType.SURPRISE==type)
            {
                // when the treasure is a surpirse the user gets random points multiplier
                double[] surpriseMultipliers = new double[5] { 0.5, 2, 3, 4, 5 };
                Random rd = new Random();
                int randomIndex = rd.Next(0, 5);
                double randomNumber = surpriseMultipliers[randomIndex];
                return (int)(randomNumber * (int)size*PointsMultiplier);
            }
            else return PointsMultiplier * (int)size * (int)type;
        }
    }
}
