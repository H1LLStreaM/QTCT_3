using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;
using System.Data.Common;
using WY.Common.Data;

namespace WY.Common.Framework
{
    public class SchemaBuffer
    {

        private const string TABLE_STRICTION = "Tables";

        private const string COLULMN_STRICTION = "Columns";

        private const string PRIMARYKEY_STRICTION = "Indexes";

        private const string INDEX_COLUMN_STRICTION = "IndexColumns";

        private static DataSet _tableSchema = null;

        private static Hashtable _typeMap = null;

        public static DataSet TableSchema
        {
            get { return SchemaBuffer._tableSchema; }
            set { SchemaBuffer._tableSchema = value; }
        }

        //private const 
        public static void InitDataBase()
        {
            using (DbConnection conn = Global.g_db.CreateNewConnection())
            {
                conn.Open();

                _typeMap = GetDataType(conn);

                _tableSchema = GetTableList(conn);
            }
        }

        public static DataTable getTableDef(string tableName)
        {
            if (_tableSchema.Tables.Contains(tableName))
            {
                return _tableSchema.Tables[tableName];
            }
            else
            {
                return null;
            }
        }

        public static Type GetLocalTypeThrDbType(String dbTypeName)
        {
            return Type.GetType((string)_typeMap[dbTypeName]);
        }

        private static Hashtable GetDataType(DbConnection conn)
        {
            DataTable db = conn.GetSchema(DbMetaDataCollectionNames.DataTypes);

            Hashtable ht = new Hashtable();

            foreach (DataRow row in db.Rows)
            {
                ht.Add(row[0], row[5]);
            }
            return ht;
        }

        private static DataSet GetTableList(DbConnection conn)
        {
            DataSet ds = new DataSet();

            DataTable tableNames = conn.GetSchema(TABLE_STRICTION);
            DataTable columnNames = conn.GetSchema(COLULMN_STRICTION);
            DataTable primaryKeys = conn.GetSchema(PRIMARYKEY_STRICTION);
            DataTable primaryKeyColumns = conn.GetSchema(INDEX_COLUMN_STRICTION);

            ArrayList list = new ArrayList();
            foreach (DataRow tbRow in tableNames.Rows)
            {
                string tableName = (string)tbRow["TABLE_NAME"];
                DataTable dt = new DataTable(tableName);

                ds.Tables.Add(dt);
                foreach (DataRow colRow in columnNames.Rows)
                {
                    string colTableName = (string)colRow["TABLE_NAME"];

                    if (tableName.Equals(colTableName))
                    {
                        string columnName = (string)colRow["COLUMN_NAME"];
                        string columnType = (string)colRow["DATA_TYPE"];
                        DataColumn dc = new DataColumn(columnName, GetLocalTypeThrDbType(columnType));
                        dt.Columns.Add(dc);
                    }
                }

                string pkName = GetTablePkName(primaryKeys, tableName);

                if (pkName != null)
                {
                    ArrayList pkList = new ArrayList();

                    foreach (DataRow pkRow in primaryKeyColumns.Rows)
                    {
                        if (pkName.Equals(pkRow["index_name"]) && tableName.Equals(pkRow["TABLE_NAME"]))
                        {
                            pkList.Add(dt.Columns[(string)pkRow["column_name"]]);
                        }
                    }

                    dt.PrimaryKey = (DataColumn[])pkList.ToArray(typeof(DataColumn));
                }

            }
            return ds;
        }

        private static string GetTablePkName(DataTable dt, string tableName)
        {
            foreach (DataRow row in dt.Rows)
            {
                if (row["TABLE_NAME"].Equals(tableName)
                    && row["index_name"].ToString().ToUpper().StartsWith("PK_"))
                {
                    return (string)row["index_name"];
                }
            }

            return null;
        }

    }
}
