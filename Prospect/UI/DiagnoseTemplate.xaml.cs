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
using System.Collections.ObjectModel;

namespace com.cloud.prospect
{
    /// <summary>
    /// Interaction logic for DiagnoseTemplate.xaml
    /// </summary>
    public partial class DiagnoseTemplate : Window
    {
        public delegate void ApplyTemplateEventHandler(object sender, string finding, string prompt);
        public event ApplyTemplateEventHandler ApplyTemplateEvent;

        MainTemplate template;

        public DiagnoseTemplate()
        {
            InitializeComponent();
            Init();
        }

        #region Event
        private void btnAddBodyPart_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbxBodyPart.Text)) return;
            var count = (from template in MainTemplate.BodyPartCollection
                         where template.Parent_Id == 0 && template.Name.Equals(tbxBodyPart.Text)
                         select template).Count();
            if (count > 0)
            {
                MessageBox.Show("该检查部位名称已经存在!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var orderId = (from template in MainTemplate.BodyPartCollection
                           where template.Parent_Id == 0
                           select template).Max(x => x.OrderID) + 1;

            MainTemplate tmp = new MainTemplate()
            {
                Parent_Id = 0,
                Name = tbxBodyPart.Text,
                OrderID = orderId,
                NodeLevel = 1
            };

            tmp.Insert();
            MainTemplate.BodyPartCollection.Add(tmp);
        }
        private void btnDeleteBodyPart_Click(object sender, RoutedEventArgs e)
        {
            MainTemplate tmp = (MainTemplate)trvTemplate.SelectedItem;
            if (tmp == null) return;
            if (String.IsNullOrEmpty(tbxBodyPart.Text) || !tmp.Name.Equals(tbxBodyPart.Text)) return;
            MessageBoxResult result = MessageBox.Show("请确认删除该部位?",
                "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;
            var count = (from template in MainTemplate.BodyPartCollection
                         where template.Parent_Id == 0 && template.Name.Equals(tbxBodyPart.Text)
                         select template).Count();
            if (count == 0)
            {
                MessageBox.Show("该检查部位名称不存在!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MainTemplate tmp2 = (from template in MainTemplate.BodyPartCollection
                                 where template.Parent_Id == 0 && template.Name.Equals(tbxBodyPart.Text)
                                 select template).First();

            MainTemplate.BodyPartCollection.Remove(tmp2);
            tmp2.DeleteThisAndChildren();
        }
        private void btnAddTemlate_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbxTemplate.Text)) return;
            MainTemplate tmp = (MainTemplate)trvTemplate.SelectedItem;
            if (tmp == null) return;
            if (String.IsNullOrEmpty(tbxBodyPart.Text)) return;

            if (tmp.Parent_Id != 0)
            {
                MessageBox.Show("请选择正确的结点！");
                return;
            }
            else
            {
                var count = (from template in tmp.Children
                             where template.Name == tbxTemplate.Text
                             select template).Count();
                if (count > 0)
                {
                    MessageBox.Show("该模板分类名称已经存在!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (tmp.Children == null) tmp.Children =
                   new System.Collections.ObjectModel.ObservableCollection<MainTemplate>();
                var orderId = (from template in tmp.Children
                               select template).Max(x => x.OrderID) + 1;
                MainTemplate tmp2 = new MainTemplate()
                {
                    Parent_Id = tmp.Template_Id,
                    Name = tbxTemplate.Text,
                    OrderID = orderId,
                    NodeLevel = 2
                };
                tmp2.Insert();
                tmp.Children.Add(tmp2);
            }

        }
        private void btnDeleteTemplate_Click(object sender, RoutedEventArgs e)
        {
            MainTemplate tmp = (MainTemplate)trvTemplate.SelectedItem;
            if (tmp == null) return;
            if (String.IsNullOrEmpty(tbxTemplate.Text) || !tmp.Name.Equals(tbxTemplate.Text)) return;
            MessageBoxResult result = MessageBox.Show("请确认删除该结点?",
             "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;
            if (tmp.Parent_Id == 0)
            {
                MessageBox.Show("请选择正确的结点！");
                return;
            }
            else
            {
                var tmp2 = (from tmp1 in MainTemplate.BodyPartCollection
                            where tmp1.Template_Id == tmp.Parent_Id
                            select tmp1).Single();
                var count = (from template in tmp2.Children
                             where template.Name == tbxTemplate.Text
                             select template).Count();
                if (count == 0)
                {
                    MessageBox.Show("该模板分类名称不存在!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                tmp2.Children.Remove(tmp);
                tmp.DeleteThisAndChildren();
            }


        }
        private void btnAddList_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tbxTemplateList.Text)) return;
            MainTemplate tmp = (MainTemplate)trvTemplate.SelectedItem;
            if (tmp == null) return;
            if (tmp.Parent_Id == 0)
            {
                MessageBox.Show("请选择正确的结点！");
                return;
            }
            else
            {
                var count = (from template in (ObservableCollection<MainTemplate>)trvTemplateLst.ItemsSource
                             where template.Name == tbxTemplateList.Text
                             select template).Count();
                if (count > 0)
                {
                    MessageBox.Show("该模板分类名称已经存在!", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (tmp.Children == null) tmp.Children = new ObservableCollection<MainTemplate>();
                var orderId = (from template in tmp.Children
                               select template).Max(x => x.OrderID) + 1;
                MainTemplate tmp2 = new MainTemplate()
                {
                    Parent_Id = tmp.Template_Id,
                    Name = tbxTemplateList.Text,
                    OrderID = orderId,
                    NodeLevel = 3
                };
                tmp2.Insert();
                trvTemplateLst.ItemsSource = tmp.LoadTemplateList();
                MainTemplate tmp3 = new MainTemplate()
                {
                    Parent_Id = tmp2.Template_Id,
                    Name = "所见",
                    OrderID = 1,
                    NodeLevel = 4
                };
                tmp3.Insert();
                tmp3 = new MainTemplate()
                {
                    Parent_Id = tmp2.Template_Id,
                    Name = "提示",
                    OrderID = 2,
                    NodeLevel = 4
                };
                tmp3.Insert();
            }
        }
        private void btnDeleteList_Click(object sender, RoutedEventArgs e)
        {
            if (trvTemplateLst.SelectedItem == null) return;
            MainTemplate template = (MainTemplate)trvTemplateLst.SelectedItem;
            MessageBoxResult result = MessageBox.Show("请确认删除该结点?",
               "确认", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.No) return;
            template.DeleteThisAndChildren();
            template.Template_Id = template.Parent_Id;
            trvTemplateLst.ItemsSource = template.LoadTemplateList();

        }
        private void btnSaveTemplate_Click(object sender, RoutedEventArgs e)
        {
            if (trvTemplateLst.SelectedItem == null) return;
            MainTemplate template = (MainTemplate)trvTemplateLst.SelectedItem;
            template.UpdateFinding(tbxFinding.Text);
            template.UpdatePrompt(tbxPrompt.Text);
            MessageBox.Show("保存模板成功！");
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ProspectWin window = (ProspectWin)this.Owner;
            if (window != null && !window.IsClosing)
            {
                this.WindowState = System.Windows.WindowState.Minimized;
                Visibility = System.Windows.Visibility.Hidden;
                e.Cancel = true;
            }
        }
        private void trvTemplateLst_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem treeViewItem = (TreeViewItem)e.OriginalSource;
            if (treeViewItem != null)
            {
                MainTemplate template = (MainTemplate)trvTemplateLst.SelectedItem;
                tbxTemplateList.Text = template.Name;
                tbxFinding.Text = template.GetUltrasoundFinding();
                tbxPrompt.Text = template.GetUltrasoundPrompt();
            }
        }
        private void trvTemplate_Selected(object sender, RoutedEventArgs e)
        {
            TreeViewItem treeViewItem = (TreeViewItem)e.OriginalSource;
            if (treeViewItem != null)
            {
                MainTemplate template = (MainTemplate)trvTemplate.SelectedItem;
                if (template.Parent_Id != 0)
                {
                    var tmp = (from tmp1 in MainTemplate.BodyPartCollection
                               where tmp1.Template_Id == template.Parent_Id
                               select tmp1).Single();
                    tbxTemplate.Text = template.Name;
                    tbxBodyPart.Text = tmp.Name;
                    trvTemplateLst.ItemsSource = template.LoadTemplateList();
                }
                else
                {

                    tbxBodyPart.Text = template.Name;
                    tbxTemplate.Text = "";
                    trvTemplateLst.ItemsSource = null;
                }
            }

        }
        private void btnApplyTemplate_Click(object sender, RoutedEventArgs e)
        {
            OnMyApplyTemplate();
            WindowState = System.Windows.WindowState.Minimized;
            Visibility = System.Windows.Visibility.Hidden;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = System.Windows.WindowState.Minimized;
        }
        #endregion

        #region Method
        void Init()
        {
            MainTemplate template = new MainTemplate();
            trvTemplate.ItemsSource = template.LoadBodyPart();
        }
        void OnMyApplyTemplate()
        {
            ApplyTemplateEventHandler handler = System.Threading.Interlocked.CompareExchange(ref ApplyTemplateEvent, null, null);
            if (handler != null)
            {
                handler(this, tbxFinding.Text, tbxPrompt.Text);
            }
        }
        #endregion












    }
}
