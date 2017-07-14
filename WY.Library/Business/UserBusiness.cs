using System;
using System.Collections.Generic;
using System.Text;
using WY.Library.Model;
using System.Collections;
using WY.Library.Dao;
using NHibernate.Expression;
using NHibernate.Mapping;
using WY.Common.Utility;
using WY.Common.Message;
using System.Data;
using WY.Common.Data;
using System.Data.Common;
using WY.Common;

namespace WY.Library.Business
{
    public class UserBusiness
    {
        #region ����������ѯ�û���Ϣ
        /// <summary>
        /// ��ѯ����
        /// </summary>
        /// <param name="hs"></param>
        /// <returns></returns>
        public static TB_User[] findUserByCondition(Hashtable hs)
        {
            try
            {
                List<ICriterion> list = new List<ICriterion>();
                //List<EqExpression> list = new List<EqExpression>();
                //List<LikeExpression> list2 = new List<LikeExpression>();
                if (hs.ContainsKey("Name"))
                {
                    list.Add(new LikeExpression("Name", "%" + hs["Name"] + "%"));
                }
                if (hs.ContainsKey("Logid"))
                {
                    list.Add(new EqExpression("LogName", hs["Logid"]));
                }
                if (hs.ContainsKey("role"))
                {
                    list.Add(new EqExpression("Role", hs["role"]));
                }
                if (hs.ContainsKey("Isdeleted"))
                {
                    list.Add(new EqExpression("Isdeleted", hs["Isdeleted"]));
                }
                TB_User[] users = TB_UserDao.FindAll(list.ToArray());
                return users;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999");
                return null;
            }
        }
        #endregion

        #region �жϹ����Ƿ��ظ�
        public static int isRepeatUserId(string number,int id)
        {
            try
            {
                TB_User[] user = TB_UserDao.FindAll(new EqExpression("LogName", number),new NotExpression(new EqExpression("Id",id)));
                return user.Length;
            }
            catch(Exception ex)
            {
                MessageHelper.ShowMessage("E999", "�������ݷ�������");
                return -1;
            }
        }
        #endregion

        #region ����ID��ѯ�û���Ϣ
        public static TB_User findUserById(int uid)
        {
            try
            {
                TB_User user = TB_UserDao.FindFirst(new EqExpression("Id", uid));
                return user;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999");
                return null;
            }
        }
        #endregion

        #region ɾ���û�(�߼�)
        public void delUser(TB_User user)
        {
            try
            {
                //user.Isdeleted = (int)EnmIsdeleted.��ɾ��;
                user.Update();
                MessageHelper.ShowMessage("I002");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999");
            }
        }
        #endregion

        #region �����û���Ϣ
        public static void updateUserInfo(TB_User user)
        {
            try
            {
                user.Update();
                //MessageHelper.ShowMessage("I001");
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999");
            }
        }
        #endregion

        #region ��ȡ�����û���Ϣ
        public static DataTable getAllUsers()
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select id,name from tb_user where isDeleted=@del";
                    DbParameter[] paramlist = {db.CreateParameter("@del",(int)EnmIsdeleted.ʹ����) };
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  //������������
                    return tb;
                }
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return new DataTable();
            }
        }

        public static DataTable getAllUsersForRatio()
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select id,CONCAT(logName,'(',name,')') as name from tb_user where isDeleted=@del";
                    DbParameter[] paramlist = { db.CreateParameter("@del", (int)EnmIsdeleted.ʹ����) };
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  //������������
                    return tb;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", ex.Message);
                return new DataTable();
            }
        }
        #endregion

        #region ��ȡ�����������깤¼����Ϣ
        public static TB_User[] getAllSalersAndWrite()
        {
            try
            {
                Order[] orders = { new Order("Id", true), new Order("Role", true) }; //�������
                TB_User[] salers = TB_UserDao.FindAll(orders,new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����), 
                                                      new OrExpression(new EqExpression("Role", (int)EnmUserRole.��������), 
                                                      new EqExpression("Role", (int)EnmUserRole.¼����Ա)));
                return salers;
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "��Ա��Ϣ��ȡ��������");
                return null;
            }
        }
        #endregion

        #region ��ȡ�����������깤¼����Ϣ
        public static TB_User[] getAllWrite()
        {
            try
            {
                Order[] orders = { new Order("Id", true), new Order("Role", true) };
                TB_User[] salers = TB_UserDao.FindAll(orders, new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����), new EqExpression("Role", (int)EnmUserRole.¼����Ա));
                return salers;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "��Ա��Ϣ��ȡ��������");
                return null;
            }
        }
        #endregion

        #region ��ȡ��������
        public static TB_User[] getAllSalers()
        {
            try
            {
                TB_User[] salers = TB_UserDao.FindAll(new Order("Id",true),new EqExpression("Isdeleted", (int)EnmIsdeleted.ʹ����), new EqExpression("Role", (int)EnmUserRole.��������));
                return salers;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "����������Ϣ��ȡ��������");
                return null;
            }
        }
        #endregion

        #region �жϵ�¼����
        public static bool checkPwd(int id, string pwd)
        {
            TB_User user = TB_UserDao.FindFirst(new EqExpression("Id", id), new EqExpression("Password", DES.Encode(pwd,Global.DB_PWDKEY)));
            if (user != null)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region �����¼����
        public static bool changePwd(int id, string newPwd)
        {
            try
            {
                TB_User user = TB_UserDao.FindFirst(new EqExpression("Id", id));
                user.PASSWORD = DES.Encode(newPwd, Global.DB_PWDKEY);
                user.Update();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        
    }
}
