using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using WY.Common.Utility;
using WY.Common.Message;
using WY.Common.Framework;
using Library.Dao;
using NHibernate.Expression;
using WY.Common.Data;
using System.Data.Common;
using System.Data;

namespace WY.Library.Business
{
    public class CableHistoryBusiness
    {
        #region 保存变更历史信息
        public static bool saveHistory(Cablehistory history)
        {
            try
            {
                history.Save();
                return true;
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return false;
            }
        }
        #endregion

        #region 判断电路代码是否为新增
        public static int isNewCable(int id)
        {
            try
            {
                Cablehistory[] his = CablehistoryDao.FindAll(new EqExpression("Cableid", id));

                return his.Length;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return -1;
            }
        }
        #endregion

        #region
        public static Cablehistory[] getHistoryById(int id)
        {
            try
            {
                Cablehistory[] historys = CablehistoryDao.FindAll(new Order("Createtime",false),new EqExpression("Cableid", id), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
                return historys;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return null;
            }
        }
        #endregion

        public static int getHistoryInfoCount(int id)
        {
            using (DbHelper db = new DbHelper())
            {
                string sql = "select count(1) from cablehistory where cableid=@id and Isdeleted = @Isdeleted";
                DbParameter[] paramlist = { db.CreateParameter("@id", id), db.CreateParameter("@Isdeleted", (int)EnmIsdeleted.使用中) };
                DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  //销售渠道数据
                return Utils.NvInt(tb.Rows[0][0].ToString());
            }
            return 0;
        }

        #region 根据ID查询电路代码
        public static DataTable getCableByCableid(int cableId)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    string sql = "select b.id,b.createTime as 日期,customername as 客户名称,cablenumber as EIP编号, "
                    + "cablestatus as 状态,b.userid as 渠道,c.name as 联系人,BoxNumber as 箱子编号,"
                    + "tel as 联系电话,c.address as 客户地址,PackageInfo as 套餐内容,money as 月租费,"
                    + "CompleteTime as 完工日期,b.remark as 备注,b.address1 as 甲端地址,b.address2 as 乙端地址,b.Person1 as 甲端联系人,b.Person2 as 乙端联系人 "
                    + "from cablehistory as b left join customer as c on b.customerid = c.id "
                    + "where b.cableid=@cableid order by b.createTime desc";

                    DbParameter[] parmar = { db.CreateParameter("cableid", cableId) };
                    return db.GetDataSet(sql, parmar).Tables[0];
                }
                catch
                {
                    return new DataTable();
                }
            }
        }
        #endregion

        #region 查找历史记录匹配账单信息
        public static bool isMathcCable(int cableid, DateTime date)
        {
            Cablehistory[] historys = CablehistoryDao.FindAll(new EqExpression("Cableid", cableid));
            if (historys.Length > 0)
            {
                for (int i = 0; i < historys.Length; i++)
                {
                    if (historys[i].Startdate == null || historys[i].Enddate == null)
                    {
                        continue;
                    }
                    DateTime start = DateTime.Parse(historys[i].Startdate.ToString());  //合同起始日期
                    DateTime end = DateTime.Parse(historys[i].Enddate.ToString());   //合同结束日期
                    if (isMath(date, start, end))
                    {
                        return true;
                    }
                    else
                    { continue; }
                }
                return false;
            }
            else
            {
                return false;
            }
        }
        #endregion


        /// <summary>
        /// 判断时间
        /// </summary>
        /// <param name="start">合同起始日期</param>
        /// <param name="end">合同截止日期</param>
        /// <param name="startDate">账单起始日期</param>
        /// <param name="endDate">账单截止日期</param>
        /// <returns></returns>
        private static bool isMath(DateTime start, DateTime end, DateTime startDate, DateTime endDate)
        {
            if (start <= startDate && end >= endDate)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断时间
        /// </summary>
        /// <param name="start">合同起始日期</param>
        /// <param name="end">合同截止日期</param>
        /// <param name="startDate">账单起始日期</param>
        /// <param name="endDate">账单截止日期</param>
        /// <returns></returns>
        private static bool isMath( DateTime date, DateTime startDate, DateTime endDate)
        {
            if (date.Date < startDate.Date)
            {
                return false;
            }
            if (date.Date > endDate.Date)
            {
                return false;
            }
            return true;
        }


        #region 根据电路代码查询历史记录
        public static DataTable getCableByCableNumber(string cablenumber)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    string sql = "select b.id as 电路历史id,c.id as 客户id,c.cableid,b.createTime as 日期,customername as 客户名称,"
                                + "cablenumber as EIP编号,cablestatus as 状态,b.userid as 渠道,c.name as 联系人,BoxNumber as 箱子编号,"
                                + "tel as 联系电话,c.address as 客户地址,PackageInfo as 套餐内容,money as 月租费,"
                                + "CompleteTime as 完工日期,b.remark as 备注,b.address1 as 甲端地址,b.address2 as 乙端地址 "
                                + "from cablehistory as b left join customer as c on b.customerid = c.id "
                                + "where b.cablenumber=@cablenumber order by b.createTime desc";

                    DbParameter[] parmar = { db.CreateParameter("cablenumber", cablenumber) };
                    return db.GetDataSet(sql, parmar).Tables[0];
                }
                catch
                {
                    return new DataTable();
                }
            }
        }
        #endregion

        #region 根据电路ID、客户ID判断是否正确
        public static bool IsCableByCableNumber(int cableid,int customerid)
        {
            Cablehistory his = CablehistoryDao.FindFirst(new EqExpression("Cableid", cableid),
                                                         new EqExpression("Customerid", customerid));
            if (his != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 根据电路ID和客户ID查询迁移时间
        public static Nullable<DateTime> getMigrateDate(int cableid, int cusid)
        {
            Cablehistory history = CablehistoryDao.FindFirst(new EqExpression("Cableid", cableid), 
                                                             new EqExpression("Customerid", cusid),
                                                             new NotNullExpression("Default1"));
            if (history != null)
            {
                //DateTime date = DateTime.Parse(history.Default1).Date;
                return history.Default1.Value.Date;
            }
            else
            {
                return null;
            }
        }
        #endregion

        #region 根据电路ID查询现在最大的Default1的值
        public static string getMaxDefault1ValueByCableid(int id)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "Select Max(default1) from cablehistory where cableid=" + id + "";
                    DataSet ds = db.GetDataSet(sql);
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    {
                        return ds.Tables[0].Rows[0][0].ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
            }
            catch
            {
                return "";
            }
        }
        #endregion
    }
}
