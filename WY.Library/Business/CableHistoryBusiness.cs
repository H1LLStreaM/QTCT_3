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
        #region ��������ʷ��Ϣ
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

        #region �жϵ�·�����Ƿ�Ϊ����
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
                Cablehistory[] historys = CablehistoryDao.FindAll(new Order("Createtime",false),new EqExpression("Cableid", id), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
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
                DbParameter[] paramlist = { db.CreateParameter("@id", id), db.CreateParameter("@Isdeleted", (int)EnmIsdeleted.ʹ����) };
                DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  //������������
                return Utils.NvInt(tb.Rows[0][0].ToString());
            }
            return 0;
        }

        #region ����ID��ѯ��·����
        public static DataTable getCableByCableid(int cableId)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    string sql = "select b.id,b.createTime as ����,customername as �ͻ�����,cablenumber as EIP���, "
                    + "cablestatus as ״̬,b.userid as ����,c.name as ��ϵ��,BoxNumber as ���ӱ��,"
                    + "tel as ��ϵ�绰,c.address as �ͻ���ַ,PackageInfo as �ײ�����,money as �����,"
                    + "CompleteTime as �깤����,b.remark as ��ע,b.address1 as �׶˵�ַ,b.address2 as �Ҷ˵�ַ,b.Person1 as �׶���ϵ��,b.Person2 as �Ҷ���ϵ�� "
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

        #region ������ʷ��¼ƥ���˵���Ϣ
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
                    DateTime start = DateTime.Parse(historys[i].Startdate.ToString());  //��ͬ��ʼ����
                    DateTime end = DateTime.Parse(historys[i].Enddate.ToString());   //��ͬ��������
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
        /// �ж�ʱ��
        /// </summary>
        /// <param name="start">��ͬ��ʼ����</param>
        /// <param name="end">��ͬ��ֹ����</param>
        /// <param name="startDate">�˵���ʼ����</param>
        /// <param name="endDate">�˵���ֹ����</param>
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
        /// �ж�ʱ��
        /// </summary>
        /// <param name="start">��ͬ��ʼ����</param>
        /// <param name="end">��ͬ��ֹ����</param>
        /// <param name="startDate">�˵���ʼ����</param>
        /// <param name="endDate">�˵���ֹ����</param>
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


        #region ���ݵ�·�����ѯ��ʷ��¼
        public static DataTable getCableByCableNumber(string cablenumber)
        {
            using (DbHelper db = new DbHelper())
            {
                try
                {
                    string sql = "select b.id as ��·��ʷid,c.id as �ͻ�id,c.cableid,b.createTime as ����,customername as �ͻ�����,"
                                + "cablenumber as EIP���,cablestatus as ״̬,b.userid as ����,c.name as ��ϵ��,BoxNumber as ���ӱ��,"
                                + "tel as ��ϵ�绰,c.address as �ͻ���ַ,PackageInfo as �ײ�����,money as �����,"
                                + "CompleteTime as �깤����,b.remark as ��ע,b.address1 as �׶˵�ַ,b.address2 as �Ҷ˵�ַ "
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

        #region ���ݵ�·ID���ͻ�ID�ж��Ƿ���ȷ
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

        #region ���ݵ�·ID�Ϳͻ�ID��ѯǨ��ʱ��
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

        #region ���ݵ�·ID��ѯ��������Default1��ֵ
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
