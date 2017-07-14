using System;
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.ComponentModel;
using System.Web.UI.WebControls;
using System.Collections;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI.HtmlControls;

namespace WY.Common.WebControls
{
    [DefaultProperty("Text"), ToolboxData("<{0}:DataGrid runat=server></{0}:DataGrid>")]
    public class DataGrid : System.Web.UI.WebControls.DataGrid, INamingContainer
    {
        /// <summary>
        /// 跳转按钮
        /// </summary>
        public Button GoToPagerButton = new Button();

        /// <summary>
        /// 跳转文本框
        /// </summary>
        public System.Web.UI.HtmlControls.HtmlInputText PageSizeInputText = new HtmlInputText();

        /// <summary>
        /// 跳转文本框
        /// </summary>
        public System.Web.UI.HtmlControls.HtmlInputText GoToPagerInputText = new HtmlInputText();

        /// <summary>
        /// 构造函数
        /// </summary>
        public DataGrid()
            : base()
        {

            //this.GridLines=GridLines.Horizontal;
            //this.BorderWidth=0;
            //this.CellPadding=3;
            this.CssClass = "datalist";
            //this.ShowFooter=true;
            this.ShowHeader = true;
            this.AutoGenerateColumns = false;
            //this.PagerStyle.CssClass="datagridPager";
            //this.FooterStyle.CssClass="datagridFooter";
            this.SelectedItemStyle.CssClass = "datagridSelectedItem";
            this.ItemStyle.CssClass = "";// "datagridItem";

            this.HeaderStyle.CssClass = "category";
            this.PagerStyle.Mode = PagerMode.NumericPages;
            this.PageSize = 20;
            this.AllowCustomPaging = false;
            this.AllowPaging = true;
            this.AllowSorting = true;
            //this.DataKeyField = "ID";

            GoToPagerInputText.Attributes.Add("class", "textbox");
            PageSizeInputText.Attributes.Add("class", "textbox");
            GoToPagerButton.CssClass = "button";

            this.ItemDataBound += new DataGridItemEventHandler(DataGrid_ItemDataBound);
            this.ItemCreated += new System.Web.UI.WebControls.DataGridItemEventHandler(this.DataGrid_ItemCreated);
            this.SortCommand += new System.Web.UI.WebControls.DataGridSortCommandEventHandler(this.SortGrid);
            GoToPagerButton.Click += new EventHandler(GoToPagerButton_Click);
        }

        /// <summary>
        /// 跳转按钮链接事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void GoToPagerButton_Click(object sender, System.EventArgs e)
        {
            if (this.GoToPagerInputText.Value != "")
            {

                if (!this.AllowCustomPaging)
                {


                    if (!Regex.IsMatch(this.GoToPagerInputText.Value, "^\\d+?\\d*$"))
                    {
                        //LoadCurrentPageIndex(0);
                        this.OnPageIndexChanged(new DataGridPageChangedEventArgs(this, 0));
                        return;
                    }
                    else
                    {
                        //***********************************************************
                        if (Regex.IsMatch(this.PageSizeInputText.Value, "^\\d+?\\d*$"))
                        {
                            int pagesize = Convert.ToInt32(this.PageSizeInputText.Value.Trim());
                            if (pagesize > 0)
                            {
                                this.PageSize = pagesize;
                            }
                        }
                        //***********************************************************

                        int gotoPager = Convert.ToInt32(this.GoToPagerInputText.Value.Trim());

                        if (gotoPager < 1)
                        {
                            //LoadCurrentPageIndex(0);
                            this.OnPageIndexChanged(new DataGridPageChangedEventArgs(this, 0));
                            return;
                        }

                        if (gotoPager > this.PageCount)
                        {
                            //LoadCurrentPageIndex(this.PageCount - 1);
                            this.OnPageIndexChanged(new DataGridPageChangedEventArgs(this, this.PageCount - 1));
                        }
                        else
                        {
                            //LoadCurrentPageIndex(gotoPager - 1);
                            this.OnPageIndexChanged(new DataGridPageChangedEventArgs(this, gotoPager - 1));
                        }
                    }
                }
                else
                {
                    //设置当前DATAGRID的CurrentPageIndex属性
                    SetCurrentPageIndexByGoToPager();
                }

            }
        }

        /// <summary>
        /// 设置当前DATAGRID的CurrentPageIndex属性
        /// </summary>
        private void SetCurrentPageIndexByGoToPager()
        {
            if (this.GoToPagerInputText.Value != "")
            {

                if (!Regex.IsMatch(this.GoToPagerInputText.Value, "^\\d+?\\d*$"))
                {
                    this.CurrentPageIndex = 0;
                    return;
                }
                else if (!Regex.IsMatch(this.PageSizeInputText.Value, "^\\d+?\\d*$"))
                {
                    return;
                }
                else
                {
                    //***********************************************************
                    if (Regex.IsMatch(this.PageSizeInputText.Value, "^\\d+?\\d*$"))
                    {
                        int pagesize = Convert.ToInt32(this.PageSizeInputText.Value.Trim());
                        if (pagesize > 0)
                        {
                            this.PageSize = pagesize;
                        }
                    }
                    //***********************************************************

                    int gotoPager = Convert.ToInt32(this.GoToPagerInputText.Value.Trim());

                    if (gotoPager < 1)
                    {
                        CurrentPageIndex = 0;
                        return;
                    }

                    if (gotoPager > PageCount)
                    {
                        CurrentPageIndex = this.PageCount - 1;
                    }
                    else
                    {
                        this.CurrentPageIndex = gotoPager - 1;
                    }
                }
            }
        }

        /// <summary>
        /// 跳转指定分页
        /// </summary>
        /// <param name="value"></param>
        public void LoadCurrentPageIndex(int value)
        {
            this.CurrentPageIndex = (value < 0) ? 0 : value;
        }

        /// <summary>
        /// 数据列表项绑定事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataGrid_ItemDataBound(object sender, System.Web.UI.WebControls.DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                e.Item.Attributes.Add("onmouseover", "this.className='mouseoverstyle'");
                if (e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    e.Item.CssClass = "mouseoutaltrstyle";
                    e.Item.Attributes.Add("onmouseout", "this.className='mouseoutaltrstyle'");
                }
                else
                {
                    e.Item.CssClass = "mouseoutstyle";
                    e.Item.Attributes.Add("onmouseout", "this.className='mouseoutstyle'");
                }
                //e.Item.Style["cursor"] = "hand";
                //e.Item.Cells[3].Attributes.Add("onclick", "alert('你点击的ID是: " + e.Item.Cells[0].Text + "!');");
                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    e.Item.Cells[i].BorderWidth = 1;
                    e.Item.Cells[i].BorderStyle = BorderStyle.Solid;
                    e.Item.Cells[i].BorderColor = System.Drawing.Color.FromArgb(234, 233, 225);

                    if (this.IsFixConlumnControls)
                    {
                        if (GetBoundColumnFieldReadOnly()[i].ToString().ToLower() == "false")
                        {
                            continue;
                        }
                    }
                    if (typeof(BoundColumn).IsInstanceOfType(this.Columns[i]))
                    {
                        if (this.Columns[i].ItemStyle.Width.Value > 0
                            && this.Columns[i].ItemStyle.Wrap == false)
                        {
                            e.Item.Cells[i].ToolTip = e.Item.Cells[i].Text;
                            //e.Item.Cells[i].Attributes.Add("Title", e.Item.Cells[i].Text);
                            e.Item.Cells[i].Text = "<div class='dataclass' style='width:" + this.Columns[i].ItemStyle.Width.Value + "px;'>" + e.Item.Cells[i].Text + "</div>";
                        }
                    }
                }
            }

            //if (!this.SaveDSViewState)
            //{
            //    this.Controls[0].EnableViewState = false;
            //}

            if (this.IsFixConlumnControls)
            {
                if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
                {
                    //int count=0;
                    for (int i = 0; i < e.Item.Cells.Count; i++)
                    {
                        e.Item.Cells[i].BorderWidth = 1;
                        e.Item.Cells[i].BorderStyle = BorderStyle.Solid;
                        e.Item.Cells[i].BorderColor = System.Drawing.Color.FromArgb(234, 233, 225);

                        if ((!e.Item.Cells[i].HasControls()))
                        {
                            if (GetBoundColumnFieldReadOnly()[i].ToString().ToLower() == "false") //判断是否存在只读属性
                            {
                                System.Web.UI.WebControls.TextBox t = new System.Web.UI.WebControls.TextBox();
                                t.ID = GetBoundColumnField()[i].ToString();
                                t.Attributes.Add("onmouseover", "if(this.className != 'FormFocus') this.className='FormBase'");
                                t.Attributes.Add("onmouseout", "if(this.className != 'FormFocus') this.className='formnoborder'");
                                t.Attributes.Add("onfocus", "this.className='FormFocus';");
                                t.Attributes.Add("onblur", "this.className='formnoborder';");
                                t.Attributes.Add("class", "formnoborder");
                                t.Text = e.Item.Cells[i].Text.Trim().Replace("&nbsp;", "");
                                t.AutoCompleteType = AutoCompleteType.None;

                                //设置宽度
                                if (this.Columns[i].ItemStyle.Width.Value > 0)
                                {
                                    t.Width = (int)this.Columns[i].ItemStyle.Width.Value;
                                }
                                else
                                {
                                    t.Width = 100;
                                }
                                e.Item.Cells[i].Controls.Add(t);
                            }

                        }
                    }
                }
            }
            //            else
            //            {
            //                foreach (System.Web.UI.Control c in e.Item.Cells[i].Controls)
            //                {
            //                    //加载discuz下拉控件
            //                    if (c is WY.A9P5.Control.DropDownList)
            //                    {
            //                        WY.A9P5.Control.DropDownList __dropdownlist = (WY.A9P5.Control.DropDownList)c;
            //                        if (__dropdownlist.SqlText != "")
            //                        {
            //                            __dropdownlist.AddTableData(__dropdownlist.SqlText);
            //                        }

            //                        try
            //                        {
            //                            __dropdownlist.SelectedValue = Convert.ToString(DataBinder.Eval(e.Item.DataItem, __dropdownlist.DataValueField));
            //                        }
            //                        catch
            //                        { ;}
            //                    }

            //                    //加载普通下拉控件
            //                    if (c is System.Web.UI.WebControls.DropDownList)
            //                    {
            //                        System.Web.UI.WebControls.DropDownList __dropdownlist = (System.Web.UI.WebControls.DropDownList)c;
            //                        try
            //                        {
            //                            __dropdownlist.SelectedValue = Convert.ToString(DataBinder.Eval(e.Item.DataItem, __dropdownlist.DataValueField));
            //                        }
            //                        catch
            //                        { ;}
            //                    }

            //                }
            //            }

            //            // count++;
            //        }
            //    }
            //}
            //else
            //{
            //    if (e.Item.ItemType == ListItemType.EditItem)
            //    {
            //        for (int i = 0; i < e.Item.Cells.Count; i++)
            //        {
            //            e.Item.Cells[i].BorderWidth = 1;
            //            e.Item.Cells[i].BorderStyle = BorderStyle.Solid;
            //            e.Item.Cells[i].BorderColor = System.Drawing.Color.FromArgb(234, 233, 225);
            //            if (e.Item.Cells[i].HasControls())
            //            {
            //                for (int j = 0; j < e.Item.Cells[i].Controls.Count; j++)
            //                {
            //                    System.Web.UI.WebControls.TextBox t = e.Item.Cells[i].Controls[j] as System.Web.UI.WebControls.TextBox;
            //                    if (t != null)
            //                    {
            //                        t.Attributes.Add("onfocus", "this.className='FormFocus';");
            //                        t.Attributes.Add("onblur", "this.className='FormBase';");
            //                        t.Attributes.Add("class", "FormBase");
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
        }

        /// <summary>
        /// 得到相应的字段列表
        /// </summary>
        /// <returns></returns>
        private ArrayList GetBoundColumnField()
        {
            System.Collections.ArrayList __arraylist = new ArrayList();
            foreach (DataGridColumn o in this.Columns)
            {
                System.Web.UI.WebControls.BoundColumn __boundcolumn = o as System.Web.UI.WebControls.BoundColumn;
                if (__boundcolumn != null)
                {
                    __arraylist.Add(__boundcolumn.DataField);
                }
                else
                {
                    System.Web.UI.WebControls.TemplateColumn __templatecolumn = o as System.Web.UI.WebControls.TemplateColumn;
                    __arraylist.Add(__templatecolumn.HeaderText);
                }
            }
            return __arraylist;
        }

        /// <summary>
        /// 数据列表项创建事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DataGrid_ItemCreated(Object sender, DataGridItemEventArgs e)
        {
            ListItemType elemType = e.Item.ItemType;

            //if (elemType == ListItemType.Item || elemType == ListItemType.AlternatingItem)
            //{
            //    for (int i = 0; i < e.Item.Cells.Count; i++)
            //    {
            //        if (typeof(BoundColumn).IsInstanceOfType(this.Columns[i]))
            //        {
            //            e.Item.Cells[i].Attributes.Add("Title", DataBinder.Eval(e.Item.DataItem, ((BoundColumn)this.Columns[i]).DataField).ToString());
            //        }
            //    }
            //}
            if (elemType == ListItemType.Pager)
            {
                TableCell cell1 = (TableCell)e.Item.Controls[0];
                //cell1.MergeStyle(DataGrid1.HeaderStyle); 
                //cell1.BackColor = Color.Navy; 
                //cell1.BorderWidth=0;
                //cell1.ColumnSpan = this.ColumnSpan; 
                cell1.HorizontalAlign = HorizontalAlign.Left;
                cell1.VerticalAlign = VerticalAlign.Bottom;
                cell1.CssClass = "datagridPager";

                LiteralControl splittable = new LiteralControl("splittable");
                splittable.Text = "</td></tr></table><table class=\"datagridpage\"><tr><td height=\"2\"></td></tr><tr><td>";
                cell1.Controls.AddAt(0, splittable);


                LiteralControl PageNumber = new LiteralControl("PageNumber");
                PageNumber.Text = " ";
                if (this.PageCount <= 1)
                {
                    try
                    {
                        cell1.Controls.RemoveAt(1); //当页数为1时, 则不显示页码
                    }
                    catch { ; }
                }
                else
                {
                    PageNumber.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                PageNumber.Text += "<font color=black>共&nbsp;<span class='pagenumber'>" + this.PageCount + "</span>&nbsp;页, 当前第&nbsp;<span class='pagenumber'>" + (this.CurrentPageIndex + 1) + "</span>&nbsp;页";

                if (this.VirtualItemCount > 0)
                {
                    PageNumber.Text += ", 共&nbsp;<span class='pagenumber'>" + this.VirtualItemCount + "</span>&nbsp;条记录&nbsp;&nbsp;";
                }


                //PageNumber.Text += "    &nbsp;&nbsp;" + ((this.PageCount > 1) ? "跳转到:" : "");
                cell1.Controls.Add(PageNumber);


                //当大于1时显示跳转按钮
                //if (this.PageCount > 1)
                //{
                //加载每页显示数量的文本框
                PageNumber = new LiteralControl("PageNumber");
                PageNumber.Text = "&nbsp;&nbsp;每页:&nbsp;";
                cell1.Controls.Add(PageNumber);

                PageSizeInputText.ID = "PageSizeInputText";
                PageSizeInputText.Attributes.Add("runat", "server");
                PageSizeInputText.Attributes.Add("onkeydown", "if(event.keyCode==13) { var gotoPageID=this.id.replace('PageSizeInputText','GoToPagerButton'); return(document.getElementById(gotoPageID)).focus();}");
                //PageSizeInputText.Size = 2;
                PageSizeInputText.Style.Add("width", "30px");
                PageSizeInputText.Value = (this.PageSize == 0) ? "1" : this.PageSize.ToString();
                cell1.Controls.Add(PageSizeInputText);

                PageNumber = new LiteralControl("PageNumber");
                PageNumber.Text = "&nbsp;条&nbsp;&nbsp;";
                cell1.Controls.Add(PageNumber);

                //加载跳转文件框
                PageNumber = new LiteralControl("PageNumber");
                PageNumber.Text = "&nbsp;&nbsp;跳转到:&nbsp;";
                cell1.Controls.Add(PageNumber);

                GoToPagerInputText.ID = "GoToPagerInputText";
                GoToPagerInputText.Attributes.Add("runat", "server");
                GoToPagerInputText.Attributes.Add("onkeydown", "if(event.keyCode==13) { var gotoPageID=this.id.replace('InputText','Button'); return(document.getElementById(gotoPageID)).focus();}");
                //GoToPagerInputText.Size = 2;
                GoToPagerInputText.Style.Add("width", "30px");
                GoToPagerInputText.Value = (this.CurrentPageIndex == 0) ? "1" : (this.CurrentPageIndex + 1).ToString();
                cell1.Controls.Add(GoToPagerInputText);

                PageNumber = new LiteralControl("PageNumber");
                PageNumber.Text = "&nbsp;页&nbsp;&nbsp;";
                cell1.Controls.Add(PageNumber);

                //加载跳转按钮	
                GoToPagerButton.ID = "GoToPagerButton";
                GoToPagerButton.Text = " Go ";
                cell1.Controls.Add(GoToPagerButton);
                //}


                e.Item.Controls.Add(cell1);

                TableCell pager = (TableCell)e.Item.Controls[0];


                for (int i = 1; i < pager.Controls.Count; i += 2)
                {
                    Object o = pager.Controls[i];

                    if (o is LinkButton)
                    {
                        LinkButton h = (LinkButton)o;

                        if (h.Text == "..." && i == 1)//pager.Controls[i].ID == "_ctl0")
                        {
                            //h.Text = "上一页";
                            continue;
                        }
                        if (i > 1 && h.Text == "...")
                        {
                            //h.Text = "下一页";
                            continue;
                        }

                        //h.Attributes.Add("href", "javascript:__doPostBack('TabControl1$tabPage1$DataGrid1$_ctl29$_ctl"+(Utils.StrToInt(h.Text)-1)+"','');");
                        //h.Attributes.Add("onclick", "javascript:__doPostBack('TabControl1$tabPage1$DataGrid1$_ctl29$_ctl1','');");
                        //h.Attributes.Add("onclick", "javascript:document.getElementById('Layer5').innerHTML ='<br /><table><tr><td valign=top><img border=\"0\" src=\"../images/ajax_loading.gif\"  /></td><td valign=middle style=\"font-size: 14px;\" >正在加载数据...<BR /></td></tr></table>';document.getElementById('success').style.display ='block';");
                    }
                    if (o is Label)
                    {
                        Label l = (Label)o;
                        if (l.Text == "..." && i == 1)//l.ID == "_ctl0") 
                        {
                            //l.Text = "上一页";
                        }

                        if (i > 1 && l.Text == "...")
                        {
                            //l.Text = "下一页";
                        }
                    }
                }

            }
            else
            {
                if ((elemType == ListItemType.AlternatingItem) || (elemType == ListItemType.Item) || (elemType == ListItemType.Header))
                {
                    foreach (System.Web.UI.Control control in e.Item.Controls)
                    {
                        TableCell cell = (TableCell)control;
                        cell.BorderWidth = 1;
                        cell.BorderColor = System.Drawing.Color.FromArgb(234, 233, 225);
                        //下面的代码暂时不可用
                        //System.Web.UI.WebControls.Style s=new System.Web.UI.WebControls.Style();
                        //s.CssClass="datagridItemtd";
                        //cell.ApplyStyle(s);
                    }
                }

                for (int i = 0; i < e.Item.Cells.Count; i++)
                {
                    if (elemType == ListItemType.Header)
                    {
                        e.Item.Cells[i].Wrap = false;
                    }
                    else
                    {
                        if (i >= 2)
                        {
                            e.Item.Cells[i].Wrap = true;
                        }
                        else
                        {
                            e.Item.Cells[i].Wrap = false;
                        }
                    }
                }
            }

        }


        #region Property SaveDSViewState
        /// <summary>
        /// 是否保存数据的ViewState值
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public bool SaveDSViewState
        {
            get
            {
                object obj = ViewState["SaveDSViewState"];
                if (obj == null) return false;
                else
                {
                    if (obj.ToString().ToLower() == "true")
                        return true;
                    else
                        return false;
                }
            }
            set
            {
                ViewState["SaveDSViewState"] = value;
            }
        }
        #endregion

        #region Property SqlText
        /// <summary>
        /// 使用的查询字符串
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string SqlText
        {
            get
            {
                return (string)ViewState["SqlText"];
            }
            set
            {
                ViewState["SqlText"] = value;
            }
        }
        #endregion

        #region Property ColumnSpan
        /// <summary>
        /// ColumnSpan数,用于在分页底部设置表格的colspan属性
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public int ColumnSpan
        {
            get
            {
                object obj = ViewState["ColumnSpan"];
                return obj == null ? 1 : (int)obj;
            }

            set
            {
                ViewState["ColumnSpan"] = value;
            }
        }
        #endregion

        #region Property TableHeaderName

        /// <summary>
        /// 表名称。
        /// </summary>
        [Description("表名称。"), Category("Appearance"), DefaultValue("")]
        public string TableHeaderName
        {
            get
            {

                object obj = ViewState["TableHeaderName"];
                return obj == null ? "1" : (string)obj;
            }
            set
            {
                ViewState["TableHeaderName"] = value;
            }
        }

        #endregion

        #region Property IsFixConlumnControls
        /// <summary>
        /// 是否安插列控件。
        /// </summary>
        [Description("是否在列表页中载入列控件"), Category("Appearance"), DefaultValue(false)]
        public bool IsFixConlumnControls
        {
            get
            {

                object o = ViewState["IsFixConlumnControls"];
                if ((o != null) && (o.ToString().ToLower() == "true"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                ViewState["IsFixConlumnControls"] = value;
            }
        }
        #endregion

        #region Property ImagePath
        /// <summary>
        /// 图片所在的文件路径。
        /// </summary>
        [Description("表头的名称。"), Category("Appearance"), DefaultValue("")]
        public string ImagePath
        {
            get
            {

                object obj = ViewState["ImagePath"];
                return obj == null ? "../resource/images/" : (string)obj;
            }
            set
            {
                ViewState["ImagePath"] = value;
            }
        }

        #endregion

        /// <summary>
        /// 得到相应的字段只读属性
        /// </summary>
        /// <returns></returns>
        private ArrayList GetBoundColumnFieldReadOnly()
        {
            System.Collections.ArrayList __arraylist = new ArrayList();
            foreach (DataGridColumn o in this.Columns)
            {
                System.Web.UI.WebControls.BoundColumn __boundcolumn = o as System.Web.UI.WebControls.BoundColumn;
                if (__boundcolumn != null)
                {
                    __arraylist.Add(__boundcolumn.ReadOnly);
                }
                else
                {
                    __arraylist.Add(true);
                }
            }
            return __arraylist;
        }

        ///// <summary> 
        ///// 输出html来在浏览器中显示控件
        ///// </summary>
        ///// <param name="output"> 要写出到的 HTML 编写器 </param>
        //protected override void Render(HtmlTextWriter output)
        //{

        //    output.WriteLine("<table class=\"ntcplist\" >\r\n");
        //    output.WriteLine("<tr class=\"head\">\r\n");
        //    output.WriteLine("<td>" + this.TableHeaderName + "</td>\r\n");
        //    output.WriteLine("</tr>\r\n");
        //    output.WriteLine("<tr>\r\n");
        //    output.WriteLine("<td>\r\n");

        //    base.Render(output);

        //    output.WriteLine("</td></tr></TABLE>");
        //}

        /// <summary>
        /// 重写数据源属性
        /// </summary>
        public override object DataSource
        {
            get
            {
                return base.DataSource;
            }
            set
            {
                base.DataSource = value;

                //当不是手工定制分页时(因为手工定制分页时会在前台指定VirtualItemCount数值)
                if (!this.AllowCustomPaging)
                {
                    if (value is DataTable)
                    {
                        this.VirtualItemCount = (value as DataTable).Rows.Count;
                    }

                    if (value is DataSet)
                    {
                        DataSet ds = value as DataSet;
                        if (ds.Tables.Count > 0)
                        {
                            this.VirtualItemCount = ds.Tables[0].Rows.Count;
                        }
                    }

                    //当为数组类型时
                    if (value.GetType().Name.ToString().IndexOf("[]") > 0)
                    {
                        Array array = value as Array;
                        if (array.Length > 0)
                        {
                            this.VirtualItemCount = array.Length;
                        }
                    }
                }
            }
        }

        #region sort

        /// <summary>
        /// 排序列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SortGrid(Object sender, DataGridSortCommandEventArgs e)
        {
            //更换排序列时，清除排序方式（第一次点击始终是升序）
            string sortexpression = ViewState["DataGridSortExpression"] == null ? "" : ViewState["DataGridSortExpression"].ToString();
            if (e.SortExpression != sortexpression) ViewState["DataGridSortType"] = null;
            ViewState["DataGridSortExpression"] = e.SortExpression;

            //SortTable(e.SortExpression, (DataTable)null);

            foreach (System.Web.UI.WebControls.DataGridColumn dc in this.Columns)
            {
                if (dc.SortExpression == e.SortExpression)
                {
                    if (dc.HeaderText.IndexOf("<img src=") >= 0)
                    {
                        if (this.DataGridSortType == "ASC")
                        {
                            //dc.HeaderText = dc.HeaderText.Replace("<img src=" + this.ImagePath + "asc.gif height=13 align=absmiddle>", "<img src=" + this.ImagePath + "desc.gif height=13 align=absmiddle>");
                            dc.HeaderStyle.CssClass = "asc";
                        }
                        else
                        {
                            //dc.HeaderText = dc.HeaderText.Replace("<img src=" + this.ImagePath + "desc.gif height=13 align=absmiddle>", "<img src=" + this.ImagePath + "asc.gif height=13 align=absmiddle>");
                            dc.HeaderStyle.CssClass = "desc";
                        }
                    }
                    else
                    {
                        if (this.DataGridSortType == "ASC")
                        {
                            //dc.HeaderText = dc.HeaderText + "<img src=" + this.ImagePath + "desc.gif height=13 align=absmiddle>";
                            dc.HeaderStyle.CssClass = "desc";
                        }
                        else
                        {
                            //dc.HeaderText = dc.HeaderText + "<img src=" + this.ImagePath + "asc.gif height=13 align=absmiddle>";
                            dc.HeaderStyle.CssClass = "asc";
                        }
                    }
                    //dc.. ..ItemStyle="sortColumn";
                    //dc.ItemStyle.BackColor = Color.AliceBlue;
                }
                else
                {
                    //dc.HeaderText = dc.HeaderText.Replace("<img", "~").Split('~')[0];
                    dc.HeaderStyle.CssClass = "";
                    //dc.ItemStyle.BackColor = Color.White;
                }
            }
        }

        /// <summary>
        /// 边框的宽度。
        /// </summary>
        [Description("表头的名称。"), Category("Appearance"), DefaultValue("ASC")]
        public string DataGridSortType
        {
            get
            {

                object obj = ViewState["DataGridSortType"];
                string ascordesc = obj == null ? "ASC" : (string)obj;
                if (ascordesc == "ASC")
                {
                    ViewState["DataGridSortType"] = "DESC";
                    return "DESC";
                }
                else
                {
                    ViewState["DataGridSortType"] = "ASC";
                    return "ASC";
                }

            }
            set
            {
                ViewState["DataGridSortType"] = value;
            }
        }

        public string GetDataGridSortType()
        {
            object obj = ViewState["DataGridSortType"];
            string ascordesc = obj == null ? "ASC" : (string)obj;
            return ascordesc == "ASC" ? "DESC" : "ASC";
        }

        private string sort;

        /// <summary>
        /// 排序字段
        /// </summary>
        [Bindable(true), Category("Appearance"), DefaultValue("")]
        public string Sort
        {
            get
            {
                object obj = ViewState["DataGridSort"];
                sort = (obj == null) ? null : obj.ToString();

                return sort;
            }
            set
            {
                ViewState["DataGridSort"] = value;

                sort = value;
            }
        }

        private void ClearSortHeader()
        {
            foreach (System.Web.UI.WebControls.DataGridColumn dc in this.Columns)
            {
                if (dc.HeaderText.IndexOf("<img src=") >= 0)
                {
                    if (this.DataGridSortType == "ASC")
                    {
                        dc.HeaderText = dc.HeaderText.Replace("<img src=" + this.ImagePath + "asc.gif height=13>", "");
                    }
                    else
                    {
                        dc.HeaderText = dc.HeaderText.Replace("<img src=" + this.ImagePath + "desc.gif height=13>", "");
                    }
                }
            }
        }
        #endregion

    }
}
