using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using WY.Common.Data;
using System.Data.Common;
using WY.Library.Model;
using System.Linq;

namespace WY.Library.Business
{
    /// <summary>
    /// 智能网渠道金额计算
    /// </summary>
    public class NetBusiness
    {
        public static void Calculater(int salerid,string year,string month )
        {
            TB_User[] arr = UserBusiness.getAllSalers();  //获取所有渠道信息;
            if (salerid > 0)
            {
                var m0 = from m in arr
                         where m.Id == salerid
                         select m;
                //if (m0 == null)
                //{
                //    if (dv1.Rows[i].DefaultCellStyle.BackColor != Color.Red)
                //    {
                //        dv1.Rows[i].Cells["errinfo"].Value = "根据客户信息未找到对应的设备信息！";
                //        dv1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                //        isErr = false;
                //    }
                //    continue;
                //}
            }
            DataTable dt1 = searchNetBill(salerid, year, month);  //销账金额
            DataTable dt2 = searchNetBill2(salerid, year, month); //欠费金额
        }

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
                        sql += "and salerId= " + salerId;
                    }
                    sql += " Order by salerId";
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
                    sql += " Order by salerId";
                    return db.GetDataSet(sql, paramlist).Tables[0];
                }
                catch
                {
                    return new DataTable();
                }
            }
        }
        #endregion
    }
}
