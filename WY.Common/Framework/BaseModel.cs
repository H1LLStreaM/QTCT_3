using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;
using WY.Common.Utility;
using System.Collections;

namespace WY.Common.Framework
{
    [Serializable]
    public class BaseModel<T>
        where T : new()
    {
        public string GetTableName()
        {
            return this.GetType().Name.ToUpper();
        }

        #region Select
        /// <summary>
        /// 默认PrimaryKey是ID，如果不是，则需重写该方法
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual T SelectByPrimaryKey(object value)
        {
            T[] list = Select("ID='" + value.ToString() + "'");
            if (list.Length > 0)
            {
                return list[0];
            }
            else
            {
                return default(T);
            }
        }

        public virtual T[] Select(string conditions)
        {
            string fields = "";

            Type t = this.GetType();
            PropertyInfo[] props = t.GetProperties();

            foreach (PropertyInfo p in props)
            {
                string fName = p.Name.ToUpper();
                fields += fName + ",";
            }
            fields = fields.Substring(0, fields.Length - 1);

            DataTable table = TableManager.Select(this.GetTableName(), fields, conditions);
            List<T> list = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T item = new T();
                BeanHelper.DataRowToModel(row, item);
                list.Add(item);
            }
            return list.ToArray();
        }

        public virtual T[] Select(params MyField[] paramfields)
        {
            string fields = "";

            Type t = this.GetType();
            PropertyInfo[] props = t.GetProperties();

            foreach (PropertyInfo p in props)
            {
                string fName = p.Name.ToUpper();
                fields += fName + ",";
            }
            fields = fields.Substring(0, fields.Length - 1);

            DataTable table = TableManager.Select(this.GetTableName(), fields, paramfields);
            List<T> list = new List<T>();
            foreach (DataRow row in table.Rows)
            {
                T item = new T();
                BeanHelper.DataRowToModel(row, item);
                list.Add(item);
            }
            return list.ToArray();
        }
        #endregion

        #region SelectOne
        public virtual T SelectOne(string conditions)
        {
            T[] list = Select(conditions);
            if (list.Length > 0)
            {
                return list[0];
            }
            else
            {
                return default(T);
            }
        }

        public virtual T SelectOne(params MyField[] paramfields)
        {
            T[] list = Select(paramfields);
            if (list.Length > 0)
            {
                return list[0];
            }
            else
            {
                return default(T);
            }
        }
        #endregion

        #region Create
        public virtual int Create()
        {
            Type t = this.GetType();
            PropertyInfo[] props = t.GetProperties();
            PropertyInfo auto_increase_field = null;

            List<MyField> fields = new List<MyField>();
            foreach (PropertyInfo p in props)
            {
                string fName = p.Name.ToUpper();
                if (!this.IsReadOnlyField(fName))
                {
                    if (fName.ToLower() == "createtime"
                        || fName.ToLower() == "updatetime")
                    {
                        DateTime now = TableManager.DBServerTime();
                        fields.Add(new MyField(fName, now, p.PropertyType));
                        p.GetSetMethod().Invoke(this, new Object[] { now });
                    }
                    else if (fName.ToLower() == "createuser"
                        || fName.ToLower() == "updateuser")
                    {
                        fields.Add(new MyField(fName, Global.g_username, p.PropertyType));
                        p.GetSetMethod().Invoke(this, new Object[] { Global.g_username });
                    }
                    else
                    {
                        Object val = p.GetGetMethod().Invoke(this, new Object[] { });
                        if (val == null)
                        {
                            fields.Add(new MyField(fName, DBNull.Value, p.PropertyType));
                        }
                        else
                        {
                            fields.Add(new MyField(fName, val, p.PropertyType));
                        }
                    }
                }
                else if (fName.ToUpper() == "ID")
                {
                    auto_increase_field = p;
                }
            }

            if (auto_increase_field != null)
            {
                int id;
                int ret = TableManager.Insert(this.GetTableName(), fields.ToArray(), out id);
                if (ret > 0)
                {
                    auto_increase_field.GetSetMethod().Invoke(this, new Object[] { id });
                }
                return ret;
            }
            else
            {
                return TableManager.Insert(this.GetTableName(), fields.ToArray());
            }
        }
        #endregion

        #region Update
        public virtual int Update()
        {
            List<MyField> datafields = new List<MyField>();
            List<MyField> paramsfileds = new List<MyField>();

            Type t = this.GetType();
            PropertyInfo[] props = t.GetProperties();

            DataTable schema = SchemaBuffer.getTableDef(this.GetTableName());
            foreach (PropertyInfo p in props)
            {
                string fName = p.Name.ToUpper();
                Object val = p.GetGetMethod().Invoke(this, new Object[] { });

                foreach (DataColumn dc in schema.PrimaryKey)
                {
                    if (dc.ColumnName.ToUpper() == fName)
                    {
                        paramsfileds.Add(new MyField(fName, val, p.PropertyType));
                        continue;
                    }
                }

                if (!this.IsReadOnlyField(fName))
                {
                    if (fName.ToLower() == "updatetime")
                    {
                        datafields.Add(new MyField(fName, TableManager.DBServerTime(), p.PropertyType));
                    }
                    else if (fName.ToLower() == "updateuser")
                    {
                        datafields.Add(new MyField(fName, Global.g_username, p.PropertyType));
                    }
                    else
                    {
                        if (val == null)
                        {
                            datafields.Add(new MyField(fName, DBNull.Value, p.PropertyType));
                        }
                        else
                        {
                            datafields.Add(new MyField(fName, val, p.PropertyType));
                        }
                    }
                }
            }

            return TableManager.Update(this.GetTableName(), datafields.ToArray(), paramsfileds.ToArray());
        }

        public virtual int Update(params MyField[] paramfileds)
        {
            List<MyField> datafields = new List<MyField>();

            Type t = this.GetType();
            PropertyInfo[] props = t.GetProperties();

            foreach (PropertyInfo p in props)
            {
                string fName = p.Name.ToUpper();
                Object val = p.GetGetMethod().Invoke(this, new Object[] { });

                if (!this.IsReadOnlyField(fName))
                {
                    if (fName.ToLower() == "updatetime")
                    {
                        datafields.Add(new MyField(fName, TableManager.DBServerTime(), p.PropertyType));
                    }
                    else if (fName.ToLower() == "updateuser")
                    {
                        datafields.Add(new MyField(fName, Global.g_username, p.PropertyType));
                    }
                    else
                    {
                        if (val == null)
                        {
                            datafields.Add(new MyField(fName, DBNull.Value, p.PropertyType));
                        }
                        else
                        {
                            datafields.Add(new MyField(fName, val, p.PropertyType));
                        }
                    }
                }
            }

            return TableManager.Update(this.GetTableName(), datafields.ToArray(), paramfileds);
        }
        #endregion

        #region Delete
        public virtual int Delete()
        {
            List<MyField> paramsfileds = new List<MyField>();

            Type t = this.GetType();
            PropertyInfo[] props = t.GetProperties();

            DataTable schema = SchemaBuffer.getTableDef(this.GetTableName());
            foreach (PropertyInfo p in props)
            {
                string fName = p.Name.ToUpper();
                Object val = p.GetGetMethod().Invoke(this, new Object[] { });

                foreach (DataColumn dc in schema.PrimaryKey)
                {
                    if (dc.ColumnName.ToUpper() == fName)
                    {
                        paramsfileds.Add(new MyField(fName, val));
                        continue;
                    }
                }
            }

            return TableManager.Delete(this.GetTableName(), paramsfileds.ToArray());
        }

        public virtual int Delete(params MyField[] paramfileds)
        {
            return TableManager.Delete(this.GetTableName(), paramfileds);
        }
        #endregion

        #region 不需要更新的字段
        /// <summary>
        /// 表中不需要更新的字段，默认：数据库自增字段ID，如还有其它，则需重写该方法
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetReadOnlyFields()
        {
            return new string[] { "ID" };
        }

        private bool IsReadOnlyField(string fldname)
        {
            string[] readonlyflds = GetReadOnlyFields();

            foreach (string fld in readonlyflds)
            {
                if (fld.ToUpper() == fldname.ToUpper())
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region ColComboList
        /// <summary>
        /// 生成列表下拉框数据
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="fieldname"></param>
        /// <param name="condition"></param>
        /// <param name="allowedit"></param>
        /// <returns></returns>
        public string ColComboList(string fieldname, string condition, params bool[] allowedit)
        {
            string strlist = "";
            //设置下拉列表是否可以手动修改值
            if (allowedit.Length > 0 && allowedit[0] == true)
            {
                strlist += "| ";
            }
            else
            {
                strlist += " ";
            }

            TableManager.SetOrder(fieldname);
            DataTable table = TableManager.Select(this.GetTableName(), "distinct " + fieldname, condition);
            foreach (DataRow row in table.Rows)
            {
                if (!string.IsNullOrEmpty(Utils.NvStr(row[0])))
                {
                    strlist += "|" + Utils.NvStr(row[0]);
                }
            }

            return strlist;
        }

        /// <summary>
        /// 生成列表下拉框数据（含全部选项）
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="fieldname"></param>
        /// <param name="condition"></param>
        /// <param name="alltext">默认：全部，可自定义</param>
        /// <param name="allowedit"></param>
        /// <returns></returns>
        public string ColComboList(string fieldname, string condition, string alltext, params bool[] allowedit)
        {
            string strlist = "";
            //设置下拉列表是否可以手动修改值
            if (allowedit.Length > 0 && allowedit[0] == true)
            {
                strlist += "| ";
            }
            else
            {
                strlist += " ";
            }

            //全部
            strlist += "|" + (string.IsNullOrEmpty(alltext) ? "全部" : alltext);

            TableManager.SetOrder(fieldname);
            DataTable table = TableManager.Select(this.GetTableName(), "distinct " + fieldname, condition);
            foreach (DataRow row in table.Rows)
            {
                if (!string.IsNullOrEmpty(Utils.NvStr(row[0])))
                {
                    strlist += "|" + Utils.NvStr(row[0]);
                }
            }

            return strlist;
        }
        #endregion

        #region GetDataMap

        public Hashtable GetHashtable(string valuefield, string textfield, string condition)
        {
            List<DictionaryEntry> list = GetDataMap(valuefield, textfield, condition);

            Hashtable ht = new Hashtable();
            foreach (DictionaryEntry item in list)
            {
                ht.Add(item.Key, item.Value);
            }
            return ht;
        }

        public List<DictionaryEntry> GetDataMap(string valuefield, string textfield, string condition)
        {
            if (valuefield.CompareTo(textfield) == 0)
            {
                TableManager.SetOrder(TableManager.GetParameterName(valuefield));
                DataTable table = TableManager.Select(this.GetTableName(), valuefield, condition);
                List<DictionaryEntry> ht = new List<DictionaryEntry>();
                foreach (DataRow row in table.Rows)
                {
                    if (Utils.NvStr(row[0]) != "")
                    {
                        ht.Add(new DictionaryEntry(Utils.NvStr(row[0]), Utils.NvStr(row[0])));
                    }
                }

                return ht;
            }
            else
            {
                TableManager.SetOrder(TableManager.GetParameterName(valuefield) + "," + TableManager.GetParameterName(textfield));
                DataTable table = TableManager.Select(this.GetTableName(), valuefield + "," + textfield, condition);
                List<DictionaryEntry> ht = new List<DictionaryEntry>();
                foreach (DataRow row in table.Rows)
                {
                    ht.Add(new DictionaryEntry(Utils.NvStr(row[0]), Utils.NvStr(row[1])));
                }

                return ht;
            }
        }

        #endregion

        #region GetDataMapDistinct

        public Hashtable GetHashtableDistinct(string valuefield, string textfield, string condition)
        {
            List<DictionaryEntry> list = GetDataMapDistinct(valuefield, textfield, condition);

            Hashtable ht = new Hashtable();
            foreach (DictionaryEntry item in list)
            {
                ht.Add(item.Key, item.Value);
            }
            return ht;
        }

        public List<DictionaryEntry> GetDataMapDistinct(string valuefield, string textfield, string condition)
        {
            if (valuefield.CompareTo(textfield) == 0)
            {
                TableManager.SetOrder(TableManager.GetParameterName(valuefield));
                DataTable table = TableManager.Select(this.GetTableName(), "distinct " + valuefield, condition);
                List<DictionaryEntry> maplist = new List<DictionaryEntry>();
                foreach (DataRow row in table.Rows)
                {
                    if (Utils.NvStr(row[0]) != "")
                    {
                        maplist.Add(new DictionaryEntry(Utils.NvStr(row[0]), Utils.NvStr(row[0])));
                    }
                }

                return maplist;
            }
            else
            {
                TableManager.SetOrder(TableManager.GetParameterName(valuefield) + "," + TableManager.GetParameterName(textfield));
                DataTable table = TableManager.Select(this.GetTableName(), "distinct " + valuefield + "," + textfield, condition);
                List<DictionaryEntry> maplist = new List<DictionaryEntry>();
                foreach (DataRow row in table.Rows)
                {
                    maplist.Add(new DictionaryEntry(Utils.NvStr(row[0]), Utils.NvStr(row[1])));
                }

                return maplist;
            }
        }

        #endregion

        #region Clone

        public T Clone()
        {
            T item = new T();
            BeanHelper.ObjectClone(this, item);
            return item;
        }

        #endregion
    }
}
