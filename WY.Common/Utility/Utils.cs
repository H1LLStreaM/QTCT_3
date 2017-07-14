using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Net;
using System.Reflection;

namespace WY.Common.Utility
{
    public class Utils
    {
        /// <summary>
        /// MD5函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(string str)
        {
            if (string.IsNullOrEmpty(str)) return "";

            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }

        /// <summary>
        /// 检测是否有Sql危险字符
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeSqlString(string str)
        {

            return !Regex.IsMatch(str, @"[-|;|,|\/|\(|\)|\[|\]|\}|\{|%|@|\*|!|\']");
        }

        /// <summary>
        /// 检测是否有危险的可能用于链接的字符串
        /// </summary>
        /// <param name="str">要判断字符串</param>
        /// <returns>判断结果</returns>
        public static bool IsSafeUserInfoString(string str)
        {
            return !Regex.IsMatch(str, @"^\s*$|^c:\\con\\con$|[%,\*" + "\"" + @"\s\t\<\>\&]|游客|^Guest");
        }

        /// <summary>
        /// 判断目录是否存在，如果不存在，则创建目录
        /// </summary>
        /// <param name="path"></param>
        public static void CreateDirectory(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        #region CreateTempFile

        /// <summary>
        /// 创建临时文件
        /// </summary>
        /// <param name="file_name"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        public static void CreateTempFile(string file_name,byte[] contents)
        {
            using (FileStream fs = new FileStream(file_name, FileMode.OpenOrCreate, FileAccess.Write))
            {
                fs.Write(contents, 0, contents.Length);
                fs.Close();
                fs.Dispose();
            }
        }

        #endregion

        #region ReadFile

        public static Byte[] ReadFile(string filename)
        {
            using (FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                Byte[] b = new byte[fs.Length];
                fs.Read(b, 0, (int)fs.Length);

                return b;
            }
        }

        #endregion

        #region GetThumbImage

        public static Image GetThumbImage(Image img, int thumbwidth, int thumbheight)
        {
            int oldh = img.Height;
            int oldw = img.Width;

            int newh, neww;

            double h1 = oldh * 1.0 / thumbheight;
            double w1 = oldw * 1.0 / thumbwidth;

            double f = (h1 > w1) ? h1 : w1;

            if (f < 1.0)
            {
                newh = oldh;
                neww = oldw;
            }
            else
            {
                newh = (int)(oldh / f);
                neww = (int)(oldw / f);
            }


            Image myThumbnail = img.GetThumbnailImage(thumbwidth, thumbheight, null, IntPtr.Zero);

            Bitmap newimg = new Bitmap(thumbwidth, thumbheight);
            Graphics g = Graphics.FromImage(newimg);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.DrawImage(myThumbnail, new Rectangle((thumbwidth - neww) / 2, (thumbheight - newh) / 2, neww, newh), new Rectangle(0, 0, myThumbnail.Width, myThumbnail.Height), GraphicsUnit.Pixel);

            return newimg;
        }

        #endregion

        /// <summary>
        /// 保存图片缩略图
        /// </summary>
        /// <param name="srcfilename">源图路径</param>
        /// <param name="destfilename">缩略图保存路径</param>
        /// <param name="width">缩略图宽度</param>
        /// <param name="height">缩略图高度</param>
        /// <param name="bstretch">是否按比例缩放(False:剪裁; True:按比例缩放)</param>
        public static void SaveThumbImage(string srcfilename, string destfilename, int width, int height, bool bstretch)
        {
            using (Image srcimage = Image.FromFile(srcfilename))
            {
                int owidth = srcimage.Width;
                int oheight = srcimage.Height;

                if (bstretch)
                {
                    #region 按比例计算出缩略图的宽度和高度
                    //按比例计算出缩略图的宽度和高度
                    int newh, neww;

                    double h1 = oheight * 1.0 / height;
                    double w1 = owidth * 1.0 / width;

                    double f = (h1 > w1) ? h1 : w1;

                    if (f < 1.0)
                    {
                        newh = oheight;
                        neww = owidth;
                    }
                    else
                    {
                        newh = (int)(oheight / f);
                        neww = (int)(owidth / f);
                    }

                    //生成缩放图
                    using (Image myThumbnail = srcimage.GetThumbnailImage(width, height, null, IntPtr.Zero))
                    {
                        using (Bitmap destimage = new Bitmap(width, height))
                        {
                            Graphics g = Graphics.FromImage(destimage);
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.Clear(Color.White);
                            g.DrawImage(myThumbnail, new Rectangle((width - neww) / 2, (height - newh) / 2, neww, newh), new Rectangle(0, 0, myThumbnail.Width, myThumbnail.Height), GraphicsUnit.Pixel);
                            g.Dispose();

                            destimage.Save(destfilename, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                    }
                    #endregion
                }
                else
                {
                    #region 剪裁超出比例范围的大小
                    //剪裁超出比例范围的大小
                    if (oheight >= (int)Math.Floor(Convert.ToDouble(owidth) * (Convert.ToDouble(height) / Convert.ToDouble(width))))
                    {
                        oheight = (int)Math.Floor(Convert.ToDouble(owidth) * (Convert.ToDouble(height) / Convert.ToDouble(width)));//等比设定高度
                    }
                    else
                    {
                        owidth = (int)Math.Floor(Convert.ToDouble(oheight) * (Convert.ToDouble(width) / Convert.ToDouble(height)));//等比设定宽度
                    }

                    //生成缩放图
                    using (Bitmap destimage = new Bitmap(width, height))
                    {
                        Graphics g = Graphics.FromImage(destimage);
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic; //设置高质量插值法
                        g.SmoothingMode = SmoothingMode.HighQuality;//设置高质量,低速度呈现平滑程度
                        g.Clear(Color.White); //清空画布并以白色填充
                        g.DrawImage(srcimage, new Rectangle(0, 0, width, height), new Rectangle(Math.Abs(srcimage.Width - owidth) / 2, Math.Abs(srcimage.Height - oheight) / 2, owidth, oheight), GraphicsUnit.Pixel);
                        g.Dispose();

                        destimage.Save(destfilename, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    #endregion
                }
            }
        }

        public static string NvStr(object value)
        {
            return ((value == null || value == DBNull.Value) ? "" : value.ToString().Trim());
        }
        public static double NvDouble(object value)
        {
            return ((value == null || value == DBNull.Value) ? 0 : Microsoft.VisualBasic.Conversion.Val(value.ToString()));
        }
        public static decimal NvDecimal(object value)
        {
            return Convert.ToDecimal(NvDouble(value));
        }
        public static int NvInt(object value)
        {
            return (int)Math.Floor(NvDouble(value));
        }
        public static bool NvBool(object value)
        {
            if (value == null || value == DBNull.Value)
            {
                return false;
            }
            else if (value.GetType() == typeof(bool))
            {
                return Convert.ToBoolean(value);
            }
            else if (value.ToString().ToUpper() == "Y"
                || value.ToString().ToUpper() == "TRUE"
                || value.ToString().ToUpper() == "是"
                || value.ToString().ToUpper() == "有")
            {
                return true;
            }
            else
            {
                return NvInt(value) != 0;
            }
        }

        #region 删除字符串2端多余的空格和回车

        /// <summary>
        /// 删除字符串2端多余的空格和回车
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string TrimSpaceAndEnter(string value)
        {
            if (value.Trim() == "") return value.Trim();

            string str1 = "";
            for (int i = 0; i < value.Length; i++)
            {
                if (value[i] != (char)Keys.Space && value[i] != (char)Keys.Enter)
                {
                    str1 = value.Substring(i);
                    break;
                }
            }
            for (int i = str1.Length - 1; i >= 0; i--)
            {
                if (str1[i] != (char)Keys.Space && str1[i] != (char)Keys.Enter)
                {
                    str1 = str1.Substring(0, i + 1);
                    break;
                }
            }
            return str1.Trim();
        }

        #endregion

        #region 替换SQL特殊字符

        public static string GetSQLStr(string value)
        {
            return value.Replace("'", "''");
        }

        #endregion

        #region 读/写配置文件
        /// <summary>
        /// 读配置文件
        /// </summary>
        /// <param name="appname">Application Name</param>
        /// <param name="keyname">Key Name</param>
        /// <param name="filename">INI File Name(full path)</param>
        /// <returns></returns>
        public static string GetIniSetting(string appname, string keyname, string defaultvalue, string filename)
        {
            StringBuilder strvalue = new StringBuilder(256);
            int i = Win32API.GetPrivateProfileString(appname, keyname, defaultvalue, strvalue, 256, filename);

            return strvalue.ToString();
        }

        /// <summary>
        /// 写配置文件
        /// </summary>
        /// <param name="appname"></param>
        /// <param name="keyname"></param>
        /// <param name="value"></param>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static void WriteIniSetting(string appname, string keyname, string value, string filename)
        {
            Win32API.WritePrivateProfileString(appname, keyname, value, filename);
        }
        #endregion

        #region 获取中文拼音首字母

        /// <summary>
        /// 获取中文拼音首字母
        /// </summary>
        /// <param name="chineseStr"></param>
        /// <returns></returns>
        public static string GetStringPY(string chineseStr)
        {
            StringBuilder sb = new StringBuilder();
            int length = chineseStr.Length;
            for (int i = 0; i < length; i++)
            {
                char chineseChar = chineseStr[i];
                sb.Append(GetCharPY(chineseChar));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 获取中文拼音首字母
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static string GetCharPY(char c)
        {
            int ascCode = Microsoft.VisualBasic.Strings.Asc(c);
            int temp = 65536 + ascCode;
            if (temp >= 45217 && temp <= 45252)
            {
                return "A";
            }
            else if (temp >= 45253 && temp <= 45760)
            {
                return "B";
            }
            else if (temp >= 45761 && temp <= 46317)
            {
                return "C";
            }
            else if (temp >= 46318 && temp <= 46825)
            {
                return "D";
            }
            else if (temp >= 46826 && temp <= 47009)
            {
                return "E";
            }
            else if (temp >= 47010 && temp <= 47296)
            {
                return "F";
            }
            else if (temp >= 47297 && temp <= 47613)
            {
                return "G";
            }
            else if (temp >= 47614 && temp <= 48118)
            {
                return "H";
            }
            else if (temp >= 48119 && temp <= 49061)
            {
                return "J";
            }
            else if (temp >= 49062 && temp <= 49323)
            {
                return "K";
            }
            else if (temp >= 49324 && temp <= 49895)
            {
                return "L";
            }
            else if (temp >= 49896 && temp <= 50370)
            {
                return "M";
            }
            else if (temp >= 50371 && temp <= 50613)
            {
                return "N";
            }
            else if (temp >= 50614 && temp <= 50621)
            {
                return "O";
            }
            else if (temp >= 50622 && temp <= 50905)
            {
                return "P";
            }
            else if (temp >= 50906 && temp <= 51386)
            {
                return "Q";
            }
            else if (temp >= 51387 && temp <= 51445)
            {
                return "R";
            }
            else if (temp >= 51446 && temp <= 52217)
            {
                return "S";
            }
            else if (temp >= 52218 && temp <= 52697)
            {
                return "T";
            }
            else if (temp >= 52698 && temp <= 52979)
            {
                return "W";
            }
            else if (temp >= 52980 && temp <= 53688)
            {
                return "X";
            }
            else if (temp >= 53689 && temp <= 54480)
            {
                return "Y";
            }
            else if (temp >= 54481 && temp <= 62289)
            {
                return "Z";
            }
            else
            {
                return c.ToString();
            }
        }

        #endregion

        #region 获取客户端IP
        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <returns></returns>
        public static string GetClientIP()
        {
            IPAddress[] iplist = Dns.GetHostAddresses(Dns.GetHostName());
            if (iplist.Length == 0)
            {
                return "";
            }
            else
            {
                string ip = iplist[0].ToString();
                if (ip.Length > 20)
                {
                    return ip.Substring(0, 20);
                }
                else
                {
                    return ip;
                }
            }
        }
        #endregion

        #region GetAssemblyStream

        /// <summary>
        /// Gets a resource stream.
        /// </summary>
        /// <param name="schemaResourceKey">The schema resource key.</param>
        /// <returns>A resource stream.</returns>
        public static Stream GetAssemblyStream(string assemblyName, string schemaResourceKey)
        {
            return Assembly.Load(assemblyName).GetManifestResourceStream(assemblyName + "." + schemaResourceKey);
        }

        #endregion

        #region 验证 ---------------------------- 正则表达式
        /// <summary>
        /// 正整数
        /// </summary>
        /// <param name="strnum"></param>
        /// <returns></returns>
        public static bool IsPositiveIntNum(string strnum)
        {
            string pattern = @"^[0-9]*$";
            Match m = Regex.Match(strnum, pattern);
            return m.Success;
        }

        /// <summary>
        /// 正浮点数
        /// </summary>
        /// <param name="strnum"></param>
        /// <returns></returns>
        public static bool IsPositiveSingleNum(string strnum)
        {
            string pattern = @"^\d+(\.\d+)?$";
            Match m = Regex.Match(strnum, pattern);
            return m.Success;
        }

        /// <summary>
        /// 数字、26个英文字母或者下划线组成的字符串
        /// </summary>
        /// <param name="strnum"></param>
        /// <returns></returns>
        public static bool IsAZaz_09(string strnum)
        {
            string pattern = @"^[a-zA-Z0-9_]*$";
            Match m = Regex.Match(strnum, pattern);
            return m.Success;
        }

        /// <summary>
        /// 0~999999999之间的整数
        /// </summary>
        /// <param name="strnum"></param>
        /// <returns></returns>
        public static bool Is0_999999999(string strnum)
        {
            if (IsPositiveIntNum(strnum) == false)
            {
                return false;
            }
            else
            {
                try
                {
                    int intNum = Convert.ToInt32(strnum);

                    if (intNum > 999999999)
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 0~999999999之间的数字
        /// </summary>
        /// <param name="strnum"></param>
        /// <returns></returns>
        public static bool Is0_999999999Single(string strnum)
        {
            if (IsPositiveSingleNum(strnum) == false)
            {
                return false;
            }
            else
            {
                try
                {
                    float floatNum = Convert.ToSingle(strnum);

                    if (floatNum > 999999999f)
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 混合文字实际长度取得

        /// <summary>
        /// 混合文字实际长度取得
        /// </summary>
        /// <param name="text">文字</param>
        /// <returns>混合文字的实际长度</returns>
        public static int GetTextLength(string text)
        {
            byte[] bytes = Encoding.Default.GetBytes(text);
            return bytes.Length;
        }

        #endregion

        #region 全角半角转换

        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }


        /// <summary> 转半角的函数(DBC case) </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        ///<remarks>
        ///全角空格为12288，半角空格为32
        ///其他字符半角(33-126)与全角(65281-65374)的对应关系是：均相差65248
        ///</remarks>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }
        #endregion

        public static string GetEnumColComboList(Type enumtype, params bool[] allowedit)
        {
            string strlist = "";
            //设置下拉列表是否可以手动修改值
            if (allowedit.Length > 0 && allowedit[0] == true)
            {
                strlist += "| ";
            }
            else
            {
                strlist += " ";
            }

            string[] names = Enum.GetNames(enumtype);
            foreach (string n in names)
            {
                strlist += "|" + n;
            }

            return strlist;
        }
    }
}
