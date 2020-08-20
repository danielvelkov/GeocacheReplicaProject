
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

namespace Geocache
{
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
                if ((Int32)GetValue(RatingCellProperty) == value)
                {
                    SetValue(RatingCellProperty, 0);
                    return; 
                }
                else if (value < 0)
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
