﻿using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.Wpf;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Geocache.Views
{
    /// <summary>
    /// Interaction logic for HideTreasure.xaml
    /// </summary>
    public partial class HideTreasurePageView : UserControl
    { 
        public HideTreasurePageView()
        {
            InitializeComponent();
        }
        private void ChromiumWebBrowser_Unloaded(object sender, RoutedEventArgs e)
        {
            (e.Source as ChromiumWebBrowser).Dispose();
        }
    }
}

