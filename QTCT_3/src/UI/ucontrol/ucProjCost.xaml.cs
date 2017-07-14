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
    /// ucProjCost.xaml 的交互逻辑
    /// </summary>
    public partial class ucProjCost : UserControl
    {
       public string mTxt1;
        public string mTxt2;
        public string mTxt3;
        public string ucID;
        private PTS_PROJ_COST _mCost;

        public PTS_PROJ_COST mCost
        {
            get { return _mCost; }
            set { _mCost = value; }
        }
        public ucProjCost(string id)
        {
            InitializeComponent();
            ucID = id;
            mCost = new PTS_PROJ_COST();
            txt1.TextChanged+=txt1_TextChanged;
            txt2.TextChanged += txt2_TextChanged;
            txt3.TextChanged += txt3_TextChanged;
        }

        public ucProjCost(string id, PTS_PROJ_COST cost)
        { 
            InitializeComponent();
            ucID = id;
            this.txt1.Text = cost.KEY1;
            this.txt2.Text = cost.KEY2;
            this.txt3.Text = cost.COST;
            mCost = cost;
            mTxt1 = txt1.Text;
            mTxt2 = txt2.Text;
            mTxt3 = txt3.Text;
            txt1.TextChanged += txt1_TextChanged;
            txt2.TextChanged += txt2_TextChanged;
            txt3.TextChanged += txt3_TextChanged;
        }

        public delegate void DelCost(ucProjCost uc);
        public event DelCost DelSelectCost;

        private void txt1_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                mTxt1 = txt1.Text;
                mCost.KEY1 = mTxt1;
            }
            catch(Exception ex)
            { }
        }

        private void txt2_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
            mTxt2 = txt2.Text;
            mCost.KEY2 = mTxt2;
            }
            catch (Exception ex)
            { }
        }

        private void txt3_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
            mTxt3 = txt3.Text;
            mCost.COST = mTxt3;
            }
            catch (Exception ex)
            { }
        }

        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DelSelectCost != null)
            {
                DelSelectCost(this);
            }
        }
    }
}

