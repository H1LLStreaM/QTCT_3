using Castle.ActiveRecord;
using System;
using WY.Common;
using System.Reflection;
using System.Collections.Generic;
using System.Text;
using WY.Common.Utility;
using WY.Common.Framework;

namespace WY.Library.Model
{
    [Serializable]
    public class BaseModel : ActiveRecordBase
    {
        public override void Create()
        {
            this.setCreateField();

            base.Create();
        }

        public override void Update()
        {
            this.setUpdateField();

            base.Update();
        }

        public override void Save()
        {
            this.setUpdateField();

            base.Save();
        }

        private void setCreateField()
        {
            try
            {
                PropertyInfo[] props = this.GetType().GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if ("createtime".Equals(p.Name.ToLower())
                        || "updatetime".Equals(p.Name.ToLower()))
                    {
                        p.SetValue(this, TableManager.DBServerTime(), null);
                    }
                    else if ("createuserid".Equals(p.Name.ToLower())
                        || "updateuserid".Equals(p.Name.ToLower()))
                    {
                        p.SetValue(this, Global.g_userid, null);
                    }
                    else if ("createuser".Equals(p.Name.ToLower())
                        || "updateuser".Equals(p.Name.ToLower()))
                    {
                        p.SetValue(this, Global.g_username, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        private void setUpdateField()
        {
            try
            {
                PropertyInfo[] props = this.GetType().GetProperties();
                foreach (PropertyInfo p in props)
                {
                    if ("updatetime".Equals(p.Name.ToLower()))
                    {
                        p.SetValue(this, TableManager.DBServerTime(), null);
                    }
                    else if ("updateuserid".Equals(p.Name.ToLower()))
                    {
                        p.SetValue(this, Global.g_userid, null);
                    }
                    else if ("updateuser".Equals(p.Name.ToLower()))
                    {
                        p.SetValue(this, Global.g_username, null);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="type"></param>
        /// <param name="condition"></param>
        public static void Delete(Type type, string condition)
        {
            DeleteAll(type, condition);
        }

    }
}
