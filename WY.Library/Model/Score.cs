using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using WY.Library.Model;

namespace Library.Model
{
	/// <summary>
	/// ��������ҵ���� (���ݳ־ò����)
	/// </summary>
	[Serializable]
	[ActiveRecord("Score")]
	public class Score : BaseModel
	{
		private int _id;
		/// <summary>
		/// ҵ��ID
		/// </summary>
		[PrimaryKey(PrimaryKeyType.Native)]
		public int Id
		{
			get { return this._id; }
			set { this._id = value; }
		}

		private int _salerid;
		/// <summary>
		/// ����ID
		/// </summary>
		[Property()]
		public int Salerid
		{
			get { return this._salerid; }
			set { this._salerid = value; }
		}

		private int _cableid;
		/// <summary>
		/// ��·����ID
		/// </summary>
		[Property()]
		public int Cableid
		{
			get { return this._cableid; }
			set { this._cableid = value; }
		}

		private int _cusid;
		/// <summary>
		/// �ͻ�ID
		/// </summary>
		[Property()]
		public int Cusid
		{
			get { return this._cusid; }
			set { this._cusid = value; }
		}

        private int _commissid;
        /// <summary>
        /// ���ID
        /// </summary>
        [Property()]
        public int Commissid
        {
            get { return _commissid; }
            set { _commissid = value; }
        }


		private decimal _money;
		/// <summary>
		/// Ӷ����
		/// </summary>
		[Property()]
		public decimal Money
		{
			get { return this._money; }
			set { this._money = value; }
		}

	}
}
