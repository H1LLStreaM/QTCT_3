using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WY.Common.Utility
{
    public class Validator
    {
        /// <summary>
        /// 是否整数
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidInteger(string checkedValue)
        {
            string pattern = @"^-?\d+$";
            Regex regex = new Regex(pattern);
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否正整数
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidPosInteger(string checkedValue)
        {
            string pattern = @"^[0-9]*[1-9][0-9]*$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否负整数
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNegInteger(string checkedValue)
        {
            string pattern = @"^[0-9]*[1-9][0-9]*$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否非负整数（正整数 + 0） 
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNotNegInteger(string checkedValue)
        {
            string pattern = @"^\d+$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否非正整数（负整数 + 0） 
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNotPosInteger(string checkedValue)
        {
            string pattern = @"^((-\d+)|(0+))$";
            return IsMatch(pattern, checkedValue);
        }


        /// <summary>
        /// 是否浮点数
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidDecimal(string checkedValue)
        {
            string pattern = @"^(-?\d+)(\.\d+)?$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否正浮点数
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidPosDecimal(string checkedValue)
        {
            string pattern = @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否负浮点数
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNegDecimal(string checkedValue)
        {
            string pattern = @"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否非负浮点数（正浮点数 + 0）
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNotNegDecimal(string checkedValue)
        {
            string pattern = @"^\d+(\.\d+)?$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 非正浮点数（负浮点数 + 0） 
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNotPosDecimal(string checkedValue)
        {
            string pattern = @"^((-\d+(\.\d+)?)|(0+(\.0+)?))$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否非负数（正整数 + 0 + 正浮点数 ） 
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNotNeg(string checkedValue)
        {
            string pattern = @"^\d+(\.{0,1}\d+){0,1}$";
            return IsMatch(pattern, checkedValue);
        }


        /// <summary>
        /// 是否为数字字符（由0-9数字组成的字符） 
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsNumberSting(string checkedValue)
        {
            string pattern = @"^[0-9]+$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否日期值（包括YYYY-MM-DD，YYYY/MM/DD，YYYY-MM-DD HH:MM:SS，YYYY-MM-DD HH:MM，HH:MM:SS等）
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidDateTime(string checkedValue)
        {
            string pattern = @"^(?=\d)(?:(?!(?:1582(?:\.|-|\/)10(?:\.|-|\/)(?:0?[5-9]|1[0-4]))|(?:1752(?:\.|-|\/)0?9(?:\.|-|\/)(?:0?[3-9]|1[0-3])))(?=(?:(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:\d\d)(?:[02468][048]|[13579][26]))\D0?2\D29)|(?:\d{4}\D(?!(?:0?[2469]|11)\D31)(?!0?2(?:\.|-|\/)(?:29|30))))(\d{4})([-\/.])(0?\d|1[012])\2((?!00)[012]?\d|3[01])(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否日期值（YYYYMM)
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidYYYYMMDate(string checkedValue)
        {
            string pattern = @"^\d{4}((0[1-9])|(1[0-2]))$";
            return IsMatch(pattern, checkedValue);
        }
        /// <summary>
        /// 是否是正确的Email
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string checkedValue)
        {
            string pattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否是正确的国内电话号码
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidPhoneNumber(string checkedValue)
        {
            string pattern = @"\d{3}-\d{6,8}|\d{4}-\d{6,8}|\d{6,8}";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否是正确的移动电话号码
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidMobilePhoneNumber(string checkedValue)
        {
            string pattern = @"^1(5\d{9}|3\d{9})$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否是正确的邮政编码
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidPostCode(string checkedValue)
        {
            string pattern = @"[1-9]\d{5}(?!\d)";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否是正确的身份证号(15位或18位)
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidIDNumber(string checkedValue)
        {
            string pattern = @"\d{15}|\d{18}";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否是正确的IP地址
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidIPAddress(string checkedValue)
        {
            string pattern = @"\d+\.\d+\.\d+\.\d+";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否是正确的字母
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidLetter(string checkedValue)
        {
            string pattern = @"^[A-Za-z]+$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否是正确的大写字母
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidCapitalLetter(string checkedValue)
        {
            string pattern = @"^[A-Z]+$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否正确的小写字母
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidSmallLetter(string checkedValue)
        {
            string pattern = @"^[a-z]+$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否正确的字母和数字组成
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNumberLetter(string checkedValue)
        {
            string pattern = @"^[A-Za-z0-9]+$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否正确的字母、数字和下划线组成
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNumberLetterUnderLine(string checkedValue)
        {
            string pattern = @"^\w+$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 是否正确的货币值
        /// </summary>
        /// <param name="checkedValue">校验值</param>
        /// <param name="integraldigit">整数位数</param>
        /// <param name="decimaldigit">小数位数</param>
        /// <returns></returns>
        public static bool IsValidCurrency(string checkedValue, int integraldigit, int decimaldigit)
        {
            string pattern = @"^-|(([1-9]\d{0," + integraldigit + @"})|(0))(\.\d{1," + decimaldigit + @"})?$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// 检测是否是正确的Url
        /// </summary>
        /// <param name="strUrl">要验证的Url</param>
        /// <returns>判断结果</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        /// <summary>
        /// 匹配
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        internal static bool IsMatch(string pattern, string checkedValue)
        {
            Regex regex = new Regex(pattern);
            return regex.IsMatch(checkedValue);
        }
    }
}
