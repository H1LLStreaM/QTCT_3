using QTCT_3.src.UI.ucontrol;
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
using WY.Common.Message;
using WY.Library.Business;
using WY.Library.Model;

namespace QTCT_3.src.UI.WPF
{
    /// <summary>
    /// frmRatioEdit.xaml 的交互逻辑
    /// </summary>
    public partial class frmRatioEdit : Window
    {
        private TB_PROJECT mProj;
        public frmRatioEdit()
        {
            InitializeComponent();
        }

        public frmRatioEdit(TB_PROJECT proj)
            : this()
        {
            mProj = proj;
            this.txtProj.Tag = mProj;
            this.txtProj.Text = mProj.OBJECTNAME;
            //this.Image.Visibility = System.Windows.Visibility.Hidden;
            //this.txtProj.PreviewMouseLeftButtonDown -= txtProj_PreviewMouseLeftButtonDown;
            //this.txtProj.KeyDown -= txtProj_KeyDown;
            BindProjTeam(mProj);
        }

        private void clearProj()
        {
            this.txtProj.Text = "";
            this.txtProj.Tag = null;
            this.panel1.Children.Clear();
            this.panel2.Children.Clear();
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            clearProj();            
        }

        private void txtProj_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtProj.Tag == null)
                {
                    frmObjectSearch frm = new frmObjectSearch();
                    frm.ShowDialog();
                    if (frm.mProj != null && frm.mProj.Id > 0)
                    {
                        this.txtProj.Tag = frm.mProj;
                        this.txtProj.Text = frm.mProj.OBJECTNAME;
                        //绑定项目成员
                        BindProjTeam(frm.mProj);
                    }
                    else
                    {
                        this.txtProj.Tag = null;
                        this.txtProj.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void txtProj_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
                clearProj();
        }

        private void BindProjTeam(TB_PROJECT proj)
        {
            this.panel1.Children.Clear();
            this.panel2.Children.Clear();

            string leader = proj.TEAMLEDER;
            string[] members = proj.TEAMMEMBER.Split('|');

            if (!string.IsNullOrEmpty(leader))
            {
                ucRatio2 uc = new ucRatio2(leader,proj.Id);
                this.panel1.Children.Add(uc);
            }
            if (members.Length > 0)
            {
                for (int i = 0; i < members.Length; i++)
                {
                    if (!string.IsNullOrEmpty(members[i]))
                    {
                        ucRatio2 uc = new ucRatio2(members[i], proj.Id);
                        this.panel2.Children.Add(uc);
                    }
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            List<TB_RATIO> list = new List<TB_RATIO>();
            decimal total = 0;
            //项目负责人
            for (int i = 0; i < panel1.Children.Count; i++)
            {
                ucRatio2 uc = panel1.Children[i] as ucRatio2;
                string usercode = uc.labName.Content.ToString();
                string ratio = uc.txtRatio.Text;
                decimal d = 0;
                decimal.TryParse(ratio, out d);
                TB_RATIO ro = new TB_RATIO();
                ro.USERCODE = usercode;
                ro.RATIO = d;
                ro.PROJECTID = (txtProj.Tag as TB_PROJECT).Id;
                ro.STATUS = 1;
                list.Add(ro);
                total += d;
            }
            //项目成员
            for (int i = 0; i < panel2.Children.Count; i++)
            {
                ucRatio2 uc = panel2.Children[i] as ucRatio2;
                string usercode = uc.labName.Content.ToString();
                string ratio = uc.txtRatio.Text;
                decimal d = 0;
                decimal.TryParse(ratio, out d);
                TB_RATIO ro = new TB_RATIO();
                ro.USERCODE = usercode;
                ro.RATIO = d;
                ro.PROJECTID = (txtProj.Tag as TB_PROJECT).Id;
                ro.STATUS = 1;
                list.Add(ro);
                total += d;
            }            
            bool flag = RatioBusiness.delete((txtProj.Tag as TB_PROJECT).Id);
            if (flag)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Save();
                }
                MessageHelper.ShowMessage("保存成功！");
                this.Close();
            }
            else
            {
                MessageHelper.ShowMessage("保存失败！");
            }
        }
    }
}
