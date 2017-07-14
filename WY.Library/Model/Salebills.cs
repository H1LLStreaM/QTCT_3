using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using WY.Library.Model;

namespace Library.Model
{
	/// <summary>
	/// 销售账单明细表 (数据持久层对象)
	/// </summary>
	[Serializable]
	[ActiveRecord("Salebills")]
	public class Salebills : BaseModel
	{
		private int _id;
		/// <summary>
		/// 业绩ID
		/// </summary>
		[PrimaryKey(PrimaryKeyType.Native)]
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		private int _cableid;
		/// <summary>
		/// 电路代码ID
		/// </summary>
		[Property()]
		public int Cableid
		{
			get { return this._cableid; }
			set { this._cableid = value; }
		}

		private int _salerid;
		/// <summary>
		/// 主销售渠道ID
		/// </summary>
		[Property()]
		public int Salerid
		{
            get { return this._salerid; }
            set { this._salerid = value; }
		}

		private int _customerid;
		/// <summary>
		/// 客户ID
		/// </summary>
		[Property()]
		public int Customerid
		{
			get { return this._customerid; }
			set { this._customerid = value; }
		}

		private int _year;
		/// <summary>
		/// 导入年度
		/// </summary>
		[Property()]
		public int Year
		{
			get { return this._year; }
			set { this._year = value; }
		}

		private int _month;
		/// <summary>
		/// 导入月度
		/// </summary>
		[Property()]
		public int Month
		{
			get { return this._month; }
			set { this._month = value; }
		}

		private string _contract;
		/// <summary>
		/// 合同编号
		/// </summary>
		[Property()]
		public string Contract
		{
			get { return this._contract; }
			set { this._contract = value; }
		}

		private string _excable;
		/// <summary>
		/// 原电路代码
		/// </summary>
		[Property()]
		public string Excable
		{
			get { return this._excable; }
			set { this._excable = value; }
		}

		private string _cablenature;
		/// <summary>
		/// 电路性质
		/// </summary>
		[Property()]
		public string Cablenature
		{
			get { return this._cablenature; }
			set { this._cablenature = value; }
		}

		private string _exspeed;
		/// <summary>
		/// 原速率
		/// </summary>
		[Property()]
		public string ExSpeed
		{
			get { return this._exspeed; }
			set { this._exspeed = value; }
		}

		private string _speed;
		/// <summary>
		/// 速率
		/// </summary>
		[Property()]
		public string Speed
		{
			get { return this._speed; }
			set { this._speed = value; }
		}

		private string _cablerange;
		/// <summary>
		/// 通信范围
		/// </summary>
		[Property()]
		public string Cablerange
		{
            get { return this._cablerange; }
            set { this._cablerange = value; }
		}

		private string _completedate;
		/// <summary>
		/// 完工日期
		/// </summary>
		[Property()]
		public string Completedate
		{
			get { return this._completedate; }
			set { this._completedate = value; }
		}

		private string _removedate;
		/// <summary>
		/// 拆机日期
		/// </summary>
		[Property()]
		public string Removedate
		{
			get { return this._removedate; }
			set { this._removedate = value; }
		}

		private string _settlementstart;
		/// <summary>
		/// 结算起始日
		/// </summary>
		[Property()]
		public string Settlementstart
		{
			get { return this._settlementstart; }
			set { this._settlementstart = value; }
		}

		private string _settlementend;
		/// <summary>
		/// 结算截止日
		/// </summary>
		[Property()]
		public string Settlementend
		{
			get { return this._settlementend; }
			set { this._settlementend = value; }
		}

        private decimal _receivable;
		/// <summary>
		/// 应收月租费
		/// </summary>
		[Property()]
        public decimal Receivable
		{
			get { return this._receivable; }
			set { this._receivable = value; }
		}

        private decimal _writeoff;
		/// <summary>
		/// 销账情况
		/// </summary>
		[Property()]
        public decimal Writeoff
		{
			get { return this._writeoff; }
			set { this._writeoff = value; }
		}

		private string _ratio;
		/// <summary>
		/// 结算比例
		/// </summary>
		[Property()]
		public string Ratio
		{
			get { return this._ratio; }
			set { this._ratio = value; }
		}

		private decimal _proxy;
		/// <summary>
		/// 代理费
		/// </summary>
		[Property()]
        public decimal Proxy
		{
			get { return this._proxy; }
			set { this._proxy = value; }
		}

		private string _writeoffstatus;
		/// <summary>
		/// 销账情况
		/// </summary>
		[Property()]
		public string Writeoffstatus
		{
			get { return this._writeoffstatus; }
			set { this._writeoffstatus = value; }
		}

        private int _flag;
        /// <summary>
        /// 结算类型标志
        /// </summary>
        [Property()]
        public int Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }

		private string _default1;
		/// <summary>
		/// 备用字段01
		/// </summary>
		[Property()]
		public string Default1
		{
			get { return this._default1; }
			set { this._default1 = value; }
		}

		private string _default2;
		/// <summary>
		/// 备用字段02
		/// </summary>
		[Property()]
		public string Default2
		{
			get { return this._default2; }
			set { this._default2 = value; }
		}

		private string _default3;
		/// <summary>
		/// 备用字段03
		/// </summary>
		[Property()]
		public string Default3
		{
			get { return this._default3; }
			set { this._default3 = value; }
		}

		private string _default4;
		/// <summary>
		/// 备用字段04
		/// </summary>
		[Property()]
		public string Default4
		{
			get { return this._default4; }
			set { this._default4 = value; }
		}

		private string _default5;
		/// <summary>
		/// 备用字段05
		/// </summary>
		[Property()]
		public string Default5
		{
			get { return this._default5; }
			set { this._default5 = value; }
		}

		private int _isdeleted;
		/// <summary>
		/// 逻辑删除标志
		/// </summary>
		[Property()]
		public int Isdeleted
		{
			get { return this._isdeleted; }
			set { this._isdeleted = value; }
		}
	

        private Nullable<DateTime> _createtime;
        /// <summary>
        /// 创建时间
        /// </summary>
        [Property()]
        public Nullable<DateTime> Createtime
        {
            get { return this._createtime; }
            set { this._createtime = value; }
        }

        private string _createuser;
        /// <summary>
        /// 创建人
        /// </summary>
        [Property()]
        public string Createuser
        {
            get { return this._createuser; }
            set { this._createuser = value; }
        }

        private Nullable<DateTime> _updatetime;
        /// <summary>
        /// 修改时间
        /// </summary>
        public Nullable<DateTime> Updatetime
        {
            get { return this._updatetime; }
            set { this._updatetime = value; }
        }

        private string _updateuser;
        /// <summary>
        /// 修改人
        /// </summary>
        [Property()]
        public string Updateuser
        {
            get { return this._updateuser; }
            set { this._updateuser = value; }
        }

        private string _updateip;
        /// <summary>
        /// 最后一次更新客户端IP
        /// </summary>
        [Property()]
        public string Updateip
        {
            get { return this._updateip; }
            set { this._updateip = value; }
        }

        //private TB_User _saler;
        ///// <summary>
        ///// 录入人员
        ///// </summary>
        //[BelongsTo("Salerid", NotFoundBehaviour = NotFoundBehaviour.Ignore, Insert = false, Update = false)]
        //public TB_User Saler
        //{
        //    get { return this._saler; }
        //    set { this._saler = value; }
        //}

        ///// <summary>
        ///// 客户
        ///// </summary>
        //private Customer _customer;
        //[BelongsTo("Customerid", NotFoundBehaviour = NotFoundBehaviour.Ignore, Insert = false, Update = false)]
        //public Customer Customer
        //{
        //    get { return this._customer; }
        //    set { this._customer = value; }
        //}

        ///// <summary>
        ///// 完工电路代码
        ///// </summary>
        //private Cable _cable;
        //[BelongsTo("Cableid", NotFoundBehaviour = NotFoundBehaviour.Ignore, Insert = false, Update = false)]
        //public Cable Cable
        //{
        //    get { return this._cable; }
        //    set { this._cable = value; }
        //}

	}
}
