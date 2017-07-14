using Castle.ActiveRecord;
using System;
using System.ComponentModel;

namespace WY.Library.Model
{
    /// <summary>
    /// 用户表(员工表) (数据持久层对象)
    /// </summary>
    [Serializable]
    [ActiveRecord("TB_EXPENSE")]
    public class TB_EXPENSE : BaseModel
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

        private int _Index;

        public int Index
        {
            get { return this._Index; }
            set { this._Index = value; }
        }

        private int _OBJECTID;
        /// <summary>
        /// OBJECTID
        /// </summary>
        [Property()]
        public int OBJECTID
        {
            get { return this._OBJECTID; }
            set { this._OBJECTID = value; }
        }

        private string _OBJECTNAME;
        /// <summary>
        /// OBJECTNAME;
        /// </summary>
        [Property()]
        public string OBJECTNAME
        {
            get { return this._OBJECTNAME; }
            set { this._OBJECTNAME = value; }
        }

        private string _BILLNO;
        /// <summary>
        /// BILLNO
        /// </summary>
        [Property()]
        public string BILLNO
        {
            get { return this._BILLNO; }
            set { this._BILLNO = value; }
        }

        private int _EXPENSTYPE;
        /// <summary>
        /// EXPENSTYPE
        /// </summary>
        [Property()]
        public int EXPENSTYPE
        {
            get { return this._EXPENSTYPE; }
            set { this._EXPENSTYPE = value; }
        }

        private string _EXPENS;
        /// <summary>
        /// EXPENS
        /// </summary>
        [Property()]
        public string EXPENS
        {
            get { return this._EXPENS; }
            set { this._EXPENS = value; }
        }

        private decimal _MONEY;
        /// <summary>
        /// MONEY
        /// </summary>
        [Property()]
        public decimal MONEY
        {
            get { return this._MONEY; }
            set { this._MONEY = value; }
        }

        private DateTime _CREATEDATE;
        /// <summary>
        /// CREATEDATE
        /// </summary>
        [Property()]
        public DateTime CREATEDATE
        {
            get { return this._CREATEDATE; }
            set { this._CREATEDATE = value; }
        }

        private string _COMMENTS;
        /// <summary>
        /// COMMENTS
        /// </summary>
        [Property()]
        public string COMMENTS
        {
            get { return this._COMMENTS; }
            set { this._COMMENTS= value; }
        }


        private int _OPUID;
        /// <summary>
        /// OPUID
        /// </summary>
        [Property()]
        public int OPUID
        {
            get { return this._OPUID; }
            set { this._OPUID = value; }
        }

        private string _OPNAME;
        /// <summary>
        /// OPNAME
        /// </summary>
        [Property()]
        public string OPNAME
        {
            get { return this._OPNAME; }
            set { this._OPNAME = value; }
        }

        private string _CUSTOMER;
        /// <summary>
        /// CUSTOMER
        /// </summary>
        [Property()]
        public string CUSTOMER
        {
            get { return _CUSTOMER; }
            set { _CUSTOMER = value; }
        }


        private string _RESPONSE;
        /// <summary>
        /// RESPONSE
        /// </summary>
        [Property()]
        public string RESPONSE
        {
            get { return this._RESPONSE; }
            set { this._RESPONSE = value; }
        }

        private int _RESPONSESTATUS = 0;
        /// <summary>
        /// 审核状态
        /// </summary>
        [Property()]
        public int RESPONSESTATUS
        {
            get { return this._RESPONSESTATUS; }
            set { this._RESPONSESTATUS = value; }
        }

        private string _strResponseStatus;

        public string StrResponseStatus
        {
            get { return _strResponseStatus; }
            set { _strResponseStatus = value; }
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

        private int _isMemeber = 1;
        /// <summary>
        /// 是否启用
        /// </summary>
        [Property()]
        public int ISMEMBER
        {
            get { return this._isMemeber; }
            set { this._isMemeber = value; }
        }

        private int _ISCOMPLETE = 0;
        /// <summary>
        /// 报销提交状态
        /// </summary>
        [Property()]
        public int ISCOMPLETE
        {
            get { return this._ISCOMPLETE; }
            set { this._ISCOMPLETE = value; }
        }

        private string _COMPLETE;
        /// <summary>
        /// 报销提交状态
        /// </summary>
        public string COMPLETE
        {
            get { return this._COMPLETE; }
            set { this._COMPLETE = value; }
        }

        private string _memeberstatus;
        /// <summary>
        /// 
        /// </summary>
        public string memeberstatus
        {
            get { return this._memeberstatus; }
            set { this._memeberstatus = value; }
        }

        private int _YEAR;
        /// <summary>
        /// OPUID
        /// </summary>
        [Property()]
        public int YEAR
        {
            get { return this._YEAR; }
            set { this._YEAR = value; }
        }

        private int _MONTH;
        /// <summary>
        /// MONTH
        /// </summary>
        [Property()]
        public int MONTH
        {
            get { return this._MONTH; }
            set { this._MONTH = value; }
        }

        private string _LEADERRESPONSE;
        /// <summary>
        /// _LEADERRESPONSE
        /// </summary>
        [Property()]
        public string LEADERRESPONSE
        {
            get { return this._LEADERRESPONSE; }
            set { this._LEADERRESPONSE = value; }
        }

        private int _LEADERRESPONSESTATUS = 0;
        /// <summary>
        /// 审核状态
        /// </summary>
        [Property()]
        public int LEADERRESPONSESTATUS
        {
            get { return this._LEADERRESPONSESTATUS; }
            set { this._LEADERRESPONSESTATUS = value; }
        }

        private string _strLeaderResponseStatus;

        public string StrLeaderResponseStatus
        {
            get { return _strLeaderResponseStatus; }
            set { _strLeaderResponseStatus = value; }
        }

        private int _ISQTCT = 0;
        /// <summary>
        /// 公司账户
        /// </summary>
        [Property()]
        public int ISQTCT
        {
            get { return this._ISQTCT; }
            set { this._ISQTCT = value; }
        }

        private int _GROUPNO;
        /// <summary>
        /// GROUPNO
        /// </summary>
        [Property()]
        public int GROUPNO
        {
            get { return this._GROUPNO; }
            set { this._GROUPNO = value; }
        }

        private string _STRGROUPNO;
        /// <summary>
        /// STRGROUPNO
        /// </summary>
        public string STRGROUPNO
        {
            get { return this._STRGROUPNO; }
            set { this._STRGROUPNO = value; }
        }


        private string _grouptotal;

        public string grouptotal
        {
            get { return _grouptotal; }
            set { this._grouptotal = value; }
        }


        private Boolean _ischecked;

        public Boolean IsChecked
        {
            get { return _ischecked; }
            set
            {
                _ischecked = value;
                OnPropertyChanged("IsChecked");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (null != PropertyChanged)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
