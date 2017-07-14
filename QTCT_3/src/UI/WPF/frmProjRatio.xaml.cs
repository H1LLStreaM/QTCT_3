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
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.WPF
{
    /// <summary>
    /// frmProjRatio.xaml 的交互逻辑
    /// </summary>
    public partial class frmProjRatio : Window
    {
        public PTS_OBJECT_TYPE_SRC item;

        public frmProjRatio()
        {
            InitializeComponent();
            item = new PTS_OBJECT_TYPE_SRC();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PTS_OBJECT_TYPE_SRC[] arr = PTS_OBJECT_TYPE_SRCDAO.FindAll();
            if (arr.Length > 0)
            { 
                List<PTS_OBJECT_TYPE_SRC> list= new List<PTS_OBJECT_TYPE_SRC>(arr);
                this.dgViewer.ItemsSource = list;
            }
        }

        private void dgViewer_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            btnSubmit_Click(null, null);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (dgViewer.SelectedItem != null)
            {
                item = dgViewer.SelectedItem as PTS_OBJECT_TYPE_SRC;
                this.Close();
            }
        }
    }
}
