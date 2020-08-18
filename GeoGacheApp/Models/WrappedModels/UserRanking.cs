using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Models.WrappedModels
{
    public class UserRanking
    {
        public int Rank { get; set; }
        public int Points { get; set; }
        public string UserName { get; set; }
        public int FoundTreasures { get; set; }
        public int HiddenTreasures { get; set; }
    }

    // old
    //public class UserRanking : INotifyPropertyChanged
    //{
    //    public event PropertyChangedEventHandler PropertyChanged;

    //    int rank;
    //    int points;
    //    string userName;

    //    public int Rank
    //    {
    //        get { return rank; }
    //        set
    //        {
    //            rank = value;
    //            OnPropertyChanged();
    //        }
    //    }

    //    public int Points
    //    {
    //        get { return points; }
    //        set
    //        {
    //            points = value;
    //            OnPropertyChanged();
    //        }
    //    }

    //    public string UserName
    //    {
    //        get { return userName; }
    //        set
    //        {
    //            userName = value;
    //            OnPropertyChanged();
    //        }
    //    }

    //    // Create the OnPropertyChanged method to raise the event
    //    // The calling member's name will be used as the parameter.
    //    protected void OnPropertyChanged([CallerMemberName] string name = null)
    //    {
    //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    //    }
    //}
}
