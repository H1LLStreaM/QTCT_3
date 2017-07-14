using System;
using System.Collections.Generic;
using System.Text;
using WY.Library.Model;
using WY.Library.Dao;
using NHibernate.Expression;
using WY.Common.Message;
using Library.Model;

namespace WY.Library.Business
{
    public class CableRatioBusiness
    {
        #region 删除
        public static bool delCableRatio(int id)
        {
            try
            {
                Businesstype type = BusinessTypeBusiness.getById(id);
                if (type != null)
                {
                    type.Isdeleted = (int)EnmIsdeleted.已删除;
                    type.Update();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                MessageHelper.ShowMessage("E999", "保存结算比例发生错误！");
                return false;
            }
        }
        #endregion

        #region 更新
        public static bool updata(int id,decimal ratio)
        {
            try
            {
                Cableratio cableratio = getById(id);
                cableratio.Ratio = ratio;
                cableratio.Update();
                return true;
            }
            catch
            {
                MessageHelper.ShowMessage("E999", "保存结算比例发生错误。");
                return false;
            }
        }
        #endregion

        #region 创建
        public static bool create(int cableclass, decimal ratio, string name)
        {
            try
            {
                Cableratio cableratio = new Cableratio();
                cableratio.Cableclass = cableclass;
                cableratio.Ratio = ratio;
                cableratio.Name = name;
                cableratio.Isdeleted = (int)EnmIsdeleted.使用中;
                cableratio.Create();
                return true;
            }
            catch
            {
                MessageHelper.ShowMessage("E999", "保存结算比例发生错误。");
                return false;
            }
        }
        #endregion

        #region 根据ID查询比例
        private static Cableratio getById(int id)
        {
            try
            {
                Cableratio ratio = CableratioDao.FindFirst(new EqExpression("Id", id));
                return ratio;
            }
            catch(Exception ex)
            {
                MessageHelper.ShowMessage("E999", "保存结算比例发生错误。");
                return null;
            }
        }
        #endregion

        #region 获取全部
        public static Cableratio[] getAll()
        {
            Cableratio[] ratio = CableratioDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
            return ratio;
        }
        #endregion

        #region 获取专网信息
        public static Cableratio[] getRatio()
        {
            Cableratio[] ratio = CableratioDao.FindAll(new EqExpression("Cableclass", (int)EnmCalbeClass.专网), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
            return ratio;
        }
        #endregion

        #region 获取上网信息
        public static Cableratio[] getRatio2()
        {
            Cableratio[] ratio = CableratioDao.FindAll(new EqExpression("Cableclass", (int)EnmCalbeClass.上网), new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中));
            return ratio;
        }
        #endregion
    }
}
