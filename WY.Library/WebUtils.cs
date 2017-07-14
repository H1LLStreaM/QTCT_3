using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;

namespace WY.Library
{
    public class WebUtils
    {

        /// <summary>
        /// վ���Ŀ¼������·����
        /// ���磺/jiaju360web/
        /// </summary>
        public static string AppRoot = "";

        /// <summary>
        /// վ��urlȫ·��
        /// ���磺http://www.jiaju360.com/
        /// </summary>
        public static string AppUrl = "";

        /// <summary>
        /// վ���Ӧ����·��
        /// </summary>
        public static string PhysicalPath = "";

        static WebUtils()
        {
            if (!HttpContext.Current.Request.ApplicationPath.EndsWith("/"))
                AppRoot = HttpContext.Current.Request.ApplicationPath + "/";
            else
                AppRoot = HttpContext.Current.Request.ApplicationPath;

            AppUrl = (new Uri(HttpContext.Current.Request.Url, AppRoot)).ToString();

            if (!HttpContext.Current.Request.PhysicalApplicationPath.EndsWith("\\"))
                PhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath + "\\";
            else
                PhysicalPath = HttpContext.Current.Request.PhysicalApplicationPath;

        }

        #region ��ʼ��������
        /// <summary>
        /// ��ʼ��������
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="table"></param>
        /// <param name="textfield"></param>
        /// <param name="valuefield"></param>
        public static void InitDropdownList(System.Web.UI.WebControls.DropDownList obj, DataTable table, string textfield, string valuefield)
        {
            obj.Items.Clear();
            foreach (DataRow row in table.Rows)
            {
                obj.Items.Add(new System.Web.UI.WebControls.ListItem(row[textfield].ToString(), row[valuefield].ToString()));
            }
        }

        public static void InitDropdownList(DropDownList obj, List<DictionaryEntry> maplist)
        {
            obj.Items.Clear();
            foreach (DictionaryEntry item in maplist)
            {
                obj.Items.Add(new ListItem(item.Value.ToString(), item.Key.ToString()));
            }
        }

        public static void InitDropdownList(DropDownList obj, List<DictionaryEntry> maplist, string defaultvalue, string defaulttext)
        {
            InitDropdownList(obj, maplist);
            obj.Items.Insert(0, new ListItem(defaulttext, defaultvalue));
        }

        #endregion

        #region ��ʼ����ѡ��
        /// <summary>
        /// ��ö���ͳ�ʼ����ѡ��
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="enumtype"></param>
        public static void InitRadioButtonList(RadioButtonList ctl, Type enumtype)
        {
            string[] arr = Enum.GetNames(enumtype);

            ctl.Items.Clear();
            foreach (string name in arr)
            {
                Enum obj = (Enum)Enum.Parse(enumtype, name);

                ctl.Items.Add(new ListItem(obj.ToString(), obj.ToString("d")));
            }
        }
        #endregion

        #region ��ʼ����ѡ��
        /// <summary>
        /// ��ö���ͳ�ʼ����ѡ��
        /// </summary>
        /// <param name="ctl"></param>
        /// <param name="enumtype"></param>
        public static void InitCheckBoxList(CheckBoxList ctl, Type enumtype)
        {
            string[] arr = Enum.GetNames(enumtype);

            ctl.Items.Clear();
            foreach (string name in arr)
            {
                Enum obj = (Enum)Enum.Parse(enumtype, name);

                ctl.Items.Add(new ListItem(obj.ToString(), obj.ToString("d")));
            }
        }
        #endregion

        #region ��ȡ��ѡ��ѡ����
        /// <summary>
        /// ��ȡ��ѡ��ѡ����(Text)
        /// </summary>
        /// <param name="ctl"></param>
        /// <returns></returns>
        public static string[] GetCheckedTextStr(CheckBoxList ctl)
        {
            List<string> arr = new List<string>();
            foreach (ListItem item in ctl.Items)
            {
                if (item.Selected)
                {
                    arr.Add(item.Text);
                }
            }

            return arr.ToArray();
        }

        /// <summary>
        /// ��ȡ��ѡ��ѡ����(Value)
        /// </summary>
        /// <param name="ctl"></param>
        /// <returns></returns>
        public static string[] GetCheckedValueStr(CheckBoxList ctl)
        {
            List<string> arr = new List<string>();
            foreach (ListItem item in ctl.Items)
            {
                if (item.Selected)
                {
                    arr.Add(item.Value);
                }
            }

            return arr.ToArray();
        }
        #endregion

        #region SetDropdownBoxValue

        public static void SetDropdownBoxValue(System.Web.UI.WebControls.DropDownList dropdownbox, string value)
        {
            dropdownbox.SelectedIndex = 0;
            for (int i = 1; i < dropdownbox.Items.Count; i++)
            {
                if (dropdownbox.Items[i].Value == value)
                {
                    dropdownbox.SelectedIndex = i;
                    break;
                }
            }
        }

        #endregion

        #region GetColIdx/GetColIdxByHeadText

        /// <summary>
        /// ���ݰ󶨵��ֶ�������ȡ�е�index
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="datafield"></param>
        /// <returns></returns>
        public static int GetColIdx(DataGrid grid, string datafield)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                if (typeof(BoundColumn).IsInstanceOfType(grid.Columns[i]))
                {
                    BoundColumn col = (BoundColumn)grid.Columns[i];
                    if (col.DataField.ToLower() == datafield.ToLower())
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// �����б��⣬��ȡ�е�index
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="headtext"></param>
        /// <returns></returns>
        public static int GetColIdxByHeadText(DataGrid grid, string headtext)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                if (grid.Columns[i].HeaderText.ToLower() == headtext.ToLower())
                {
                    return i;
                }
            }

            return -1;
        }

        #endregion



        /// <summary>
        /// ���ҳ�������ű�
        /// </summary>
        /// <param name="page">��ǰPage����</param>
        /// <param name="strjs">�ű�����</param>
        public static void JS(System.Web.UI.Page page, string strjs)
        {
            page.ClientScript.RegisterStartupScript(page.GetType(), "startupjs", "<script>" + strjs + "</script>");
        }

        public static string GetPager(string url, int pageindex, int pagesize, int rowcount)
        {
            if (pageindex == 0) pageindex = 1;

            int pagecount = rowcount / pagesize;
            if ((rowcount % pagesize) > 0)
            {
                pagecount += 1;
            }
            if (pagecount <= 1)
            {
                return "";
            }

            StringBuilder sbhtml = new StringBuilder();

            //sbhtml.Append("<input type=\"hidden\" name=\"pageno\" value=\"" + pageindex + "\" />");
            sbhtml.Append("<table border=\"0\" cellspacing=\"5\" cellpadding=\"0\">");
            sbhtml.Append("<tr class=\"page\">");

            if (url.IndexOf("?") < 0)
            {
                url += "?";
            }
            else
            {
                url += "&";
            }

            if (pageindex > 1)
            {
                sbhtml.Append("<td align=\"center\" valign=\"bottom\"><a href=\"" + url + "page=" + (pageindex - 1).ToString() + "\" >��һҳ</a></td>");
            }

            for (int i = 1; i <= pagecount; i++)
            {
                if (i == pageindex)
                {
                    sbhtml.Append("<td align=\"center\" >" + i.ToString() + "</td>");
                }
                else
                {
                    sbhtml.Append("<td align=\"center\"><a href=\"" + url + "page=" + i.ToString() + "\" >" + i.ToString() + "</a></td>");
                }
            }

            if (pageindex < pagecount)
            {
                sbhtml.Append("<td align=\"center\" valign=\"bottom\"><a href=\"" + url + "page=" + (pageindex + 1).ToString() + "\" >��һҳ</a></td>");
            }

            //sbhtml.Append("<td height=\"15\">&nbsp;<td>");
            sbhtml.Append("<tr>");
            sbhtml.Append("</table>");

            return sbhtml.ToString();
        }

        public static string GetPager2(string url, int pageindex, int pagesize, int rowcount)
        {
            if (pageindex == 0) pageindex = 1;

            int pagecount = rowcount / pagesize;
            if ((rowcount % pagesize) > 0)
            {
                pagecount += 1;
            }
            if (pagecount <= 1)
            {
                return "";
            }

            StringBuilder sbhtml = new StringBuilder();

            //sbhtml.Append("<input type=\"hidden\" name=\"pageno\" value=\"" + pageindex + "\" />");
            sbhtml.Append("<table border=\"0\" cellspacing=\"5\" cellpadding=\"0\">");
            sbhtml.Append("<tr class=\"page\">");

            if (url.IndexOf("?") < 0)
            {
                url += "?";
            }
            else
            {
                url += "&";
            }


            sbhtml.Append("<td align=\"center\" valign=\"bottom\"><span class='fenye'><a href=\"" + url + "page=1\" >��ҳ</a></span></td>");

            //
            int sp = 1, ep = pagecount;

            if (pageindex > 4)
            {
                sp = pageindex - 4;

                if (pagecount - pageindex > 5)
                {
                    ep = pageindex + 5;
                }
                else
                {
                    ep = pagecount;
                    if (pagecount - 9 < 1)
                    {
                        sp = 1;
                    }
                    else
                    {
                        sp = pagecount - 9;
                    }
                }
            }
            if (ep - sp > 10)
            {
                ep = 10;
            }

            for (int i = sp; i <= ep; i++)
            {
                if (i == pageindex)
                {
                    sbhtml.Append("<td align=\"center\" ><span class='fenye_1'>" + i.ToString() + "</span></td>");
                }
                else
                {
                    sbhtml.Append("<td align=\"center\"><span class='fenye'><a href=\"" + url + "page=" + i.ToString() + "\" >" + i.ToString() + "</a></span></td>");
                }
            }

            //for (int i = 1; i <= pagecount; i++)
            //{
            //    if (i == pageindex)
            //    {
            //        sbhtml.Append("<td align=\"center\" ><span class='fenye_1'>" + i.ToString() + "</span></td>");
            //    }
            //    else
            //    {
            //        sbhtml.Append("<td align=\"center\"><span class='fenye'><a href=\"" + url + "page=" + i.ToString() + "\" >" + i.ToString() + "</a></span></td>");
            //    }
            //}

            sbhtml.Append("<td align=\"center\" valign=\"bottom\"><span class='fenye'><a href=\"" + url + "page=" + pagecount.ToString() + "\" >ĩҳ</a></span></td>");

            //sbhtml.Append("<td height=\"15\">&nbsp;<td>");
            sbhtml.Append("<tr>");
            sbhtml.Append("</table>");

            return sbhtml.ToString();
        }

    }
}
