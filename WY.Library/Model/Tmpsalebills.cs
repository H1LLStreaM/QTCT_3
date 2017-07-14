using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using WY.Library.Model;

namespace Library.Model
{
	/// <summary>
	/// 临时账单表
	/// </summary>
	[Serializable]
	[ActiveRecord("Tmpsalebills")]
	public class Tmpsalebills : BaseModel
	{
		private string _cable;
		/// <summary>
		/// 电路代码
		/// </summary>
		[Property()]
		public string Cable
		{
			get { return this._cable; }
			set { this._cable = value; }
		}

		private string _customername;
		/// <summary>
		/// 客户名称
		/// </summary>
		[Property()]
		public string Customername
		{
			get { return this._customername; }
			set { this._customername = value; }
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

		private string _oldcable;
		/// <summary>
		/// 原电路代码
		/// </summary>
		[Property()]
		public string Oldcable
		{
			get { return this._oldcable; }
			set { this._oldcable = value; }
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

		private string _speedold;
		/// <summary>
		/// 原速率
		/// </summary>
		[Property()]
		public string Speedold
		{
			get { return this._speedold; }
			set { this._speedold = value; }
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

		private string _range;
		/// <summary>
		/// 通信范围
		/// </summary>
		[Property()]
		public string Range
		{
			get { return this._range; }
			set { this._range = value; }
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

		private string _receivable;
		/// <summary>
		/// 应收月租费
		/// </summary>
		[Property()]
		public string Receivable
		{
			get { return this._receivable; }
			set { this._receivable = value; }
		}

		private string _writeoff;
		/// <summary>
		/// 销账金额
		/// </summary>
		[Property()]
		public string Writeoff
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

		private string _proxy;
		/// <summary>
		/// 代理费
		/// </summary>
		[Property()]
		public string Proxy
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

        private string _errinfo;
        /// <summary>
        /// 错误信息
        /// </summary>
        [Property()]
        public string Errinfo
        {
            get { return _errinfo; }
            set { _errinfo = value; }
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

		private int _createuserid;
		/// <summary>
		/// 创建人
		/// </summary>
		[Property()]
		public int Createuserid
		{
			get { return this._createuserid; }
			set { this._createuserid = value; }
		}

		private Nullable<DateTime> _updatetime;
		/// <summary>
		/// 修改时间
		/// </summary>
		[Property()]
		public Nullable<DateTime> Updatetime
		{
			get { return this._updatetime; }
			set { this._updatetime = value; }
		}

		private int _updateuserid;
		/// <summary>
		/// 修改人
		/// </summary>
		[Property()]
		public int Updateuserid
		{
			get { return this._updateuserid; }
			set { this._updateuserid = value; }
		}

	}
}
