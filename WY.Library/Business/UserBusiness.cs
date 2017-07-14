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
        #region 根据条件查询用户信息
        /// <summary>
        /// 查询条件
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

        #region 判断工号是否重复
        public static int isRepeatUserId(string number,int id)
        {
            try
            {
                TB_User[] user = TB_UserDao.FindAll(new EqExpression("LogName", number),new NotExpression(new EqExpression("Id",id)));
                return user.Length;
            }
            catch(Exception ex)
            {
                MessageHelper.ShowMessage("E999", "保存数据发生错误。");
                return -1;
            }
        }
        #endregion

        #region 根据ID查询用户信息
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

        #region 删除用户(逻辑)
        public void delUser(TB_User user)
        {
            try
            {
                //user.Isdeleted = (int)EnmIsdeleted.已删除;
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

        #region 更新用户信息
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

        #region 获取所有用户信息
        public static DataTable getAllUsers()
        {
            try
            {
                using (DbHelper db = new DbHelper())
                {
                    string sql = "select id,name from tb_user where isDeleted=@del";
                    DbParameter[] paramlist = {db.CreateParameter("@del",(int)EnmIsdeleted.使用中) };
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  //销售渠道数据
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
                    DbParameter[] paramlist = { db.CreateParameter("@del", (int)EnmIsdeleted.使用中) };
                    DataTable tb = db.GetDataSet(sql, paramlist).Tables[0];  //销售渠道数据
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

        #region 获取所有渠道和完工录入信息
        public static TB_User[] getAllSalersAndWrite()
        {
            try
            {
                Order[] orders = { new Order("Id", true), new Order("Role", true) }; //排序规则
                TB_User[] salers = TB_UserDao.FindAll(orders,new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中), 
                                                      new OrExpression(new EqExpression("Role", (int)EnmUserRole.销售渠道), 
                                                      new EqExpression("Role", (int)EnmUserRole.录入人员)));
                return salers;
            }
            catch(Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "人员信息获取发生错误。");
                return null;
            }
        }
        #endregion

        #region 获取所有渠道和完工录入信息
        public static TB_User[] getAllWrite()
        {
            try
            {
                Order[] orders = { new Order("Id", true), new Order("Role", true) };
                TB_User[] salers = TB_UserDao.FindAll(orders, new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中), new EqExpression("Role", (int)EnmUserRole.录入人员));
                return salers;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "人员信息获取发生错误。");
                return null;
            }
        }
        #endregion

        #region 获取所有渠道
        public static TB_User[] getAllSalers()
        {
            try
            {
                TB_User[] salers = TB_UserDao.FindAll(new Order("Id",true),new EqExpression("Isdeleted", (int)EnmIsdeleted.使用中), new EqExpression("Role", (int)EnmUserRole.销售渠道));
                return salers;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                MessageHelper.ShowMessage("E999", "销售渠道信息获取发生错误。");
                return null;
            }
        }
        #endregion

        #region 判断登录密码
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

        #region 变更登录密码
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
