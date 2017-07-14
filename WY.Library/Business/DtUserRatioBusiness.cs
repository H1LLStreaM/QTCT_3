using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using Library.Dao;
using WY.Common.Data;
using WY.Common.Message;

namespace WY.Library.Business
{
    public class DtUserRatioBusiness
    {
        public static bool delAll()
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "delete from dt_UserRatio";
                    db.ExecuteNonQuery(sql);
                    return true;
                }
            }
            catch (Exception ex)
            {               
                return false;
            }
        }

        public static Dt_userratio[] Query()
        {
            try
            {
                Dt_userratio[] arr = Dt_userratioDao.FindAll();
                return arr;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
