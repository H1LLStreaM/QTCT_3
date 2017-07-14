using Castle.ActiveRecord;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WY.Library.Model
{
    [Serializable]
    [ActiveRecord("TB_PROJECT")]
    public class TB_PROJECT : BaseModel
    {
        private int _Index;

        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }

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

        private string _CONTRACTNO;
        /// <summary>
        /// 合同编号
        /// </summary>
        [Property()]
        public string CONTRACTNO
        {
            get { return this._CONTRACTNO; }
            set { this._CONTRACTNO = value; }
        }

        private string _OBJECTNAME;
        /// <summary>
        /// 工程名称
        /// </summary>
        [Property()]
        public string OBJECTNAME
        {
            get { return this._OBJECTNAME; }
            set { this._OBJECTNAME = value; }
        }

        private string _COMPANYNAME;
        /// <summary>
        /// 客户公司名称
        /// </summary>
        [Property()]
        public string COMPANYNAME
        {
            get { return this._COMPANYNAME; }
            set { this._COMPANYNAME = value; }
        }

        private string _ADDRESS;
        /// <summary>
        /// 工程地址
        /// </summary>
        [Property()]
        public string ADDRESS
        {
            get { return this._ADDRESS; }
            set { this._ADDRESS = value; }
        }

        private DateTime _BILLDATE;
        /// <summary>
        /// 开票时间
        /// </summary>
        [Property()]
        public DateTime BILLDATE
        {
            get { return this._BILLDATE; }
            set { this._BILLDATE = value; }
        }

        private string _BILLSTATUS;
        /// <summary>
        /// BILLSTATUS
        /// </summary>
        [Property()]
        public string BILLSTATUS
        {
            get { return this._BILLSTATUS; }
            set { this._BILLSTATUS = value; }
        }

        private DateTime _BEGINDATE;
        /// <summary>
        /// BEGINDATE
        /// </summary>
        [Property()]
        public DateTime BEGINDATE
        {
            get { return this._BEGINDATE; }
            set { this._BEGINDATE = value; }
        }

        private DateTime _ENDDATE;
        /// <summary>
        /// ENDDATE
        /// </summary>
        [Property()]
        public DateTime ENDDATE
        {
            get { return this._ENDDATE; }
            set { this._ENDDATE = value; }
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

        private int _OBJECTTYPE;
        /// <summary>
        /// OBJECTTYPE
        /// </summary>
        [Property()]
        public int OBJECTTYPE
        {
            get { return this._OBJECTTYPE; }
            set { this._OBJECTTYPE = value; }
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

        private string _OBJECTRATIO;
        /// <summary>
        /// 工程提成比率
        /// </summary>
        [Property()]
        public string OBJECTRATIO
        {
            get { return this._OBJECTRATIO; }
            set { this._OBJECTRATIO = value; }
        }

        private string _TEAMMEMBER;
        /// <summary>
        /// 项目成员ID组
        /// </summary>
        [Property()]
        public string TEAMMEMBER
        {
            get { return this._TEAMMEMBER; }
            set { this._TEAMMEMBER = value; }
        }

        private string _TEAMLEDER;
        /// <summary>
        /// 项目负责人ID
        /// </summary>
        [Property()]
        public string TEAMLEDER
        {
            get { return this._TEAMLEDER; }
            set { this._TEAMLEDER = value; }
        }

        private string _TEAMLEDERNAME;
        /// <summary>
        /// 项目负责人名称
        /// </summary>
        [Property()]
        public string TEAMLEDERNAME
        {
            get { return this._TEAMLEDERNAME; }
            set { this._TEAMLEDERNAME = value; }
        }

        private string _MEMO;
        /// <summary>
        /// Table_Value
        /// </summary>
        [Property()]
        public string MEMO
        {
            get { return this._MEMO; }
            set { this._MEMO = value; }
        }

        
        private string _CREATEUSER;
        /// <summary>
        /// 销售人员
        /// </summary>
        [Property()]        
        public string CREATEUSER
        {
            get { return _CREATEUSER; }
            set { _CREATEUSER = value; }
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

        private decimal _RATIO1;
        /// <summary>
        /// _RATIO1
        /// </summary>
        [Property()]
        public decimal RATIO1
        {
            get { return this._RATIO1; }
            set { this._RATIO1 = value; }
        }

        private decimal _RATIO2;
        /// <summary>
        /// RATIO2
        /// </summary>
        [Property()]
        public decimal RATIO2
        {
            get { return this._RATIO2; }
            set { this._RATIO2 = value; }
        }

        private decimal _ZHEKOU;
        /// <summary>
        /// ZHEKOU
        /// </summary>
        [Property()]
        public decimal ZHEKOU
        {
            get { return this._ZHEKOU; }
            set { this._ZHEKOU = value; }
        }

        private string _ProjIdentity;

        public string ProjIdentity
        {
            get { return _ProjIdentity; }
            set { _ProjIdentity = value; }
        }
    }
}
