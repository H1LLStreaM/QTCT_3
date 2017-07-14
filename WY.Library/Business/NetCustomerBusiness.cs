using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using NHibernate.Expression;
using Library.Dao;
using WY.Common.Utility;
using WY.Common.Message;
using WY.Common.Data;
using System.Data.Common;
using System.Data;

namespace WY.Library.Business
{
    public class NetCustomerBusiness
    {
        #region 保存客户信息
        public static void saveCustomerInfo(Dt_netcustomers cus)
        {
            try
            {
                if (findCustomerByName(cus.Customername) != null)
                {
                    MessageHelper.ShowMessage("E004");
                    return;
                }
                cus.Save();
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999");
            }
        }
        #endregion

        #region 根据客户名称获取客户信息(精确查询)
        public static Dt_netcustomers findCustomerByName(string cusName)
        {
            try
            {
                Dt_netcustomers cus = Dt_netcustomersDao.FindFirst(new EqExpression("Customername", cusName), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
                return cus;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return null;
            }
        }
        #endregion

        #region 根据客户名称获取客户信息(模糊查询)
        public static DataTable findCustomerByFuzzyName(string cusName,string eqno)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    if (string.IsNullOrEmpty(eqno))
                    {
                        string sql = "select * from dt_netcustomers where "
                                   + "customername like '%" + cusName + "%' and Isdeleted=" + (int)EnmIsdeleted.使用中 + " order by customername";
                        DataSet ds = db.GetDataSet(sql);
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                        {
                            return ds.Tables[0];
                        }
                        else
                        {
                            return new DataTable("empty");
                        }
                    }
                    else
                    {
                        string sql = "Select * from dt_netcustomers where id in (Select cusId from dt_equpment where "
                                   + "equpmentno='" + eqno + "' and Isdeleted<>" + (int)EnmIsdeleted.已删除 + ") and Isdeleted=" + (int)EnmIsdeleted.使用中 + " Order by customername";
                        DataSet ds = db.GetDataSet(sql);
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                        {
                            return ds.Tables[0];
                        }
                        else
                        {
                            return new DataTable("empty");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new DataTable("error");
            }
        }
        #endregion

        #region 更新客户信息
        public static bool updateCustomerInfo(Dt_netcustomers cus)
        {
            try
            {
                cus.Update();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999");
                return false;
            }
        }
        #endregion

        #region  查询所有客户信息
        public static Dt_netcustomers[] getAllCustomers()
        {
            try
            {
                Dt_netcustomers[] cus = Dt_netcustomersDao.FindAll(new Order("Customername", true), 
                                                                   new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
                return cus;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E005", "客户");
                return null;
            }
        }
        #endregion 

        #region 根据客户ID获取客户信息
        public static Dt_netcustomers findCustomerById(int id)
        {
            try
            {
                Dt_netcustomers cus = Dt_netcustomersDao.FindFirst(new EqExpression("Id", id));
                return cus;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999");
                return null;
            }
        }
        #endregion

        #region 删除客户
        public static void delCustomer(int customerId)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    db.TrnStart();
                    //string sql = "Update commission set endTime=@end , Isdeleted=@del where cableid in (select id from cable where customerid=@id)";
                    ////DateTime endtime = TableManager.DBServerTime().Date;
                    //DbParameter[] parmar2 = { db.CreateParameter("@end", endtime), db.CreateParameter("@del", (int)EnmIsdeleted.已删除), db.CreateParameter("@id", customerId) };
                    //db.ExecuteNonQuery(sql, parmar2);  //删除提成比率

                    string sql = "Update cable set Isdeleted=@del where customerid =@id";
                    //DbParameter[] parmar1 = { db.CreateParameter("@del", (int)EnmIsdeleted.已删除), db.CreateParameter("@id", customerId) };
                    //db.ExecuteNonQuery(sql, parmar1);  //删除电路代码

                    sql = "Update dt_NetCustomers set Isdeleted=@del where id=@id";
                    DbParameter[] parmar2 = { db.CreateParameter("@del", (int)EnmIsdeleted.已删除), db.CreateParameter("@id", customerId) };
                    db.ExecuteNonQuery(sql, parmar2); //删除客户
                    db.TrnCommit();
                    MessageHelper.ShowMessage("I002");
                }
                catch (Exception ex)
                {
                    db.TrnRollBack();
                    Log.Error(ex.Message);
                    MessageHelper.ShowMessage("E999", "电路代码删除失败。");

                }
            }
        }
        #endregion
    }
}
