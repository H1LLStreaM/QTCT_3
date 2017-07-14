using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WY.Library.Model
{
    /// <summary>
    /// 用户表(员工表) (数据持久层对象)
    /// </summary>
    [Serializable]
    [ActiveRecord("TB_BILL")]
    public class TB_BILL : BaseModel
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
        /// 工程ID
        /// </summary>
        [Property()]
        public int PROJECTID
        {
            get { return this._PROJECTID; }
            set { this._PROJECTID = value; }
        }

        private string _BILLNUMBER;
        /// <summary>
        /// 发票号码
        /// </summary>
        [Property()]
        public string BILLNUMBER
        {
            get { return this._BILLNUMBER; }
            set { this._BILLNUMBER = value; }
        }

        private DateTime _CREATEDATE;
        /// <summary>
        /// 开票日期
        /// </summary>
        [Property()]
        public DateTime CREATEDATE
        {
            get { return this._CREATEDATE; }
            set { this._CREATEDATE = value; }
        }

        private decimal _MONEY;
        /// <summary>
        /// 开票金额
        /// </summary>
        [Property()]
        public decimal MONEY
        {
            get { return _MONEY; }
            set { _MONEY = value; }
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
