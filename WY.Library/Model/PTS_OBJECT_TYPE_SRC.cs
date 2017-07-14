using Castle.ActiveRecord;
using System;

namespace WY.Library.Model
{
    [Serializable]
    [ActiveRecord("PTS_OBJECT_TYPE_SRC")]
    public class PTS_OBJECT_TYPE_SRC : BaseModel
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

        private string _OBJECTTYPENAME;
        /// <summary>
        /// 工程类型名称
        /// </summary>
        [Property()]
        public string OBJECTTYPENAME
        {
            get { return this._OBJECTTYPENAME; }
            set { this._OBJECTTYPENAME = value; }
        }

        private decimal _RATIO1;
        /// <summary>
        /// 分配比例
        /// </summary>
        [Property()]
        public decimal RATIO1
        {
            get { return this._RATIO1; }
            set { this._RATIO1 = value; }
        }

        private decimal _RATIO2;
        /// <summary>
        /// 分配比例
        /// </summary>
        [Property()]
        public decimal RATIO2
        {
            get { return this._RATIO2; }
            set { this._RATIO2 = value; }
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

        private string _StatusDESC;
        /// <summary>
        /// 是否启用
        /// </summary>
        public string StatusDESC
        {
            get { return this._StatusDESC; }
            set { this._StatusDESC = value; }
        }
    }
}
