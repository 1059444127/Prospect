using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Controls.Primitives;

namespace com.cloud.prospect
{
    public partial class SimpleStlyesResourceDic : ResourceDictionary
    {
        public SimpleStlyesResourceDic()
        {
            InitializeComponent();

        }

        public void btnUp_Click(object sender, RoutedEventArgs e)
        {
            var ug = (UniformGrid)LogicalTreeHelper.GetParent((Button)sender);
            var gd = (Grid)VisualTreeHelper.GetParent(ug);
            var bd = (Border)VisualTreeHelper.GetParent(gd);
            var tbx = (TextBox)VisualTreeHelper.GetParent(bd);
            int value = 0;
            if (Int32.TryParse(tbx.Text, out value))
            {
                tbx.Text = (++value).ToString();
            }

        }

        public void btnDwon_Click(object sender, RoutedEventArgs e)
        {
            var ug = (UniformGrid)LogicalTreeHelper.GetParent((Button)sender);
            var gd = (Grid)VisualTreeHelper.GetParent(ug);
            var bd = (Border)VisualTreeHelper.GetParent(gd);
            var tbx = (TextBox)VisualTreeHelper.GetParent(bd);
            int value = 0;
            if (Int32.TryParse(tbx.Text, out value))
            {
                tbx.Text = value <= 0 ? "0" : (--value).ToString();
            }
        }
    }
}
