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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WY.Library.Model;

namespace QTCT_3.src.UI.ucontrol
{
    /// <summary>
    /// ucRatio3.xaml 的交互逻辑
    /// </summary>
    public partial class ucRatio3 : UserControl
    {
        public string RATIO1, RATIO2;
        public string ucID;
        private PTS_OBJECT_TYPE_SRC _mSrc;

        public PTS_OBJECT_TYPE_SRC mSrc
        {
            get { return _mSrc; }
            set { _mSrc = value; }
        }

        public ucRatio3(string id)
        {
            InitializeComponent();
            ucID = id;
        }

        public ucRatio3(PTS_OBJECT_TYPE_SRC src)
        {
            InitializeComponent();
            if (src != null)
            {
                mSrc = src;
                this.txtRatio1.Text = src.RATIO1.ToString();
                this.txtRatio2.Text = src.RATIO2.ToString();
                ucID = mSrc.ID.ToString();
            }
        }

        public delegate void DelRatio(ucRatio3 uc);
        public event DelRatio DelSelectRatio;

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DelSelectRatio != null)
            {
                DelSelectRatio(this);
            }
        }

        private void txtRatio1_TextChanged(object sender, TextChangedEventArgs e)
        {
            RATIO1 = txtRatio1.Text;
        }

        private void txtRatio2_TextChanged(object sender, TextChangedEventArgs e)
        {
            RATIO2 = txtRatio2.Text;
        }
    }
}
