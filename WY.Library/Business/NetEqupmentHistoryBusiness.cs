using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using Library.Dao;
using NHibernate.Expression;

namespace WY.Library.Business
{
    public class NetEqupmentHistoryBusiness
    {
        public static Dt_salermoney Query(int year, int month, string shebeihao)
        {
            try
            {
                Dt_salermoney d = Dt_salermoneyDao.FindFirst(new EqExpression("Year", year), new EqExpression("Month", month), new EqExpression("Shebeihao", shebeihao));
                return d;
            }
            catch
            {
                return null; 
            }
        }

        public static Dt_equpmargin[] Query(int Eqid)
        {
            try
            {
                Dt_equpmargin[] d = Dt_equpmarginDao.FindAll(new Order("Createtime", true), new EqExpression("Eqid", Eqid));
                return d;
            }
            catch
            {
                return null;
            }
        }
    }
}
