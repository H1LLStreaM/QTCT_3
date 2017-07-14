using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace WY.Common.Utility
{
    [Serializable]
    public class AjaxResult
    {
        const string FIRST_ERROR = "first_error";

        private bool _success;
        /// <summary>
        /// 是否成功
        /// </summary>
        public bool Success
        {
            get { return _success; }
            set { _success = value; }
        }

        private object _value;
        /// <summary>
        /// 返回值
        /// </summary>
        public object Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private string _message;
        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private List<AjaxErrorInfo> _errorlist = new List<AjaxErrorInfo>();
        /// <summary>
        /// 错误列表（有多个错误信息时）
        /// </summary>
        public List<AjaxErrorInfo> Errorlist
        {
            get { return _errorlist; }
            set { _errorlist = value; }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string Error
        {
            get
            {
                if (this._errorlist.Count > 0)
                {
                    return this._errorlist[0].Error;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                this.AddError(FIRST_ERROR, value);
            }
        }

        public void AddError(string key, string value)
        {
            foreach (AjaxErrorInfo item in this._errorlist)
            {
                if (item.Key.ToLower() == key.ToLower())
                {
                    item.Error = value;
                    return;
                }
            }

            _errorlist.Add(new AjaxErrorInfo(key, value));
        }

        public AjaxErrorInfo GetError(string key)
        {
            foreach (AjaxErrorInfo item in this._errorlist)
            {
                if (item.Key.ToLower() == key.ToLower())
                {
                    return item;
                }
            }

            return null;
        }
    }

    public class AjaxErrorInfo
    {
        private string _key;

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string _error;

        public string Error
        {
            get { return _error; }
            set { _error = value; }
        }

        public AjaxErrorInfo(string key, string value)
        {
            this._key = key;
            this._error = value;
        }
    }
}
