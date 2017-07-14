using System;
using System.Collections.Generic;
using System.Text;
using Castle.ActiveRecord;
using WY.Library.Model;
using System.Threading.Tasks;
using System.ComponentModel;

namespace WY.Library.Model
{
	/// <summary>
	/// 用户表(员工表) (数据持久层对象)
	/// </summary>
	[Serializable]
    [ActiveRecord("tb_user")]
	public class TB_User : BaseModel
	{
		private int _Id;
		/// <summary>
		/// 用户ID
		/// </summary>
        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
		{
			get { return this._Id; }
			set { this._Id = value; }
		}

        private string _User_Code;
        /// <summary>
        /// 逻辑删除标志
        /// </summary>
        [Property()]
        public string USER_CODE
        {
            get { return this._User_Code; }
            set { this._User_Code = value; }
        }

        private string _USER_NAME;
		/// <summary>
		/// 用户姓名
		/// </summary>
        [Property()]
        public string USER_NAME
		{
            get { return this._USER_NAME; }
            set { this._USER_NAME = value; }
		}

        private string _User_ID;
		/// <summary>
		/// 登录名
		/// </summary>
        [Property()]
        public string USER_ID
		{
            get { return this._User_ID; }
            set { this._User_ID = value; }
		}

        private string _Password;
		/// <summary>
		/// 登录密码
		/// </summary>
        [Property()]
        public string PASSWORD
		{
			get { return this._Password; }
			set { this._Password = value; }
		}

		private int _RoleID;
		/// <summary>
		/// 角色
		/// </summary>
        [Property()]
		public int ROLEID
		{
            get { return this._RoleID; }
            set { this._RoleID = value; }
		}

		private int _Status=1;
		/// <summary>
		/// 是否启用
		/// </summary>
        [Property()]
		public int STATUS
		{
            get { return this._Status; }
            set { this._Status = value; }
		}

        private string _DEPT;
        /// <summary>
        /// 部门
        /// </summary>
        [Property()]
        public string DEPT
        {
            get { return this._DEPT; }
            set { this._DEPT = value; }
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
