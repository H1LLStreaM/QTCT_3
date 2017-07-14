using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Web;
using WY.Common;
using System.Collections;
using WY.Common.Utility;

namespace WY.Library.Page
{
    public class BasePage : System.Web.UI.Page
    {
        public const string CHECK_ALL_HTML = "<input title=\"全选/取消全选\" onclick=\"CheckAllForm(this.form)\" type=\"checkbox\" name=\"chkall\" id=\"chkall\" />";
        public const string CHECK_COL_HTML = "<input id=\"id\" class=\"datagridrowcheck\" onclick=\"javascript:SH_SelectOne(this)\" type=\"checkbox\" value=\"{0}\" name=\"id\" />";
        
        public const string ROWNO_COL_KEY = "rowno";
        protected int rowno_col_index = -1;
        protected int idatarowindex;

        protected string pageid;

        protected string pagemode;

        protected List<string> _userpowers;

        protected override void OnLoad(EventArgs e)
        {
            Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);

            pageid = Request.QueryString["pageid"];
            //pagemode = this.GetPageMode().ToString();

            //if (string.IsNullOrEmpty(pageid)
            //    && this.Request.UrlReferrer != null
            //    && !string.IsNullOrEmpty(this.Request.UrlReferrer.Query))
            //{
            //    try
            //    {
            //        string strquery = this.Request.UrlReferrer.Query.Substring(1);
            //        string[] arr = strquery.Split('&');
            //        foreach (string p in arr)
            //        {
            //            string[] param = p.Split('=');
            //            if (param[0].ToLower() == "pageid")
            //            {
            //                pageid = param[1];
            //                Response.Redirect(this.Request.Url.ToString() + "&pageid=" + pageid);
            //                return;
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.Error(ex);
            //    }
            //}

            //检查是否登录
            if (WYGlobal.guserid <= 0)
            {
                Context.Response.Redirect("/adminlogin.aspx");
                return;
            }

            if (!this.CheckUserpower())
            {
                Context.Response.Redirect("/nopermission.aspx");
                return;
            }

            //this.InitUserpower();

            base.OnLoad(e);
        }

        //protected virtual void InitUserpower()
        //{

        //    if (this.UserpowerIsReadOnly())
        //    {
        //        this.SetEditControlVisible(false);
        //    }
        //    else
        //    {
        //        this.SetEditControlVisible(true);
        //    }

        //}

        //protected enmPageMode GetPageMode()
        //{
        //    if (Request.QueryString["add"] == "1")
        //    {
        //        return enmPageMode.新增;
        //    }
        //    else if (Request.QueryString["edit"] == "1")
        //    {
        //        return enmPageMode.编辑;
        //    }
        //    else if (Request.QueryString["view"] == "1")
        //    {
        //        return enmPageMode.查看;
        //    }
        //    else
        //    {
        //        return enmPageMode.无;
        //    }
        //}

        /// <summary>
        /// 检查用户是否有权限访问该页
        /// </summary>
        /// <returns></returns>
        protected bool CheckUserpower()
        {
            //this._userpowers = UserroleBusiness.GetUserPower(this.pageid);

            //if (this._userpowers.Count != 0)
            //{
            //    return true;
            //}
            //else
            //{
            //    return false;
            //}
            return true;
        }

        ///// <summary>
        ///// 获取用户对本页面的操作权限
        ///// </summary>
        ///// <returns></returns>
        //protected List<string> GetUserpower()
        //{
        //    return UserroleBusiness.GetUserPower(this.pageid);
        //}

        //protected bool UserpowerIsReadOnly()
        //{
        //    this._userpowers = UserroleBusiness.GetUserPower(this.pageid);

        //    if (this._userpowers.Count == 1
        //        && this._userpowers[0] == enmUserpower.查询.ToString())
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}

        protected virtual void SetEditControlVisible(bool visible)
        {
            Panel panel = (Panel)this.FindControl("pnlEdit");
            if (panel != null)
            {
                panel.Visible = visible;
            }
        }

        /// <summary>
        /// 注册启动脚本
        /// </summary>
        /// <param name="script">脚本内容，不含script标签</param>
        protected void JS(string script)
        {
            this.JS(Guid.NewGuid().ToString(), script);
        }

        /// <summary>
        /// 注册启动脚本
        /// </summary>
        /// <param name="key"></param>
        /// <param name="script">脚本内容，不含script标签</param>
        protected void JS(string key, string script)
        {
            string strscript = "<script>" + script + "</script>";

            this.Page.ClientScript.RegisterStartupScript(this.Page.GetType(), key, strscript);
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="sourceTable">数据源</param>
        /// <param name="fields">字段信息(格式：字段名[必须],显示名[必须],显示格式)</param>
        /// <param name="filename">需保存的文件名</param>
        protected void ExportToExcel(DataTable sourceTable, object[] fields, string filename)
        {
            idatarowindex = 1;

            DataGrid GridView1 = GetGridViewForExcel(fields);
            GridView1.ItemDataBound += new DataGridItemEventHandler(GridView1_ItemDataBound);
            GridView1.DataSource = sourceTable;
            GridView1.DataBind();

            GridView1.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(8, 116, 178);
            GridView1.HeaderStyle.ForeColor = System.Drawing.Color.White;
            GridView1.HeaderStyle.Height = 20;
            GridView1.HeaderStyle.HorizontalAlign = HorizontalAlign.Center;

            GridView1.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

            HttpContext.Current.Response.Charset = "utf-8";
            HttpContext.Current.Response.AppendHeader("Content-Disposition", "attachment;filename=" + HttpUtility.UrlEncode(filename));
            //HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.GetEncoding("GB2312");
            HttpContext.Current.Response.Write("<meta http-equiv=Content-Type content=text/html;charset=utf-8>");
            HttpContext.Current.Response.ContentType = "application/ms-excel";//image/JPEG;text/HTML;image/GIF;vnd.ms-excel/msword   
            System.IO.StringWriter tw = new System.IO.StringWriter();
            System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(tw);
            GridView1.RenderControl(hw);
            HttpContext.Current.Response.Write(tw.ToString());
            HttpContext.Current.Response.End();
        }

        private void GridView1_ItemDataBound(object sender, DataGridItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                if (rowno_col_index != -1)
                {
                    e.Item.Cells[rowno_col_index].Text = (idatarowindex++).ToString();
                }
            }
        }

        /// <summary>
        /// 创建导出excel格式（可重载）
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        protected virtual DataGrid GetGridViewForExcel(object[] fields)
        {
            DataGrid GridView1 = new DataGrid();
            GridView1.AutoGenerateColumns = false;
            BoundColumn col = null;

            rowno_col_index = -1;
            foreach (object item in fields)
            {
                string[] arrinfo = (string[])item;

                if (arrinfo[0] == ROWNO_COL_KEY)
                {
                    TemplateColumn templatecol = new TemplateColumn();
                    templatecol.HeaderText = arrinfo[1];
                    GridView1.Columns.Add(templatecol);
                    rowno_col_index = GridView1.Columns.Count - 1;
                }
                else
                {
                    col = new BoundColumn();
                    col.DataField = arrinfo[0];
                    col.HeaderText = arrinfo[1];
                    if (arrinfo.Length > 2)
                    {
                        col.DataFormatString = "{0:" + arrinfo[2] + "}";
                    }
                    GridView1.Columns.Add(col);
                }
            }

            return GridView1;
        }

        protected string getUrl(string pagename, params DictionaryEntry[] args)
        {
            string url = pagename + "?pageid=" + this.pageid;
            foreach (DictionaryEntry item in args)
            {
                url += "&" + item.Key.ToString() + "=" + HttpUtility.UrlEncode(item.Value.ToString());
            }
            return url;
        }
        protected string getEditUrl(string pagename, int id, params DictionaryEntry[] args)
        {
            string url = pagename + "?edit=1&pageid=" + this.pageid + "&id=" + id.ToString();
            foreach (DictionaryEntry item in args)
            {
                url += "&" + item.Key.ToString() + "=" + HttpUtility.UrlEncode(item.Value.ToString());
            }
            return url;
        }
        protected string getAddUrl(string pagename, params DictionaryEntry[] args)
        {
            string url = pagename + "?add=1&pageid=" + this.pageid;
            foreach (DictionaryEntry item in args)
            {
                url += "&" + item.Key.ToString() + "=" + HttpUtility.UrlEncode(item.Value.ToString());
            }
            return url;
        }
    }
}
