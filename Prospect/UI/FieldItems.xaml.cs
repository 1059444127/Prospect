using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for FieldItems.xaml
    /// </summary>
    public partial class FieldItems : Window
    {
        public FieldItems()
        {
            InitializeComponent();
            DataContext = Config.SelectedColunms;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender != null)
            {
                Grid grid = (Grid)sender;
                CheckBox cbx = (CheckBox)grid.Children[0];
                cbx.IsChecked = cbx.IsChecked == true ? false : true;
            }
        }
    }
}
