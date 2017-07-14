using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using Library.Dao;
using NHibernate.Expression;
using WY.Common.Data;
using System.Data.Common;

namespace WY.Library.Business
{
    public class BusinessTypeBusiness
    {
        public static Businesstype[] getAllBusinessType()
        {
            Businesstype[] bs = BusinesstypeDao.FindAll(new Order("Businessclass",true), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
            return bs;
        }

        public static Businesstype getById(int id)
        {
            Businesstype bs = BusinesstypeDao.FindFirst(new EqExpression("Id",id) ,new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
            return bs;
        }
        public static void save(Businesstype bt)
        {
            bt.Save();           
        }
        public static void update(int id, string businessname)
        {
            Businesstype bs = getById(id);
            bs.Businessname = businessname;
            bs.Update();
        }

        public static int getIdByName(int businessclass, string businessname)
        {
            Businesstype bt = BusinesstypeDao.FindFirst(new EqExpression("Businessclass", businessclass), new EqExpression("Businessname", businessname),new EqExpression("Isdeleted",(int)EnmIsdeleted.ʹ����));
            return bt.Id;
        }

        #region ��ȡר����Ϣ
        public static Businesstype[] getRatio()
        {
            Businesstype[] ratio = BusinesstypeDao.FindAll(new EqExpression("Businessclass", (int)EnmCalbeClass.����), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
            return ratio;
        }
        #endregion

        #region ��ȡ������Ϣ
        public static Businesstype[] getRatio2()
        {
            Businesstype[] ratio = BusinesstypeDao.FindAll(new EqExpression("Businessclass", (int)EnmCalbeClass.ר��), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
            return ratio;
        }
        #endregion

        public static int getCountById(string businssname, int businessclass)
        {
            using (DbHelper db = new DbHelper())
            {
                string sql = "select count(1) from cable where CableClass=@class and ContractType=@type";
                DbParameter[] paramlist = { db.CreateParameter("@class", businessclass), db.CreateParameter("@type", businssname) };
                return int.Parse(db.GetDataSet(sql, paramlist).Tables[0].Rows[0][0].ToString());
            }
        }
    }
}
