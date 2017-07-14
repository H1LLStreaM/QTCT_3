using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using WY.Common.Utility;
using System.Web.Configuration;
using System.Text.RegularExpressions;

namespace WY.Common.Data
{
    /// <summary>
    /// 数据库操作类
    /// </summary>
    public class DbHelper : IDisposable
    {
        private const string APPSETTING_KEY = "dbhelper_connectionstring";
        public const string SQL_PROVIDERNAME = "System.Data.SqlClient";

        public enum enmDBVerifyType
        {
            SQLServer,
            Windows
        }

        private static string mprovidername = "";
        private static string mconnectionstring = "";

        private DbConnection dbcon;
        private DbTransaction dbtrans;
        private string mstrTaihiSQL;

        private DbProviderFactory mfactory;

        #region 静态构造
        //static DbHelper()
        //{
        //    Initialize();
        //    //Initialize(WebConfigurationManager.ConnectionStrings["showa"].ConnectionString);
        //}
        #endregion

        #region 构造
        /// <summary>
        /// 构造一个数据库操作类的实例
        /// </summary>
        public DbHelper()
        {
            mfactory = DbProviderFactories.GetFactory(mprovidername);

            dbcon = this.CreateNewConnection();
        }

        /// <summary>
        /// 指定connectionstring构造一个数据库操作类的实例
        /// </summary>
        /// <param name="connectionstringkey">web.config中connectionstring的name</param>
        public DbHelper(string providername, string connectionstring)
        {
            mprovidername = providername;
            mconnectionstring = connectionstring;

            mfactory = DbProviderFactories.GetFactory(mprovidername);

            dbcon = this.CreateNewConnection();
        }
        #endregion

        #region Initialize (static)
        /// <summary>
        /// 初始化DBHelper的数据源提供者名称和连接字符串
        /// </summary>
        public static void Initialize()
        {
            string value = WebConfigurationManager.AppSettings[APPSETTING_KEY];

            Regex connectionStringRegex = new Regex(@"ConnectionString\s*=\s*\$\{(?<ConnectionStringName>[^}]+)\}");
            if (connectionStringRegex.IsMatch(value))
            {
                string connectionstringkey = connectionStringRegex.Match(value).
                               Groups["ConnectionStringName"].Value;

                DbHelper.Initialize(connectionstringkey);
            }
        }
        
        /// <summary>
        /// 初始化DBHelper的数据源提供者名称和连接字符串
        /// </summary>
        /// <param name="connectionstringkey">web.config中connectionstring的name</param>
        public static void Initialize(string connectionstringkey)
        {

            mprovidername = WebConfigurationManager.ConnectionStrings[connectionstringkey].ProviderName;
            mconnectionstring = WebConfigurationManager.ConnectionStrings[connectionstringkey].ConnectionString;
        }

        /// <summary>
        /// 初始化DBHelper的数据源提供者名称和连接字符串
        /// </summary>
        /// <param name="providername">数据源提供者名称</param>
        /// <param name="connectionstring">数据库连接字符串</param>
        public static void Initialize(string providername, string connectionstring)
        {
            mprovidername = providername;
            mconnectionstring = connectionstring;
        }
        #endregion

        #region Dispose

        public void Dispose()
        {
            //关闭连接
            this.Close();

            dbcon.Dispose();

            dbcon = null;

            System.GC.SuppressFinalize(this);
        }

        #endregion

        #region property

        /// <summary>
        /// 数据提供者名称
        /// </summary>
        public string DbProvider
        {
            get { return mprovidername; }
            set { mprovidername = value; }
        }

        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        public string ConnectionString
        {
            get { return mconnectionstring; }
            set { mconnectionstring = value; }
        }

        private string _username;
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }

        /// <summary>
        /// 获取当前数据库连接
        /// </summary>
        public DbConnection DBConnection
        {
            get
            {
                return dbcon;
            }
        }

        /// <summary>
        /// 取得最后一次执行的sql语句
        /// </summary>
        public string TaihiSQL
        {
            get
            {
                return mstrTaihiSQL;
            }
        }
        #endregion

        #region CreateNewConnection
        public DbConnection CreateNewConnection()
        {
            DbConnection con = mfactory.CreateConnection();
            con.ConnectionString = mconnectionstring;
            return con;
        }
        #endregion

        #region Open/Close
        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <returns></returns>
        public void Open()
        {
            dbcon.ConnectionString = mconnectionstring;

            //打开连接
            dbcon.Open();
        }

        /// <summary>
        /// 关闭数据库连接
        /// </summary>
        /// <returns></returns>
        public void Close()
        {
            if (dbcon.State == System.Data.ConnectionState.Open)
            {
                dbcon.Close();
            }
        }
        #endregion

        #region CreateCommand (private)
        private DbCommand CreateCommand()
        {
            if (dbcon.State == System.Data.ConnectionState.Closed) this.Open();

            DbCommand cmd = dbcon.CreateCommand();

            if (dbtrans != null) cmd.Transaction = dbtrans;

            return cmd;
        }
        #endregion

        #region CreateParameter
        /// <summary>
        /// 创建sql参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, object value)
        {
            DbParameter param = mfactory.CreateParameter();

            param.ParameterName = name;
            param.Value = (value == null ? DBNull.Value : value);

            return param;
        }

        /// <summary>
        /// 创建sql参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbtype"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, object value, DbType dbtype)
        {
            DbParameter param = mfactory.CreateParameter();

            param.ParameterName = name;
            param.Value = (value == null ? DBNull.Value : value);
            param.DbType = dbtype;

            return param;
        }

        /// <summary>
        /// 创建sql参数
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbtype"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public DbParameter CreateParameter(string name, object value, DbType dbtype, int size)
        {
            DbParameter param = mfactory.CreateParameter();

            param.ParameterName = name;
            param.Value = (value == null ? DBNull.Value : value);
            param.DbType = dbtype;
            param.Size = size;

            return param;
        }
        #endregion

        #region ExecuteReader
        /// <summary>
        /// 执行sql语句（select），返回OleDbDataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string sql)
        {
            mstrTaihiSQL = sql;
            if (Global._allowsqllog)
            {
                Log.Info("sql.log", sql);
            }

            try
            {
                using (DbConnection con = this.CreateNewConnection())
                {
                    con.Open();

                    using (DbCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        return cmd.ExecuteReader();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(sql);
                throw ex;
            }
        }

        /// <summary>
        /// 执行sql语句（select），返回OleDbDataReader
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DbDataReader ExecuteReader(string sql, DbParameter[] arrparm)
        {
            mstrTaihiSQL = sql;
            if (Global._allowsqllog)
            {
                Log.Info("sql.log", sql);
            }

            try
            {
                using (DbConnection con = this.CreateNewConnection())
                {
                    con.Open();

                    using (DbCommand cmd = con.CreateCommand())
                    {
                        cmd.CommandText = sql;

                        //执行sql的参数
                        cmd.Parameters.Clear();
                        for (int i = 0; i < arrparm.Length; i++)
                        {
                            cmd.Parameters.Add(arrparm[i]);
                        }

                        return cmd.ExecuteReader();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(sql);
                throw ex;
            }
        }
        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// 执行sql语句（insert，update，delete）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>sql执行影响的记录数</returns>
        public int ExecuteNonQuery(string sql)
        {
            mstrTaihiSQL = sql;
            if (Global._allowsqllog)
            {
                Log.Info("sql.log", sql);
            }

            try
            {
                using (DbCommand cmd = this.CreateCommand())
                {
                    cmd.CommandText = sql;
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Log.Error(sql);
                throw ex;
            }
        }

        /// <summary>
        /// 执行带参数的sql语句（insert，update，delete）
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>sql执行影响的记录数</returns>
        public int ExecuteNonQuery(string sql, DbParameter[] arrparm)
        {
            mstrTaihiSQL = sql;
            if (Global._allowsqllog)
            {
                Log.Info("sql.log", sql);
            }

            try
            {
                using (DbCommand cmd = this.CreateCommand())
                {
                    cmd.CommandText = sql;

                    //执行sql的参数
                    cmd.Parameters.Clear();
                    for (int i = 0; i < arrparm.Length; i++)
                    {
                        cmd.Parameters.Add(arrparm[i]);
                    }

                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Log.Error(sql);
                throw ex;
            }
        }
        #endregion

        #region ExecuteProcedure
        /// <summary>
        /// 执行存储过程，返回DbDataReader
        /// </summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="arrparm">参数</param>
        /// <returns>存储过程执行结果DbDataReader</returns>
        public DbDataReader ExeDataReaderProcedure(string procname, DbParameter[] arrparm)
        {
            mstrTaihiSQL = procname;
            if (Global._allowsqllog)
            {
                Log.Info("sql.log", procname);
            }

            using (DbConnection con = this.CreateNewConnection())
            {
                con.Open();

                using (DbCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = procname;

                    //执行sql的参数
                    cmd.Parameters.Clear();
                    for (int i = 0; i < arrparm.Length; i++)
                    {
                        cmd.Parameters.Add(arrparm[i]);
                    }

                    return cmd.ExecuteReader();
                }
            }
        }

        /// <summary>
        /// 执行存储过程，返回DataSet
        /// </summary>
        /// <param name="procname">存储过程名称</param>
        /// <param name="arrparm">参数</param>
        /// <returns>存储过程执行结果DataSet</returns>
        public DataSet ExeDatasetProcedure(string procname, DbParameter[] arrparm)
        {
            mstrTaihiSQL = procname;
            if (Global._allowsqllog)
            {
                Log.Info("sql.log", procname);
            }

            using (DbCommand cmd = this.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = procname;

                //执行sql的参数
                cmd.Parameters.Clear();
                for (int i = 0; i < arrparm.Length; i++)
                {
                    cmd.Parameters.Add(arrparm[i]);
                }

                DataSet ds = new DataSet();
                DbDataAdapter da = mfactory.CreateDataAdapter();
                da.SelectCommand = cmd;

                da.Fill(ds);

                return ds;
            }
        }
        #endregion

        #region GetDataSet
        /// <summary>
        /// 执行sql语句返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql)
        {
            mstrTaihiSQL = sql;
            if (Global._allowsqllog)
            {
                Log.Info("sql.log", sql);
            }

            try
            {
                using (DbCommand cmd = this.CreateCommand())
                {
                    DataSet ds = new DataSet();
                    DbDataAdapter da = mfactory.CreateDataAdapter();
                    cmd.CommandText = sql;
                    da.SelectCommand = cmd;

                    da.Fill(ds);

                    return ds;
                }
            }
            catch (Exception ex)
            {
                Log.Error(sql);
                throw ex;
            }
        }

        /// <summary>
        /// 执行sql语句返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataSet GetDataSet(string sql, DbParameter[] arrparm)
        {
            mstrTaihiSQL = sql;
            if (Global._allowsqllog)
            {
                Log.Info("sql.log", sql);
            }

            try
            {
                using (DbCommand cmd = this.CreateCommand())
                {
                    cmd.CommandText = sql;

                    //执行sql的参数
                    cmd.Parameters.Clear();
                    for (int i = 0; i < arrparm.Length; i++)
                    {
                        cmd.Parameters.Add(arrparm[i]);
                    }

                    DataSet ds = new DataSet();
                    DbDataAdapter da = mfactory.CreateDataAdapter();
                    da.SelectCommand = cmd;

                    da.Fill(ds);

                    return ds;
                }
            }
            catch (Exception ex)
            {
                Log.Error(sql);
                throw ex;
            }
        }

        #endregion

        #region Transaction处理
        /// <summary>
        /// 开始事务处理
        /// </summary>
        /// <returns></returns>
        public void TrnStart()
        {
            if (dbcon.State == System.Data.ConnectionState.Closed) this.Open();

            dbtrans = dbcon.BeginTransaction();
        }

        /// <summary>
        /// 提交事务
        /// </summary>
        /// <returns></returns>
        public void TrnCommit()
        {
            if (dbcon.State == System.Data.ConnectionState.Closed) this.Open();

            if (dbtrans != null)
            {
                dbtrans.Commit();
            }
        }

        /// <summary>
        /// 撤销事务
        /// </summary>
        /// <returns></returns>
        public void TrnRollBack()
        {
            try
            {
                if (dbcon.State == System.Data.ConnectionState.Closed) this.Open();

                if (dbtrans != null)
                {
                    dbtrans.Rollback();
                }
            }
            catch { }
        }
        #endregion

        #region BuildConnectString

        public static string BuildConnectString(enmDBVerifyType type, string server, string database, string user, string password)
        {
            string connstr = "";

            if (type == enmDBVerifyType.SQLServer)
            {
                connstr = "Data Source=" + server + ";Initial Catalog=" + database + ";User ID=" + user + ";Password=" + password + ";";
            }
            else
            {
                connstr = "Data Source=" + server + ";Initial Catalog=" + database + ";Integrated Security=True;";
            }

            return connstr;
        }

        #endregion

    }

    public class MyTransaction : IDisposable
    {
        private bool _transopening = false;

        public MyTransaction()
        {
            this._transopening = true;
            Global.g_db.TrnStart();
        }

        public void Commit()
        {
            Global.g_db.TrnCommit();
            this._transopening = false;
        }

        public void Rollback()
        {
            Global.g_db.TrnRollBack();
            this._transopening = false;
        }

        #region IDisposable 成员

        public void Dispose()
        {
            if (this._transopening)
            {
                Global.g_db.TrnRollBack();
                this._transopening = false;
            }
        }

        #endregion
    }
}
