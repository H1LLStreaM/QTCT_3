using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WY.Library.Model
{
    [Serializable]
    [ActiveRecord("PTS_PROJ_COST")]
    public class PTS_PROJ_COST : BaseModel
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

        private string _KEY1;
        /// <summary>
        /// 合同金额区间1
        /// </summary>
        [Property()]
        public string KEY1
        {
            get { return this._KEY1; }
            set { this._KEY1 = value; }
        }

        private string _KEY2;
        /// <summary>
        /// 合同金额区间2
        /// </summary>
        [Property()]
        public string KEY2
        {
            get { return this._KEY2; }
            set { this._KEY2 = value; }
        }

        private string _COST;
        /// <summary>
        /// 成本金额
        /// </summary>
        [Property()]
        public string COST
        {
            get { return this._COST; }
            set { this._COST = value; }
        }
    }
}
