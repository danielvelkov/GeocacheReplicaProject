using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.Wpf;
using System;
using System.Collections.Generic;
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

namespace Geocache.Views.BrowsersViews
{
    /// <summary>
    /// Interaction logic for HomePageBrowserView.xaml
    /// </summary>
    public partial class HomePageBrowserView : UserControl
    {
        public HomePageBrowserView()
        {
            
            InitializeComponent();
            InitBrowser();
        }

        public void InitBrowser()
        {
            var settings = new CefSettings();

            settings.BrowserSubprocessPath = System.IO.Path.GetFullPath("CefSharp.BrowserSubprocess.exe");

            settings.RegisterScheme(new CefCustomScheme
            {
                SchemeName = "localfolder",
                DomainName = "cefsharp",
                SchemeHandlerFactory = new FolderSchemeHandlerFactory(
            rootFolder: @"D:\PS kursov proekt\kursov proekt-2019_04_24\kursov proekt\Geocaching\GeoCacheGame\GeoGacheApp\html",
            hostName: "cefsharp",
            defaultPage: "map.html" // will default to map.html
        )
            });

            Cef.Initialize(settings);

            //goes to the particular scheme
            var browser = new ChromiumWebBrowser("localfolder://cefsharp/");
            mainGrid.Children.Add(browser);
            Grid.SetRow(browser, 2);

        }
    }
}
