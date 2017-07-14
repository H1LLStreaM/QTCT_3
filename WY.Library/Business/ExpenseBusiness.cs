using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using WY.Common.Data;
using WY.Common.Utility;
using WY.Library.Model;

namespace WY.Library.Business
{
    public class ExpenseBusiness
    {
        public static decimal getTotalExpense(int projectId)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    db.TrnStart();
                    string sql = "Select SUM(MONEY) From TB_EXPENSE WHERE OBJECTID="+projectId+" AND STATUS=1 AND ISCOMPLETE=1 AND LEADERRESPONSESTATUS=2 AND RESPONSESTATUS=2 ";
                    DataSet ds = db.GetDataSet(sql);
                    if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                    {
                        return Utils.NvDecimal(ds.Tables[0].Rows[0][0]);
                    }
                    else
                    {
                        return 0;
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }
    }
}
