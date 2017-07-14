using System;
using System.Collections.Generic;
using System.Text;
using WY.Common.Utility;
using System.Data;

namespace WY.Common.Framework
{
    [Flags]
    public enum MyFieldEditFlags
    {
        Select = 1,
        Insert = 2,
        Update = 4,
        None = 8
    }

    public enum enmQueryMode
    {
        等于,
        包含,
        不等于,
        大于,
        小于
    }

    public class MyField
    {
        private string _headText;
        /// <summary>
        /// 标题
        /// </summary>
        public string HeadText
        {
            get { return _headText; }
            set { _headText = value; }
        }

        private string _fieldName;
        /// <summary>
        /// 字段名
        /// </summary>
        public string FieldName
        {
            get { return _fieldName; }
            set { _fieldName = value; }
        }

        private object _fieldValue;
        /// <summary>
        /// 字段值
        /// </summary>
        public object FieldValue
        {
            get { return _fieldValue; }
            set { _fieldValue = value; }
        }

        private DbType _dataType;
        /// <summary>
        /// 数据类型
        /// </summary>
        public DbType DataType
        {
            get { return _dataType; }
            set { _dataType = value; }
        }

        private int _minLength = 0;
        /// <summary>
        /// <para>最小长度</para>
        /// <para>0表示不限</para>
        /// </summary>
        public int MinLength
        {
            get { return _minLength; }
            set { _minLength = value; }
        }

        private int _maxLength = 0;
        /// <summary>
        /// <para>最大长度</para>
        /// <para>0表示不限</para>
        /// </summary>
        public int MaxLength
        {
            get { return _maxLength; }
            set { _maxLength = value; }
        }

        private bool _nullable = true;
        /// <summary>
        /// 是否可为空，默认是
        /// </summary>
        public bool Nullable
        {
            get { return _nullable; }
            set { _nullable = value; }
        }

        private bool _uniqueKey = false;
        /// <summary>
        /// 是否唯一键
        /// </summary>
        public bool UniqueKey
        {
            get { return _uniqueKey; }
            set { _uniqueKey = value; }
        }

        private string _displayFormat;
        /// <summary>
        /// 显示格式
        /// </summary>
        public string DisplayFormat
        {
            get { return _displayFormat; }
            set { _displayFormat = value; }
        }

        private MyFieldEditFlags _editFlags = MyFieldEditFlags.Select | MyFieldEditFlags.Insert | MyFieldEditFlags.Update;
        /// <summary>
        /// 模式：允许为Select、Insert、Update中的一个或多个
        /// </summary>
        public MyFieldEditFlags EditFlags
        {
            get { return _editFlags; }
            set { _editFlags = value; }
        }

        private bool _allowQuery = false;
        /// <summary>
        /// 允许查询
        /// </summary>
        public bool AllowQuery
        {
            get { return _allowQuery; }
            set { _allowQuery = value; }
        }

        private enmQueryMode _queryMode = enmQueryMode.等于;
        /// <summary>
        /// 查询方式
        /// </summary>
        public enmQueryMode QueryMode
        {
            get { return _queryMode; }
            set { _queryMode = value; }
        }

        private int _col;
        /// <summary>
        /// 所在行
        /// </summary>
        public int Col
        {
            get { return _col; }
            set { _col = value; }
        }

        private int _row;
        /// <summary>
        /// 所在列
        /// </summary>
        public int Row
        {
            get { return _row; }
            set { _row = value; }
        }

        public MyField()
        {
            ;
        }

        public MyField(string fieldName)
        {
            this._fieldName = fieldName;
        }

        public MyField(string fieldName, object fieldValue)
        {
            this._fieldName = fieldName;
            this._fieldValue = fieldValue;
        }

        public MyField(string fieldName, object fieldValue, enmQueryMode queryMode)
        {
            this._fieldName = fieldName;
            this._fieldValue = fieldValue;
            this._queryMode = queryMode;
        }

        public MyField(string fieldName, object fieldValue, Type type)
        {
            this._fieldName = fieldName;
            this._fieldValue = fieldValue;
            this._dataType = TableManager.TypeToDbType(type);
        }

        /// <summary>
        /// <para>itemArray[0]:HeadText,string,必须</para>
        /// <para>itemArray[1]:FieldName,string,必须</para>
        /// <para>itemArray[2]:Nullable,bool</para>
        /// <para>itemArray[3]:MaxLength,int</para>
        /// <para>itemArray[4]:DisplayFormat,string</para>
        /// <para>itemArray[5]:MinLength,int</para>
        /// <para>itemArray[6]:EditFlags,EnumEditFlags</para>
        /// </summary>
        /// <param name="itemArray"></param>
        public MyField(object[] itemArray)
        {
            this._headText = itemArray[0].ToString();
            this._fieldName = itemArray[1].ToString();
            if (itemArray.Length > 2)
            {
                this._nullable = (bool)itemArray[2];
            }
            if (itemArray.Length > 3)
            {
                this._maxLength = (int)itemArray[3];
            }
            if (itemArray.Length > 4)
            {
                this._displayFormat = (string)itemArray[4];
            }
            if (itemArray.Length > 5)
            {
                this._minLength = (int)itemArray[5];
            }
            if (itemArray.Length > 6)
            {
                this._editFlags = (MyFieldEditFlags)itemArray[6];
            }
        }

        public MyField Clone()
        {
            MyField ret = new MyField();

            BeanHelper.ObjectClone(this, ret);

            return ret;
        }

        #region CreateArray
        /// <summary>
        /// <para>itemArray[n][0]:HeadText,string,必须</para>
        /// <para>itemArray[n][1]:FieldName,string,必须</para>
        /// <para>itemArray[n][2]:Nullable,bool</para>
        /// <para>itemArray[n][3]:MaxLength,int</para>
        /// <para>itemArray[n][4]:DisplayFormat,string</para>
        /// <para>itemArray[n][5]:MinLength,int</para>
        /// <para>itemArray[n][6]:EditFlags,EnumEditFlags</para>
        /// </summary>
        /// <param name="itemArray"></param>
        /// <returns></returns>
        public static MyField[] CreateArray(object[] itemArray)
        {
            MyField[] arr = new MyField[itemArray.Length - 1];
            for (int i = 0; i < itemArray.Length; i++)
            {
                arr[i] = new MyField((object[])itemArray[i]);
            }
            return arr;
        }
        #endregion

    }
}
