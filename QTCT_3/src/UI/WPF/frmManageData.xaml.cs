using NHibernate.Expression;
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
using WY.Common.Utility;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.src.UI.WPF
{
    /// <summary>
    /// frmManageData.xaml 的交互逻辑
    /// </summary>
    public partial class frmManageData : Window
    {
        PTS_OBJECT_TYPE_SRC mSrc;

        public frmManageData()
        {
            InitializeComponent();
            mSrc = new PTS_OBJECT_TYPE_SRC();
            BindSourceData();
        }

        private void BindSourceData()
        {
            try
            {
                //提成比率
                pts_proj_ratio[] arr = pts_proj_ratioDao.FindAll();
                if (arr.Length > 0)
                {
                    for (int i = 0; i < arr.Length; i++)
                    {
                        ucRatio uc = new ucRatio(arr[i].ID.ToString(), arr[i]);
                        uc.DelSelectRatio += uc_DelSelectRatio;
                        this.panel1.Children.Add(uc);
                    }
                }

                //项目成本
                PTS_PROJ_COST[] arr2 = PTS_PROJ_COSTDAO.FindAll();
                if (arr2.Length > 0)
                {
                    for (int i = 0; i < arr2.Length; i++)
                    {
                        ucProjCost uc = new ucProjCost(arr2[i].ID.ToString(), arr2[i]);
                        uc.DelSelectCost += uc_DelSelectCost;
                        this.panel2.Children.Add(uc);
                    }
                }

                //可分配比例
                PTS_OBJECT_TYPE_SRC[] arr3 = PTS_OBJECT_TYPE_SRCDAO.FindAll(new EqExpression("STATUS", 1));
                if (arr3.Length > 0)
                {
                    for (int i = 0; i < arr3.Length; i++)
                    {
                        ucRatio3 uc = new ucRatio3(arr3[i]);
                        uc.DelSelectRatio += uc_DelSelectRatio;
                        this.panel3.Children.Add(uc);
                    }
                }
                //if (src != null)
                //{
                //    mSrc = src;
                //    this.txtRatio_1.Text = src.RATIO1.ToString();
                //    this.txtRatio_2.Text = src.RATIO2.ToString();
                //}
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }        

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //新增
                ucRatio uc = new ucRatio(System.Guid.NewGuid().ToString());
                uc.DelSelectRatio += uc_DelSelectRatio;
                this.panel1.Children.Add(uc);
            }
            catch (Exception ex)
            { }
        }       

        void uc_DelSelectRatio(ucRatio uc)
        {
            foreach (UserControl _uc in panel1.Children)
            {
                if ((_uc as ucRatio).ucID == uc.ucID)
                {
                    panel1.Children.Remove(_uc);
                    break;
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                pts_proj_ratioDao.DeleteAll();
                List<pts_proj_ratio> list = getRatioList();
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Save();
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private List<pts_proj_ratio> getRatioList()
        {
            List<pts_proj_ratio> list = new List<pts_proj_ratio>();
            foreach (UserControl _uc in panel1.Children)
            {
                pts_proj_ratio obj = new pts_proj_ratio();
                obj.KEY1 = (_uc as ucRatio).mRatio.KEY1;
                obj.KEY2 = (_uc as ucRatio).mRatio.KEY2;
                obj.RATIO = (_uc as ucRatio).mRatio.RATIO;
                list.Add(obj);
            }
            return list;
        }

        #region  项目成本管理
        private void btnAdd2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //新增
                ucProjCost uc = new ucProjCost(System.Guid.NewGuid().ToString());
                uc.DelSelectCost += uc_DelSelectCost;
                this.panel2.Children.Add(uc);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private void btnSave2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PTS_PROJ_COSTDAO.DeleteAll();
                List<PTS_PROJ_COST> list = getCostList();

                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Save();
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        void uc_DelSelectCost(ucProjCost uc)
        {
            try
            {
                foreach (UserControl _uc in panel2.Children)
                {
                    if ((_uc as ucProjCost).ucID == uc.ucID)
                    {
                        panel2.Children.Remove(_uc);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private List<PTS_PROJ_COST> getCostList()
        {
            List<PTS_PROJ_COST> list = new List<PTS_PROJ_COST>();
            foreach (UserControl _uc in panel2.Children)
            {
                PTS_PROJ_COST obj = new PTS_PROJ_COST();
                obj.KEY1 = (_uc as ucProjCost).mCost.KEY1;
                obj.KEY2 = (_uc as ucProjCost).mCost.KEY2;
                obj.COST = (_uc as ucProjCost).mCost.COST;
                list.Add(obj);
            }
            return list;
        }
        #endregion

        #region 可分配提成比率
        private void btnAdd3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //新增
                ucRatio3 uc = new ucRatio3(System.Guid.NewGuid().ToString());
                uc.DelSelectRatio += uc_DelSelectRatio;
                this.panel3.Children.Add(uc);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        void uc_DelSelectRatio(ucRatio3 uc)
        {
            try
            {
                foreach (UserControl _uc in panel3.Children)
                {
                    if ((_uc as ucRatio3).ucID == uc.ucID)
                    {
                        panel3.Children.Remove(_uc);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        private List<PTS_OBJECT_TYPE_SRC> getRatioList3()
        {
            List<PTS_OBJECT_TYPE_SRC> list = new List<PTS_OBJECT_TYPE_SRC>();
            foreach (UserControl _uc in panel3.Children)
            {
                PTS_OBJECT_TYPE_SRC obj = new PTS_OBJECT_TYPE_SRC();
                obj.RATIO1 = Math.Round(decimal.Parse((_uc as ucRatio3).RATIO1),2);
                obj.RATIO2 =  Math.Round(decimal.Parse((_uc as ucRatio3).RATIO2),2);
                //obj.RATIO = (_uc as ucRatio).mRatio.RATIO;
                list.Add(obj);
            }
            return list;
        }

        private void btnSave3_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PTS_OBJECT_TYPE_SRCDAO.DeleteAll();
                List<PTS_OBJECT_TYPE_SRC> list = getRatioList3();
                for (int i = 0; i < list.Count; i++)
                {
                    list[i].Save();
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
        }

        //private void btnSave3_Click(object sender, RoutedEventArgs e)
        //{
        //    if (mSrc != null)
        //    {
        //        mSrc.RATIO1 = Utils.NvDecimal(this.txtRatio_1.Text);
        //        mSrc.RATIO2 = Utils.NvDecimal(this.txtRatio_2.Text);
        //        mSrc.Update();
        //    }
        //    else
        //    {
        //        PTS_OBJECT_TYPE_SRC src = new PTS_OBJECT_TYPE_SRC();
        //        src.RATIO1 = Utils.NvDecimal(this.txtRatio_1.Text);
        //        src.RATIO2 = Utils.NvDecimal(this.txtRatio_2.Text);
        //        src.STATUS = 1;
        //        src.Save();
        //        mSrc = src;
        //    }
        //}
        #endregion

        
        
    }
}
