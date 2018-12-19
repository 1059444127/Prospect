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
    /// Interaction logic for SimpleTemplate1.xaml
    /// </summary>
    public partial class SimpleTemplate2 : Window
    {
        public delegate void ApplyTemplateEventHandler(object sender, string prompt);
        public event ApplyTemplateEventHandler ApplyTemplateEvent;

        private SimpleTemplateViewModel viewModel;

        public SimpleTemplate2()
        {
            InitializeComponent();
            InitViewModel();
        }

        #region Event
        private void TreeView_Selected(object sender, RoutedEventArgs e)
        {
            if (trvTemplate.SelectedItem.GetType().Equals(typeof(SimpleTemplate)))
            {
                SimpleTemplate template = (SimpleTemplate)trvTemplate.SelectedItem;
                tbxTemplateName.Text = template.Name;
                tbxPrompt.Text = template.Description;
            }
            else
            {
                tbxTemplateName.Text = "";
                tbxPrompt.Text = "";
            }
        }
        private void btnApplyTemplate_Click(object sender, RoutedEventArgs e)
        {
            OnMyApplyTemplate();
            Close();
        }

        private void btnAddTemplate_Click(object sender, RoutedEventArgs e)
        {
            SimpleTemplate template = new SimpleTemplate()
            {
                Name = tbxTemplateName.Text,
                Description = tbxPrompt.Text,
                Type = "2"
            };
            template.Insert();
            triTemplate.ItemsSource = viewModel.LoadTemplate(2);
            MessageBox.Show("添加模板成功!");
        }
        private void btnDeleteTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (trvTemplate.SelectedItem.GetType().Equals(typeof(SimpleTemplate)))
            {
                SimpleTemplate template = (SimpleTemplate)trvTemplate.SelectedItem;
                MessageBoxResult result = MessageBox.Show("请确认是否删除模板“"
                    + template.Name + "”？",
                   "删除确认", MessageBoxButton.YesNo, MessageBoxImage.Information);

                if (result == MessageBoxResult.Yes)
                {
                    template.Delete();
                    triTemplate.ItemsSource = viewModel.LoadTemplate(2);
                }
            }
        }
        private void btnSaveTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (trvTemplate.SelectedItem.GetType().Equals(typeof(SimpleTemplate)))
            {
                SimpleTemplate template = (SimpleTemplate)trvTemplate.SelectedItem;
                template.Name = tbxTemplateName.Text;
                template.Description = tbxPrompt.Text;
                template.Update();
                //triTemplate.ItemsSource = viewModel.LoadTemplate(1);
                MessageBox.Show("保存模板成功!");
            }
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #endregion

        #region Method
        void InitViewModel()
        {
            viewModel = new SimpleTemplateViewModel(StartUpWindow.SimpleTemplate1);
            triTemplate.ItemsSource = viewModel.LoadTemplate(2);
        }
        void OnMyApplyTemplate()
        {
            ApplyTemplateEventHandler handler = System.Threading.Interlocked.CompareExchange(ref ApplyTemplateEvent, null, null);
            if (handler != null)
            {
                handler(this, tbxPrompt.Text);
            }
        }
        #endregion

      












    }
}
