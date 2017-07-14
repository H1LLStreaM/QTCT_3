using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using WY.Library.Model;

namespace Library.Model
{
	/// <summary>
	/// 销售渠道业绩表 (数据持久层对象)
	/// </summary>
	[Serializable]
	[ActiveRecord("Score")]
	public class Score : BaseModel
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

		private int _salerid;
		/// <summary>
		/// 渠道ID
		/// </summary>
		[Property()]
		public int Salerid
		{
			get { return this._salerid; }
			set { this._salerid = value; }
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

		private int _cusid;
		/// <summary>
		/// 客户ID
		/// </summary>
		[Property()]
		public int Cusid
		{
			get { return this._cusid; }
			set { this._cusid = value; }
		}

        private int _commissid;
        /// <summary>
        /// 提成ID
        /// </summary>
        [Property()]
        public int Commissid
        {
            get { return _commissid; }
            set { _commissid = value; }
        }


		private decimal _money;
		/// <summary>
		/// 佣金金额
		/// </summary>
		[Property()]
		public decimal Money
		{
			get { return this._money; }
			set { this._money = value; }
		}

	}
}
