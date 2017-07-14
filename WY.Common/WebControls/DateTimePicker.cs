using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Web;

namespace WY.Common.WebControls
{
    public class DateTimePicker : TextBox
    {
        private string _formatString = "yyyy-MM-dd";
        /// <summary>
        /// ���ڸ�ʽ
        /// </summary>
        public string FormatString
        {
            get { return _formatString; }
            set { _formatString = value; }
        }

        private Nullable<DateTime> _value;
        /// <summary>
        /// ��ǰ����/ʱ��ֵ
        /// </summary>
        public Nullable<DateTime> Value
        {
            get
            {
                if (this.Text.Trim() == "")
                {
                    return new Nullable<DateTime>();
                }
                else
                {
                    DateTime d;
                    if (DateTime.TryParse(this.Text.Trim(), out d))
                    {
                        return d;
                    }
                    else
                    {
                        this.Text = "";
                        return new Nullable<DateTime>();
                        //throw new Exception("��������ڸ�ʽ����ȷ��");
                    }
                }
            }
            set
            {
                if (value.HasValue)
                {
                    this.Text = value.Value.ToString(this._formatString);
                }
                else
                {
                    this.Text = "";
                }
            }
        }

        public override bool ReadOnly
        {
            //get
            //{
            //    return base.ReadOnly;
            //}
            set
            {
                //base.ReadOnly = value;
                this.Attributes.Add("contentEditable", "false");
            }
        }

        private bool _showClear = true;
        /// <summary>
        /// �Ƿ���ʾ�����ť
        /// </summary>
        public bool ShowClear
        {
            get { return _showClear; }
            set { _showClear = value; }
        }

        #region Render

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            try
            {
                writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Position, "relative");
                writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

                base.Render(writer);

                writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Position, "absolute");
                writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Left, (this.Width.Value - 15) + "px");
                writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Top, "2px");
                //writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Position, "relative");
                //writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.MarginLeft, "-18px");
                //writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.MarginTop, "0px");
                //writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Top, "4px");
                if (this.Enabled)
                {
                    writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Cursor, "pointer");
                    writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Src, "../js/DatePicker/skin/datePicker.gif");
                    if (this._showClear)
                    {
                        writer.AddAttribute("onclick", "WdatePicker({el:'" + this.ClientID + "',dateFmt:'" + this._formatString + "'})");
                    }
                    else
                    {
                        writer.AddAttribute("onclick", "WdatePicker({el:'" + this.ClientID + "',dateFmt:'" + this._formatString + "',isShowClear:false})");
                    }
                }
                else
                {
                    writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Src, "../js/DatePicker/skin/datePicker_Disabled.gif");
                }
                writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Img);

                writer.RenderEndTag();

                writer.RenderEndTag();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

    }
}
