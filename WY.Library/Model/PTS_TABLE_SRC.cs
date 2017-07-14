using System;
using Castle.ActiveRecord;

namespace WY.Library.Model
{
    /// <summary>
    /// 用户表(员工表) (数据持久层对象)
    /// </summary>
    [Serializable]
    [ActiveRecord("pts_table_src")]
    public class PTS_TABLE_SRC : BaseModel
    {
        private int _ID;
        /// <summary>
        /// ID
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native)]
        public int ID
        {
            get { return this._ID; }
            set { this._ID = value; }
        }

        private string _Table_Id;
        /// <summary>
        /// 静态数据类型标识（只做查询用）
        /// </summary>
        [Property()]
        public string TABLE_ID
        {
            get { return this._Table_Id; }
            set { this._Table_Id = value; }
        }

        private string _Table_Name;
        /// <summary>
        /// 静态数据类型名称
        /// </summary>
        [Property()]
        public string TABLE_NAME
        {
            get { return this._Table_Name; }
            set { this._Table_Name = value; }
        }

        private string _Table_Value;
        /// <summary>
        /// 静态数据值
        /// </summary>
        [Property()]
        public string TABLE_VALUE
        {
            get { return this._Table_Value; }
            set { this._Table_Value = value; }
        }

        private string _Desc;
        /// <summary>
        /// 静态数据注释）
        /// </summary>
        [Property()]
        public string DESC
        {
            get { return this._Desc; }
            set { this._Desc = value; }
        }

        private int _Status = 1;
        /// <summary>
        /// 是否启用
        /// </summary>
        [Property()]
        public int STATUS
        {
            get { return this._Status; }
            set { this._Status = value; }
        }
    }
}
