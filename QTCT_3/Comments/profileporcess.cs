using NHibernate.Expression;
using System;
using System.Collections.Generic;
using WY.Common.Message;
using WY.Common.Utility;
using WY.Library.Business;
using WY.Library.Dao;
using WY.Library.Model;

namespace QTCT_3.Comments
{
    /// <summary>
    /// 项目工程利润及个人提成金额计算
    /// </summary>
    public class profileporcess
    {
        /// <summary>
        /// 计算项目利润(公司)
        /// </summary>
        /// <param name="proj"></param>
        /// <returns></returns>
        public projProfileClass getProfile(TB_PROJECT proj)
        {
            projProfileClass profile = new projProfileClass();
            try
            {
                profile.projName = proj.OBJECTNAME;
                profile.saler = proj.CREATEUSER;
                profile.projDate = proj.BEGINDATE.ToShortDateString();
                profile.contractNumber = proj.CONTRACTNO;
                profile.hshtj = proj.MONEY;
                profile.whshtj = Math.Round(proj.MONEY * decimal.Parse("0.94"), 2);
                TB_BILL[] billarr = TB_BILLDAO.FindAll(new EqExpression("PROJECTID", proj.Id), new EqExpression("STATUS", 1));
                if (billarr.Length > 0)
                    profile.bills = billarr[0].BILLNUMBER;
                //报销
                TB_EXPENSE[] arr = TB_EXPENSEDAO.FindAll(new EqExpression("STATUS", 1), new EqExpression("OBJECTID", proj.Id));
                if (arr.Length > 0)
                {
                    List<TB_EXPENSE> ls = new List<TB_EXPENSE>(arr);
                    profile.expens = new List<TB_EXPENSE>();
                    profile.expens = ls;
                }
                if (arr.Length > 0)
                    profile.xmmlr = xmmlrProcess(new List<TB_EXPENSE>(arr), profile.whshtj);
                else
                    profile.xmmlr = profile.whshtj;
                //毛利润
                profile.mlv = 0;
                if (profile.whshtj > 0)
                    profile.mlv = Math.Round(profile.xmmlr / profile.whshtj * 100, 2);  //项目毛利润/未含税合同价(%)
                //项目净利润
                profile.xmjlr = Math.Round(profile.xmmlr * Utils.NvDecimal("0.8"), 2);
                //项目工程师提成比率
                profile.xmgcstc = xmgcstcProcess(profile.mlv);
                //项目工程师提成金额
                profile.xmgcstje = Math.Round(profile.xmjlr * profile.xmgcstc / 100, 2);
                //公司留存
                profile.gslc = Math.Round(profile.xmjlr - profile.xmgcstje, 2);
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
            return profile;
        }

        /// <summary>
        /// 计算项目毛利润
        /// </summary>
        /// <param name="ls"></param>
        /// <param name="money"></param>
        /// <returns></returns>
        private decimal xmmlrProcess(List<TB_EXPENSE> ls, decimal money)
        {
            decimal rtn = 0;
            //decimal cost = RatioBusiness.QueryProjCost(money);  //项目成本金额
            //TB_EXPENSE expense = new TB_EXPENSE();
            //expense.MONEY = cost;
            //expense.EXPENS = "项目成本";
            //ls.Add(expense);
            for (int i = 0; i < ls.Count; i++)
            {
                rtn += ls[i].MONEY;
            }
            rtn = money - rtn;
            return rtn;
        }

        /// <summary>
        /// 计算毛利率
        /// </summary>
        /// <returns></returns>
        private decimal xmgcstcProcess(decimal mlv)
        {
            decimal rtn = 0;
            rtn = RatioBusiness.QueryRatio(pts_proj_ratioDao.FindAll(), mlv);
            return rtn;
        }

        #region 计算个人提成金额

        public List<TB_PERSONAL_PROFILE> personalProcess(projProfileClass ppc, int projId)
        {
            decimal money = ppc.xmgcstje; //总项目工程师提成金额
            List<TB_PERSONAL_PROFILE> rtn = new List<TB_PERSONAL_PROFILE>();
            try
            {
                TB_PROJECT proj = TB_PROJECTDAO.FindFirst(new EqExpression("Id", projId));
                if (proj != null)
                {
                    TB_RATIO[] arr = TB_RATIODAO.FindAll(new EqExpression("PROJECTID", projId), new EqExpression("STATUS", 1));
                    if (arr.Length > 0)
                    {
                        decimal totalAmount = 0;
                        for (int i = 0; i < arr.Length; i++)
                        {
                            totalAmount += arr[i].RATIO;
                        }
                        //全局提成比率
                        decimal ratio1 = proj.RATIO1 / 100;
                        decimal ratio2 = proj.RATIO2 / 100;
                        //分配全局
                        decimal _money60 = Math.Round(money * ratio1, 2);
                        decimal _money40 = Math.Round(money * ratio2, 2);
                        decimal avgmoney60 = Math.Round(_money60 / arr.Length, 2);  //均分提成
                        //个人提成份额
                        for (int i = 0; i < arr.Length; i++)
                        {
                            TB_PERSONAL_PROFILE tpp = new TB_PERSONAL_PROFILE();
                            tpp.USERCODE = arr[i].USERCODE;
                            tpp.AMOUNT = arr[i].RATIO;
                            tpp.PROFILE1 = avgmoney60;
                            tpp.PROFILE2 = Math.Round(_money40 / totalAmount * arr[i].RATIO, 2);
                            rtn.Add(tpp);
                        }
                    }
                }
                else
                    return rtn;
            }
            catch (Exception ex)
            {
                MessageHelper.ShowMessage(ex.Message);
            }
            return rtn;
        }

        #endregion 计算个人提成金额
    }
}