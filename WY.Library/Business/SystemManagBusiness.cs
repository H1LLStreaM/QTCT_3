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
        #region 保存/更新Excel读取格式
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
                    newex.Isdeleted = (int)EnmIsdeleted.使用中;
                    newex.Create();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", value + "设置保存失败");
            }
        }
        #endregion

        #region 读取所有Excel设置
        public static void readAllExcelSettings()
        {
            try
            {
                Excelset[] ex = ExcelsetDao.FindAll();
                for (int i = 0; i < ex.Length; i++)
                {
                    switch (ex[i].Excelkey)
                    {
                        case "起始行数:":
                            Global.row_Startline = ex[i].Point;
                            break;
                        case "合同编号:":
                            Global.col_contractnumber = ex[i].Point;
                            break;
                        case "电路代码:":
                            Global.col_Calbenumber = ex[i].Point;
                            break;
                        case "原电路代码:":
                            Global.col_exCalbenumber = ex[i].Point;
                            break;
                        case "客户名称:":
                            Global.col_Cusname = ex[i].Point;
                            break;
                        case "原速率:":
                            Global.col_Speed = ex[i].Point;
                            break;
                        case "现速率:":
                            Global.col_exSpeed = ex[i].Point;
                            break;
                        case "电路性质:":
                            Global.col_cableNature = ex[i].Point;
                            break;
                        case "通信范围:":
                            Global.col_Range = ex[i].Point;
                            break;
                        case "完工日期:":
                            Global.col_Completedate = ex[i].Point;
                            break;
                        case "拆机日期:":
                            Global.col_Removedate = ex[i].Point;
                            break;
                        case "结算起始日期:":
                            Global.col_Settlementstart = ex[i].Point;
                            break;
                        case "结算截止日期:":
                            Global.col_Settlementend = ex[i].Point;
                            break;
                        case "应收月租费:":
                            Global.col_Receivable = ex[i].Point;
                            break;
                        case "销帐金额:":
                            Global.col_Writeoff = ex[i].Point;
                            break;
                        case "结算比例:":
                            Global.col_Ratio = ex[i].Point;
                            break;
                        case "代理费:":
                            Global.col_Proxy = ex[i].Point;
                            break;
                        case "销帐情况:":
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

        #region 获取系统默认设置提成比例
        public static DataTable getRatio(int businesstypeid)
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select * from datamanage where businessid=@bid and isdeleted=" + (int)EnmIsdeleted.使用中 + "";
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
