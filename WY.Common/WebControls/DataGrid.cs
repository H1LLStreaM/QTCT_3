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
        /// ��ת��ť
        /// </summary>
        public Button GoToPagerButton = new Button();

        /// <summary>
        /// ��ת�ı���
        /// </summary>
        public System.Web.UI.HtmlControls.HtmlInputText PageSizeInputText = new HtmlInputText();

        /// <summary>
        /// ��ת�ı���
        /// </summary>
        public System.Web.UI.HtmlControls.HtmlInputText GoToPagerInputText = new HtmlInputText();

        /// <summary>
        /// ���캯��
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
        /// ��ת��ť�����¼�
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
                    //���õ�ǰDATAGRID��CurrentPageIndex����
                    SetCurrentPageIndexByGoToPager();
                }

            }
        }

        /// <summary>
        /// ���õ�ǰDATAGRID��CurrentPageIndex����
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
        /// ��תָ����ҳ
        /// </summary>
        /// <param name="value"></param>
        public void LoadCurrentPageIndex(int value)
        {
            this.CurrentPageIndex = (value < 0) ? 0 : value;
        }

        /// <summary>
        /// �����б�����¼�
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
                //e.Item.Cells[3].Attributes.Add("onclick", "alert('������ID��: " + e.Item.Cells[0].Text + "!');");
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
                            if (GetBoundColumnFieldReadOnly()[i].ToString().ToLower() == "false") //�ж��Ƿ����ֻ������
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

                                //���ÿ��
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
            //                    //����discuz�����ؼ�
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

            //                    //������ͨ�����ؼ�
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
        /// �õ���Ӧ���ֶ��б�
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
        /// �����б�����¼�
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
                        cell1.Controls.RemoveAt(1); //��ҳ��Ϊ1ʱ, ����ʾҳ��
                    }
                    catch { ; }
                }
                else
                {
                    PageNumber.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                }
                PageNumber.Text += "<font color=black>��&nbsp;<span class='pagenumber'>" + this.PageCount + "</span>&nbsp;ҳ, ��ǰ��&nbsp;<span class='pagenumber'>" + (this.CurrentPageIndex + 1) + "</span>&nbsp;ҳ";

                if (this.VirtualItemCount > 0)
                {
                    PageNumber.Text += ", ��&nbsp;<span class='pagenumber'>" + this.VirtualItemCount + "</span>&nbsp;����¼&nbsp;&nbsp;";
                }


                //PageNumber.Text += "    &nbsp;&nbsp;" + ((this.PageCount > 1) ? "��ת��:" : "");
                cell1.Controls.Add(PageNumber);


                //������1ʱ��ʾ��ת��ť
                //if (this.PageCount > 1)
                //{
                //����ÿҳ��ʾ�������ı���
                PageNumber = new LiteralControl("PageNumber");
                PageNumber.Text = "&nbsp;&nbsp;ÿҳ:&nbsp;";
                cell1.Controls.Add(PageNumber);

                PageSizeInputText.ID = "PageSizeInputText";
                PageSizeInputText.Attributes.Add("runat", "server");
                PageSizeInputText.Attributes.Add("onkeydown", "if(event.keyCode==13) { var gotoPageID=this.id.replace('PageSizeInputText','GoToPagerButton'); return(document.getElementById(gotoPageID)).focus();}");
                //PageSizeInputText.Size = 2;
                PageSizeInputText.Style.Add("width", "30px");
                PageSizeInputText.Value = (this.PageSize == 0) ? "1" : this.PageSize.ToString();
                cell1.Controls.Add(PageSizeInputText);

                PageNumber = new LiteralControl("PageNumber");
                PageNumber.Text = "&nbsp;��&nbsp;&nbsp;";
                cell1.Controls.Add(PageNumber);

                //������ת�ļ���
                PageNumber = new LiteralControl("PageNumber");
                PageNumber.Text = "&nbsp;&nbsp;��ת��:&nbsp;";
                cell1.Controls.Add(PageNumber);

                GoToPagerInputText.ID = "GoToPagerInputText";
                GoToPagerInputText.Attributes.Add("runat", "server");
                GoToPagerInputText.Attributes.Add("onkeydown", "if(event.keyCode==13) { var gotoPageID=this.id.replace('InputText','Button'); return(document.getElementById(gotoPageID)).focus();}");
                //GoToPagerInputText.Size = 2;
                GoToPagerInputText.Style.Add("width", "30px");
                GoToPagerInputText.Value = (this.CurrentPageIndex == 0) ? "1" : (this.CurrentPageIndex + 1).ToString();
                cell1.Controls.Add(GoToPagerInputText);

                PageNumber = new LiteralControl("PageNumber");
                PageNumber.Text = "&nbsp;ҳ&nbsp;&nbsp;";
                cell1.Controls.Add(PageNumber);

                //������ת��ť	
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
                            //h.Text = "��һҳ";
                            continue;
                        }
                        if (i > 1 && h.Text == "...")
                        {
                            //h.Text = "��һҳ";
                            continue;
                        }

                        //h.Attributes.Add("href", "javascript:__doPostBack('TabControl1$tabPage1$DataGrid1$_ctl29$_ctl"+(Utils.StrToInt(h.Text)-1)+"','');");
                        //h.Attributes.Add("onclick", "javascript:__doPostBack('TabControl1$tabPage1$DataGrid1$_ctl29$_ctl1','');");
                        //h.Attributes.Add("onclick", "javascript:document.getElementById('Layer5').innerHTML ='<br /><table><tr><td valign=top><img border=\"0\" src=\"../images/ajax_loading.gif\"  /></td><td valign=middle style=\"font-size: 14px;\" >���ڼ�������...<BR /></td></tr></table>';document.getElementById('success').style.display ='block';");
                    }
                    if (o is Label)
                    {
                        Label l = (Label)o;
                        if (l.Text == "..." && i == 1)//l.ID == "_ctl0") 
                        {
                            //l.Text = "��һҳ";
                        }

                        if (i > 1 && l.Text == "...")
                        {
                            //l.Text = "��һҳ";
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
                        //����Ĵ�����ʱ������
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
        /// �Ƿ񱣴����ݵ�ViewStateֵ
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
        /// ʹ�õĲ�ѯ�ַ���
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
        /// ColumnSpan��,�����ڷ�ҳ�ײ����ñ���colspan����
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
        /// �����ơ�
        /// </summary>
        [Description("�����ơ�"), Category("Appearance"), DefaultValue("")]
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
        /// �Ƿ񰲲��пؼ���
        /// </summary>
        [Description("�Ƿ����б�ҳ�������пؼ�"), Category("Appearance"), DefaultValue(false)]
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
        /// ͼƬ���ڵ��ļ�·����
        /// </summary>
        [Description("��ͷ�����ơ�"), Category("Appearance"), DefaultValue("")]
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
        /// �õ���Ӧ���ֶ�ֻ������
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
        ///// ���html�������������ʾ�ؼ�
        ///// </summary>
        ///// <param name="output"> Ҫд������ HTML ��д�� </param>
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
        /// ��д����Դ����
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

                //�������ֹ����Ʒ�ҳʱ(��Ϊ�ֹ����Ʒ�ҳʱ����ǰָ̨��VirtualItemCount��ֵ)
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

                    //��Ϊ��������ʱ
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
        /// �����б�
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SortGrid(Object sender, DataGridSortCommandEventArgs e)
        {
            //����������ʱ���������ʽ����һ�ε��ʼ��������
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
        /// �߿�Ŀ�ȡ�
        /// </summary>
        [Description("��ͷ�����ơ�"), Category("Appearance"), DefaultValue("ASC")]
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
        /// �����ֶ�
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
