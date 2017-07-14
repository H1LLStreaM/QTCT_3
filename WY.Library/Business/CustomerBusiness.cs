using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using WY.Common.Utility;
using WY.Common.Message;
using Library.Dao;
using NHibernate.Expression;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using WY.Common.Data;
using System.Data.Common;
using WY.Library.Dao;
using Castle.ActiveRecord;
using WY.Common.Framework;

namespace WY.Library.Business
{
    public class CustomerBusiness
    {
        #region 保存客户信息
        public static void saveCustomerInfo(Customer cus)
        {
            try
            {
                if (findCustomerByName(cus.Customername) != null)
                {
                    MessageHelper.ShowMessage("E004");
                    return;
                }
                cus.Save();
                //MessageHelper.ShowMessage("I001");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999");
            }
        }
        #endregion

        #region 根据客户名称获取客户信息(精确查询)
        public static Customer findCustomerByName(string cusName)
        {
            try
            {
                Customer cus = CustomerDao.FindFirst(new EqExpression("Customername", cusName), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
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
        public static DataTable findCustomerByFuzzyName(string cusName)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select id,customername from customer where "
                               + "customername like '%" + cusName + "%' and Isdeleted="+(int)EnmIsdeleted.使用中+"";
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
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new DataTable("error");
            }
        }
        #endregion

        #region 根据客户ID获取客户信息
        public static Customer findCustomerById(int id)
        {
            try
            {
                Customer cus = CustomerDao.FindFirst(new EqExpression("Id", id));
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

        #region  获取所有客户信息
        public static Customer[] getAllCustomers()
        {
            try
            {
                Customer[] cus = CustomerDao.FindAll(new Order("Customername", true), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
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

        #region 更新客户信息
        public static bool updateCustomerInfo(Customer cus)
        {
            try
            {
                cus.Update();
                //MessageHelper.ShowMessage("I001");
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

        #region 删除客户
        public static void delCustomer(int customerId, DateTime endtime)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    db.TrnStart();
                    string sql = "Update commission set endTime=@end , Isdeleted=@del where cableid in (select id from cable where customerid=@id)";
                    //DateTime endtime = TableManager.DBServerTime().Date;
                    DbParameter[] parmar2 = { db.CreateParameter("@end", endtime), db.CreateParameter("@del", (int)EnmIsdeleted.已删除), db.CreateParameter("@id", customerId) };
                    db.ExecuteNonQuery(sql, parmar2);  //删除提成比率

                    sql = "Update cable set Isdeleted=@del where customerid =@id";
                    DbParameter[] parmar = { db.CreateParameter("@del", (int)EnmIsdeleted.已删除), db.CreateParameter("@id", customerId) };
                    db.ExecuteNonQuery(sql, parmar);  //删除电路代码

                    sql = "Update customer set Isdeleted=@del where id=@id";
                    DbParameter[] parmar3 = { db.CreateParameter("@del", (int)EnmIsdeleted.已删除), db.CreateParameter("@id", customerId) };
                    db.ExecuteNonQuery(sql, parmar); //删除客户
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

        #region
        public static DataTable getCustomerByCondition(string customer,int salerid)
        {
            try
            {
                string sql;
                using (DbHelper db = new DbHelper())
                {
                    if (string.IsNullOrEmpty(customer) && salerid == 0)
                    {
                        sql = "select id,customername from customer where isDeleted=@del order by customername";
                        DbParameter[] paramlist = { db.CreateParameter("@del", (int)EnmIsdeleted.使用中) };
                        DataTable tb = db.GetDataSet(sql, paramlist).Tables[0]; //查询结果表
                        return tb;
                    }
                    else if(!string.IsNullOrEmpty(customer) && salerid==0)
                    {
                        //sql = "select c.id,c.customername from cable as b left join customer as c on b.customerid = c.id "
                        //           + "left join tb_user as u on b.userId =u.id "
                        //           + "where c.isDeleted ="+(int)EnmIsdeleted.使用中+" and b.isDeleted="+(int)EnmIsdeleted.使用中+" and u.isDeleted="+(int)EnmIsdeleted.使用中+" "
                        //           + " and c.customername like '%"+customer+"%' "
                        //           + " group by c.id order by c.id ";
                        sql = "select id,customername from customer where customername like @cus and isDeleted=@del order by customername";
                        DbParameter[] paramlist = { db.CreateParameter("@cus","%"+customer+"%"),db.CreateParameter("@del",(int)EnmIsdeleted.使用中)};
                        DataTable tb = db.GetDataSet(sql, paramlist).Tables[0]; //查询结果表
                        return tb;
                    }
                    else if (string.IsNullOrEmpty(customer) && salerid > 0)
                    {
                        sql = "select c.id,c.customername from cable as b left join customer as c on b.customerid = c.id "
                                  + "left join tb_user as u on b.userId =u.id "
                                  + "where c.isDeleted =@del and b.isDeleted=@del and u.isDeleted=@del "
                                  + " and b.userid=@uid "
                                  + " group by c.id order by c.id "
                                  + " order by c.customername ";
                        DbParameter[] paramlist = { db.CreateParameter("@del", (int)EnmIsdeleted.使用中), db.CreateParameter("@uid", salerid) };
                        DataTable tb = db.GetDataSet(sql, paramlist).Tables[0]; //查询结果表
                        return tb;
                    }
                    else
                    {
                        sql = "select c.id,c.customername from cable as b left join customer as c on b.customerid = c.id "
                                  + " left join tb_user as u on b.userId =u.id "
                                  + " where c.isDeleted =@del and b.isDeleted=@del and u.isDeleted=@del "
                                  + " and  b.userid=@uid and c.customername like @cus "
                                  + " group by c.id order by c.id ";
                        DbParameter[] paramlist = { db.CreateParameter("@del", (int)EnmIsdeleted.使用中), db.CreateParameter("@uid", salerid), db.CreateParameter("@cus", "%" + customer + "%") };
                        DataTable tb = db.GetDataSet(sql, paramlist).Tables[0]; //查询结果表
                        return tb;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999",ex.Message);
                return new DataTable();
            }
        }
        #endregion
    }
}
