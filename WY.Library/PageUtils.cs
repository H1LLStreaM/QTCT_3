using System;
using System.Collections.Generic;
using System.Text;

namespace WY.Library
{
    public class PageUtils
    {
        public static string GetPageInfo(int pageindex, int pagecount, string url, string queryName)
        {
            string jointStr = "";
            if (url.IndexOf('?') > 0)
            {
                jointStr = "&";
            }
            else
            {
                jointStr = "?";
            }

            if (string.IsNullOrEmpty(queryName))
            {
                queryName = "page";
            }

            string retString = "<script language='javascript'>\n" +
                                "function " + queryName + "Go()\n" +
                                "{\n" +
                                    "if (check" + queryName + "Index()) {\n" +
                                        "window.location.href = '" + url + jointStr + queryName + "=' + document.getElementById('" + queryName + "Index').value;\n" +
                                    "}\n" +
                                "}\n" +
                                "function check" + queryName + "Index()\n" +
                                "{\n" +
                                    "var index = document.getElementById('" + queryName + "Index').value;\n" +
                                    "if (index == '')\n" +
                                    "{\n" +
                                        "alert('请输入页码!');\n" +
                                        "document.getElementById('" + queryName + "Index').focus();\n" +
                                        "return false;\n" +
                                    "}\n" +
                                    "var i;\n" +
                                    "for (i = 0; i < index.length; i++)\n" +
                                    "{\n" +
                                        "if ('0123456789'.indexOf(index.substring(i, 1)) == -1)\n" +
                                        "{\n" +
                                            "alert('请输入正确的页码!');\n" +
                                            "document.getElementById('" + queryName + "Index').focus();\n" +
                                            "return false;\n" +
                                        "}\n" +
                                    "}\n" +
                                    "if (index <= 0 || index > " + pagecount + ")\n" +
                                    "{\n" +
                                        "alert('请输入正确的页码!');\n" +
                                        "document.getElementById('" + queryName + "Index').focus();\n" +
                                        "return false;\n" +
                                    "}\n" +

                                    "return true;\n" +
                                "}\n" +
                                "</script>";

            string splitStr = "<div class=\"separator\">|</div>";
            //retString += "<div style=\"width:100%; height:40px;\"><div class=\"pagination\"><table border='0' cellspacing='0' cellpadding='0'><tr><td style='padding-top:0px !important;padding-top:23px;' nowrap>";
            //retString += "<ul>";

            //if (pageindex > 1)
            //{
            //    retString += "<li><a class=\"nextpage\" href=\"" + url + jointStr + queryName + "=" + (pageindex - 1) + "\">上一页</a></li>";
            //    retString += "<li>&nbsp;</li>";
            //}
            //if (pageindex > 3)
            //{
            //    retString += "<li><a href=\"" + url + jointStr + queryName + "=1\">1</a></li>";
            //    retString += "<li class=\"separator\">...</li>";
            //}
            //int startPage = (pageindex - 2 > 1) ? pageindex - 2 : 1;
            //if (pageindex >= pagecount - 2)
            //{
            //    startPage = (pagecount - 4 > 0) ? pagecount - 4 : 1;
            //}
            //for (int i = startPage; i < startPage + 5; i++)
            //{
            //    if (i <= pagecount)
            //    {
            //        if (i == pageindex)
            //        {
            //            retString += "<li><a class=\"currentpage\" href=\"" + url + jointStr + queryName + "=" + i + "\">" + i + "</a></li>";
            //        }
            //        else
            //        {
            //            retString += "<li><a href=\"" + url + jointStr + queryName + "=" + i + "\">" + i + "</a></li>";
            //        }
            //        retString += splitStr;
            //    }
            //}
            //if (pageindex < pagecount - 2)
            //{
            //    if (retString.EndsWith(splitStr))
            //    {
            //        retString = retString.Substring(0, retString.LastIndexOf(splitStr));
            //    }
            //    retString += "<li class=\"separator\">...</li>";
            //    retString += "<li><a href=\"" + url + jointStr + queryName + "=" + pagecount + "\">" + pagecount + "</a></li>";
            //}
            //if (pageindex < pagecount)
            //{
            //    retString += "<li>&nbsp;</li>";
            //    retString += "<li><a class=\"nextpage\" href=\"" + url + jointStr + queryName + "=" + (pageindex + 1) + "\">下一页</a></li>";
            //}
            //retString += "</ul></td><td nowrap>";
            //retString += "&nbsp;&nbsp;第&nbsp;<input type='text' size='1' id='" + queryName + "Index" + "'>" + "&nbsp;页&nbsp;" +
            //    "<img src='"+ WebUtils.AppUrl + "resource/images/pageinfo/go.gif' onclick='" + queryName + "Go();' value='Go'>";
            //retString += "</td></tr></table></div></div>";



            retString += "<div style=\"width:100%; height:40px;\"><div class=\"pagination\"><div style='height:40px;float:left;margin-top:5px;'>";

            if (pageindex > 1)
            {
                retString += "<div><a class=\"nextpage\" href=\"" + url + jointStr + queryName + "=" + (pageindex - 1) + "\">上一页</a></div>";
                retString += "<div>&nbsp;</div>";
            }
            if (pageindex > 3)
            {
                retString += "<div><a href=\"" + url + jointStr + queryName + "=1\">1</a></div>";
                retString += "<div class=\"separator\">...</div>";
            }
            int startPage = (pageindex - 2 > 1) ? pageindex - 2 : 1;
            if (pageindex >= pagecount - 2)
            {
                startPage = (pagecount - 4 > 0) ? pagecount - 4 : 1;
            }
            for (int i = startPage; i < startPage + 5; i++)
            {
                if (i <= pagecount)
                {
                    if (i == pageindex)
                    {
                        retString += "<div><a class=\"currentpage\" href=\"" + url + jointStr + queryName + "=" + i + "\">" + i + "</a></div>";
                    }
                    else
                    {
                        retString += "<div><a href=\"" + url + jointStr + queryName + "=" + i + "\">" + i + "</a></div>";
                    }
                    retString += splitStr;
                }
            }
            if (pageindex < pagecount - 2)
            {
                if (retString.EndsWith(splitStr))
                {
                    retString = retString.Substring(0, retString.LastIndexOf(splitStr));
                }
                retString += "<div class=\"separator\">...</div>";
                retString += "<div><a href=\"" + url + jointStr + queryName + "=" + pagecount + "\">" + pagecount + "</a></div>";
            }
            if (pageindex < pagecount)
            {
                retString += "<div>&nbsp;</div>";
                retString += "<div><a class=\"nextpage\" href=\"" + url + jointStr + queryName + "=" + (pageindex + 1) + "\">下一页</a></div>";
            }
            retString += "</div><div style='height:40px;float:left;'>";
            retString += "&nbsp;&nbsp;第&nbsp;<input type='text' size='1' id='" + queryName + "Index" + "'>" + "&nbsp;页&nbsp;</div><div style='height:40px;float:left;margin-top:5px;'>" +
                "<img src='" + WebUtils.AppUrl + "resource/images/pageinfo/go.gif' onclick='" + queryName + "Go();' value='Go' style='cursor:pointer;' >";
            retString += "</div></div></div>";







            return retString;
        }


        public static string GetPageInfo2(int pageindex, int pagecount, string pageName)
        {
            if (string.IsNullOrEmpty(pageName))
            {
                pageName = "page";
            }

            string key = Guid.NewGuid().ToString().Replace("-", "");

            string retString = "<script language='javascript'>\n" +
                                "var pageindex" + key + " = " + pageindex + ";\n" +
                                "var pagecount" + key + " = " + pagecount + ";\n" +
                                "function set" + pageName + "Index(index) {\n" +
                                    "pageindex" + key + " = index;\n" +
                                    "document.getElementById(\"" + pageName + "PageIndex\").innerHTML = index;\n" +
                                    "if (index > 1) {\n" +
                                        "document.getElementById(\"" + pageName + "LeftArrow\").src = \"" + WebUtils.AppUrl + "resource/images/pageinfo/left_a.gif\";\n" +
                                        "document.getElementById(\"" + pageName + "LeftArrow\").onclick = function() {set" + pageName + "Index(pageindex" + key + " - 1);Jump" + pageName + "(pageindex" + key + ");};\n" +
                                        "document.getElementById(\"" + pageName + "LeftArrow\").style.cursor = \"pointer\"\n" +
                                    "} else {\n" +
                                        "document.getElementById(\"" + pageName + "LeftArrow\").src = \"" + WebUtils.AppUrl + "resource/images/pageinfo/left_b.gif\";\n" +
                                        "document.getElementById(\"" + pageName + "LeftArrow\").onclick = \"\";\n" +
                                        "document.getElementById(\"" + pageName + "LeftArrow\").style.cursor = \"default\"\n" +
                                    "}\n" +
                                    "if (index < pagecount" + key + ") {\n" +
                                        "document.getElementById(\"" + pageName + "RightArrow\").src = \"" + WebUtils.AppUrl + "resource/images/pageinfo/right_a.gif\";\n" +
                                        "document.getElementById(\"" + pageName + "RightArrow\").onclick = function() {set" + pageName + "Index(pageindex" + key + " + 1);Jump" + pageName + "(pageindex" + key + ");};\n" +
                                        "document.getElementById(\"" + pageName + "RightArrow\").style.cursor = \"pointer\"\n" +
                                    "} else {\n" +
                                        "document.getElementById(\"" + pageName + "RightArrow\").src = \"" + WebUtils.AppUrl + "resource/images/pageinfo/right_b.gif\";\n" +
                                        "document.getElementById(\"" + pageName + "RightArrow\").onclick = \"\";\n" +
                                        "document.getElementById(\"" + pageName + "RightArrow\").style.cursor = \"default\"\n" +
                                    "}\n" +
                                "}\n" +
                                "</script>";

            retString += "<div style=\"width:100%; height:40px;\"><div class=\"pagination\"><table border='0' cellspacing='0' cellpadding='0'><tr><td nowrap>";
            retString += "<ul>";

            if (pageindex > 1)
            {
                retString += "<li><img id=\"" + pageName + "LeftArrow\" style=\"cursor:pointer;\" src=\"" + WebUtils.AppUrl + "resource/images/pageinfo/left_a.gif\" onclick=\"javascript:set" + pageName + "Index(pageindex" + key + " - 1);Jump" + pageName + "(pageindex" + key + ");\" /></li>";
            }
            else
            {
                retString += "<li><img id=\"" + pageName + "LeftArrow\" src=\"" + WebUtils.AppUrl + "resource/images/pageinfo/left_b.gif\"  /></li>";
            }
            retString += "<li>&nbsp;</li>";
            retString += "<li><label id=\"" + pageName + "PageIndex\" >" + pageindex + "</label></li>";
            retString += "<li>&nbsp;</li>";
            if (pageindex < pagecount)
            {
                retString += "<li><img id=\"" + pageName + "RightArrow\" style=\"cursor:pointer;\" src=\"" + WebUtils.AppUrl + "resource/images/pageinfo/right_a.gif\" onclick=\"javascript:set" + pageName + "Index(pageindex" + key + " + 1);Jump" + pageName + "(pageindex" + key + ");\"/></li>";
            }
            else
            {
                retString += "<li><img id=\"" + pageName + "RightArrow\" src=\"" + WebUtils.AppUrl + "resource/images/pageinfo/right_b.gif\"/></li>";
            }
            retString += "</ul></td></tr></table></div></div>";

            return retString;
        }

    }
}
