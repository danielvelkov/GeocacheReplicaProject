using GalaSoft.MvvmLight.Messaging;
using Geocache.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Geocache.Views.PopUpViews
{
    /// <summary>
    /// Interaction logic for LeaderboardView.xaml
    /// </summary>
    public partial class LeaderboardView : Window
    {
        public bool sortedByRank = false;
        bool sortedByName = true;
        bool sortedByPoints = false;
        bool sortedByFoundTreasures = false;
        bool sortedByTreasuresHidden= false;
        bool sortedByDateJoined = false;

        public LeaderboardView()
        {
            
            InitializeComponent();
            Unloaded += LeaderboardView_Unloaded; ;
            Messenger.Default.Register<CloseWindowEventArgs>(this, CloseWindow);
        }

        private void LeaderboardView_Unloaded(object sender, RoutedEventArgs e)
        {
            Messenger.Default.Unregister<CloseWindowEventArgs>(this);
        }

        private void CloseWindow(CloseWindowEventArgs obj)
        {
            this.DialogResult = true;
            this.Close();
        }

        //not a good way to sort, couldve been easier in vm but the view should handle sorting

        private void SortByRank(object sender, RoutedEventArgs e)
        {
            //check if any sorting is applied, if it is remove it
            if (leaderboardList.Items.SortDescriptions.Count != 0)
                leaderboardList.Items.SortDescriptions.Clear();
            if (sortedByRank= (!sortedByRank))
            leaderboardList.Items.SortDescriptions.Add(
                new SortDescription("Rank", ListSortDirection.Descending));
            else leaderboardList.Items.SortDescriptions.Add(
                new SortDescription("Rank", ListSortDirection.Ascending));
            
        }

        private void SortByName(object sender, RoutedEventArgs e)
        {
            if (leaderboardList.Items.SortDescriptions.Count != 0)
                leaderboardList.Items.SortDescriptions.Clear();
            if (sortedByName = (!sortedByName))
                leaderboardList.Items.SortDescriptions.Add(
                    new SortDescription("UserName", ListSortDirection.Descending));
            else leaderboardList.Items.SortDescriptions.Add(
                new SortDescription("UserName", ListSortDirection.Ascending));
        }
        private void SortByPoints(object sender, RoutedEventArgs e)
        {
            if(leaderboardList.Items.SortDescriptions.Count !=0)
                leaderboardList.Items.SortDescriptions.Clear();
            if (sortedByPoints = (!sortedByPoints))
                leaderboardList.Items.SortDescriptions.Add(
                    new SortDescription("Points", ListSortDirection.Descending));
            else leaderboardList.Items.SortDescriptions.Add(
                new SortDescription("Points", ListSortDirection.Ascending));
        }
        private void SortByTreasuresHidden(object sender, RoutedEventArgs e)
        {
            if (leaderboardList.Items.SortDescriptions.Count != 0)
                leaderboardList.Items.SortDescriptions.Clear();
            if (sortedByTreasuresHidden = (!sortedByTreasuresHidden))
                leaderboardList.Items.SortDescriptions.Add(
                    new SortDescription("HiddenTreasures", ListSortDirection.Descending));
            else leaderboardList.Items.SortDescriptions.Add(
                new SortDescription("HiddenTreasures", ListSortDirection.Ascending));
        }
        private void SortByFoundTreasures(object sender, RoutedEventArgs e)
        {
            if (leaderboardList.Items.SortDescriptions.Count != 0)
                leaderboardList.Items.SortDescriptions.Clear();
            if (sortedByFoundTreasures = (!sortedByFoundTreasures))
                leaderboardList.Items.SortDescriptions.Add(
                    new SortDescription("FoundTreasures", ListSortDirection.Descending));
            else leaderboardList.Items.SortDescriptions.Add(
                new SortDescription("FoundTreasures", ListSortDirection.Ascending));
        }
        private void SortByDateJoined(object sender, RoutedEventArgs e)
        {
            if (leaderboardList.Items.SortDescriptions.Count != 0)
                leaderboardList.Items.SortDescriptions.Clear();
            if (sortedByDateJoined = (!sortedByDateJoined))
                leaderboardList.Items.SortDescriptions.Add(
                    new SortDescription("Joined", ListSortDirection.Descending));
            else leaderboardList.Items.SortDescriptions.Add(
                new SortDescription("Joined", ListSortDirection.Ascending));
        }

    }
}
