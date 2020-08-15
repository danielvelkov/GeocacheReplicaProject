using GalaSoft.MvvmLight.Ioc;
using Geocache.Models;
using Geocache.Views.PopUpViews;
using System;
using System.Collections.Generic;
using System.Linq;
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
}
