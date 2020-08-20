using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace GeocacheV2
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GeoGacheApp"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is 
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:GeoGacheApp;assembly=GeoGacheApp"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:RatingCell/>
    ///
    /// </summary>
    public partial class RatingCell : StackPanel
    {


        public static readonly DependencyProperty RatingCellProperty = DependencyProperty.Register(
            "RatingValue",
            typeof(Int32),
            typeof(RatingCell),
            new PropertyMetadata(0, new PropertyChangedCallback(RatingValueChanged)));

        public RatingCell()
        {
            InitializeComponent();
        }

        public Int32 RatingValue
        {
            get
            {

                return (Int32)GetValue(RatingCellProperty);
            }
            set
            {
                if (value < 0)
                {
                    SetValue(RatingCellProperty, 0);
                }
                else if (value > 5)
                {
                    SetValue(RatingCellProperty, 5);
                }
                else
                {
                    SetValue(RatingCellProperty, value);
                }
            }
        }

        private static void RatingValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            RatingCell parent = sender as RatingCell;
            Int32 ratingValue = (Int32)e.NewValue;
            UIElementCollection children = parent.Children;

            ToggleButton button = null;
            for (Int32 i = 0; i < ratingValue; i++)
            {
                button = children[i] as ToggleButton;
                button.IsChecked = true;
            }

            for (Int32 i = ratingValue; i < children.Count; i++)
            {
                button = children[i] as ToggleButton;
                button.IsChecked = false;
            }
        }

        private void RatingButtonClickEventHandler(Object sender, RoutedEventArgs e)
        {
            ToggleButton button = sender as ToggleButton;
            RatingValue = Int32.Parse((String)button.Tag);
        }

    }
}
