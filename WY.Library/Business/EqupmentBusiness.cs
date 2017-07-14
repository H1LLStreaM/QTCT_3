using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using WY.Common;
using Library.Dao;
using NHibernate.Expression;
using WY.Common.Utility;
using WY.Common.Message;
using WY.Common.Data;
using System.Data.Common;
using System.Data;

namespace WY.Library.Business
{
    public class EqupmentBusiness
    {
        /// <summary>
        /// 保存设备
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Save(Dt_equpment obj)
        {
            try
            {
                obj.Save();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 更新设备
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool Modify(Dt_equpment obj)
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

        /// <summary>
        /// 根据客户ID查询设备信息
        /// </summary>
        /// <param name="cusId"></param>
        /// <returns></returns>
        public static Dt_equpment[] queryByCusID(int cusId)
        {
            try
            {
                if (true) //(Global.g_usergroupid == (int)EnmUserRole.销售总监)
                {
                    Dt_equpment[] c = Dt_equpmentDao.FindAll(new Order("Createtime", false), new EqExpression("Cusid", cusId), new NotExpression(new EqExpression("Isdeleted", (int)EnmIsdeleted.已删除)));
                    return c;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据设备号查询
        /// </summary>
        /// <param name="cusId"></param>
        /// <returns></returns>
        public static Dt_equpment queryByEqNo(string Equpmentno)
        {
            try
            {
                if (true) //(Global.g_usergroupid == (int)EnmUserRole.销售总监)
                {
                    Dt_equpment c = Dt_equpmentDao.FindFirst(new Order("Createtime", false), new EqExpression("Equpmentno", Equpmentno), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
                    return c;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 根据设备号查询
        /// </summary>
        /// <param name="cusId"></param>
        /// <returns></returns>
        public static Dt_equpment queryByEqID(int eqId)
        {
            try
            {
                if (true) //(Global.g_usergroupid == (int)EnmUserRole.销售总监)
                {
                    Dt_equpment c = Dt_equpmentDao.FindFirst(new EqExpression("Id", eqId));
                    return c;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return null;
            }
        }

        #region 删除选择月度的销账账单
        public static bool delAllBillsInDate(string year, string month)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "Update dt_netMoney set Isdeleted=" + (int)EnmIsdeleted.已删除 + " where year=@year and month=@month ";
                    DbParameter[] param = { db.CreateParameter("@year", year), db.CreateParameter("@month", month) };
                    db.ExecuteNonQuery(sql, param);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "删除" + year.ToString() + "年" + month.ToString() + "月度账单发生错误。");
                return false;
            }
        }

        public static bool delAllBillsInDate2(string year, string month)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "Update dt_netnoMoney set Isdeleted=" + (int)EnmIsdeleted.已删除 + " where year=@year and month=@month ";
                    DbParameter[] param = { db.CreateParameter("@year", year), db.CreateParameter("@month", month) };
                    db.ExecuteNonQuery(sql, param);
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage("E999", "删除" + year.ToString() + "年" + month.ToString() + "月度账单发生错误。");
                return false;
            }
        }
        #endregion

        #region 查询销账与欠费信息
        /// <summary>
        /// 销账
        /// </summary>
        /// <param name="salerId"></param>
        /// <param name="startYear"></param>
        /// <param name="startMonth"></param>
        /// <returns></returns>
        public static DataTable searchNetBill(int salerId, string startYear, string startMonth)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    DbParameter[] paramlist = { db.CreateParameter("y", startYear), db.CreateParameter("m", startMonth), db.CreateParameter("@del", (int)EnmIsdeleted.使用中) };
                    string sql = "select * from dt_netmoney where year=@y and month=@m and Isdeleted=@del ";
                    if (salerId > 0)
                    {
                        sql += "and salerId= "+salerId;
                        
                    }
                    return db.GetDataSet(sql, paramlist).Tables[0];
                }
                catch
                {
                    return new DataTable();
                }
            }
        }

        /// <summary>
        /// 欠费
        /// </summary>
        /// <param name="salerId"></param>
        /// <param name="startYear"></param>
        /// <param name="startMonth"></param>
        /// <returns></returns>
        public static DataTable searchNetBill2(int salerId, string startYear, string startMonth)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    DbParameter[] paramlist = { db.CreateParameter("y", startYear), db.CreateParameter("m", startMonth), db.CreateParameter("@del", (int)EnmIsdeleted.使用中) };
                    string sql = "select * from dt_netnomoney where year=@y and month=@m and Isdeleted=@del ";
                    if (salerId > 0)
                    {
                        sql += "and salerId= " + salerId;

                    }
                    return db.GetDataSet(sql, paramlist).Tables[0];
                }
                catch
                {
                    return new DataTable();
                }
            }
        }
        #endregion

        #region 设备过户
        public static bool Margin(int eid, int cusid)
        {
            return false;
        }
        #endregion
    }
}
