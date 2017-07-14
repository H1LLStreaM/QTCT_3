using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Configuration;
using System.Web;
using KZ.Common.WebControls;

namespace KZ.Common
{
    /// <summary>
    /// �õ������ļ���Message��Ϣ�ĵ�̬��
    /// </summary>
    public sealed class Message
    {
        static Message instance = null;
        static readonly object padlock = new object();

        private IDictionary<string, UserMessage> messagelist;
        //private IDictionary<string, string> messagelist_en;

        private Message()
        {
            string configfile = HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath + "/Message.config");
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(configfile);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xmldoc.NameTable);
            nsmgr.AddNamespace("messageConfig", "http://www.wewin.com.cn/messageConfig");
            XmlNodeList messageNodes = xmldoc.SelectNodes("//messageConfig:Message", nsmgr);
            messagelist = new Dictionary<string, UserMessage>();
            string key, value;
            foreach (XmlNode messageNode in messageNodes)
            {
                key = messageNode.Attributes["id"].Value;
                value = messageNode.Attributes["value"].Value;
                messagelist.Add(key, new UserMessage(key, value));
            }

            //messageNodes = xmldoc.SelectSingleNode("//messageConfig:ENG", nsmgr).ChildNodes;
            //messagelist_en = new Dictionary<string, string>();
            //foreach (XmlNode messageNode in messageNodes)
            //{
            //    key = messageNode.Attributes["id"].Value;
            //    value = messageNode.Attributes["value"].Value;
            //    messagelist_en.Add(key, value);
            //}
        }

        public UserMessage GetMessage(string messageid, params string[] args)
        {
            UserMessage message = null;

            message = (UserMessage)messagelist[messageid];

            if (message == null)
            {
                message = UserMessage.UnkownMessage;
                message.Args = new string[] { messageid };
            }
            else
            {
                message.Args = args;
            }

            return message;
        }

        /// <summary>
        /// ����message��idֵ�õ�message��value
        /// </summary>
        /// <param name="key">message��idֵ</param>
        /// <param name="messageParams">message���滻����ֵ</param>
        /// <returns>message��value</returns>
        public string GetMessageText(string messageid, params string[] args)
        {
            UserMessage message = GetMessage(messageid, args);
            return string.Format(message.Text, message.Args);
        }

        public static void Show(System.Web.UI.Page page, UserMessage message)
        {
            Show(page, message.Key, message.Args);
        }

        public static void Show(System.Web.UI.Page page, string messageid, params string[] args)
        {
            string message = Message.Instance.GetMessageText(messageid, args);
            page.ClientScript.RegisterStartupScript(page.GetType(), Guid.NewGuid().ToString(), "<script>" + "alert('" + message + "')" + "</script>");
            //WebMessageBox.ShowAutoHide(page, GetTitle(messageid), message, GetIcon(messageid));
        }

        public static void ShowAndRedirect(System.Web.UI.Page page, string redirecturl, UserMessage message)
        {
            ShowAndRedirect(page, redirecturl, message.Key, message.Args);
        }

        public static void ShowAndRedirect(System.Web.UI.Page page, string redirecturl, string messageid, params string[] messageParams)
        {
            string message = Message.Instance.GetMessageText(messageid, messageParams);
            page.ClientScript.RegisterStartupScript(page.GetType(), Guid.NewGuid().ToString(), "<script>" + "alert('" + message + "');location.href='" + redirecturl + "';" + "</script>");
            //WebMessageBox.ShowAutoHideAndRedirect(page, GetTitle(messageid), message, GetIcon(messageid), redirecturl);
        }

        public static void ShowAndRunJS(System.Web.UI.Page page, string js, UserMessage message)
        {
            ShowAndRunJS(page, js, message.Key, message.Args);
        }

        public static void ShowAndRunJS(System.Web.UI.Page page, string js, string messageid, params string[] messageParams)
        {
            string message = Message.Instance.GetMessageText(messageid, messageParams);
            page.ClientScript.RegisterStartupScript(page.GetType(), Guid.NewGuid().ToString(), "<script>" + "alert('" + message + "');" + js + "</script>");
            //WebMessageBox.ShowAutoHideAndRedirect(page, GetTitle(messageid), message, GetIcon(messageid), redirecturl);
        }

        public static void RunJSAndShow(System.Web.UI.Page page, string js, UserMessage message)
        {
            RunJSAndShow(page, js, message.Key, message.Args);
        }

        public static void RunJSAndShow(System.Web.UI.Page page, string js, string messageid, params string[] messageParams)
        {
            if (!js.EndsWith(";")) js += ";";

            string message = Message.Instance.GetMessageText(messageid, messageParams);
            page.ClientScript.RegisterStartupScript(page.GetType(), Guid.NewGuid().ToString(), "<script>" + js + "alert('" + message + "');" + "</script>");
            //WebMessageBox.ShowAutoHideAndRedirect(page, GetTitle(messageid), message, GetIcon(messageid), redirecturl);
        }

        public static string GetTitle(string messageid)
        {
            if (messageid.StartsWith("E"))
            {
                return "����";
            }
            else if (messageid.StartsWith("I"))
            {
                return "��Ϣ";
            }
            else if (messageid.StartsWith("W"))
            {
                return "����";
            }
            else
            {
                return "��ҳ��Ϣ";
            }
        }

        public static EmnMessageBoxIcon GetIcon(string messageid)
        {
            if (messageid.StartsWith("E"))
            {
                return EmnMessageBoxIcon.Error;
            }
            else if (messageid.StartsWith("I"))
            {
                return EmnMessageBoxIcon.Information;
            }
            else if (messageid.StartsWith("W"))
            {
                return EmnMessageBoxIcon.Warning;
            }
            else
            {
                return EmnMessageBoxIcon.None;
            }
        }

        /// <summary>
        /// �õ�Message�����ȫ�ַ��ʵ㷽��
        /// </summary>
        public static Message Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new Message();
                    }
                    return instance;
                }
            }
        }

        /// <summary>
        /// ���¼��������ļ�
        /// </summary>
        public static void Reload()
        {
            lock (padlock)
            {
                instance = null;
                instance = new Message();
            }
        }
    }

    public class UserMessage
    {
        public static UserMessage UnkownMessage = new UserMessage("E999", "��Ϣ����[{0}]������!");

        private string _key;

        public string Key
        {
            get { return _key; }
            set { _key = value; }
        }

        private string _text;

        public string Text
        {
            get { return _text; }
            set { _text = value; }
        }

        private string[] _args;

        public string[] Args
        {
            get { return _args; }
            set { _args = value; }
        }

        public UserMessage(string key, string text)
        {
            this._key = key;
            this._text = text;
        }
    }
}
