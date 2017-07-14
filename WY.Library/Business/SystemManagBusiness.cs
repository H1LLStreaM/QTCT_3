using System;
using System.Collections.Generic;
using System.Text;
using WY.Common.Utility;
using WY.Common.Message;
using Library.Model;
using Library.Dao;
using NHibernate.Expression;
using WY.Common;
using System.Data;
using WY.Common.Data;
using System.Data.Common;

namespace WY.Library.Business
{
    public class SystemManagBusiness
    {
        #region ����/����Excel��ȡ��ʽ
        public static void saveExcelSetting(string value,int point)
        {
            try
            {
                Excelset ex = ExcelsetDao.FindFirst(new EqExpression("Excelkey",value));
                if (ex != null)
                {
                    ex.Point = point;
                    ex.Update();
                }
                else
                {
                    Excelset newex = new Excelset();
                    newex.Excelkey = value;
                    newex.Point = point;
                    newex.Isdeleted = (int)EnmIsdeleted.ʹ����;
                    newex.Create();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", value + "���ñ���ʧ��");
            }
        }
        #endregion

        #region ��ȡ����Excel����
        public static void readAllExcelSettings()
        {
            try
            {
                Excelset[] ex = ExcelsetDao.FindAll();
                for (int i = 0; i < ex.Length; i++)
                {
                    switch (ex[i].Excelkey)
                    {
                        case "��ʼ����:":
                            Global.row_Startline = ex[i].Point;
                            break;
                        case "��ͬ���:":
                            Global.col_contractnumber = ex[i].Point;
                            break;
                        case "��·����:":
                            Global.col_Calbenumber = ex[i].Point;
                            break;
                        case "ԭ��·����:":
                            Global.col_exCalbenumber = ex[i].Point;
                            break;
                        case "�ͻ�����:":
                            Global.col_Cusname = ex[i].Point;
                            break;
                        case "ԭ����:":
                            Global.col_Speed = ex[i].Point;
                            break;
                        case "������:":
                            Global.col_exSpeed = ex[i].Point;
                            break;
                        case "��·����:":
                            Global.col_cableNature = ex[i].Point;
                            break;
                        case "ͨ�ŷ�Χ:":
                            Global.col_Range = ex[i].Point;
                            break;
                        case "�깤����:":
                            Global.col_Completedate = ex[i].Point;
                            break;
                        case "�������:":
                            Global.col_Removedate = ex[i].Point;
                            break;
                        case "������ʼ����:":
                            Global.col_Settlementstart = ex[i].Point;
                            break;
                        case "�����ֹ����:":
                            Global.col_Settlementend = ex[i].Point;
                            break;
                        case "Ӧ�������:":
                            Global.col_Receivable = ex[i].Point;
                            break;
                        case "���ʽ��:":
                            Global.col_Writeoff = ex[i].Point;
                            break;
                        case "�������:":
                            Global.col_Ratio = ex[i].Point;
                            break;
                        case "�����:":
                            Global.col_Proxy = ex[i].Point;
                            break;
                        case "�������:":
                            Global.col_WriteoffStatus = ex[i].Point;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            { }
        }
        #endregion

        #region ��ȡϵͳĬ��������ɱ���
        public static DataTable getRatio(int businesstypeid)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select * from datamanage where businessid=@bid and isdeleted=" + (int)EnmIsdeleted.ʹ���� + "";
                    DbParameter[] paramlist = { db.CreateParameter("@bid",businesstypeid)};
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];
                    return tb;
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                return new DataTable();
            }
        }
        #endregion
    }
}
