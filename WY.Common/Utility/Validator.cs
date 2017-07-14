using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace WY.Common.Utility
{
    public class Validator
    {
        /// <summary>
        /// �Ƿ�����
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
        /// �Ƿ�������
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidPosInteger(string checkedValue)
        {
            string pattern = @"^[0-9]*[1-9][0-9]*$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ�����
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNegInteger(string checkedValue)
        {
            string pattern = @"^[0-9]*[1-9][0-9]*$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ�Ǹ������������� + 0�� 
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNotNegInteger(string checkedValue)
        {
            string pattern = @"^\d+$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ���������������� + 0�� 
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNotPosInteger(string checkedValue)
        {
            string pattern = @"^((-\d+)|(0+))$";
            return IsMatch(pattern, checkedValue);
        }


        /// <summary>
        /// �Ƿ񸡵���
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidDecimal(string checkedValue)
        {
            string pattern = @"^(-?\d+)(\.\d+)?$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ���������
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidPosDecimal(string checkedValue)
        {
            string pattern = @"^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ񸺸�����
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNegDecimal(string checkedValue)
        {
            string pattern = @"^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ�Ǹ����������������� + 0��
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNotNegDecimal(string checkedValue)
        {
            string pattern = @"^\d+(\.\d+)?$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �������������������� + 0�� 
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNotPosDecimal(string checkedValue)
        {
            string pattern = @"^((-\d+(\.\d+)?)|(0+(\.0+)?))$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ�Ǹ����������� + 0 + �������� �� 
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNotNeg(string checkedValue)
        {
            string pattern = @"^\d+(\.{0,1}\d+){0,1}$";
            return IsMatch(pattern, checkedValue);
        }


        /// <summary>
        /// �Ƿ�Ϊ�����ַ�����0-9������ɵ��ַ��� 
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsNumberSting(string checkedValue)
        {
            string pattern = @"^[0-9]+$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ�����ֵ������YYYY-MM-DD��YYYY/MM/DD��YYYY-MM-DD HH:MM:SS��YYYY-MM-DD HH:MM��HH:MM:SS�ȣ�
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidDateTime(string checkedValue)
        {
            string pattern = @"^(?=\d)(?:(?!(?:1582(?:\.|-|\/)10(?:\.|-|\/)(?:0?[5-9]|1[0-4]))|(?:1752(?:\.|-|\/)0?9(?:\.|-|\/)(?:0?[3-9]|1[0-3])))(?=(?:(?!000[04]|(?:(?:1[^0-6]|[2468][^048]|[3579][^26])00))(?:(?:\d\d)(?:[02468][048]|[13579][26]))\D0?2\D29)|(?:\d{4}\D(?!(?:0?[2469]|11)\D31)(?!0?2(?:\.|-|\/)(?:29|30))))(\d{4})([-\/.])(0?\d|1[012])\2((?!00)[012]?\d|3[01])(?:$|(?=\x20\d)\x20))?((?:(?:0?[1-9]|1[012])(?::[0-5]\d){0,2}(?:\x20[aApP][mM]))|(?:[01]\d|2[0-3])(?::[0-5]\d){1,2})?$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ�����ֵ��YYYYMM)
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidYYYYMMDate(string checkedValue)
        {
            string pattern = @"^\d{4}((0[1-9])|(1[0-2]))$";
            return IsMatch(pattern, checkedValue);
        }
        /// <summary>
        /// �Ƿ�����ȷ��Email
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidEmail(string checkedValue)
        {
            string pattern = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ�����ȷ�Ĺ��ڵ绰����
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidPhoneNumber(string checkedValue)
        {
            string pattern = @"\d{3}-\d{6,8}|\d{4}-\d{6,8}|\d{6,8}";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ�����ȷ���ƶ��绰����
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidMobilePhoneNumber(string checkedValue)
        {
            string pattern = @"^1(5\d{9}|3\d{9})$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ�����ȷ����������
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidPostCode(string checkedValue)
        {
            string pattern = @"[1-9]\d{5}(?!\d)";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ�����ȷ�����֤��(15λ��18λ)
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidIDNumber(string checkedValue)
        {
            string pattern = @"\d{15}|\d{18}";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ�����ȷ��IP��ַ
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidIPAddress(string checkedValue)
        {
            string pattern = @"\d+\.\d+\.\d+\.\d+";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ�����ȷ����ĸ
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidLetter(string checkedValue)
        {
            string pattern = @"^[A-Za-z]+$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ�����ȷ�Ĵ�д��ĸ
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidCapitalLetter(string checkedValue)
        {
            string pattern = @"^[A-Z]+$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ���ȷ��Сд��ĸ
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidSmallLetter(string checkedValue)
        {
            string pattern = @"^[a-z]+$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ���ȷ����ĸ���������
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNumberLetter(string checkedValue)
        {
            string pattern = @"^[A-Za-z0-9]+$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ���ȷ����ĸ�����ֺ��»������
        /// </summary>
        /// <param name="checkedValue"></param>
        /// <returns></returns>
        public static bool IsValidNumberLetterUnderLine(string checkedValue)
        {
            string pattern = @"^\w+$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// �Ƿ���ȷ�Ļ���ֵ
        /// </summary>
        /// <param name="checkedValue">У��ֵ</param>
        /// <param name="integraldigit">����λ��</param>
        /// <param name="decimaldigit">С��λ��</param>
        /// <returns></returns>
        public static bool IsValidCurrency(string checkedValue, int integraldigit, int decimaldigit)
        {
            string pattern = @"^-|(([1-9]\d{0," + integraldigit + @"})|(0))(\.\d{1," + decimaldigit + @"})?$";
            return IsMatch(pattern, checkedValue);
        }

        /// <summary>
        /// ����Ƿ�����ȷ��Url
        /// </summary>
        /// <param name="strUrl">Ҫ��֤��Url</param>
        /// <returns>�жϽ��</returns>
        public static bool IsURL(string strUrl)
        {
            return Regex.IsMatch(strUrl, @"^(http|https)\://([a-zA-Z0-9\.\-]+(\:[a-zA-Z0-9\.&%\$\-]+)*@)*((25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])|localhost|([a-zA-Z0-9\-]+\.)*[a-zA-Z0-9\-]+\.(com|edu|gov|int|mil|net|org|biz|arpa|info|name|pro|aero|coop|museum|[a-zA-Z]{1,10}))(\:[0-9]+)*(/($|[a-zA-Z0-9\.\,\?\'\\\+&%\$#\=~_\-]+))*$");
        }

        /// <summary>
        /// ƥ��
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
