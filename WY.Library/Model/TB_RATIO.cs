using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WY.Library.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    [ActiveRecord("TB_RATIO")]
    public class TB_RATIO : BaseModel
    {
        private int _Id;
        /// <summary>
        /// ID
        /// </summary>
        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
        {
            get { return this._Id; }
            set { this._Id = value; }
        }

        private int _PROJECTID;
        /// <summary>
        /// PROJECTID
        /// </summary>
        [Property()]
        public int PROJECTID
        {
            get { return this._PROJECTID; }
            set { this._PROJECTID = value; }
        }        

        private decimal _RATIO;
        /// <summary>
        /// RATIO
        /// </summary>
        [Property()]
        public decimal RATIO
        {
            get { return this._RATIO; }
            set { this._RATIO = value; }
        }

        private string _USERCODE;
        /// <summary>
        /// USERCODE
        /// </summary>
        [Property()]
        public string USERCODE
        {
            get { return this._USERCODE; }
            set { this._USERCODE = value; }
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
