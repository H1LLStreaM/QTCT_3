using System;
using System.Collections.Generic;
using System.Text;
using System.Web;

namespace WY.Common.WebControls
{
    public enum EmnMessageBoxIcon
    {
        None,
        Information,
        Warning,
        Error
    }

    public class WebMessageBox : System.Web.UI.WebControls.WebControl
    {
        /// <summary>
        /// 消息框默认宽度
        /// </summary>
        private const int DEFAULT_WIDTH = 260;

        /// <summary>
        /// 消息框默认高度
        /// </summary>
        private const int DEFAULT_HEIGHT = 120;

        /// <summary>
        /// 自动隐藏默认时间（毫秒）
        /// </summary>
        private const int DEFAULT_INTERVAL = 1000;

        private string icon_baseurl;

        public WebMessageBox()
        {
            this.Width = DEFAULT_WIDTH;
            this.Height = DEFAULT_HEIGHT;

            icon_baseurl = HttpContext.Current.Request.ApplicationPath + "/images/popup/";
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            StringBuilder sbhtml = new StringBuilder();

            sbhtml.AppendFormat("<div id=\"{0}\" class=\"popup\" style=\"width:{1};height:{2};\">\r\n", this.ClientID, this.Width, this.Height);
            sbhtml.AppendFormat(" <div class=\"popup-overlay\" style=\"width:{0}px;height:{1}px;\"></div>\r\n", this.Width.Value - 10, this.Height.Value - 15);
            sbhtml.AppendFormat(" <div class=\"popup-container\" style=\"width:{0}px;height:{1}px;\">\r\n", this.Width.Value - 10, this.Height.Value - 15);
            sbhtml.Append("     <div class=\"popup-titlebar\">\r\n");
            sbhtml.AppendFormat("     {0}\r\n", this._title);
            sbhtml.Append("       <div class=\"popup-titlebar-close\" onmouseover=\"this.className='popup-titlebar-close-hover';\" onmouseout=\"this.className='popup-titlebar-close';\" onclick=\"document.getElementById('" + this.ClientID + "').style.display='none';\"></div>\r\n");
            sbhtml.Append("     </div>\r\n");
            sbhtml.Append("     <div class=\"popup-content\">\r\n");
            sbhtml.Append("     <table class=\"popup-content-table\"><tr>\r\n");
            sbhtml.AppendFormat("     <td valign=\"top\" width=\"40\"><img border=\"0\" src=\"{0}\" /></td>\r\n", this.GetIconUrl());
            sbhtml.AppendFormat("     <td valign=\"bottom\" style=\"font-size: 12px; line-height:24px;\" >{0}<BR /></td>\r\n", this._message);
            sbhtml.Append("     </tr></table>\r\n");
            sbhtml.Append("     <br />\r\n");
            sbhtml.Append(" </div>\r\n");
            sbhtml.Append("</div>\r\n");

            if (this._autoHideInterval > 0)
            {
                string useragent = HttpContext.Current.Request.UserAgent;
                if (useragent.IndexOf("MSIE 6.0") >= 0)
                {
                    sbhtml.AppendFormat("<script>setTimeout(\"document.getElementById('{0}').style.display='none';\"," + this._autoHideInterval + ");</script>\r\n", this.ClientID);
                }
                else
                {
                    sbhtml.AppendFormat("<script>setTimeout(\"hideDivInterval('{0}')\"," + this._autoHideInterval + ");</script>\r\n", this.ClientID);
                }
            }

            if (!string.IsNullOrEmpty(this._redirecturl))
            {
                sbhtml.AppendFormat("<script>setTimeout(\"window.location.href='{0}'\"," + this._autoHideInterval + ");</script>\r\n", this._redirecturl);
            }

            writer.Write(sbhtml.ToString());
        }

        protected string GetIconUrl()
        {
            switch (this._msgIcon)
            {
                case EmnMessageBoxIcon.Information:
                    return icon_baseurl + "information.gif";
                case EmnMessageBoxIcon.Warning:
                    return icon_baseurl + "warning.gif";
                case EmnMessageBoxIcon.Error:
                    return icon_baseurl + "error.gif";
                case EmnMessageBoxIcon.None:
                    return icon_baseurl + "information.gif";
                default:
                    return icon_baseurl + "information.gif";
            }
        }

        private string _title;

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _message;

        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        private int _autoHideInterval = 0;

        public int AutoHideInterval
        {
            get { return _autoHideInterval; }
            set { _autoHideInterval = value; }
        }

        private EmnMessageBoxIcon _msgIcon;

        public EmnMessageBoxIcon MsgIcon
        {
            get { return _msgIcon; }
            set { _msgIcon = value; }
        }

        private string _redirecturl;

        public string Redirecturl
        {
            get { return _redirecturl; }
            set { _redirecturl = value; }
        }

        #region static Show/ShowAutoHide/ShowAutoHideRedirect

        public static void Show(System.Web.UI.Page page, string title, string message, EmnMessageBoxIcon icon)
        {
            Show(page, title, message, icon, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public static void Show(System.Web.UI.Page page, string title, string message, EmnMessageBoxIcon icon, int width, int height)
        {
            WebMessageBox msgbox = new WebMessageBox();
            msgbox.Title = title;
            msgbox.Message = message;
            msgbox.MsgIcon = icon;
            msgbox.Width = width;
            msgbox.Height = height;
            foreach (System.Web.UI.Control ctl in page.Controls)
            {
                if (typeof(System.Web.UI.HtmlControls.HtmlForm).IsInstanceOfType(ctl))
                {
                    ctl.Controls.Add(msgbox);
                    return;
                }
            }
            page.Controls.Add(msgbox);
        }

        public static void ShowAutoHide(System.Web.UI.Page page, string title, string message, EmnMessageBoxIcon icon)
        {
            ShowAutoHide(page, title, message, icon, DEFAULT_INTERVAL, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public static void ShowAutoHide(System.Web.UI.Page page, string title, string message, EmnMessageBoxIcon icon, int width, int height)
        {
            ShowAutoHide(page, title, message, icon, DEFAULT_INTERVAL, width, height);
        }

        public static void ShowAutoHide(System.Web.UI.Page page, string title, string message, EmnMessageBoxIcon icon, int hideinterval)
        {
            ShowAutoHide(page, title, message, icon, hideinterval, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public static void ShowAutoHide(System.Web.UI.Page page, string title, string message, EmnMessageBoxIcon icon, int hideinterval, int width, int height)
        {
            WebMessageBox msgbox = new WebMessageBox();
            msgbox.Title = title;
            msgbox.Message = message;
            msgbox.MsgIcon = icon;
            msgbox.Width = width;
            msgbox.Height = height;
            msgbox.AutoHideInterval = hideinterval;
            try
            {
                foreach (System.Web.UI.Control ctl in page.Controls)
                {
                    if (typeof(System.Web.UI.HtmlControls.HtmlForm).IsInstanceOfType(ctl))
                    {
                        ctl.Controls.Add(msgbox);
                        return;
                    }
                }
            }
            catch { }
            page.Controls.Add(msgbox);
        }

        public static void ShowAutoHideAndRedirect(System.Web.UI.Page page, string title, string message, EmnMessageBoxIcon icon, string redirecturl)
        {
            ShowAutoHideAndRedirect(page, title, message, icon, redirecturl, DEFAULT_INTERVAL, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public static void ShowAutoHideAndRedirect(System.Web.UI.Page page, string title, string message, EmnMessageBoxIcon icon, string redirecturl, int width, int height)
        {
            ShowAutoHideAndRedirect(page, title, message, icon, redirecturl, DEFAULT_INTERVAL, width, height);
        }

        public static void ShowAutoHideAndRedirect(System.Web.UI.Page page, string title, string message, EmnMessageBoxIcon icon, string redirecturl, int hideinterval)
        {
            ShowAutoHideAndRedirect(page, title, message, icon, redirecturl, hideinterval, DEFAULT_WIDTH, DEFAULT_HEIGHT);
        }

        public static void ShowAutoHideAndRedirect(System.Web.UI.Page page, string title, string message, EmnMessageBoxIcon icon, string redirecturl, int hideinterval, int width, int height)
        {
            WebMessageBox msgbox = new WebMessageBox();
            msgbox.Title = title;
            msgbox.Message = message;
            msgbox.MsgIcon = icon;
            msgbox.Width = width;
            msgbox.Height = height;
            msgbox.AutoHideInterval = hideinterval;
            msgbox.Redirecturl = redirecturl;
            try
            {
                foreach (System.Web.UI.Control ctl in page.Controls)
                {
                    if (typeof(System.Web.UI.HtmlControls.HtmlForm).IsInstanceOfType(ctl))
                    {
                        ctl.Controls.Add(msgbox);
                        return;
                    }
                }
            }
            catch { }
            page.Controls.Add(msgbox);
        }

        #endregion

    }
}
