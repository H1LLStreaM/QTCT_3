using System;
using System.Collections.Generic;
using System.Text;

namespace WY.Common.Utility
{
    public class MoneyCHSHelper
    {
        public static string GetNumberName(string p_strNumber)
        {
            string[] v_strNumber = p_strNumber.Split('.');
            string v_strIntNumberName = "", v_strDecNumberName = "", v_strLastIntBillionNumberName = "", v_strLastIntNumberName = "";
            for (int i = 0; i < v_strNumber.Length; i++)
            {
                if (i == 0)
                    v_strIntNumberName = GetIntNumberName(v_strNumber[i].ToString());
                if (i == 1)
                    v_strDecNumberName = GetDecNumberName(v_strNumber[i].ToString());
            }
            if (v_strIntNumberName.IndexOf("ÒÚ") >= 0)
                if (v_strIntNumberName.Substring(v_strIntNumberName.IndexOf("ÒÚ"), 3).IndexOf("Ç§") == -1)
                {
                    foreach (char v_strName in v_strIntNumberName)
                    {
                        v_strLastIntBillionNumberName = v_strLastIntBillionNumberName + v_strName;
                        if (v_strName.ToString() == "ÒÚ")
                            v_strLastIntBillionNumberName = v_strLastIntBillionNumberName + "Áã";
                    }
                }

            if (v_strLastIntBillionNumberName == "")
                v_strLastIntBillionNumberName = v_strIntNumberName;

            if (v_strLastIntBillionNumberName.IndexOf("Íò") >= 0)
                if (v_strLastIntBillionNumberName.Substring(v_strLastIntBillionNumberName.IndexOf("Íò"), 3).IndexOf("Ç§") == -1)
                    foreach (char v_strName in v_strLastIntBillionNumberName)
                    {
                        v_strLastIntNumberName = v_strLastIntNumberName + v_strName;
                        if (v_strName.ToString() == "Íò")
                            v_strLastIntNumberName = v_strLastIntNumberName + "Áã";
                    }
            if (v_strLastIntNumberName == "")
                v_strLastIntNumberName = v_strLastIntBillionNumberName;
            if (v_strLastIntNumberName != "")
            {
                if (v_strDecNumberName == "")
                    return v_strLastIntNumberName.Trim() + "Ô²Õû";
                else
                    return v_strLastIntNumberName.Trim() + "Ô²" + v_strDecNumberName.Trim();
            }
            else
                if (v_strDecNumberName != "")
                    return v_strDecNumberName.Trim();
            return "";
        }
        private static string GetIntNumberName(string p_strIntNumberName)
        {
            string v_intNumberName = GetInvertingNumberName(p_strIntNumberName);
            string v_strBillion = "";
            string v_strMillion = "";
            string v_strThousand = "";
            string v_strBillionNumberName = "";
            string v_strMillionNumberName = "";
            string v_strThousandNumberName = "";
            for (int i = 0; i < v_intNumberName.Length; i++)
            {
                if (i <= 3)
                    v_strThousand = v_strThousand + v_intNumberName[i].ToString();
                if (i > 3 && i <= 7)
                    v_strMillion = v_strMillion + v_intNumberName[i].ToString();
                if (i > 7)
                    v_strBillion = v_strBillion + v_intNumberName[i].ToString();
            }
            if (v_strBillion.Length > 0)
                v_strBillionNumberName = GetBillionNumberName(GetInvertingNumberName(v_strBillion));

            if (v_strMillion.Length > 0)
                v_strMillionNumberName = GetMillionNumberName(GetInvertingNumberName(v_strMillion));

            if (v_strThousand.Length > 0)
                v_strThousandNumberName = GetThousandNumberName(GetInvertingNumberName(v_strThousand));
            return v_strBillionNumberName.Trim() + v_strMillionNumberName.Trim() + v_strThousandNumberName.Trim();
        }

        private static string GetDecNumberName(string p_strDecNamberName)
        {
            string v_strDecNumberName = "";
            if (p_strDecNamberName.Length == 1)
                p_strDecNamberName = "0" + p_strDecNamberName;
            for (int i = 0; i < p_strDecNamberName.Length; i++)
            {
                if (i == 0 && Convert.ToInt32(p_strDecNamberName[i].ToString()) > 0)
                    v_strDecNumberName = v_strDecNumberName + GetEngNumberName(Convert.ToInt32(p_strDecNamberName[i].ToString())) + "½Ç";
                if (i == 1 && Convert.ToInt32(p_strDecNamberName[i].ToString()) > 0)
                    v_strDecNumberName = v_strDecNumberName + GetEngNumberName(Convert.ToInt32(p_strDecNamberName[i].ToString())) + "·Ö";
            }
            return v_strDecNumberName;
        }
        private static string GetBillionNumberName(string p_strBillionNumberName)
        {
            string v_intNumberName = GetInvertingNumberName(p_strBillionNumberName);
            string v_strBillion = "";
            string v_strMillion = "";
            string v_strThousand = "";
            string v_strBillionNumberName = "";
            string v_strMillionNumberName = "";
            string v_strThousandNumberName = "";
            for (int i = 0; i < v_intNumberName.Length; i++)
            {
                if (i <= 3)
                    v_strThousand = v_strThousand + v_intNumberName[i].ToString();
                if (i > 3 && i <= 7)
                    v_strMillion = v_strMillion + v_intNumberName[i].ToString();
                if (i > 7)
                    v_strBillion = v_strBillion + v_intNumberName[i].ToString();
            }
            if (v_strBillion.Length > 0)
                v_strBillionNumberName = GetBillionNumberName(GetInvertingNumberName(v_strBillion));
            if (v_strMillion.Length > 0)
                v_strMillionNumberName = GetMillionNumberName(GetInvertingNumberName(v_strMillion));
            if (v_strThousand.Length > 0)
                v_strThousandNumberName = GetThousandNumberName(GetInvertingNumberName(v_strThousand));
            return v_strBillionNumberName = v_strBillionNumberName + v_strMillionNumberName + v_strThousandNumberName + "ÒÚ";
        }
        private static string GetMillionNumberName(string p_strMillionNumberName)
        {
            string v_strMillion = p_strMillionNumberName;
            string v_strMillionNumberName = "";
            if (v_strMillion.Length == 1)
                v_strMillion = "000" + v_strMillion;
            if (v_strMillion.Length == 2)
                v_strMillion = "00" + v_strMillion;
            if (v_strMillion.Length == 3)
                v_strMillion = "0" + v_strMillion;
            for (int i = 0; i < v_strMillion.Length; i++)
            {
                if (i == 0 && Convert.ToInt32(v_strMillion[i].ToString()) > 0)
                    v_strMillionNumberName = v_strMillionNumberName + GetEngNumberName(Convert.ToInt32(v_strMillion[i].ToString())) + "Ç§";
                if (i == 1 && Convert.ToInt32(v_strMillion[i].ToString()) > 0)
                    v_strMillionNumberName = v_strMillionNumberName + GetEngNumberName(Convert.ToInt32(v_strMillion[i].ToString())) + "°Ù";
                else
                    if (i == 1 && Convert.ToInt32(v_strMillion[i - 1].ToString()) > 0)
                        v_strMillionNumberName = v_strMillionNumberName + GetEngNumberName(Convert.ToInt32(v_strMillion[i].ToString()));
                if (i == 2 && Convert.ToInt32(v_strMillion[i].ToString()) > 0)
                    v_strMillionNumberName = v_strMillionNumberName + GetEngNumberName(Convert.ToInt32(v_strMillion[i].ToString()) * 10);
                else
                    if (i == 2 && Convert.ToInt32(v_strMillion[i - 1].ToString()) > 0)
                        v_strMillionNumberName = v_strMillionNumberName + GetEngNumberName(Convert.ToInt32(v_strMillion[i].ToString()));
                if (i == 3 && Convert.ToInt32(v_strMillion[i].ToString()) > 0)
                    v_strMillionNumberName = v_strMillionNumberName + GetEngNumberName(Convert.ToInt32(v_strMillion[i].ToString()));
            }
            if (v_strMillionNumberName == "")
                return v_strMillionNumberName.Trim();
            if (v_strMillionNumberName[v_strMillionNumberName.Length - 1].ToString() == "Áã")
                v_strMillionNumberName = v_strMillionNumberName.Substring(0, v_strMillionNumberName.Length - 1);
            return v_strMillionNumberName + "Íò";
        }
        private static string GetThousandNumberName(string p_strThousandNumberName)
        {
            string v_strThousand = p_strThousandNumberName;
            string v_strThousandNumberName = "";
            if (v_strThousand.Length == 1)
                v_strThousand = "000" + v_strThousand;
            if (v_strThousand.Length == 2)
                v_strThousand = "00" + v_strThousand;
            if (v_strThousand.Length == 3)
                v_strThousand = "0" + v_strThousand;

            for (int i = 0; i < v_strThousand.Length; i++)
            {
                if (i == 0 && Convert.ToInt32(v_strThousand[i].ToString()) > 0)
                    v_strThousandNumberName = v_strThousandNumberName + GetEngNumberName(Convert.ToInt32(v_strThousand[i].ToString())) + "Ç§";
                if (i == 1 && Convert.ToInt32(v_strThousand[i].ToString()) > 0)
                    v_strThousandNumberName = v_strThousandNumberName + GetEngNumberName(Convert.ToInt32(v_strThousand[i].ToString())) + "°Ù";
                else
                    if (i == 1 && Convert.ToInt32(v_strThousand[i - 1].ToString()) > 0)
                        v_strThousandNumberName = v_strThousandNumberName + GetEngNumberName(Convert.ToInt32(v_strThousand[i].ToString()));
                if (i == 2 && Convert.ToInt32(v_strThousand[i].ToString()) > 0)
                    v_strThousandNumberName = v_strThousandNumberName + GetEngNumberName(Convert.ToInt32(v_strThousand[i].ToString()) * 10);
                else
                    if (i == 2 && Convert.ToInt32(v_strThousand[i - 1].ToString()) > 0)
                        v_strThousandNumberName = v_strThousandNumberName + GetEngNumberName(Convert.ToInt32(v_strThousand[i].ToString()));
                if (i == 3 && Convert.ToInt32(v_strThousand[i].ToString()) > 0)
                    v_strThousandNumberName = v_strThousandNumberName + GetEngNumberName(Convert.ToInt32(v_strThousand[i].ToString()));
            }
            return v_strThousandNumberName;
        }


        private static string GetInvertingNumberName(string p_strIntNumberName)
        {
            string v_InvertingNumberName = "", v_IntNumberName = p_strIntNumberName;
            for (int i = p_strIntNumberName.Length - 1; i >= 0; i--)
            {
                v_InvertingNumberName = v_InvertingNumberName + v_IntNumberName[i].ToString();
            }
            return v_InvertingNumberName;
        }

        private static string GetEngNumberName(int p_intNumber)
        {
            string v_strEngNumberName = "";
            switch (p_intNumber)
            {
                case 0:
                    v_strEngNumberName = "Áã";
                    break;
                case 1:
                    v_strEngNumberName = "Ò¼";
                    break;
                case 2:
                    v_strEngNumberName = "·¡";
                    break;
                case 3:
                    v_strEngNumberName = "Èþ";
                    break;
                case 4:
                    v_strEngNumberName = "ËÁ";
                    break;
                case 5:
                    v_strEngNumberName = "Îé";
                    break;
                case 6:
                    v_strEngNumberName = "Â½";
                    break;
                case 7:
                    v_strEngNumberName = "Æâ";
                    break;
                case 8:
                    v_strEngNumberName = "°Æ";
                    break;
                case 9:
                    v_strEngNumberName = "¾Á";
                    break;
                case 10:
                    v_strEngNumberName = "Ò¼Ê°";
                    break;
                case 20:
                    v_strEngNumberName = "·¡Ê°";
                    break;
                case 30:
                    v_strEngNumberName = "ÈþÊ°";
                    break;
                case 40:
                    v_strEngNumberName = "ËÁÊ°";
                    break;
                case 50:
                    v_strEngNumberName = "ÎéÊ°";
                    break;
                case 60:
                    v_strEngNumberName = "Â½Ê°";
                    break;
                case 70:
                    v_strEngNumberName = "ÆâÊ°";
                    break;
                case 80:
                    v_strEngNumberName = "°ÆÊ°";
                    break;
                case 90:
                    v_strEngNumberName = "¾ÁÊ°";
                    break;
            }
            return v_strEngNumberName;
        }


    }
}
