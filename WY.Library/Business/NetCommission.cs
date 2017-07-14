using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using WY.Common.Utility;
using Library.Dao;
using NHibernate.Expression;
using System.Data;
using WY.Common.Data;
using WY.Common.Message;

namespace WY.Library.Business
{
    /// <summary>
    /// 智能网提成比例业务
    /// </summary>
    public class NetCommission
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dt_commission[] Query()
        {
            try
            {
                Dt_commission[] c = Dt_commissionDao.FindAll(new Order("Name", true), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
                return c;
                //return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Dt_commission QueryById(int id)
        {
            try
            {
                Dt_commission c = Dt_commissionDao.FindFirst(new Order("Id", true), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
                return c;
                //return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DataTable QueryForDataTable()
        {
            DataTable result = null;
            //string strStartDate = year.ToString() + "-" + month.ToString() + "-1";
            //DateTime startDate = DateTime.Parse(strStartDate).Date; //当月第一天
            //int days = DateTime.DaysInMonth(year, month);
            //DateTime endDate = startDate.AddMonths(1).AddSeconds(-1);   //当月最后一天
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "Select id,memo From dt_commission Where 1=1";                    
                    DataTable dt = db.GetDataSet(sql).Tables[0];
                    result = dt;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Save(Dt_commission obj)
        {
            try
            {
                obj.Save();
                return true;
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Modify(Dt_commission obj)
        {
            try
            {
                obj.Update();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

    }
}
