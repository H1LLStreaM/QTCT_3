using System;
using System.Collections.Generic;
using System.Text;
using Library.Model;
using Library.Dao;
using WY.Common.Utility;
using WY.Common.Message;
using NHibernate.Expression;

namespace WY.Library.Business
{
    public class DataManageBusiness
    {
        #region ��ȡ������ɱ�����Ϣ
        public static Datamanage[] getbyBusinessId(int businessId)
        {
            Datamanage[] ds = DatamanageDao.FindAll(new Order("Sortindex", true), new EqExpression("Businessid", businessId));
            return ds;
        }

       
        #endregion

        #region ����
        public static void save(Datamanage[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                list[i].Save();
            }
        }
        #endregion

        #region ����
        public static void update(Datamanage[] list)
        {
            for (int i = 0; i < list.Length; i++)
            {
                list[i].Update();
            }
        }
        #endregion

        #region ��ȡ��·����
        public static Datamanage[] getAllCableRatio()
        {
            try
            {
                Datamanage[] ds = DatamanageDao.FindAll(new EqExpression("Usertype",-1));
                return ds;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "��ȡ��·���������Ϣ��������");
                return null;
            }
        }

        //public static decimal getCableRatio(string name)
        //{
        //    //try
        //    //{
        //    //    Datamanage ds = DatamanageDao.FindFirst(new EqExpression("Dataname", name));
        //    //    return ds.Datavalue;
        //    //}
        //    //catch (Exception ex)
        //    //{
        //    //    Log.Error(ex.Message);
        //    //    MessageHelper.ShowMessage("E999", "��ȡ��·���������Ϣ��������");
        //    //    return 0;
        //    //}
        //}
        #endregion

        #region �����˵���ʽ��Ϣ
        public static bool UpExcelFormat(Excelset set)
        {
            try
            {
                set.Update();
                return true;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                //MessageHelper.ShowMessage("E999", ds.Dataname + "����ʧ��");
                return false;
            }
        }
        #endregion

        #region ��ȡĬ����ɱ���
        public static Datamanage getDefaultValue(string str, int businesstypeid)
        {
            Datamanage dm = DatamanageDao.FindFirst(new EqExpression("Dataname", str), new EqExpression("Businessid", businesstypeid));
            return dm;
        }
        #endregion
    }
}
