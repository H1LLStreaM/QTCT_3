using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WY.Common.Data;
using WY.Library.Dao;
using WY.Library.Model;

namespace WY.Library.Business
{
    public class RatioBusiness
    {
        public static bool delete(int projectid)
        {
            bool rtn = true;
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    string sql = "Update TB_RATIO SET STATUS=0 WHERE PROJECTID= " + projectid;
                    db.ExecuteNonQuery(sql);
                }
                catch (Exception ex)
                {
                    rtn = false;
                }
            }
            return rtn;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="list"></param>
        /// <param name="mlv"></param>
        /// <returns></returns>
        public static decimal QueryRatio(pts_proj_ratio[] list, decimal mlv)
        {
            decimal rtn = 0;
            try 
            {
                for (int i = 0; i < list.Length; i++)
                {
                    decimal key1 = decimal.Parse(list[i].KEY1);
                    decimal key2 = decimal.Parse(list[i].KEY2);
                    if (key1 < key2)
                    {
                        if (mlv < key2 &&mlv >= key1)
                        {
                            return decimal.Parse(list[i].RATIO);
                        }
                    }
                    else if (key2 < key1)
                    {
                        if (mlv < key1 && mlv >= key2)
                        {
                            return decimal.Parse(list[i].RATIO);
                        }
                    }
                    else
                    {
                        return rtn;
                    }
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 项目成本费用
        /// </summary>
        /// <param name="money">合同金额</param>
        /// <returns></returns>
        public static decimal QueryProjCost(decimal money)
        {
            decimal rtn = 0;
            try
            {
                PTS_PROJ_COST[] list = PTS_PROJ_COSTDAO.FindAll();
                for (int i = 0; i < list.Length; i++)
                {
                    decimal key1 = decimal.Parse(list[i].KEY1);
                    decimal key2 = decimal.Parse(list[i].KEY2);
                    if (key1 < key2)
                    {
                        if (money < key2 && money >= key1)
                        {
                            return decimal.Parse(list[i].COST);
                        }
                    }
                    else if (key2 < key1)
                    {
                        if (money < key1 && money >= key2)
                        {
                            return decimal.Parse(list[i].COST);
                        }
                    }
                    else
                    {
                        return rtn;
                    }
                }
                return rtn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
