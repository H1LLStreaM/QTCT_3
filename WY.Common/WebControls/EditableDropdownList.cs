using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI.WebControls;
using System.Collections;
using WY.Common.Utility;

namespace WY.Common.WebControls
{
    public class EditableDropdownList : TextBox
    {
        #region property

        private int _dropdownWidth;

        public int DropdownWidth
        {
            get { return _dropdownWidth; }
            set { _dropdownWidth = value; }
        }

        private bool _isAutoPost = false;

        public bool IsAutoPost
        {
            get { return _isAutoPost; }
            set { _isAutoPost = value; }
        }

        public List<DictionaryEntry> Items
        {
            get
            {
                if (ViewState["DataSource"] == null)
                {
                    return new List<DictionaryEntry>();
                }
                else
                {
                    return (List<DictionaryEntry>)ViewState["DataSource"];
                }
            }
            set
            {
                ViewState["DataSource"] = value;
            }
        }

        private string _selectedValue;

        public string SelectedValue
        {
            get
            {
                if (this.Page.IsPostBack)
                {
                    return this.Page.Request[this.ClientID + "_selectedid"];
                }
                else
                {
                    return _selectedValue;
                }
            }
            set
            {
                _selectedValue = value;

                foreach (DictionaryEntry item in this.Items)
                {
                    if (item.Key.ToString() == value)
                    {
                        base.Text = item.Value.ToString();
                    }
                }
            }
        }

        /// <summary>
        /// 该属性用来返回客户端脚本注册名称
        /// </summary>
        private string ScriptName
        {
            get
            {
                return "EditableDropdownList";
            }
        }

        private int _maxDropdownRows = 10;
        /// <summary>
        /// 下拉框最大行数
        /// </summary>
        public int MaxDropdownRows
        {
            get { return _maxDropdownRows; }
            set { _maxDropdownRows = value; }
        }

        /// <summary>
        /// 下拉框数据区域ClientID（Ajax用）
        /// </summary>
        public string DropdownLayerClientID
        {
            get { return this.ClientID + "_dropdownItems"; }
        }

        /// <summary>
        /// 
        /// </summary>
        public string ValueTextBoxClientID
        {
            get { return this.ClientID + "_selectedid"; }
        }

        #endregion

        #region MyRegion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
        }

        #endregion

        #region AddItem

        public void AddItem(DictionaryEntry item)
        {
            List<DictionaryEntry> list = this.Items;
            list.Add(item);
            this.Items = list;
        }

        #endregion

        #region Render

        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            try
            {
                this.Attributes.Add("MaxDropdownRows", this._maxDropdownRows.ToString());
                //Image img = new Image();
                //img.ImageUrl = "../images/dropdown.gif";
                //this.Controls.Add(img);

                writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Position, "relative");
                writer.AddStyleAttribute("z-index", "100");
                writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

                base.Render(writer);

                writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Position, "absolute");
                writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Left, (this.Width.Value - 15) + "px");
                writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Top, "3px");
                writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Width, "15px");
                writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Height, "15px");
                writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Cursor, "pointer");
                if (this.Items.Count == 0)
                {
                    //writer.AddStyleAttribute(System.Web.UI.HtmlTextWriterStyle.Display, "none");
                }
                writer.AddAttribute("id", this.ClientID + "_dropdownbutton");
                writer.AddAttribute("class", "dropdownbutton");
                writer.AddAttribute("onmouseover", "this.className='dropdownbutton2';");
                writer.AddAttribute("onmouseout", "this.className='dropdownbutton';");
                writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Div);

                //writer.AddAttribute(System.Web.UI.HtmlTextWriterAttribute.Src, "../images/dropdown2.gif");
                //writer.RenderBeginTag(System.Web.UI.HtmlTextWriterTag.Img);
                //writer.RenderEndTag();

                writer.RenderEndTag();


                writer.RenderEndTag();
                if (!this.Page.ClientScript.IsStartupScriptRegistered(this.ScriptName))
                {
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), this.ScriptName, this.GetJavascript(), false);
                    this.Page.ClientScript.RegisterStartupScript(this.GetType(), Guid.NewGuid().ToString(), this.GetMyJavascript(), false);
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        protected override void AddAttributesToRender(System.Web.UI.HtmlTextWriter writer)
        {
            try
            {
                base.AddAttributesToRender(writer);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region GetJavascript

        private string GetMyJavascript()
        {
            double width = this.Width.Value; // 250;
            double height = 30;
            if (this.Items.Count < this._maxDropdownRows)
            {
                height = 19 * this.Items.Count;
            }
            else
            {
                height = 19 * this._maxDropdownRows;
            }

            try
            {
                JavaScriptWriter js = new JavaScriptWriter(true);

                //js.AddLine("document.write(\"<div id='" + this.ClientID + "_selectLayer' style='position: absolute; border:1px solid #ccc; overflow-x:hidden;overflow-y:auto; z-index: 9999; width:" + width + "px; height:" + height + "px;background-color:#FFFF97; display: none'>\");");
                js.AddLine("document.write(\"<div style='position: absolute; z-index: 9998; width:" + width + "px; height:" + height + "px;background-color:#C0C0C0; display: none'></div>\");");
                js.AddLine("document.write(\"<div id='" + this.ClientID + "_selectLayer' style='position: absolute; border:1px solid #676767; overflow-x:hidden;overflow-y:auto; z-index: 9999; width:" + width + "px; height:" + height + "px;background-color:#FFFF97; display: none'>\");");
                js.AddLine("document.write(\"<iframe frameborder='0' style='width:100%;height:100%;position:absolute;left:0px;top:0px;'></iframe>\")");
                js.AddLine("document.write(\"<div id='" + this.DropdownLayerClientID + "' style='width:100%;height:100%;position:absolute;left:0px;top:0px;background-color:#FFFF97;'>\")");
                js.AddLine("document.write(\"" + this.GetDropdownItemsHTML() + "\");");
                js.AddLine("document.write(\"</div>\");");
                js.AddLine("document.write(\"" + "<input type='hidden' id='" + this.ValueTextBoxClientID + "' name='" + this.ValueTextBoxClientID + "' value='" + this.SelectedValue + "' />" + "\");");
                js.AddLine("document.write(\"</div>\");");
                js.AddLine("initEditableDropdown('" + this.ClientID + "');");

                return js.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetJavascript()
        {
            try
            {
                JavaScriptWriter js = new JavaScriptWriter(true);
                js.AddLine("document.write(\"<style>\");");
                js.AddLine("document.write(\".dropdownbutton{background:url(../images/dropdown2.gif) no-repeat;}\");");
                js.AddLine("document.write(\".dropdownbutton2{background:url(../images/dropdown2.gif) no-repeat -13px 0px;}\");");
                js.AddLine("document.write(\".label{overflow:hidden;white-space:nowrap;text-overflow: ellipsis;text-align:left;}\");");
                js.AddLine("document.write(\".rowitem{border-bottom:1px dotted #ccc; height:18px;cursor:default;overflow:hidden;white-space:nowrap;text-overflow: ellipsis;}\");");
                js.AddLine("document.write(\".rowitem2{border-bottom:1px dotted #ccc; height:18px;cursor:default;background-color:#6BCFFA;overflow:hidden;white-space:nowrap;text-overflow: ellipsis;}\");");
                //js.AddLine("document.write(\".rowitem:hover{background-color:#6BCFFA;}\");");
                js.AddLine("document.write(\"</style>\");");

                js.AddLine("function setDropdownLayerLocation(txt,layer,retcount)");
                js.AddLine("{");
                js.AddLine("    var offset=$(txt).offset();");
                js.AddLine("    var maxrows=parseInt($(txt).attr('MaxDropdownRows'));");
                js.AddLine("    var iheight;");
                js.AddLine("    if(retcount<maxrows) iheight=19*retcount;else iheight=19*maxrows;");
                js.AddLine("    if(iheight==0) iheight=19;");
                js.AddLine("    $(layer).css({");
                js.AddLine("        top: offset.top + txt.offsetHeight + 2,");
                js.AddLine("        left: offset.left,");
                js.AddLine("        height: iheight");
                js.AddLine("    });");
                js.AddLine("    $(layer).prev().css({");
                js.AddLine("        top: offset.top + txt.offsetHeight + 2 + 5,");
                js.AddLine("        left: offset.left + 5,");
                js.AddLine("        height: iheight");
                js.AddLine("    });");
                js.AddLine("    $(layer).show();");
                js.AddLine("}");

                js.AddLine("function initEditableDropdown(id,enablesearch)");
                js.AddLine("{");
                js.AddLine("    var txt=document.getElementById(id);");
                js.AddLine("    var dropdownbtn=document.getElementById(id+\"_dropdownbutton\");");
                js.AddLine("    var valtxt=document.getElementById(id+\"_selectedid\");");
                js.AddLine("    var layer=document.getElementById(id+\"_selectLayer\");");
                //js.AddLine("    var openme=function(){$(layer).show();}");
                //js.AddLine("    txt.onfocus=function(event){");
                js.AddLine("    dropdownbtn.onclick=function(event){");
                //js.AddLine("        setDropdownLayerLocation(txt,layer,);");
                js.AddLine("        searchDropdownItem(id,''); $(layer).show();txt.focus();txt.select();");
                js.AddLine("    }");
                //js.AddLine("    $(txt).keyup(function(event){if(event.keyCode==9||event.keyCode==13){searchDropdownItem(id,'');}else{$(valtxt).val('');searchDropdownItem(id,this.value);}});");
                js.AddLine("    $(txt).keyup(function(event){");
                js.AddLine("        switch(event.keyCode){");
                js.AddLine("            case 9:case 13:case 16:case 17:case 18:case 20:case 27:case 33:case 34:case 35:case 36:case 37:case 38:case 39:case 40:");
                js.AddLine("                break;");
                js.AddLine("            default:");
                js.AddLine("                $(valtxt).val('');");
                js.AddLine("                searchDropdownItem(id,this.value);");
                js.AddLine("                break;");
                js.AddLine("        }");
                js.AddLine("    });");
                js.AddLine("    $(txt).keydown(function(event){if(event.keyCode==9||event.keyCode==13) {$(layer).hide();}});");
                //js.AddLine("    $(txt).mousedown(function(event){setTimeout(openme,100);});");
                js.AddLine("    $(layer).mousedown(function(event){event.stopPropagation();});");
                js.AddLine("    $(document).mousedown(function(event){$(layer).hide();});");
                js.AddLine("}");
                js.AddLine("");
                js.AddLine("function setDropdownSelectedItem(txtid,value,text)");
                js.AddLine("{");
                js.AddLine("    var txt = document.getElementById(txtid);");
                js.AddLine("    var valtxt = document.getElementById(txtid+\"_selectedid\");");
                js.AddLine("    var layer = document.getElementById(txtid+\"_selectLayer\");");
                js.AddLine("    $(layer).hide();");
                js.AddLine("    valtxt.value = value; ");
                js.AddLine("    txt.value = text; ");
                js.AddLine("    txt.focus();txt.select();");
                if (this.IsAutoPost)
                {
                    js.AddLine("    __doPostBack(clientid, \"\");");
                }
                js.AddLine("}");
                js.AddLine("");
                js.AddLine("function searchDropdownItem(id,text)");
                js.AddLine("{");
                js.AddLine("    var txt=document.getElementById(id);");
                js.AddLine("    var layer = document.getElementById(id+'_selectLayer');");
                js.AddLine("    var dropdown = document.getElementById(id+'_dropdownItems');");
                js.AddLine("    var items = $(dropdown).children();");
                js.AddLine("    var retcount=0;");
                js.AddLine("    for(var i=0;i<items.length;i++){");
                js.AddLine("        var arr = $(items.get(i)).children();");
                js.AddLine("        if(text!=''){");
                js.AddLine("            var ret=false;");
                js.AddLine("            for(var j=0;j<arr.length;j++){");
                js.AddLine("                var itemvalue = arr.get(j).innerText;");
                js.AddLine("                if(itemvalue.toLowerCase().indexOf(text.toLowerCase())>=0){");
                js.AddLine("                    ret=true;break;");
                js.AddLine("                }");
                js.AddLine("            }");
                js.AddLine("            if(ret) {");
                js.AddLine("                retcount++;");
                js.AddLine("                $(items.get(i)).show();");
                js.AddLine("            }");
                js.AddLine("            else{");
                js.AddLine("                $(items.get(i)).hide();");
                js.AddLine("            }");
                js.AddLine("        }");
                js.AddLine("        else");
                js.AddLine("        {");
                js.AddLine("            $(items.get(i)).show();");
                js.AddLine("            retcount++;");
                js.AddLine("        }");
                js.AddLine("    }");
                js.AddLine("    setDropdownLayerLocation(txt,layer,retcount);");
                js.AddLine("    if(retcount>0){");
                js.AddLine("        $(layer).show();");
                js.AddLine("    }");
                js.AddLine("    else{");
                js.AddLine("        $(layer).hide();");
                js.AddLine("    }");
                js.AddLine("}");

                return js.ToString();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

        #region GetDropdownItemsHTML

        public string GetDropdownItemsHTML()
        {
            return EditableDropdownList.GetDropdownItemsHTML(this.Items, this.ClientID, this.Width);
        }

        public static string GetDropdownItemsHTML(List<DictionaryEntry> datalist, string clientId, Unit width)
        {
            string html = "";

            foreach (DictionaryEntry item in datalist)
            {
                html += "<div class='rowitem' onmouseout='this.className=\\\"rowitem\\\"' onmouseover='this.className=\\\"rowitem2\\\"' style='width:" + width.ToString() + "px;padding-left:2px;line-height:18px;' onclick='setDropdownSelectedItem(\\\"" + clientId + "\\\",\\\"" + item.Key + "\\\",\\\"" + item.Value + "\\\")'>";
                html += "<div>" + item.Value.ToString() + "</div>";
                html += "<div style='display:none'>" + Utils.GetSpell(item.Value.ToString()) + "</div>";
                html += "</div>";
            }

            return html;
        }

        #endregion
    }
}
