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
    public class FoundTreasureArgs : ViewModelBase
    {

        private string name;
        private string description;
        public FoundTreasureArgs(Location prevTresLocation,int TreasID)
        {
            FoundTreasureLocation = prevTresLocation;
            FoundTreasureId = TreasID;
            using (var unitOfWork = new UnitOfWork(new GeocachingContext()))
            {
                Name = unitOfWork.Treasures.Get(FoundTreasureId).Name;
                Description = unitOfWork.Treasures.Get(FoundTreasureId).Description;
            }
        }

        public Location FoundTreasureLocation { get; set; }
        public int FoundTreasureId { get; set; }
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
