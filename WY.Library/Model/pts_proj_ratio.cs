using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WY.Library.Model
{
    [Serializable]
    [ActiveRecord("pts_proj_ratio")]
    public class pts_proj_ratio : BaseModel
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
        /// 毛利率区间1
        /// </summary>
        [Property()]
        public string KEY1
        {
            get { return this._KEY1; }
            set { this._KEY1 = value; }
        }

        private string _KEY2;
        /// <summary>
        /// 毛利率区间2
        /// </summary>
        [Property()]
        public string KEY2
        {
            get { return this._KEY2; }
            set { this._KEY2 = value; }
        }

        private string _RATIO;
        /// <summary>
        /// 提成比例
        /// </summary>
        [Property()]
        public string RATIO
        {
            get { return this._RATIO; }
            set { this._RATIO = value; }
        }

    }
}
