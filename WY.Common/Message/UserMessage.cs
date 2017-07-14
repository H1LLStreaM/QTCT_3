using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WY.Common.Message
{
    public enum EnmMessageLevel
    {
        消息,
        确认,
        警告,
        错误
    }

    public class UserMessage
    {
        //public static UserMessage UnkownMessage = new UserMessage("E999", "消息编码[{0}]不存在!");
        public static UserMessage UnkownMessage = new UserMessage("E999", "{0}");
        private string _code;

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }
        private EnmMessageLevel _type;

        public EnmMessageLevel Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private MessageBoxIcon _icon;

        public MessageBoxIcon Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }
        private MessageBoxButtons _button;

        public MessageBoxButtons Button
        {
            get { return _button; }
            set { _button = value; }
        }
        private MessageBoxDefaultButton _defaultbutton;

        public MessageBoxDefaultButton Defaultbutton
        {
            get { return _defaultbutton; }
            set { _defaultbutton = value; }
        }

        private string[] _args;

        public string[] Args
        {
            get { return _args; }
            set { _args = value; }
        }

        private UserMessage(string msgid, string text)
        {
            _code = msgid;
            _text = text;
        }

        public UserMessage()
        {
        }
    }
}
