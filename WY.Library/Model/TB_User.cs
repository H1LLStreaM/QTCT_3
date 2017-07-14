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
	/// �û���(Ա����) (���ݳ־ò����)
	/// </summary>
	[Serializable]
    [ActiveRecord("tb_user")]
	public class TB_User : BaseModel
	{
		private int _Id;
		/// <summary>
		/// �û�ID
		/// </summary>
        [PrimaryKey(PrimaryKeyType.Native)]
        public int Id
		{
			get { return this._Id; }
			set { this._Id = value; }
		}

        private string _User_Code;
        /// <summary>
        /// �߼�ɾ����־
        /// </summary>
        [Property()]
        public string USER_CODE
        {
            get { return this._User_Code; }
            set { this._User_Code = value; }
        }

        private string _USER_NAME;
		/// <summary>
		/// �û�����
		/// </summary>
        [Property()]
        public string USER_NAME
		{
            get { return this._USER_NAME; }
            set { this._USER_NAME = value; }
		}

        private string _User_ID;
		/// <summary>
		/// ��¼��
		/// </summary>
        [Property()]
        public string USER_ID
		{
            get { return this._User_ID; }
            set { this._User_ID = value; }
		}

        private string _Password;
		/// <summary>
		/// ��¼����
		/// </summary>
        [Property()]
        public string PASSWORD
		{
			get { return this._Password; }
			set { this._Password = value; }
		}

		private int _RoleID;
		/// <summary>
		/// ��ɫ
		/// </summary>
        [Property()]
		public int ROLEID
		{
            get { return this._RoleID; }
            set { this._RoleID = value; }
		}

		private int _Status=1;
		/// <summary>
		/// �Ƿ�����
		/// </summary>
        [Property()]
		public int STATUS
		{
            get { return this._Status; }
            set { this._Status = value; }
		}

        private string _DEPT;
        /// <summary>
        /// ����
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
