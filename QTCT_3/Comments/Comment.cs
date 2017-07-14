using NHibernate.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WY.Common.Message;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.Comments
{
    public class Comment
    {
        /// <summary>
        /// 线路工程查询
        /// </summary>
        /// <param name="objectName">工程名称</param>
        /// <param name="leaderName">工程负责人</param>
        /// <param name="projectType">工程类型</param>
        /// <param name="start">工期开始时间</param>
        /// <param name="end">工期结束时间</param>
        /// <returns></returns>
        public static List<TB_PROJECT> QueryProject(int projID, string leader, string projectType, DateTime? start = null, DateTime? end = null)
        {
            List<TB_PROJECT> list = new List<TB_PROJECT>();
            try
            {
                List<ICriterion> IClist = new List<ICriterion>();
                IClist.Add(new EqExpression("STATUS", 1));
                if (projID>0)
                {
                    IClist.Add(new LikeExpression("Id", projID));
                }
                if (!string.IsNullOrEmpty(leader))
                {
                    IClist.Add(new LikeExpression("TEAMLEDER", leader));
                }
                if (!string.IsNullOrEmpty(projectType))
                {
                    IClist.Add(new LikeExpression("OBJECTTYPENAME", "%"+projectType+"%"));
                }
                if (start != null && end!=null)
                {
                    BetweenExpression betw1 = new BetweenExpression("BEGINDATE", start,end);//(new GeExpression("BEGINDATE", start), new LeExpression("ENDDATE", end));
                    //BetweenExpression betw2 = new BetweenExpression("ENDDATE", start, end);
                    IClist.Add(betw1);
                }
                TB_PROJECT[] arr = TB_PROJECTDAO.FindAll(IClist.ToArray());
                if (arr!=null && arr.Length > 0)
                {
                    list = new List<TB_PROJECT>(arr);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
            return list;
        }


        /// <summary>
        /// 线路工程查询
        /// </summary>
        /// <param name="objectName">工程名称</param>
        /// <param name="leaderName">工程负责人</param>
        /// <param name="projectType">工程类型</param>
        /// <param name="start">工期开始时间</param>
        /// <param name="end">工期结束时间</param>
        /// <returns></returns>
        public static List<TB_PROJECT> QueryProject(string projName, string leader, string projectType, DateTime? start = null, DateTime? end = null)
        {
            List<TB_PROJECT> list = new List<TB_PROJECT>();
            try
            {
                List<ICriterion> IClist = new List<ICriterion>();
                IClist.Add(new EqExpression("STATUS", 1));
                if (!string.IsNullOrEmpty(projName))
                {
                    IClist.Add(new LikeExpression("OBJECTNAME", "%" + projName + "%"));
                }
                if (!string.IsNullOrEmpty(leader))
                {
                    IClist.Add(new LikeExpression("TEAMLEDER", leader));
                }
                if (!string.IsNullOrEmpty(projectType))
                {
                    IClist.Add(new LikeExpression("OBJECTTYPENAME", "%" + projectType + "%"));
                }
                if (start != null && end != null)
                {
                    BetweenExpression betw1 = new BetweenExpression("BEGINDATE", start, end);//(new GeExpression("BEGINDATE", start), new LeExpression("ENDDATE", end));
                    //BetweenExpression betw2 = new BetweenExpression("ENDDATE", start, end);
                    IClist.Add(betw1);
                }
                TB_PROJECT[] arr = TB_PROJECTDAO.FindAll(IClist.ToArray());
                if (arr != null && arr.Length > 0)
                {
                    list = new List<TB_PROJECT>(arr);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
            return list;
        }

        /// <summary>
        /// 报销查询
        /// </summary>
        /// <param name="expenseType">报销类型 0：项目  1：个人</param>
        /// <param name="objectId">项目ID 为个人报销项目值为0</param>
        /// <param name="expenseType2">报销类型(餐费，旅行费等)</param>
        /// <param name="start">查询开始时间</param>
        /// <param name="end">查询结束时间</param>
        /// <returns></returns>
        public static List<TB_EXPENSE> QueryExpense(Object obj,int expenseType, int objectId, int expenseType2, int year,int month)
        {
            List<TB_EXPENSE> list = new List<TB_EXPENSE>();
            try
            {
                List<ICriterion> IClist = new List<ICriterion>();
                if (obj!=null)
                    IClist.Add(new EqExpression("OPNAME", (obj as TB_User).USER_CODE));
                IClist.Add(new EqExpression("STATUS", 1));
                IClist.Add(new EqExpression("OBJECTID", objectId));
                IClist.Add(new EqExpression("YEAR", year));
                IClist.Add(new EqExpression("MONTH", month));
                IClist.Add(new EqExpression("ISCOMPLETE", 1)); //只能看到已提交的报销
                if (expenseType2 > 0)
                {
                    IClist.Add(new EqExpression("EXPENSTYPE", expenseType2));
                }
                TB_EXPENSE[] arr = TB_EXPENSEDAO.FindAll(IClist.ToArray());
                if (arr!=null && arr.Length > 0)
                {
                    list = new List<TB_EXPENSE>(arr);
                }
                for (int i = 0; i < list.Count; i++)
                {
                    if (list[i].ISMEMBER == 1)
                        list[i].memeberstatus = "◎";
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
            return list;
        }

        
        /// <summary>
        /// 报销查询
        /// </summary>
        /// <param name="usercode">工号</param>
        /// <param name="expenseType">报销类型</param>
        /// <param name="objectId">项目ID</param>
        /// <param name="start">开始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns></returns>
        public static List<TB_EXPENSE> QueryExpense2(string usercode, int expenseType, int objectId,DateTime? start = null, DateTime? end = null)
        {
            List<TB_EXPENSE> list = new List<TB_EXPENSE>();
            try
            {
                List<ICriterion> IClist = new List<ICriterion>();
                IClist.Add(new EqExpression("STATUS", 1));
                if(!string.IsNullOrEmpty(usercode))
                    IClist.Add(new EqExpression("OPNAME", usercode));               
                if (objectId>0)
                    IClist.Add(new EqExpression("OBJECTID", objectId));
                if (expenseType > 0)
                    IClist.Add(new EqExpression("EXPENSTYPE", expenseType));
                if (start != null && end != null)
                {
                    BetweenExpression and1 = new BetweenExpression("CREATEDATE", start, end);
                    IClist.Add(and1);
                }
                TB_EXPENSE[] arr = TB_EXPENSEDAO.FindAll(IClist.ToArray());
                if (arr != null && arr.Length > 0)
                {
                    list = new List<TB_EXPENSE>(arr);
                }
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
            return list;
        }

        public static string setProjIdentity(string identity)
        {
            string rtn = "";
            if (!string.IsNullOrEmpty(identity))
            {
                string[] arr = identity.Split('|');
                if (arr.Length > 0)
                {
                    for (int i = 0; i < arr.Length; i++)
                    { 
                        string str = arr[i];
                        switch (str)
                        { 
                            case "yn":
                                rtn += "院内,";
                                break;
                            case "yw":
                                rtn += "院外,";
                                break;
                            case "wl":
                                rtn += "网络,";
                                break;
                            case "zx":
                                rtn += "专线,";
                                break;
                            case "wx":
                                rtn += "卫星,";
                                break;
                            case "wifi":
                                rtn += "无线,";
                                break;
                            case "3D":
                                rtn += "3D,";
                                break;
                            case "2D":
                                rtn += "2D,";
                                break;
                            case "xwj":
                                rtn += "显微镜,";
                                break;
                            case "qj":
                                rtn += "腔镜,";
                                break;
                            case "jrl":
                                rtn += "介入类,";
                                break;
                            case "kfl":
                                rtn += "开放类,";
                                break;
                            case "fhl":
                                rtn += "复合类,";
                                break;
                            case "qt":
                                rtn += "其他,";
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            return rtn;
        }
    }
}
