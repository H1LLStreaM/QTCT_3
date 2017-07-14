using NHibernate.Expression;
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
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.ucontrol
{
    /// <summary>
    /// ucRatio.xaml 的交互逻辑
    /// </summary>
    public partial class ucRatio : UserControl
    {
        public string mTxt1;
        public string mTxt2;
        public string mTxt3;
        public string ucID;
        private pts_proj_ratio _mRatio;

        public pts_proj_ratio mRatio
        {
            get { return _mRatio; }
            set { _mRatio = value; }
        }
        public ucRatio(string id)
        {
            InitializeComponent();
            ucID = id;
            mRatio = new pts_proj_ratio();
            txt1.TextChanged+=txt1_TextChanged;
            txt2.TextChanged += txt2_TextChanged;
            txt3.TextChanged += txt3_TextChanged;
        }

        public ucRatio(string id, pts_proj_ratio ratio)
        { 
            InitializeComponent();
            ucID = id;
            this.txt1.Text = ratio.KEY1;
            this.txt2.Text = ratio.KEY2;
            this.txt3.Text = ratio.RATIO;
            mRatio = ratio;
            mTxt1 = txt1.Text;
            mTxt2 = txt2.Text;
            mTxt3 = txt3.Text;
            txt1.TextChanged += txt1_TextChanged;
            txt2.TextChanged += txt2_TextChanged;
            txt3.TextChanged += txt3_TextChanged;
        }

        public delegate void DelRatio(ucRatio uc);
        public event DelRatio DelSelectRatio;

        private void txt1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                mTxt1 = txt1.Text;
                mRatio.KEY1 = mTxt1;
            }
            catch(Exception ex)
            { }
        }

        private void txt2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
            mTxt2 = txt2.Text;
            mRatio.KEY2 = mTxt2;
            }
            catch (Exception ex)
            { }
        }

        private void txt3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
            mTxt3 = txt3.Text;
            mRatio.RATIO = mTxt3;
            }
            catch (Exception ex)
            { }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DelSelectRatio != null)
            {
                DelSelectRatio(this);
            }
        }
    }
}
