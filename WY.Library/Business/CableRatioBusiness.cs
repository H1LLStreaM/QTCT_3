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
        #region ɾ��
        public static bool delCableRatio(int id)
        {
            try
            {
                Businesstype type = BusinessTypeBusiness.getById(id);
                if (type != null)
                {
                    type.Isdeleted = (int)EnmIsdeleted.��ɾ��;
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
                MessageHelper.ShowMessage("E999", "������������������");
                return false;
            }
        }
        #endregion

        #region ����
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
                MessageHelper.ShowMessage("E999", "������������������");
                return false;
            }
        }
        #endregion

        #region ����
        public static bool create(int cableclass, decimal ratio, string name)
        {
            try
            {
                Cableratio cableratio = new Cableratio();
                cableratio.Cableclass = cableclass;
                cableratio.Ratio = ratio;
                cableratio.Name = name;
                cableratio.Isdeleted = (int)EnmIsdeleted.ʹ����;
                cableratio.Create();
                return true;
            }
            catch
            {
                MessageHelper.ShowMessage("E999", "������������������");
                return false;
            }
        }
        #endregion

        #region ����ID��ѯ����
        private static Cableratio getById(int id)
        {
            try
            {
                Cableratio ratio = CableratioDao.FindFirst(new EqExpression("Id", id));
                return ratio;
            }
            catch(Exception ex)
            {
                MessageHelper.ShowMessage("E999", "������������������");
                return null;
            }
        }
        #endregion

        #region ��ȡȫ��
        public static Cableratio[] getAll()
        {
            Cableratio[] ratio = CableratioDao.FindAll(new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
            return ratio;
        }
        #endregion

        #region ��ȡר����Ϣ
        public static Cableratio[] getRatio()
        {
            Cableratio[] ratio = CableratioDao.FindAll(new EqExpression("Cableclass", (int)EnmCalbeClass.ר��), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
            return ratio;
        }
        #endregion

        #region ��ȡ������Ϣ
        public static Cableratio[] getRatio2()
        {
            Cableratio[] ratio = CableratioDao.FindAll(new EqExpression("Cableclass", (int)EnmCalbeClass.����), new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����));
            return ratio;
        }
        #endregion
    }
}
