using GalaSoft.MvvmLight.Ioc;
using Geocache.Models;
using Geocache.Views.PopUpViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Geocache.Helper
{
    public class FoundTreasureWindowController 
    {
        /// <summary>
        /// Show FoundTreasureWindow
        /// </summary>
        public bool? ShowPopUp(FoundTreasureArgs args)
        {
            if (SimpleIoc.Default.ContainsCreated<FoundTreasureArgs>())
            {
                 
            }
            else
            SimpleIoc.Default.Register(() => args);

            TreasureFoundView view = new TreasureFoundView();
            return view.ShowDialog();
        }
    }

    public class FoundTreasureArgs
    {
        public Location FoundTreasureLocation { get; set; }
        public int FoundTreasureId { get; set; }

    }
}
