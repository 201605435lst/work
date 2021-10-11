using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace SnAbp.StdBasic.Services
{
    public class StringUtil
    {
        #region IP操作
        /// <summary>
        /// 验证一个字符串是否是IP地址
        /// </summary>
        /// <param name="strValue">The string value.</param>
        /// <returns></returns>
        public static bool CheckIP(string strValue)
        {

            if (strValue.Length > 16)
            {
                return false;
            }
            string[] strSplit = strValue.Split('.');
            if (strSplit.Length == 4)
            {
                bool blnChecked = CheckNumber(strSplit[0], 1, 255);
                if (blnChecked)
                {
                    for (int i = 1; i < 4; i++)
                    {
                        blnChecked = CheckNumber(strSplit[i], 0, 255);
                        if (!blnChecked)
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 判断一个字符串 是否介于某个数值范围之间
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="intMin"></param>
        /// <param name="intMax"></param>
        /// <returns></returns>
        private static bool CheckNumber(string strValue, int intMin, int intMax)
        {
            bool blnRtn = CheckSignedNumericValidity(strValue);
            if (blnRtn)
            {
                int intTemp = Convert.ToInt32(strValue);
                if (intTemp <= intMax && intTemp >= intMin)
                {
                    blnRtn = true;
                }
                else
                {
                    return false;
                }
            }
            return blnRtn;
        }

        /// <summary>
        /// 获取本机IP
        /// </summary>
        /// <returns></returns>
        public static string getLocalIP()
        {
            string strHostName = Dns.GetHostName(); //得到本机的主机名
            IPHostEntry ipEntry = Dns.GetHostEntry(strHostName); //取得本机IP
            string strAddr = ipEntry.AddressList[0].ToString();
            return (strAddr);
        }
        #endregion

        #region 字符串验证
        /// <summary>
        /// 判断字符串与条件是否匹配
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="regx">The pattern.</param>
        /// <returns></returns>
        public static bool IsMatch(string input, Regex regx)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }
            return regx.IsMatch(input);
        }

        /// <summary>
        /// 判断是否数字 0～9，正数或负数
        /// </summary>
        /// <param name="strValue">要判断的字符串</param>
        /// <returns>如果是数字返回 true 否则返回 false </returns>
        public static bool CheckSignedNumericValidity(string strValue)
        {
            Regex regx = new Regex(@"^[\-]?[0-9]*[\.]?[0-9]*$");
            return IsMatch(strValue, regx);
        }

        /// <summary>
        /// 判断是否数字 0～9
        /// </summary>
        /// <param name="strValue">要判断的字符串</param>
        /// <returns>如果是数字返回 true 否则返回 false </returns>
        public static bool CheckUnsignedNumericValidity(string strValue)
        {
            Regex regx = new Regex(@"^[0-9]*$");
            return IsMatch(strValue, regx);
        }

        /// <summary>
        /// 判断是否大小写字母
        /// </summary>
        /// <param name="strValue">要判断的字符串</param>
        /// <returns>如果是大小写字母返回 true 否则返回 false </returns>
        public static bool CheckalphabetValidity(string strValue)
        {
            Regex regx = new Regex(@"^[a-zA-Z]*$");
            return IsMatch(strValue, regx);
        }

        /// <summary>
        /// 判断字符串是否为Int32整数
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool CheckInt32Validity(string Value)
        {
            try
            {
                Convert.ToInt32(Value);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断是否为自定义名称（即包含 汉字、字母、数字、横线、下划线、中文括号、英文括号、点）
        /// </summary>
        /// <param name="strValue">要判断的字符串</param>
        /// <returns>如果是数字返回 true 否则返回 false </returns>
        public static bool CheckCustomerNameValidity(string strValue)
        {
            Regex regx = new Regex(@"^[a-zA-Z0-9|\u4e00-\u9fa5][-|a-zA-Z0-9|\u4e00-\u9fa5|_|(|)|（|）|.]*$");
            return IsMatch(strValue, regx);
        }

        /// <summary>
        /// 检查是否有汉字
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool CheckCodeValidity(string strValue)
        {
            Regex regx = new Regex(@"^[\u4e00-\u9fa5]+");
            return !IsMatch(strValue, regx);
        }

        /// <summary>
        /// 检查是否只包含数字
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool CheckNumberValidity(string strValue)
        {
            Regex regx = new Regex(@"^\d+$");
            return IsMatch(strValue, regx);
        }

        /// <summary>
        /// 检查字符串是否为GUID格式
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool CheckGUIDValidity(string strValue)
        {
            Regex regx = new Regex(@"^(\w|\-){36}$");
            return IsMatch(strValue, regx);
        }

        /// <summary>
        /// 检查字符串是否未含有空格
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool CheckSpaceValidity(string strValue)
        {
            int b = strValue.IndexOf(" ");
            if (b != -1)
            {
               
                return false;
            }
            return true;
        }

        /// <summary>
        /// 判断一个字符串是否 是"yyyy-MM-dd"的日期格式
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool CheckDateValidity(string strValue)
        {
            if (strValue.Length != 10)
            {
                return false;
            }
            string[] strSplit = strValue.Split('-');
            if (strSplit.Length != 3)
            {
                return false;
            }
            try
            {
                Convert.ToDateTime(strValue);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查给定的字符串是否是有效的电子邮件地址
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool CheckEMailValidity(string strValue)
        {
            Regex regx = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
            return IsMatch(strValue, regx);
        }

        /// <summary>
        /// 检查给定的字符串是否是有效的身份证号
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool CheckIDCardValidity(string strValue)
        {
            int intCid = strValue.Length;
            if (intCid != 15)
            {
                double dbiSum = 0;
                System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{17}(\d|x)$");
                System.Text.RegularExpressions.Match mc = rg.Match(strValue);
                if (!mc.Success)
                {
                    return false;
                }
                strValue = strValue.ToLower();
                strValue = strValue.Replace("x", "a");

                try
                {
                    DateTime.Parse(strValue.Substring(6, 4) + "-" + strValue.Substring(10, 2) + "-" + strValue.Substring(12, 2));
                }
                catch
                {
                    return false;
                }
                for (int i = 17; i >= 0; i--)
                {
                    dbiSum += (System.Math.Pow(2, i) % 11) * int.Parse(strValue[17 - i].ToString().Trim(), System.Globalization.NumberStyles.HexNumber);

                }
                if (dbiSum % 11 != 1)
                {
                    return false;
                }

                return true;
            }
            else
            {
                string ereg;
                System.Text.RegularExpressions.Regex rg = new System.Text.RegularExpressions.Regex(@"^\d{15}");
                System.Text.RegularExpressions.Match mc = rg.Match(strValue);
                if (!mc.Success)
                {
                    return false;
                }
                else
                {

                    if ((Convert.ToInt32(strValue.Substring(6, 2)) + 1900) % 4 == 0 || ((Convert.ToInt32(strValue.Substring(6, 2)) + 1900) % 100 == 0 && (Convert.ToInt32(strValue.Substring(6, 2)) + 1900) % 4 == 0))
                    {
                        ereg = @"^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|[1-2][0-9]))[0-9]{3}$";//测试出生日期的合法性 
                    }
                    else
                    {
                        ereg = @"^[1-9][0-9]{5}[0-9]{2}((01|03|05|07|08|10|12)(0[1-9]|[1-2][0-9]|3[0-1])|(04|06|09|11)(0[1-9]|[1-2][0-9]|30)|02(0[1-9]|1[0-9]|2[0-8]))[0-9]{3}$";//测试出生日期的合法性 
                    }
                    if (Regex.IsMatch(strValue, ereg))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }

        /// <summary>
        /// 检查给定的字符串是否是有效的手机号码
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool CheckCellphoneValidity(string strValue)
        {
            string strMobi = strValue;
            System.Text.RegularExpressions.Regex rgg = new System.Text.RegularExpressions.Regex(@"^\d{11}$");
            System.Text.RegularExpressions.Match mcc = rgg.Match(strValue);
            if (!mcc.Success)
            {
                return false;
            }
            if (strMobi.Substring(0, 1) != "1")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查给定的字符是否是有效的座机号码
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool CheckTelephoneValidity(string strValue)
        {
            //如：010-88486488,010-88486488-88(888 ,8888)
            //  0731-8848648,0512-88486488,0731-8848648-88(888,8888)
            //8612-12456678-1234

            Regex regx = new Regex(@"(^0\d{2,3}-\d{7,8}$)|(^0\d{2,3}-\d{7,8}-(\d{2,3}|\d{3,4})$)|(^\d{4}-\d{7,8}-\d{3,4}$)");
            return IsMatch(strValue, regx);
        }

        /// <summary>
        /// 检查给定的字符串是否是有效的车牌号
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool CheckCarNumberValidity(string strValue)
        {
            Regex regx = new Regex(@"^[\u4E00-\u9FA5]{1}[A-Z]{1}[A-Z0-9]{5}$");
            return IsMatch(strValue, regx);
        }

        /// <summary>
        /// 检查给定字符串是否是有效的文件夹名称
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool CheckDirectoryNameValidity(string strValue)
        {
            if (strValue == null || strValue.Trim() == string.Empty)
            {
                return false;
            }

            if (strValue.StartsWith(".") || strValue.EndsWith("."))
            {
                return false;
            }

            char[] charString = new char[11] { '\\', '/', ':', '*', '?', '%', '"', '<', '>', '|', '\'' };
            for (int i = 0; i < charString.Length; i++)
            {
                if (strValue.IndexOf(charString[i]) != -1)
                {
                    return false;
                }
            }

            return true;

        }

        /// <summary>
        /// 检查给定字符串是否是有效的文件名称
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool CheckFileNameValidity(string strValue)
        {
            if (strValue == null || strValue.Trim() == string.Empty)
            {
                return false;
            }
            char[] charString = new char[10] { '\\', '/', ':', '*', '?', '"', '<', '>', '|', '\'' };
            for (int i = 0; i < charString.Length; i++)
            {
                if (strValue.IndexOf(charString[i]) != -1)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 字符串其他操作
        /// <summary>
        /// 按指定的长度截取字符串
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="intLength"></param>
        /// <returns></returns>
        public static string InterceptString(string strValue, int intLength)
        {
            if (strValue == null)
            {
                return null;
            }
            if (strValue.Length <= intLength)
            {
                return strValue;
            }
            return strValue.Substring(0, intLength);
        }

        /// <summary>
        /// 检查当前字符串中是否有中文字符
        /// </summary>
        /// <param name="strValue"></param>
        /// <returns></returns>
        public static bool ContainCHS(string strValue)
        {
            bool blnRtn = false;

            for (int i = 0; i < strValue.Length; i++)
            {
                int intRtn = Encoding.Default.GetByteCount(strValue[i].ToString().Trim());
                if (intRtn > 1)
                {
                    blnRtn = true;
                }
            }
            return blnRtn;
        }

        /// <summary>
        /// 获取汉字的拼音简码
        /// </summary>
        /// <param name="text">汉字</param>
        /// <returns></returns>
        public static string GetSpellCode(string text)
        {
            StringBuilder myStr = new StringBuilder();
            char[] txtChars = text.ToCharArray();
            foreach (char c in txtChars)
            {
                byte[] arrCN = Encoding.Default.GetBytes(c.ToString());
                if (arrCN.Length > 1)
                {
                    int area = (short)arrCN[0];
                    int pos = (short)arrCN[1];
                    int code = (area << 8) + pos;
                    int[] areacode = { 45217, 45253, 45761, 46318, 46826, 47010, 47297, 47614, 48119, 48119, 49062, 49324, 49896, 50371, 50614, 50622, 50906, 51387, 51446, 52218, 52698, 52698, 52698, 52980, 53689, 54481 };
                    for (int j = 0; j < 26; j++)
                    {
                        int max = 55290;
                        if (j != 25) max = areacode[j + 1];
                        if (areacode[j] <= code && code < max)
                        {
                            myStr.Append(Encoding.Default.GetString(new byte[] { (byte)(65 + j) }));
                        }
                    }
                }
                else
                {
                    myStr.Append(c);
                }
            }
            return myStr.ToString();
        }

        /// <summary>
        /// 文件大小转换成KB和MB
        /// </summary>
        /// <param name="strSize"></param>
        /// <returns></returns>
        public static string ConvertSizeFromLong(string strSize)
        {
            if (strSize.Length == 0)
            {
                return "";
            }
            string strRtn = string.Empty;
            string strFlag = string.Empty;
            double dSize = double.Parse(strSize);
            double dRemainSize = 0;
            int intTemp = (int)dSize;
            if (intTemp != 0)
            {
                strFlag = "B";
                dRemainSize = dSize;
                intTemp = (int)(dSize / 1024);
                if (intTemp != 0)
                {
                    strFlag = "K";
                    dRemainSize = dSize / 1024;
                    intTemp = (int)(dSize / (1024 * 1024));
                    if (intTemp != 0)
                    {
                        strFlag = "M";
                        dRemainSize = dSize / (1024 * 1024);
                        intTemp = (int)(dSize / (1024 * 1024 * 1024));
                        if (intTemp != 0)
                        {
                            strFlag = "G";
                            dRemainSize = dSize / (1024 * 1024 * 1024);
                        }
                    }
                }
            }

            dRemainSize = Math.Round(dRemainSize, 2);
            strRtn = dRemainSize.ToString().Trim();
            if (strFlag != "B" && strRtn.IndexOf(".") == -1)
            {
                strRtn += ".00";
            }

            switch (strFlag)
            {
                case "B":
                    strRtn = strRtn + " 字节";
                    break;
                case "K":
                    strRtn = strRtn + " KB";
                    break;
                case "M":
                    strRtn = strRtn + " MB";
                    break;
                case "G":
                    strRtn = strRtn + " GB";
                    break;
                default:
                    break;
            }

            return strRtn;
        }

        /// 获取起始字符和终止字符中间的值
        /// <summary>
        /// 获取起始字符和终止字符中间的值
        /// </summary>
        /// <param name="target">字符串</param>
        /// <param name="start">开始</param>
        /// <param name="end">结束</param>
        /// <returns></returns> 
        public static string[] GetMidValue(string target, string start, string end)
        {
            string pattern = String.Format("(?<={0}).[^{1}]*(?={2})", Regex.Escape(start), Regex.Escape(end), Regex.Escape(end));
            Regex reg = new Regex(pattern, RegexOptions.Multiline | RegexOptions.Singleline);
            MatchCollection mc = reg.Matches(target);
            List<string> segs = new List<string>();
            IEnumerator itor = mc.GetEnumerator();
            while (itor.MoveNext())
            {
                if (itor.Current != null)
                {
                    segs.Add(itor.Current.ToString());
                }
            }
            return segs.ToArray();
        }
        #endregion
    }
}

