using GalaSoft.MvvmLight;
using Geocache.Database;
using Geocache.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Models.WrappedModels
{
    public class SearchedTreasureArgs : ViewModelBase
    {
        private string name;
        private string description;
        public SearchedTreasureArgs(Location prevTresLocation,int TreasID)
        {
            SearchedTreasureLocation = prevTresLocation;
            SearchedTreasureID = TreasID;
            using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
            {
                Name = unitOfWork.Treasures.Get(SearchedTreasureID).Name;
                Description = unitOfWork.Treasures.Get(SearchedTreasureID).Description;
            }
        }

        public Location SearchedTreasureLocation { get; set; }
        public int SearchedTreasureID { get; set; }
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
                RaisePropertyChanged("Description");
            }
        }
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

    }
}
