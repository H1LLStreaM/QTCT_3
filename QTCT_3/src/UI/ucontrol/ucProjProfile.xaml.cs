using NHibernate.Expression;
using QTCT_3.Comments;
using QTCT_3.src.UI.WPF;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WY.Common.Message;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.ucontrol
{
    /// <summary>
    /// ucProjProfile.xaml 的交互逻辑
    /// </summary>
    public partial class ucProjProfile : UserControl
    {
        public ucProjProfile()
        {
            InitializeComponent();
        }

        private void txtProj_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtProj.Tag == null)
                {
                    frmObjectSearch frm = new frmObjectSearch(1);
                    frm.ShowDialog();
                    if (frm.mProj != null && frm.mProj.Id > 0)
                    {
                        this.txtProj.Tag = frm.mProj;
                        this.txtProj.Text = frm.mProj.OBJECTNAME;                        
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

        private void imgClear_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClearProj();
        }

        private void ClearProj()
        {
            this.txtProj.Tag = null;
            this.txtProj.Text = string.Empty;
        }

        #region 人员文本框操作
        private void txtUser_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (txtUser.Tag == null)
                {
                    frmUserSearch frm = new frmUserSearch();
                    frm.ShowDialog();
                    if (frm.mUser != null && frm.mUser[0].Id > 0)
                    {
                        this.txtUser.Tag = frm.mUser[0];
                        this.txtUser.Text = frm.mUser[0].USER_CODE;
                    }
                    else
                    {
                        this.txtUser.Tag = null;
                        this.txtUser.Text = "";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void txtUser_KeyDown(object sender, KeyEventArgs e)
        {
            ClearUser();
        }

        private void imgDel2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ClearUser();
        }

        private void ClearUser()
        {
            this.txtUser.Tag = null;
            this.txtUser.Text = string.Empty;
        }
        #endregion       

        /// <summary>
        /// 查询已结算项目的成员提成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            dgProfile.ItemsSource=null;
            if (this.txtProj.Tag == null)
            {
                MessageHelper.ShowMessage("请选择需要查询的项目");
                return;
            }
            else 
            {
                TB_PROJECT proj = this.txtProj.Tag as TB_PROJECT;
                if (proj.BILLSTATUS == "已结算")
                {
                    profileporcess pp = new profileporcess();
                    projProfileClass ppc = pp.getProfile(TB_PROJECTDAO.FindFirst(new EqExpression("Id", proj.Id)));
                    List<TB_PERSONAL_PROFILE> list = pp.personalProcess(ppc, proj.Id);
                    List<TB_PERSONAL_PROFILE> _list = new List<TB_PERSONAL_PROFILE>();
                    if (txtUser.Tag == null)
                    {
                        _list = list;
                    }
                    else
                    {
                        TB_User user = txtUser.Tag as TB_User;
                        _list = list.FindAll(a => a.USERCODE == user.USER_CODE);
                    }
                    if (_list.Count > 0)
                    {
                        for (int i = 0; i < _list.Count; i++)
                        {
                            _list[i].INDEX = i + 1;
                            _list[i].USERNAME = TB_UserDao.FindFirst(new EqExpression("USER_CODE", _list[i].USERCODE)).USER_NAME;
                        }
                        dgProfile.ItemsSource = list;
                    }
                }
                else
                {
                    MessageHelper.ShowMessage("该项目未结算");
                    return;
                }            
            }
        }
    }
}
