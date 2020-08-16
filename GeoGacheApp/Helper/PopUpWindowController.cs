using GalaSoft.MvvmLight.Ioc;
using Geocache.Models;
using Geocache.Views.PopUpViews;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Geocache.Helper
{
    public class PopUpWindowController 
    {
        /// <summary>
        /// Show FoundTreasureWindow
        /// </summary>
        public bool? ShowPopUp(Window popUpWindow)
        {
            return popUpWindow.ShowDialog();
        }
    }

    public class FoundTreasureArgs
    {
        public Location FoundTreasureLocation { get; set; }
        public int FoundTreasureId { get; set; }

    }

    public class UserRanking : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        int rank;
        int points;
        string userName;

        public int Rank
        {
            get { return rank; }
            set
            {
                rank = value;
                OnPropertyChanged();
            }
        }

        public int Points
        {
            get { return points; }
            set
            {
                points = value;
                OnPropertyChanged();
            }
        }

        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                OnPropertyChanged();
            }
        }

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
