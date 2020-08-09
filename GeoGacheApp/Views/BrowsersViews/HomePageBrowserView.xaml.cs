using CefSharp;
using CefSharp.SchemeHandler;
using CefSharp.Wpf;
using Geocache.Helper;
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
            InitBrowser();
            InitializeComponent();
            
        }
        // should be done only once. See cefsharp general usage
        // i think cef has a default shutdown method that runs when exiting
        // i think... :)
        public void InitBrowser()
        {
            if (!Cef.IsInitialized)
            {
                var settings = new CefSettings();

                settings.BrowserSubprocessPath = System.IO.Path.GetFullPath("CefSharp.BrowserSubprocess.exe");

                settings.RegisterScheme(new CefCustomScheme
                {
                    SchemeName = "localfolder",
                    DomainName = "cefsharp",
                    SchemeHandlerFactory = new FolderSchemeHandlerFactory(
                rootFolder: string.Format(@"{0}\html", System.AppDomain.CurrentDomain.BaseDirectory),
                hostName: "cefsharp",
                defaultPage: "map.html" // will default to map.html
                )
                });

                Cef.Initialize(settings);
            }
        }
    }
}
