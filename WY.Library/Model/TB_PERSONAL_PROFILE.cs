using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WY.Library.Model
{
    [Serializable]
    [ActiveRecord("TB_PERSONAL_PROFILE")]
    public class TB_PERSONAL_PROFILE : BaseModel
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

        private string _USERCODE;
        /// <summary>
        /// 工号
        /// </summary>
        [Property()]
        public string USERCODE
        {
            get { return this._USERCODE; }
            set { this._USERCODE = value; }
        }

        private decimal _PROFILE1;
        /// <summary>
        /// 均分提成金额
        /// </summary>
        [Property()]
        public decimal PROFILE1
        {
            get { return this._PROFILE1; }
            set { this._PROFILE1 = value; }
        }

        private decimal _PROFILE2;
        /// <summary>
        /// 可分配提成金额
        /// </summary>
        [Property()]
        public decimal PROFILE2
        {
            get { return this._PROFILE2; }
            set { this._PROFILE2 = value; }
        }

        private decimal _AMOUNT;
        /// <summary>
        /// 可分配份额
        /// </summary>
        [Property()]
        public decimal AMOUNT
        {
            get { return this._AMOUNT; }
            set { this._AMOUNT = value; }
        }

        private int _INDEX;
        /// <summary>
        /// 序列号
        /// </summary>
        public int INDEX
        {
            get { return _INDEX; }
            set { _INDEX = value; }
        }

        private string _USERNAME;
        /// <summary>
        /// 姓名
        /// </summary>
        public string USERNAME
        {
            get { return _USERNAME; }
            set { _USERNAME = value; }
        }

    }
}
