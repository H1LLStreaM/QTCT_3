using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using Library.Dao;
using NHibernate.Expression;
using WY.Common.Utility;
using WY.Common.Message;
using WY.Common.Data;
using System.Data.Common;

namespace WY.Library.Business
{
    public class NetRatioBusiness
    {
        /// <summary>
        /// 获取所有提成比例
        /// </summary>
        /// <returns></returns>
        public static Dt_commission[] query()
        {
            try
            {
                return Dt_commissionDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 获取所有提成比例
        /// </summary>
        /// <returns></returns>
        public static Dt_commission queryById(int salerId)
        {
            try
            {
                Dt_userratio ur = Dt_userratioDao.FindFirst(new EqExpression("Salerid", salerId));
                if (ur != null)
                {
                    return Dt_commissionDao.FindFirst(new EqExpression("Id", ur.Ratio));
                }
                else return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return null;
            }
        }



        #region 保存电路代码
        public static int cableSave(Dt_commission c)
        {
            try
            {
                c.Save();
                return c.Id;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return -1;
            }
        }
        #endregion

        #region 更新电路代码(更新后需要提示启用日期)
        public static void cableUpdate(Dt_commission c)
        {
            try
            {
                if (c.Updatetime == null)
                {
                    c.Updatetime = DateTime.Now;
                }
                c.Update();
                using (DbHelper db = new DbHelper())
                {
                    string sql = "Update cable set updateTime=@time where id=@id";
                    DbParameter[] parmar = { db.CreateParameter("@time", c.Updatetime), db.CreateParameter("@id", c.Id) };
                    db.ExecuteNonQuery(sql, parmar);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
        #endregion
    }
}
