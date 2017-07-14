using System;
using System.Collections.Generic;
using System.Text;

namespace WY.Common.Utility
{
    #region Number2English
    public class Number2English
    {
        private string[] StrNO = new string[19];
        private string[] Unit = new string[9];
        private string[] StrTens = new string[9];

        #region NumberToString
        public string NumberToString(double Number)
        {
            string Str;
            string BeforePoint;
            string AfterPoint;
            string tmpStr;
            int nBit;
            string CurString;
            int nNumLen;
            Init();
            Str = Math.Round(Number, 2).ToString("0.00");
            if (Str.IndexOf(".") == -1)
            {
                BeforePoint = Str;
                AfterPoint = "";
            }
            else
            {
                BeforePoint = Str.Substring(0, Str.IndexOf("."));
                AfterPoint = Str.Substring(Str.IndexOf(".") + 1, Str.Length - Str.IndexOf(".") - 1);
            }
            if (BeforePoint.Length > 12)
            {
                return null;
            }
            Str = "";
            while (BeforePoint.Length > 0)
            {
                nNumLen = Len(BeforePoint);
                if (nNumLen % 3 == 0)
                {
                    CurString = Left(BeforePoint, 3);
                    BeforePoint = Right(BeforePoint, nNumLen - 3);
                }
                else
                {
                    CurString = Left(BeforePoint, (nNumLen % 3));
                    BeforePoint = Right(BeforePoint, nNumLen - (nNumLen % 3));
                }
                nBit = Len(BeforePoint) / 3;
                tmpStr = DecodeHundred(CurString);
                if ((BeforePoint == Len(BeforePoint).ToString() || nBit == 0) && Len(CurString) == 3)
                {
                    if (System.Convert.ToInt32(Left(CurString, 1)) != 0 & System.Convert.ToInt32(Right(CurString, 2)) != 0)
                    {
                        //tmpStr = Left(tmpStr, tmpStr.IndexOf(Unit[3]) + Len(Unit[3])) + Unit[7] + " " + Right(tmpStr, Len(tmpStr) - (tmpStr.IndexOf(Unit[3]) + Len(Unit[3])));
                        tmpStr = Left(tmpStr, tmpStr.IndexOf(Unit[3]) + Len(Unit[3])) + " " + Right(tmpStr, Len(tmpStr) - (tmpStr.IndexOf(Unit[3]) + Len(Unit[3])));
                    }
                    else
                    {
                        tmpStr = " " + tmpStr;
                    }
                }
                if (nBit == 0)
                {
                    Str = Convert.ToString(Str + " " + tmpStr).Trim();
                }
                else
                {
                    Str = Convert.ToString(Str + " " + tmpStr + " " + Unit[nBit - 1]).Trim();
                }
                if (Left(Str, 3) == Unit[7])
                {
                    Str = Convert.ToString(Right(Str, Len(Str) - 3)).Trim();
                }
                if (BeforePoint == Len(BeforePoint).ToString())
                {
                    return "";
                }
            }
            BeforePoint = Str;
            if (Len(AfterPoint) > 0)
            {
                //nnn Dollars And xxx Cents
                AfterPoint = Unit[8] + " " + Unit[7] + " " + DecodeHundred(AfterPoint) + " " + Unit[6];
            }
            else
            {
                //nnn Dollars Only
                AfterPoint = Unit[4];
            }

            return BeforePoint + " " + AfterPoint;
        }

        #endregion

        #region Init
        private void Init()
        {
            if (StrNO[0] != "One")
            {
                StrNO[0] = "One";
                StrNO[1] = "Two";
                StrNO[2] = "Three";
                StrNO[3] = "Four";
                StrNO[4] = "Five";
                StrNO[5] = "Six";
                StrNO[6] = "Seven";
                StrNO[7] = "Eight";
                StrNO[8] = "Nine";
                StrNO[9] = "Ten";
                StrNO[10] = "Eleven";
                StrNO[11] = "Twelve";
                StrNO[12] = "Thirteen";
                StrNO[13] = "Fourteen";
                StrNO[14] = "Fifteen";
                StrNO[15] = "Sixteen";
                StrNO[16] = "Seventeen";
                StrNO[17] = "Eighteen";
                StrNO[18] = "Nineteen";
                StrTens[0] = "Ten";
                StrTens[1] = "Twenty";
                StrTens[2] = "Thirty";
                StrTens[3] = "Forty";
                StrTens[4] = "Fifty";
                StrTens[5] = "Sixty";
                StrTens[6] = "Seventy";
                StrTens[7] = "Eighty";
                StrTens[8] = "Ninety";
                Unit[0] = "Thousand";
                Unit[1] = "Million";
                Unit[2] = "Billion";
                Unit[3] = "Hundred";
                Unit[4] = "Dollars Only";
                Unit[5] = "Point";
                Unit[6] = "Cents";
                Unit[7] = "And";
                Unit[8] = "Dollars";
            }
        }

        #endregion

        #region DecodeHundred
        private string DecodeHundred(string HundredString)
        {
            int tmp;
            string rtn = "";
            if (Len(HundredString) > 0 && Len(HundredString) <= 3)
            {
                switch (Len(HundredString))
                {
                    case 1:
                        tmp = System.Convert.ToInt32(HundredString);
                        if (tmp != 0)
                        {
                            rtn = StrNO[tmp - 1].ToString();
                        }
                        break;
                    case 2:
                        tmp = System.Convert.ToInt32(HundredString);
                        if (tmp != 0)
                        {
                            if ((tmp < 20))
                            {
                                rtn = StrNO[tmp - 1].ToString();
                            }
                            else
                            {
                                if (System.Convert.ToInt32(Right(HundredString, 1)) == 0)
                                {
                                    rtn = StrTens[Convert.ToInt32(tmp / 10) - 1].ToString();
                                }
                                else
                                {
                                    rtn = Convert.ToString(StrTens[Convert.ToInt32(tmp / 10) - 1] + " " + StrNO[System.Convert.ToInt32(Right(HundredString, 1)) - 1]);
                                }
                            }
                        }
                        break;
                    case 3:
                        if (System.Convert.ToInt32(Left(HundredString, 1)) != 0)
                        {
                            rtn = Convert.ToString(StrNO[System.Convert.ToInt32(Left(HundredString, 1)) - 1] + " " + Unit[3] + " " + DecodeHundred(Right(HundredString, 2)));
                        }
                        else
                        {
                            rtn = DecodeHundred(Right(HundredString, 2)).ToString();
                        }
                        break;
                    default:
                        break;
                }
            }
            return rtn;
        }

        #endregion

        #region Left
        private string Left(string str, int n)
        {
            return str.Substring(0, n);
        }
        #endregion

        #region Right
        private string Right(string str, int n)
        {
            return str.Substring(str.Length - n, n);
        }
        #endregion

        #region Len
        private int Len(string str)
        {
            return str.Length;
        }
        #endregion
    }
    #endregion


    public class MoneyENGHelper
    {
        

        public static string GetNumberName(string p_strNumber)
        {
            if (string.IsNullOrEmpty(p_strNumber))
            {
                return "";
            }
            else
            {
                return new Number2English().NumberToString(Utils.NvDouble(p_strNumber)).ToUpper();
            }

            //string[] v_strNumber = p_strNumber.Split('.');
            //string v_strIntNumberName = "", v_strDecNumberName = "";

            //for (int i = 0; i < v_strNumber.Length; i++)
            //{
            //    if (i == 0)
            //        v_strIntNumberName = GetIntNumberName(v_strNumber[i].ToString());
            //    if (i == 1)
            //        v_strDecNumberName = GetDecNumberName(v_strNumber[i].ToString());

            //}
            //if (v_strIntNumberName.Trim() != "")
            //{
            //    if (v_strDecNumberName == "")
            //        return v_strIntNumberName.Trim() + " DOLLAR ONLY ";
            //    return v_strIntNumberName.Trim() + " AND " + v_strDecNumberName.Trim() + " CENTS ";
            //}
            //else
            //    if (v_strDecNumberName != "")
            //        return v_strDecNumberName.Trim() + " CENTS ";
            //return "";
        }


        private static string GetIntNumberName(string p_strIntNumberName)
        {
            string v_intNumberName = GetInvertingNumberName(p_strIntNumberName);
            string v_strBillion = "";
            string v_strMillion = "";
            string v_strThousand = "";
            string v_strHundred = "";
            string v_strBillionNumberName = "";
            string v_strMillionNumberName = "";
            string v_strThousandNumberName = "";
            string v_strHundredNumberName = "";

            for (int i = 0; i < v_intNumberName.Length; i++)
            {
                if (i <= 2)
                    v_strHundred = v_strHundred + v_intNumberName[i].ToString();
                if (i > 2 && i <= 5)
                    v_strThousand = v_strThousand + v_intNumberName[i].ToString();
                if (i > 5 && i <= 8)
                    v_strMillion = v_strMillion + v_intNumberName[i].ToString();
                if (i > 8)
                    v_strBillion = v_strBillion + v_intNumberName[i].ToString();
            }

            if (v_strBillion.Length > 0)
                v_strBillionNumberName = GetBillionNumberName(GetInvertingNumberName(v_strBillion));
            if (v_strMillion.Length > 0)
                v_strMillionNumberName = GetMillionNumberName(GetInvertingNumberName(v_strMillion));
            if (v_strThousand.Length > 0)
                v_strThousandNumberName = GetThousandNumberName(GetInvertingNumberName(v_strThousand));
            if (v_strHundred.Length > 0)
                v_strHundredNumberName = GetHundredNumberName(GetInvertingNumberName(v_strHundred));
            return v_strBillionNumberName.Trim() + ' ' + v_strMillionNumberName.Trim() + ' ' + v_strThousandNumberName.Trim() + ' ' + v_strHundredNumberName.Trim();
        }


        private static string GetDecNumberName(string p_strDecNamberName)
        {
            string v_strDecNumberName = "";
            if (p_strDecNamberName.Length == 1)
                p_strDecNamberName = p_strDecNamberName + "0";
            for (int i = 0; i < p_strDecNamberName.Length; i++)
            {
                if (i == 0 && Convert.ToInt32(p_strDecNamberName[i].ToString()) > 1)
                    v_strDecNumberName = v_strDecNumberName + GetEngNumberName(Convert.ToInt32(p_strDecNamberName[i].ToString()) * 10);
                if (i == 1 && Convert.ToInt32(p_strDecNamberName[i].ToString()) >= 0)
                    if (Convert.ToInt32(p_strDecNamberName[0].ToString()) == 1)
                        v_strDecNumberName = v_strDecNumberName + " " + GetEngNumberName(Convert.ToInt32(p_strDecNamberName[i - 1].ToString()) * 10 + Convert.ToInt32(p_strDecNamberName[i].ToString()));
                    else
                        v_strDecNumberName = v_strDecNumberName + " " + GetEngNumberName(Convert.ToInt32(p_strDecNamberName[i].ToString()));
            }
            return v_strDecNumberName;
        }

        private static string GetBillionNumberName(string p_strBillionNumberName)
        {
            string v_strBillion = "";
            string v_strMillion = "";
            string v_strThousand = "";
            string v_strHundred = "";
            string v_strBillionNumberName = "";
            string v_strMillionNumberName = "";
            string v_strThousandNumberName = "";
            string v_strHundredNumberName = "";

            for (int i = 0; i < p_strBillionNumberName.Length; i++)
            {
                if (i <= 2)
                    v_strHundred = v_strHundred + p_strBillionNumberName[i].ToString();
                if (i > 2 && i <= 5)
                    v_strThousand = v_strThousand + p_strBillionNumberName[i].ToString();
                if (i > 5 && i <= 8)
                    v_strMillion = v_strMillion + p_strBillionNumberName[i].ToString();
                if (i > 8)
                    v_strBillion = v_strBillion + p_strBillionNumberName[i].ToString();
            }

            if (v_strMillion.Length > 0)
                v_strMillionNumberName = GetMillionNumberName(GetInvertingNumberName(v_strMillion));
            if (v_strThousand.Length > 0)
                v_strThousandNumberName = GetThousandNumberName(GetInvertingNumberName(v_strThousand));
            if (v_strHundred.Length > 0)
                v_strHundredNumberName = GetHundredNumberName(GetInvertingNumberName(v_strHundred));
            return v_strBillionNumberName = v_strBillionNumberName + v_strMillionNumberName + v_strThousandNumberName + v_strHundredNumberName + " BiLLION ";
        }

        private static string GetMillionNumberName(string p_strMillionNumberName)
        {
            string v_strMillion = p_strMillionNumberName;
            string v_strMillionNumberName = "";
            if (v_strMillion.Length == 1)
                v_strMillion = "00" + v_strMillion;
            if (v_strMillion.Length == 2)
                v_strMillion = "0" + v_strMillion;
            for (int i = 0; i < v_strMillion.Length; i++)
            {
                if (i == 0 && Convert.ToInt32(v_strMillion[i].ToString()) > 0)
                    v_strMillionNumberName = v_strMillionNumberName + GetEngNumberName(Convert.ToInt32(v_strMillion[i].ToString())) + " HUNDRED ";
                if (i == 1 && Convert.ToInt32(v_strMillion[i].ToString()) > 1)
                    v_strMillionNumberName = v_strMillionNumberName + GetEngNumberName(Convert.ToInt32(v_strMillion[i].ToString()) * 10);
                if (i == 2 && Convert.ToInt32(v_strMillion[i].ToString()) > 0)
                    if (Convert.ToInt32(v_strMillion[1].ToString()) == 1)
                        v_strMillionNumberName = v_strMillionNumberName + " " + GetEngNumberName(Convert.ToInt32(v_strMillion[i].ToString()) + 10);
                    else
                        v_strMillionNumberName = v_strMillionNumberName + " " + GetEngNumberName(Convert.ToInt32(v_strMillion[i].ToString()));
            }
            if (v_strMillionNumberName == "")
                return v_strMillionNumberName;
            return v_strMillionNumberName = v_strMillionNumberName + " MILLION ";

        }
        private static string GetThousandNumberName(string p_strThousandNumberName)
        {
            string v_strThousand = p_strThousandNumberName;
            string v_strThousandNumberName = "";
            if (v_strThousand.Length == 1)
                v_strThousand = "00" + v_strThousand;
            if (v_strThousand.Length == 2)
                v_strThousand = "0" + v_strThousand;
            for (int i = 0; i < v_strThousand.Length; i++)
            {
                if (i == 0 && Convert.ToInt32(v_strThousand[i].ToString()) > 0)
                    v_strThousandNumberName = v_strThousandNumberName + GetEngNumberName(Convert.ToInt32(v_strThousand[i].ToString())) + " HUNDRED ";
                if (i == 1 && Convert.ToInt32(v_strThousand[i].ToString()) > 1)
                    v_strThousandNumberName = v_strThousandNumberName + GetEngNumberName(Convert.ToInt32(v_strThousand[i].ToString()) * 10);
                if (i == 2 && Convert.ToInt32(v_strThousand[i].ToString()) > 0)
                    if (Convert.ToInt32(v_strThousand[1].ToString()) == 1)
                        v_strThousandNumberName = v_strThousandNumberName + " " + GetEngNumberName(Convert.ToInt32(v_strThousand[i].ToString()) + 10);
                    else
                        v_strThousandNumberName = v_strThousandNumberName + " " + GetEngNumberName(Convert.ToInt32(v_strThousand[i].ToString()));
            }
            if (v_strThousandNumberName == "")
                return v_strThousandNumberName;
            return v_strThousandNumberName = v_strThousandNumberName + " THOUSAND ";
        }
        private static string GetHundredNumberName(string p_strHundredNumberName)
        {
            string v_strHundred = p_strHundredNumberName;
            string v_strHundredNumberName = "";
            if (v_strHundred.Length == 1)
                v_strHundred = "00" + v_strHundred;
            if (v_strHundred.Length == 2)
                v_strHundred = "0" + v_strHundred;
            for (int i = 0; i < v_strHundred.Length; i++)
            {
                if (i == 0 && Convert.ToInt32(v_strHundred[i].ToString()) > 0)
                    v_strHundredNumberName = v_strHundredNumberName + GetEngNumberName(Convert.ToInt32(v_strHundred[i].ToString())) + " HUNDRED ";
                if (i == 1 && Convert.ToInt32(v_strHundred[i].ToString()) > 1)
                    v_strHundredNumberName = v_strHundredNumberName + GetEngNumberName(Convert.ToInt32(v_strHundred[i].ToString()) * 10);
                if (i == 2 && Convert.ToInt32(v_strHundred[i].ToString()) > 0)
                    if (Convert.ToInt32(v_strHundred[1].ToString()) == 1)
                        v_strHundredNumberName = v_strHundredNumberName + " " + GetEngNumberName(Convert.ToInt32(v_strHundred[i].ToString()) + 10);
                    else
                        v_strHundredNumberName = v_strHundredNumberName + " " + GetEngNumberName(Convert.ToInt32(v_strHundred[i].ToString()));
            }
            return v_strHundredNumberName;
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
                case 1:
                    v_strEngNumberName = "ONE";
                    break;
                case 2:
                    v_strEngNumberName = "TWO";
                    break;
                case 3:
                    v_strEngNumberName = "THREE";
                    break;
                case 4:
                    v_strEngNumberName = "FOUR";
                    break;
                case 5:
                    v_strEngNumberName = "FIVE";
                    break;
                case 6:
                    v_strEngNumberName = "SIX";
                    break;
                case 7:
                    v_strEngNumberName = "SEVEN";
                    break;
                case 8:
                    v_strEngNumberName = "EIGHT";
                    break;
                case 9:
                    v_strEngNumberName = "NINE";
                    break;
                case 10:
                    v_strEngNumberName = "TEN";
                    break;
                case 11:
                    v_strEngNumberName = "ELEVEN";
                    break;
                case 12:
                    v_strEngNumberName = "TWELVE";
                    break;
                case 13:
                    v_strEngNumberName = "THIRTEEN";
                    break;
                case 14:
                    v_strEngNumberName = "FOURTEEN";
                    break;
                case 15:
                    v_strEngNumberName = "FIFTEEN";
                    break;
                case 16:
                    v_strEngNumberName = "SIXTEEN";
                    break;
                case 17:
                    v_strEngNumberName = "SEVENTEEN";
                    break;
                case 18:
                    v_strEngNumberName = "EIGHTEEN";
                    break;
                case 19:
                    v_strEngNumberName = "NINETEEN";
                    break;
                case 20:
                    v_strEngNumberName = "TWENTY";
                    break;
                case 30:
                    v_strEngNumberName = "THIRTY";
                    break;
                case 40:
                    v_strEngNumberName = "FORTY";
                    break;
                case 50:
                    v_strEngNumberName = "FIFTY";
                    break;
                case 60:
                    v_strEngNumberName = "SIXTY";
                    break;
                case 70:
                    v_strEngNumberName = "SEVENTY";
                    break;
                case 80:
                    v_strEngNumberName = "EIGHTY";
                    break;
                case 90:
                    v_strEngNumberName = "NINETY";
                    break;
            }
            return v_strEngNumberName;
        }


    }
}
