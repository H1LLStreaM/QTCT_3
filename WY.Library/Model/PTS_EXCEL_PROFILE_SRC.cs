using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WY.Library.Model
{
    [Serializable]
    [ActiveRecord("PTS_EXCEL_PROFILE_SRC")]
    public class PTS_EXCEL_PROFILE_SRC : BaseModel
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

        private string _ITEMKEY;
        /// <summary>
        /// ITEMKEY
        /// </summary>
        [Property()]
        public string ITEMKEY
        {
            get { return this._ITEMKEY; }
            set { this._ITEMKEY = value; }
        }

        private decimal _ITEMVALUE;
        /// <summary>
        /// ITEMVALUE
        /// </summary>
        [Property()]
        public decimal ITEMVALUE
        {
            get { return this._ITEMVALUE; }
            set { this._ITEMVALUE = value; }
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
