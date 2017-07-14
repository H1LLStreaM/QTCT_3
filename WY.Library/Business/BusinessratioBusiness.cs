using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using Library.Dao;
using NHibernate.Expression;

namespace WY.Library.Business
{ 
    /// <summary>
    /// 账单结算比例
    /// </summary>
    public class BusinessratioBusiness
    {
        public static Businessratio[] getByTypeId(int typeid)
        {
            Businessratio[] brs = BusinessratioDao.FindAll(new EqExpression("Businesstypeid", typeid), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
            return brs;
        }

        public static Businessratio getByTypeId(int typeid,string cablestatus)
        {
            //string strCableStatus = GlobalBusiness.getCableStatus(cablestatus);
            Businessratio brs = BusinessratioDao.FindFirst(new EqExpression("Businesstypeid", typeid), new EqExpression("Dataname", cablestatus), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
            return brs;
        }

        public static void save(Businessratio[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                list[i].Save();
            }
        }

        public static void update(Businessratio[] brs)
        {
            for (int i = 0; i < brs.Length; i++)
            {
                brs[i].Update();
            }
        }

       
    }
}
