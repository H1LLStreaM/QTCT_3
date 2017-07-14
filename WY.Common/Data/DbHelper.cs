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
    /// ���ݿ������
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

        #region ��̬����
        //static DbHelper()
        //{
        //    Initialize();
        //    //Initialize(WebConfigurationManager.ConnectionStrings["showa"].ConnectionString);
        //}
        #endregion

        #region ����
        /// <summary>
        /// ����һ�����ݿ�������ʵ��
        /// </summary>
        public DbHelper()
        {
            mfactory = DbProviderFactories.GetFactory(mprovidername);

            dbcon = this.CreateNewConnection();
        }

        /// <summary>
        /// ָ��connectionstring����һ�����ݿ�������ʵ��
        /// </summary>
        /// <param name="connectionstringkey">web.config��connectionstring��name</param>
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
        /// ��ʼ��DBHelper������Դ�ṩ�����ƺ������ַ���
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
        /// ��ʼ��DBHelper������Դ�ṩ�����ƺ������ַ���
        /// </summary>
        /// <param name="connectionstringkey">web.config��connectionstring��name</param>
        public static void Initialize(string connectionstringkey)
        {

            mprovidername = WebConfigurationManager.ConnectionStrings[connectionstringkey].ProviderName;
            mconnectionstring = WebConfigurationManager.ConnectionStrings[connectionstringkey].ConnectionString;
        }

        /// <summary>
        /// ��ʼ��DBHelper������Դ�ṩ�����ƺ������ַ���
        /// </summary>
        /// <param name="providername">����Դ�ṩ������</param>
        /// <param name="connectionstring">���ݿ������ַ���</param>
        public static void Initialize(string providername, string connectionstring)
        {
            mprovidername = providername;
            mconnectionstring = connectionstring;
        }
        #endregion

        #region Dispose

        public void Dispose()
        {
            //�ر�����
            this.Close();

            dbcon.Dispose();

            dbcon = null;

            System.GC.SuppressFinalize(this);
        }

        #endregion

        #region property

        /// <summary>
        /// �����ṩ������
        /// </summary>
        public string DbProvider
        {
            get { return mprovidername; }
            set { mprovidername = value; }
        }

        /// <summary>
        /// ���ݿ������ַ���
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
        /// ��ȡ��ǰ���ݿ�����
        /// </summary>
        public DbConnection DBConnection
        {
            get
            {
                return dbcon;
            }
        }

        /// <summary>
        /// ȡ�����һ��ִ�е�sql���
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
        /// �����ݿ�����
        /// </summary>
        /// <returns></returns>
        public void Open()
        {
            dbcon.ConnectionString = mconnectionstring;

            //������
            dbcon.Open();
        }

        /// <summary>
        /// �ر����ݿ�����
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
        /// ����sql����
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
        /// ����sql����
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
        /// ����sql����
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
        /// ִ��sql��䣨select��������OleDbDataReader
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
        /// ִ��sql��䣨select��������OleDbDataReader
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

                        //ִ��sql�Ĳ���
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
        /// ִ��sql��䣨insert��update��delete��
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>sqlִ��Ӱ��ļ�¼��</returns>
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
        /// ִ�д�������sql��䣨insert��update��delete��
        /// </summary>
        /// <param name="sql"></param>
        /// <returns>sqlִ��Ӱ��ļ�¼��</returns>
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

                    //ִ��sql�Ĳ���
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
        /// ִ�д洢���̣�����DbDataReader
        /// </summary>
        /// <param name="procname">�洢��������</param>
        /// <param name="arrparm">����</param>
        /// <returns>�洢����ִ�н��DbDataReader</returns>
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

                    //ִ��sql�Ĳ���
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
        /// ִ�д洢���̣�����DataSet
        /// </summary>
        /// <param name="procname">�洢��������</param>
        /// <param name="arrparm">����</param>
        /// <returns>�洢����ִ�н��DataSet</returns>
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

                //ִ��sql�Ĳ���
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
        /// ִ��sql��䷵��DataSet
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
        /// ִ��sql��䷵��DataSet
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

                    //ִ��sql�Ĳ���
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

        #region Transaction����
        /// <summary>
        /// ��ʼ������
        /// </summary>
        /// <returns></returns>
        public void TrnStart()
        {
            if (dbcon.State == System.Data.ConnectionState.Closed) this.Open();

            dbtrans = dbcon.BeginTransaction();
        }

        /// <summary>
        /// �ύ����
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
        /// ��������
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

        #region IDisposable ��Ա

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
