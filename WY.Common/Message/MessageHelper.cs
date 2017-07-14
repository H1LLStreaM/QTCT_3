using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Collections;
using WY.Common.Framework;
using System.Data;
using System.Reflection;
using System.IO;

namespace WY.Common.Message
{
    public class MessageHelper
    {
        //public const string MESSAGE_FILENAME = "message.config";
        const string MESSAGE_ITEM = "Item";
        const string MESSAGE_ID = "ID";
        const string MESSAGE_TEXT = "Text";
        const string MESSAGE_LEVEL = "Level";

        private static Hashtable mMsgDef = new Hashtable();

        static MessageHelper()
        {
            ReLoad();
        }

        public static string GetMessageText(string msgid, params string[] args)
        {
            UserMessage message = GetMessage(msgid, args);
            return string.Format(message.Text, message.Args);
        }

        public static UserMessage GetMessage(string msgid, params string[] args)
        {
            UserMessage message = null;

            message = (UserMessage)mMsgDef[msgid];

            if (message == null)
            {
                message = UserMessage.UnkownMessage;
                message.Args = new string[] { msgid };
            }
            else
            {
                message.Args = args;
            }

            return message;
        }

        #region 初始化消息文本
        public static void ReLoad()
        {
            mMsgDef.Clear();
            //XmlDocument doc = new XmlDocument();
            ////doc.Load(Application.StartupPath + "\\" + MessageHelper.MESSAGE_FILENAME);
            //Assembly assembly = Assembly.Load("WY.Common");
            //Stream stream = assembly.GetManifestResourceStream("WY.Common.Message.message.config");
            //doc.Load(stream);

            //XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            //nsmgr.AddNamespace("messageConfig", "http://www.we-win.com.cn/messageConfig");
            //XmlNodeList messageNodes = doc.SelectNodes("//messageConfig:Item", nsmgr);
            ////XmlNodeList nodes = doc.DocumentElement.SelectNodes(MESSAGE_ITEM);
            //foreach (XmlNode node in messageNodes)
            //{
            //    UserMessage message = new UserMessage();
            //    message.Code = node.Attributes[MESSAGE_ID].Value;
            //    message.Text = node.Attributes[MESSAGE_TEXT].Value;
            //    message.Type = (EnmMessageLevel)Enum.Parse(typeof(EnmMessageLevel), node.Attributes[MESSAGE_LEVEL].Value.ToString());
            //    FillMessage(message);
            //    mMsgDef.Add(message.Code, message);
            //}
        }

        private static void FillMessage(UserMessage message)
        {
            switch (message.Type)
            {
                case EnmMessageLevel.消息:
                    message.Icon = MessageBoxIcon.Information;
                    message.Button = MessageBoxButtons.OK;
                    message.Defaultbutton = MessageBoxDefaultButton.Button1;
                    break;
                case EnmMessageLevel.警告:
                    message.Icon = MessageBoxIcon.Warning;
                    message.Button = MessageBoxButtons.YesNo;
                    message.Defaultbutton = MessageBoxDefaultButton.Button2;
                    break;
                case EnmMessageLevel.确认:
                    message.Icon = MessageBoxIcon.Question;
                    message.Button = MessageBoxButtons.YesNo;
                    message.Defaultbutton = MessageBoxDefaultButton.Button1;
                    break;
                case EnmMessageLevel.错误:
                    message.Icon = MessageBoxIcon.Exclamation;
                    message.Button = MessageBoxButtons.OK;
                    message.Defaultbutton = MessageBoxDefaultButton.Button1;
                    break;
                default:
                    message.Icon = MessageBoxIcon.None;
                    message.Button = MessageBoxButtons.OK;
                    message.Defaultbutton = MessageBoxDefaultButton.Button1;
                    break;
            }

        }
        #endregion

        #region 显示消息 (public)
        /// <summary>
        /// 显示消息
        /// </summary>
        /// <param name="strmsgid">消息代码</param>
        /// <param name="args">参数（可以是多个）</param>
        /// <returns>用户点击的那个按钮</returns>
        public static DialogResult ShowMessage(string strmsgid, params string[] args)
        {

            UserMessage message = (UserMessage)mMsgDef[strmsgid];

            if (message == null)
            {
                message = UserMessage.UnkownMessage;
                message.Args = new string[] { strmsgid };
            }
            else
            {
                message.Args = args;
            }

            return ShowMessage(message);
        }

        public static DialogResult ShowMessage(UserMessage message)
        {
            DialogResult result = MessageBox.Show(string.Format(message.Text, message.Args)
                                                , message.Type.ToString()
                                                , message.Button
                                                , message.Icon
                                                , message.Defaultbutton);

            return result;
        }
        #endregion
    }
}
