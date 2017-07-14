using System;
using System.Collections.Generic;
using System.Text;

namespace WY.Library.Page
{
    public class BasePopUserControl : System.Web.UI.UserControl
    {
        private string _title = "БъЬт";

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        private int _width = 200;

        public int Width
        {
            get { return _width; }
            set { _width = value; }
        }

        private int _height = 80;

        public int Height
        {
            get { return _height; }
            set { _height = value; }
        }

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddAttribute("id", this.ClientID);
            writer.AddAttribute("class", "jtDialogBox");
            writer.AddStyleAttribute("z-index", "9999");
            writer.AddStyleAttribute("width", this._width + "px");
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

            writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Width, "100%");
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Table);

            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr);
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);

            //********************************************************
            //header
            writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Cellpadding, "0");
            writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Cellspacing, "0");
            writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Border, "0");
            writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Width, "100%");
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Table);

            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr);

            writer.AddAttribute("class", "tbLeft");
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
            writer.RenderEndTag();  //td

            //title --- start
            writer.AddAttribute("class", "Title");
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);
            writer.WriteEncodedText(this._title);
            writer.RenderEndTag();  //td
            //title --- end

            //close --- start
            writer.AddAttribute("class", "tbRight");
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);

            writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Href, "javascript:void(0)");
            //writer.AddAttribute("onclick", "BOX_remove('" + this.ClientID + "');");
            this.SetCloseMethod(writer);
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.A);

            writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Src, WebUtils.AppRoot + "images/js_alert/window_close.gif");
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Img);
            writer.RenderEndTag();  //img

            writer.RenderEndTag();  //a

            writer.RenderEndTag();  //td
            //close --- end

            writer.RenderEndTag();  //tr
            writer.RenderEndTag();  //table
            //********************************************************

            writer.RenderEndTag();  //td
            writer.RenderEndTag();  //tr

            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Tr);
            writer.AddAttribute("class", "MainPanel");
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Td);

            //********************************************************
            //content
            writer.AddAttribute("class", "ContentArea");
            writer.AddStyleAttribute("height", this._height + "px");
            writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

            base.Render(writer);

            writer.RenderEndTag();

            //********************************************************

            writer.RenderEndTag();  //td
            writer.RenderEndTag();  //tr
                
            writer.RenderEndTag();  //table

            writer.RenderEndTag();  //div
        }

        public virtual void SetCloseMethod(System.Web.UI.HtmlTextWriter writer)
        {
            writer.AddAttribute("onclick", "BOX_remove('" + this.ClientID + "');");
        }
    }
}
