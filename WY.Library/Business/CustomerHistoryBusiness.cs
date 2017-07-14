using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using WY.Common.Utility;
using WY.Common.Message;
using Library.Dao;
using NHibernate.Expression;
using System.Data;
using WY.Common.Data;
using System.Data.Common;
using Castle.ActiveRecord;
using WY.Common.Framework;
using WY.Common;
using WY.Library.Model;
using System.Collections;

namespace WY.Library.Business
{
    public class CustomerHistoryBusiness
    {
        public static void save(Customerhistory history)
        {
            history.Save();
        }

        public static Customerhistory[] getHistory(int customerid)
        {
            Customerhistory[] historys = CustomerhistoryDao.FindAll(new EqExpression("Customerid", customerid));
            return historys;
        }

        public static bool isCustomerExis(int Customerid, string Customername)
        {
            Customerhistory his = CustomerhistoryDao.FindFirst(new EqExpression("Customerid", Customerid), 
                                                               new EqExpression("Customername", Customername), 
                                                               new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
            if (his != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int getHistoryByCustomername(string Customername)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "Select c.id from customerhistory as ch join customer as c "
                               + "on c.id=ch.customerid where c.isdeleted=" + (int)EnmIsdeleted.使用中 + " "
                               + "and ch.customername='" + Customername + "'";
                    DataSet ds = db.GetDataSet(sql);
                    return Utils.NvInt(ds.Tables[0].Rows[0][0].ToString());
                }
            }
            catch
            {
                return 0;
            }
        }
    }
}
