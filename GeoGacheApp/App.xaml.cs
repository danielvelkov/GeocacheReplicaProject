﻿using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.Wpf;
using GalaSoft.MvvmLight.Threading;
using Geocache.ViewModel;
using Geocache.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Geocache
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static App()
        {
            DispatcherHelper.Initialize();
        }
        
    }
}
