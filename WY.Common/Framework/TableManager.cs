using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using WY.Common.Utility;
using WY.Common.Data;

namespace WY.Common.Framework
{
    public class TableManager
    {
        public const string PARAM_PREFIX = "@";
        public const string FIELD_PREFIX = "[";     //字段名和表名前缀
        public const string FIELD_SUFFIX = "]";     //字段名和表名后缀

        public const string CREATE_TIME = "createtime";
        public const string CREATE_USER = "createuser";
        public const string UPDATE_TIME = "updatetime";
        public const string UPDATE_USER = "updateuser";

        #region 排序
        private static string _order;
        /// <summary>
        /// 设置排序规则，仅下次调用Select或SelectByPage时有效
        /// </summary>
        /// <param name="order">字符串中不要包含ORDER BY</param>
        public static void SetOrder(string order)
        {
            //以第一次设定为准，忽略之后的设定
            if (string.IsNullOrEmpty(_order))
            {
                _order = order;
            }
        }
        #endregion

        #region 分组
        private static string _groupby;
        /// <summary>
        /// 设置分组规则，仅下次调用Select或SelectByPage时有效
        /// </summary>
        /// <param name="groupby"></param>
        public static void SetGroupBy(string groupby)
        {
            //以第一次设定为准，忽略之后的设定
            if (string.IsNullOrEmpty(_groupby))
            {
                _groupby = groupby;
            }
        }

        private static string _having;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="having"></param>
        public static void SetHaving(string having)
        {
            //以第一次设定为准，忽略之后的设定
            if (string.IsNullOrEmpty(_having))
            {
                _having = having;
            }
        }
        #endregion

        #region Select
        
        public static DataTable Select(string tablename, string fields, params MyField[] paramfields)
        {
            return Select(Global.g_db, tablename, fields, paramfields);
        }

        /// <summary>
        /// 如需排序，查询前调用SetOrder方法设置排序规则
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="fields"></param>
        /// <param name="paramfields"></param>
        /// <returns></returns>
        public static DataTable Select(DbHelper db, string tablename, string fields, params MyField[] paramfields)
        {
            StringBuilder sql = new StringBuilder();
            List<DbParameter> paramlist = new List<DbParameter>();
            sql.Append("SELECT " + fields + "\r\n");
            sql.Append("FROM " + getTablenameString(tablename) + "\r\n");
            sql.Append("WHERE 1=1 \r\n");
            foreach (MyField fld in paramfields)
            {
                if (fld.QueryMode == enmQueryMode.等于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + "=" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.不等于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + "<>" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.包含)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + " LIKE '%'+" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "+'%'\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.大于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + ">" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.小于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + "<" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "\r\n");
                }
                paramlist.Add(db.CreateParameter(GetParameterName(fld.FieldName), fld.FieldValue));
            }
            if (!string.IsNullOrEmpty(_groupby))
            {
                sql.Append("GROUP BY " + _groupby + "\r\n");

                //清空分组字段
                _groupby = string.Empty;
            }
            if (!string.IsNullOrEmpty(_having))
            {
                sql.Append("HAVING " + _having + "\r\n");

                //清空
                _having = string.Empty;
            }
            if (!string.IsNullOrEmpty(_order))
            {
                sql.Append("ORDER BY " + _order);

                //清空排序字段
                _order = string.Empty;
            }

            return db.GetDataSet(sql.ToString(), paramlist.ToArray()).Tables[0];
        }

        public static DataTable Select(string tablename, string fields, string condition)
        {
            return Select(Global.g_db, tablename, fields, condition);
        }

        /// <summary>
        /// 如需排序，查询前调用SetOrder方法设置排序规则
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="fields"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static DataTable Select(DbHelper db,string tablename, string fields, string condition)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT " + fields + "\r\n");
            sql.Append("FROM " + getTablenameString(tablename) + "\r\n");
            if (!string.IsNullOrEmpty(condition))
            {
                sql.Append("WHERE " + condition + "\r\n");
            }
            if (!string.IsNullOrEmpty(_groupby))
            {
                sql.Append("GROUP BY " + _groupby + "\r\n");

                //清空分组字段
                _groupby = string.Empty;
            }
            if (!string.IsNullOrEmpty(_having))
            {
                sql.Append("HAVING " + _having + "\r\n");

                //清空
                _having = string.Empty;
            }
            if (!string.IsNullOrEmpty(_order))
            {
                sql.Append("ORDER BY " + _order);

                //清空排序字段
                _order = string.Empty;
            }

            return db.GetDataSet(sql.ToString()).Tables[0];
        }
        #endregion

        #region SelectByPage
        ///// <summary>
        ///// 如需排序，查询前调用SetOrder方法设置排序规则
        ///// </summary>
        ///// <param name="pageno">当前页码，从1开始</param>
        ///// <param name="pagesize">每页显示记录数</param>
        ///// <param name="rowscount">out总记录数</param>
        ///// <param name="tablename"></param>
        ///// <param name="fields"></param>
        ///// <param name="paramfields"></param>
        ///// <returns></returns>
        //public static DataTable SelectByPage(int pageno, int pagesize, out int rowscount, string tablename, string fields, params MyField[] paramfields)
        //{
        //    return SelectByPage(pageno, pagesize, out rowscount, tablename, fields, getConditionString(paramfields));
        //}

        ///// <summary>
        ///// 如需排序，查询前调用SetOrder方法设置排序规则
        ///// </summary>
        ///// <param name="pageno">当前页码，从1开始</param>
        ///// <param name="pagesize">每页显示记录数</param>
        ///// <param name="rowscount">out总记录数</param>
        ///// <param name="tablename"></param>
        ///// <param name="fields"></param>
        ///// <param name="condition"></param>
        ///// <param name="order"></param>
        ///// <returns></returns>
        //public static DataTable SelectByPage(int pageno, int pagesize, out int rowscount, string tablename, string fields, string condition)
        //{
        //    StringBuilder countsql = new StringBuilder();
        //    countsql.Append("SELECT COUNT(1) FROM " + getTablenameString(tablename) + "\r\n");
        //    if (!string.IsNullOrEmpty(condition))
        //    {
        //        countsql.Append("WHERE " + condition + "\r\n");
        //    }
        //    rowscount = Convert.ToInt32(Global.g_db.GetDataSet(countsql.ToString()).Tables[0].Rows[0][0]);

        //    StringBuilder sql = new StringBuilder();
        //    sql.Append("SELECT " + fields + "\r\n");
        //    sql.Append("FROM " + tablename + "\r\n");
        //    if (!string.IsNullOrEmpty(condition))
        //    {
        //        sql.Append("WHERE " + condition + "\r\n");
        //    }
        //    if (!string.IsNullOrEmpty(_order))
        //    {
        //        sql.Append("ORDER BY " + _order + "\r\n");

        //        //清空排序字段
        //        _order = string.Empty;
        //    }
        //    sql.Append("LIMIT " + ((pageno - 1) * pagesize) + "," + pagesize);

        //    return Global.g_db.GetDataSet(sql.ToString()).Tables[0];
        //}
        #endregion

        #region SelectCount

        public static int SelectCount(string tablename, params MyField[] paramfields)
        {
            return SelectCount(Global.g_db, tablename, paramfields);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="fields"></param>
        /// <param name="paramfields"></param>
        /// <returns></returns>
        public static int SelectCount(DbHelper db, string tablename, params MyField[] paramfields)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT count(1) as rowscount\r\n");
            sql.Append("FROM " + getTablenameString(tablename) + "\r\n");
            sql.Append("WHERE 1=1 \r\n");
            List<DbParameter> paramlist = new List<DbParameter>();
            foreach (MyField fld in paramfields)
            {
                if (fld.QueryMode == enmQueryMode.等于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + "=" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.不等于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + "<>" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.包含)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + " LIKE '%'+" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "+'%'\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.大于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + ">" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.小于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + "<" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "\r\n");
                }
                //sql.Append("AND " + getFieldString(fld.FieldName) + "=" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "\r\n");
                paramlist.Add(db.CreateParameter(GetParameterName(fld.FieldName), fld.FieldValue));
            }
            if (!string.IsNullOrEmpty(_groupby))
            {
                sql.Append("GROUP BY " + _groupby + "\r\n");

                //清空分组字段
                _groupby = string.Empty;
            }
            if (!string.IsNullOrEmpty(_having))
            {
                sql.Append("HAVING " + _having + "\r\n");

                //清空
                _having = string.Empty;
            }

            DataTable tablecount = db.GetDataSet(sql.ToString(), paramlist.ToArray()).Tables[0];
            if (tablecount.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(tablecount.Rows[0][0]);
            }
        }

        public static int SelectCount(string tablename, string condition)
        {
            return SelectCount(Global.g_db, tablename, condition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <param name="fields"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static int SelectCount(DbHelper db, string tablename, string condition)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("SELECT count(1) as rowscount\r\n");
            sql.Append("FROM " + getTablenameString(tablename) + "\r\n");
            if (!string.IsNullOrEmpty(condition))
            {
                sql.Append("WHERE " + condition + "\r\n");
            }
            if (!string.IsNullOrEmpty(_groupby))
            {
                sql.Append("GROUP BY " + _groupby + "\r\n");

                //清空分组字段
                _groupby = string.Empty;
            }
            if (!string.IsNullOrEmpty(_having))
            {
                sql.Append("HAVING " + _having + "\r\n");

                //清空
                _having = string.Empty;
            }

            DataTable tablecount = db.GetDataSet(sql.ToString()).Tables[0];
            if (tablecount.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(tablecount.Rows[0][0]);
            }
        }

        #endregion

        #region Insert
        
        public static int Insert(string tablename, MyField[] fields, out int id)
        {
            return Insert(Global.g_db, tablename, fields, out id);
        }

        public static int Insert(DbHelper db, string tablename, MyField[] fields, out int id)
        {
            int ret = Insert(tablename, fields);

            if (ret > 0)
            {
                DataTable table = db.GetDataSet("SELECT @@IDENTITY").Tables[0];
                id = Convert.ToInt32(table.Rows[0][0]);
            }
            else
            {
                id = -1;
            }

            return ret;
        }

        public static int Insert(string tablename, MyField[] fields)
        {
            return Insert(Global.g_db, tablename, fields);
        }

        public static int Insert(DbHelper db, string tablename, MyField[] fileds)
        {
            fileds = addCreateField(fileds);

            StringBuilder sql = new StringBuilder();
            StringBuilder sqlval = new StringBuilder();
            List<DbParameter> paramlist = new List<DbParameter>();
            sql.Append("INSERT INTO " + tablename + "\n");
            sql.Append("(");
            sqlval.Append("VALUES\n");
            sqlval.Append("(");
            foreach (MyField fld in fileds)
            {
                if ((fld.EditFlags & MyFieldEditFlags.Insert) == MyFieldEditFlags.Insert)
                {
                    if (!string.IsNullOrEmpty(fld.FieldName))
                    {
                        sql.Append(getFieldString(fld.FieldName) + ",");
                        sqlval.Append(PARAM_PREFIX + GetParameterName(fld.FieldName) + ",");
                        paramlist.Add(db.CreateParameter(GetParameterName(fld.FieldName), FormatInputData(fld)));
                    }
                }
            }


            sql.Remove(sql.Length - 1, 1);
            sqlval.Remove(sqlval.Length - 1, 1);
            sql.Append(")\n");
            sqlval.Append(")\n");

            sql.Append(sqlval.ToString());

            return db.ExecuteNonQuery(sql.ToString(), paramlist.ToArray());
        }
        #endregion

        #region Update
        
        public static int Update(string tablename, MyField[] datafields, params MyField[] paramfields)
        {
            return Update(Global.g_db, tablename, datafields, paramfields);
        }

        public static int Update(DbHelper db, string tablename, MyField[] datafields, params MyField[] paramfields)
        {
            datafields = addUpdateField(datafields);

            StringBuilder sql = new StringBuilder();
            List<DbParameter> paramlist = new List<DbParameter>();
            sql.Append("UPDATE " + getTablenameString(tablename) + "\r\n");
            sql.Append("SET ");
            foreach (MyField fld in datafields)
            {
                if ((fld.EditFlags & MyFieldEditFlags.Update) == MyFieldEditFlags.Update)
                {
                    if (!string.IsNullOrEmpty(fld.FieldName))
                    {
                        sql.Append(getFieldString(fld.FieldName) + "=" + PARAM_PREFIX + GetParameterName(fld.FieldName) + ",");
                        paramlist.Add(db.CreateParameter(GetParameterName(fld.FieldName), FormatInputData(fld)));
                    }
                }
            }
            sql.Remove(sql.Length - 1, 1);
            sql.Append("\r\n");

            sql.Append("WHERE 1=1 \r\n");
            foreach (MyField fld in paramfields)
            {
                if (fld.QueryMode == enmQueryMode.等于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + "=" + PARAM_PREFIX + "p_" + GetParameterName(fld.FieldName) + "\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.不等于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + "<>" + PARAM_PREFIX + "p_" + GetParameterName(fld.FieldName) + "\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.包含)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + " LIKE '%'+" + PARAM_PREFIX + "p_" + GetParameterName(fld.FieldName) + "+'%'\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.大于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + ">" + PARAM_PREFIX + "p_" + GetParameterName(fld.FieldName) + "\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.小于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + "<" + PARAM_PREFIX + "p_" + GetParameterName(fld.FieldName) + "\r\n");
                }
                //sql.Append("AND " + getFieldString(fld.FieldName) + "=" + PARAM_PREFIX + "p_" + GetParameterName(fld.FieldName) + "\r\n");
                paramlist.Add(db.CreateParameter("p_" + GetParameterName(fld.FieldName), fld.FieldValue));
            }

            return db.ExecuteNonQuery(sql.ToString(), paramlist.ToArray());
        }

        public static int Update(string tablename, MyField[] datafields, string conditions)
        {
            return Update(Global.g_db, tablename, datafields, conditions);
        }

        public static int Update(DbHelper db, string tablename, MyField[] datafields, string conditions)
        {
            datafields = addUpdateField(datafields);

            StringBuilder sql = new StringBuilder();
            List<DbParameter> paramlist = new List<DbParameter>();
            sql.Append("UPDATE " + getTablenameString(tablename) + "\r\n");
            sql.Append("SET ");
            foreach (MyField fld in datafields)
            {
                if ((fld.EditFlags & MyFieldEditFlags.Update) == MyFieldEditFlags.Update)
                {
                    if (!string.IsNullOrEmpty(fld.FieldName))
                    {
                        sql.Append(getFieldString(fld.FieldName) + "=" + PARAM_PREFIX + fld.FieldName + ",");
                        paramlist.Add(db.CreateParameter(fld.FieldName, FormatInputData(fld)));
                    }
                }
            }
            sql.Remove(sql.Length - 1, 1);
            sql.Append("\r\n");

            if (!string.IsNullOrEmpty(conditions))
            {
                sql.Append("WHERE " + conditions);
            }

            return db.ExecuteNonQuery(sql.ToString(), paramlist.ToArray());
        }
        #endregion

        #region Delete
        public static int Delete(string tablename, params MyField[] paramfields)
        {
            return Delete(Global.g_db, tablename, paramfields);
        }

        public static int Delete(DbHelper db, string tablename, params MyField[] paramfields)
        {
            StringBuilder sql = new StringBuilder();
            List<DbParameter> paramlist = new List<DbParameter>();
            sql.Append("DELETE FROM " + getTablenameString(tablename) + "\r\n");
            sql.Append("WHERE 1=1 \r\n");
            foreach (MyField fld in paramfields)
            {
                if (fld.QueryMode == enmQueryMode.等于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + "=" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.不等于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + "<>" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.包含)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + " LIKE '%'+" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "+'%'\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.大于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + ">" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "\r\n");
                }
                else if (fld.QueryMode == enmQueryMode.小于)
                {
                    sql.Append("AND " + getFieldString(fld.FieldName) + "<" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "\r\n");
                }
                //sql.Append("AND " + getFieldString(fld.FieldName) + "=" + PARAM_PREFIX + GetParameterName(fld.FieldName) + "\r\n");
                paramlist.Add(db.CreateParameter(GetParameterName(fld.FieldName), fld.FieldValue));
            }

            return db.ExecuteNonQuery(sql.ToString(), paramlist.ToArray());
        }

        public static int Delete(string tablename, string conditions)
        {
            return Delete(Global.g_db, tablename, conditions);
        }

        public static int Delete(DbHelper db, string tablename, string conditions)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("DELETE FROM " + getTablenameString(tablename) + "\r\n");
            if (!string.IsNullOrEmpty(conditions))
            {
                sql.Append("WHERE " + conditions);
            }

            return db.ExecuteNonQuery(sql.ToString());
        }
        #endregion

        #region addCreateField / addUpdateField

        public static MyField[] addCreateField(MyField[] fileds)
        {
            bool flg_createtime = false, flg_createuser = false;
            bool flg_updatetime = false, flg_updateuser = false;

            foreach (MyField fld in fileds)
            {
                if (fld.FieldName.ToLower() == CREATE_TIME) flg_createtime = true;
                if (fld.FieldName.ToLower() == CREATE_USER) flg_createuser = true;
                if (fld.FieldName.ToLower() == UPDATE_TIME) flg_updatetime = true;
                if (fld.FieldName.ToLower() == UPDATE_USER) flg_updateuser = true;
            }

            Nullable<DateTime> now = new Nullable<DateTime>();

            List<MyField> list = new List<MyField>();
            list.AddRange(fileds);
            if (!flg_createtime)
            {
                if (!now.HasValue) now = TableManager.DBServerTime();
                list.Add(new MyField("createtime", now, typeof(DateTime)));
            }
            if (!flg_createuser)
            {
                list.Add(new MyField("createuser", Global.g_username));
            }
            if (!flg_updatetime)
            {
                if (!now.HasValue) now = TableManager.DBServerTime();
                list.Add(new MyField("updatetime", now, typeof(DateTime)));
            }
            if (!flg_updateuser)
            {
                list.Add(new MyField("updateuser", Global.g_username));
            }

            return list.ToArray();
        }

        public static MyField[] addUpdateField(MyField[] fileds)
        {
            bool flg_updatetime = false, flg_updateuser = false;

            foreach (MyField fld in fileds)
            {
                if (fld.FieldName.ToLower() == UPDATE_TIME) flg_updatetime = true;
                if (fld.FieldName.ToLower() == UPDATE_USER) flg_updateuser = true;
            }

            List<MyField> list = new List<MyField>();
            list.AddRange(fileds);
            if (!flg_updatetime)
            {
                list.Add(new MyField("updatetime", TableManager.DBServerTime(), typeof(DateTime)));
            }
            if (!flg_updateuser)
            {
                list.Add(new MyField("updateuser", Global.g_username));
            }

            return list.ToArray();
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
        public static string ColComboList(string tablename, string fieldname, string condition, params bool[] allowedit)
        {
            string strlist = "";
            //设置下拉列表是否可以手动修改值
            if (allowedit != null && allowedit[0] == true)
            {
                strlist += "| ";
            }
            else
            {
                strlist += " ";
            }

            TableManager.SetOrder(fieldname);
            DataTable table = TableManager.Select(tablename, "distinct " + fieldname, condition);
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

        #region DBServerTime

        /// <summary>
        /// 获取数据库服务器时间
        /// </summary>
        /// <returns></returns>
        public static DateTime DBServerTime()
        {
            string sql = "select CURRENT_TIMESTAMP";  //select CURRENT_TIMESTAMP
            using (DbHelper db = new DbHelper())
            {
                return Convert.ToDateTime(db.GetDataSet(sql).Tables[0].Rows[0][0]);
            }
        }

        /// <summary>
        /// 获取数据库服务器时间
        /// </summary>
        /// <returns></returns>
        public static DateTime DBServerTime(DbHelper db)
        {
            string sql = "SELECT getdate()";
            return Convert.ToDateTime(db.GetDataSet(sql).Tables[0].Rows[0][0]);
        }

        #endregion

        #region 生成查询字段
        /// <summary>
        /// 生成查询字段
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string BuildSelectFieldString(MyField[] selectfields)
        {
            string strfields = "";
            foreach (MyField item in selectfields)
            {
                if (!string.IsNullOrEmpty(item.FieldName))
                {
                    strfields += getFieldString(item.FieldName) + ",";
                }
            }
            if (strfields.Length > 1) strfields = strfields.Substring(0, strfields.Length - 1);

            return strfields;
        }
        #endregion

        #region 将数组转换为In条件格式的字符串

        /// <summary>
        /// 将数组转换为In条件格式的字符串
        /// </summary>
        /// <param name="arr"></param>
        /// <returns></returns>
        public static string BuildInStr(string[] arr)
        {
            string str = "";

            foreach (string value in arr)
            {
                str += "'" + value + "',";
            }

            if (str.Length > 0) str = str.Substring(0, str.Length - 1);
            return str;
        }

        public static string BuildInStr(int[] arr)
        {
            string str = "";

            foreach (int value in arr)
            {
                str += value.ToString() + ",";
            }

            if (str.Length > 0) str = str.Substring(0, str.Length - 1);
            return str;
        }

        #endregion

        #region GetParameterName
        /// <summary>
        /// GetParameterName
        /// </summary>
        /// <param name="fieldname"></param>
        /// <returns></returns>
        public static string GetParameterName(string fieldname)
        {
            if (fieldname.Trim() == "") return "";

            string newfieldname = fieldname;

            //如果字段名中包含“AS”，取“AS”后面的名称
            if (newfieldname.ToUpper().Contains(" AS "))
            {
                newfieldname = newfieldname.Substring(newfieldname.ToUpper().IndexOf(" AS ") + 4).Trim();
            }

            //如果字段名中包含“.”，取“.”后面的名称
            if (newfieldname.Contains("."))
            {
                newfieldname = newfieldname.Substring(newfieldname.IndexOf(".") + 1);
            }

            return newfieldname;
        }

        #endregion

        #region TypeToDbType

        public static DbType TypeToDbType(Type type)
        {
            if (type == typeof(string))
            {
                return DbType.String;
            }
            else if (type == typeof(Int16))
            {
                return DbType.Int16;
            }
            else if (type == typeof(Int32))
            {
                return DbType.Int32;
            }
            else if (type == typeof(Int64))
            {
                return DbType.Int64;
            }
            else if (type == typeof(decimal) || type == typeof(float) || type == typeof(double))
            {
                return DbType.Decimal;
            }
            else if (type == typeof(DateTime) || type == typeof(Nullable<DateTime>))
            {
                return DbType.DateTime;
            }
            else if (type == typeof(bool))
            {
                return DbType.Boolean;
            }
            else if (type == typeof(Type[]))
            {
                return DbType.Binary;
            }
            else
            {
                return DbType.String;
            }
        }

        #endregion

        #region FormatInputData

        public static object FormatInputData(MyField fld)
        {
            switch (fld.DataType)
            {
                case DbType.String:
                    return Utils.NvStr(fld.FieldValue);
                case DbType.Date:
                    if (Utils.NvStr(fld.FieldValue) == "")
                    {
                        return DBNull.Value;
                    }
                    else
                    {
                        return Convert.ToDateTime(fld.FieldValue).Date;
                    }
                case DbType.DateTime:
                    if (Utils.NvStr(fld.FieldValue) == "")
                    {
                        return DBNull.Value;
                    }
                    else
                    {
                        return Convert.ToDateTime(fld.FieldValue);
                    }
                case DbType.Boolean:
                    return Utils.NvBool(fld.FieldValue);
                case DbType.Int16:
                case DbType.Int32:
                case DbType.Int64:
                    return Utils.NvInt(fld.FieldValue);
                case DbType.Decimal:
                    return Utils.NvDecimal(fld.FieldValue);
                case DbType.Object:
                    return fld.FieldValue;
                case DbType.Binary:
                    return fld.FieldValue;
                default:
                    return Utils.NvStr(fld.FieldValue);
            }
        }

        #endregion

        #region 私有函数
        private static string getTablenameString(string tablename)
        {
            if (tablename.Trim().IndexOf(" ") >= 0 || tablename.Trim().IndexOf(".") >= 0)
            {
                return tablename;
            }
            else
            {
                return FIELD_PREFIX + tablename + FIELD_SUFFIX;
            }
        }

        private static string getFieldString(string fieldname)
        {
            if (fieldname.Trim().IndexOf(" ") >= 0 || fieldname.Trim().IndexOf(".") >= 0)
            {
                return fieldname;
            }
            else
            {
                return FIELD_PREFIX + fieldname + FIELD_SUFFIX;
            }
        }
        #endregion

        #region DefaultFields

        public static MyField[] DefaultFields()
        {
            List<MyField> fields = new List<MyField>();

            MyField fld = new MyField();
            fld.FieldName = "CreateTime";
            fld.FieldValue = TableManager.DBServerTime();
            fld.EditFlags = MyFieldEditFlags.Insert;
            fields.Add(fld);

            fld = new MyField();
            fld.FieldName = "CreateUser";
            fld.FieldValue = Global.g_username;
            fld.EditFlags = MyFieldEditFlags.Insert;
            fields.Add(fld);

            fld = new MyField();
            fld.FieldName = "UpdateTime";
            fld.FieldValue = TableManager.DBServerTime();
            fld.EditFlags = MyFieldEditFlags.Insert | MyFieldEditFlags.Update;
            fields.Add(fld);

            fld = new MyField();
            fld.FieldName = "UpdateUser";
            fld.FieldValue = Global.g_username;
            fld.EditFlags = MyFieldEditFlags.Insert | MyFieldEditFlags.Update;
            fields.Add(fld);

            return fields.ToArray();
        }

        #endregion

    }
}
