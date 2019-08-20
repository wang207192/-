using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MaterialEvaluationCal.Common
{
    public class Methods : BaseData
    {
        #region 获取标准数据
        ///// <summary>
        ///// 获取标准表数据   Table[JYDBH=123&SYRQ=].Field
        ///// </summary>
        ///// <param name="format">参数</param>
        ///// <param name="condition">条件</param>
        ///// <returns></returns>
        //private string GetExtraData(string format, string condition)
        //{
        //    var s = format.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
        //    if (s.Length != 2)
        //    {
        //        throw new Exception(format + "格式错误");
        //    }

        //}
        /// <summary>
        /// 获取标准表数据
        /// </summary>
        /// <param name="format">参数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        private string GetExtraData(string format, Func<IDictionary<string, string>, bool> condition)
        {
            var s = format.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != 2)
            {
                throw new Exception(format + "格式错误");
            }
            if (!dataExtraTmp.ContainsKey(s[0]))
            {
                throw new Exception(s[0] + "表数据不存在");
            }
            var tableData = dataExtraTmp[s[0]];
            var fieldData = tableData.FirstOrDefault(condition);
            if (fieldData != null && !fieldData.ContainsKey(s[1]))
            {
                throw new Exception(s[0] + "表不存在" + s[1] + "字段");
            }
            if (fieldData != null)
            {
                return fieldData[s[1]].Trim();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取标准参数数量
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private int GetExtraDataCount(string table)
        {
            if (!dataExtraTmp.ContainsKey(table))
            {
                throw new Exception(table + "表数据不存在");
            }
            var tableData = dataExtraTmp[table];
            return tableData.Count;
        }

        /// <summary>
        /// 获取标准表数据   Table[JYDBH=123&SYRQ=].Field
        /// </summary>
        /// <param name="format">参数</param>
        /// <param name="condition">序列</param>
        /// <returns></returns>
        private string GetExtraData(string format, int condition = 0)
        {
            var s = format.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != 2)
            {
                throw new Exception(format + "格式错误");
            }
            if (!dataExtraTmp.ContainsKey(s[0]))
            {
                throw new Exception(s[0] + "表数据不存在");
            }
            var tableData = dataExtraTmp[s[0]];
            if (tableData.Count > condition)
            {
                var fieldData = tableData[condition];
                if (!fieldData.ContainsKey(s[1]))
                {
                    throw new Exception(s[0] + "表不存在" + s[1] + "字段");
                }
                return fieldData[s[1]].Trim();
            }
            else
            {
                throw new Exception("序号:" + condition + ";超过表存在的条数");
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="format"></param>
        /// <param name="t">0：字符串（默认）；1：数字；2：日期</param>
        /// <param name="asc"></param>
        private void SortExtraData(string format, int t = 0, bool asc = true)
        {
            var s = format.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (s.Length != 2)
            {
                throw new Exception(format + "格式错误");
            }
            if (!dataExtraTmp.ContainsKey(s[0]))
            {
                throw new Exception(s[0] + "表数据不存在");
            }
            var tableData = dataExtraTmp[s[0]];
            if (tableData.Count > 0)
            {
                var fieldData = tableData[0];
                if (!fieldData.ContainsKey(s[1]))
                {
                    throw new Exception(s[0] + "表不存在" + s[1] + "字段");
                }
                if (asc)
                {
                    if (t == 1)
                    {
                        tableData = tableData.OrderBy(x => Convert.ToDouble(x[s[1]])).ToList();
                    }
                    else if (t == 2)
                    {
                        tableData = tableData.OrderBy(x => Convert.ToDateTime(x[s[1]])).ToList();
                    }
                    else
                    {
                        tableData = tableData.OrderBy(x => x[s[1]]).ToList();
                    }
                }
                else
                {
                    if (t == 1)
                    {
                        tableData = tableData.OrderByDescending(x => Convert.ToDouble(x[s[1]])).ToList();
                    }
                    else if (t == 2)
                    {
                        tableData = tableData.OrderByDescending(x => Convert.ToDateTime(x[s[1]])).ToList();
                    }
                    else
                    {
                        tableData = tableData.OrderByDescending(x => x[s[1]]).ToList();
                    }
                }
                dataExtraTmp[s[0]] = tableData;
            }
        }
        #endregion

        #region 获取上传数据

        /// <summary>
        /// 获取上传表数据
        /// </summary>
        /// <param name="format">参数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public string GetData(string format, Func<IDictionary<string, string>, bool> condition)
        {
            var s = format.Split(new char[] { '.' });
            if (s.Length != 3)
            {
                throw new Exception(format + "格式错误");
            }
            if (!retDataTmp.ContainsKey(s[0]))
            {
                throw new Exception(s[0] + "检测项目不存在");
            }
            var jcxmData = retDataTmp[s[0]];

            if (!jcxmData.ContainsKey(s[1]))
            {
                throw new Exception(s[1] + "表数据不存在");
            }

            var tableData = jcxmData[s[1]];

            var fieldData = tableData.FirstOrDefault(condition);
            if (fieldData != null && !fieldData.ContainsKey(s[2]))
            {
                throw new Exception(s[1] + "表不存在" + s[2] + "字段");
            }
            if (fieldData != null)
            {
                return fieldData[s[2]].Trim();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取上传表数据
        /// </summary>
        /// <param name="format">参数</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public int GetDataCount(string format)
        {
            var s = format.Split(new char[] { '.' });
            if (s.Length != 2)
            {
                throw new Exception(format + "格式错误");
            }
            if (!retDataTmp.ContainsKey(s[0]))
            {
                throw new Exception(s[0] + "检测项目不存在");
            }
            var jcxmData = retDataTmp[s[0]];

            if (!jcxmData.ContainsKey(s[1]))
            {
                throw new Exception(s[1] + "表数据不存在");
            }

            var tableData = jcxmData[s[1]];
            return tableData.Count;
        }

        /// <summary>
        /// 获取上传表数据
        /// </summary>
        /// <param name="format">参数</param>
        /// <param name="condition">序列</param>
        /// <returns></returns>
        public string GetData(string format, int condition = 0)
        {
            var s = format.Split(new char[] { '.' });
            if (s.Length != 3)
            {
                throw new Exception(format + "格式错误");
            }
            if (!retDataTmp.ContainsKey(s[0]))
            {
                throw new Exception(s[0] + "检测项目不存在");
            }
            var jcxmData = retDataTmp[s[0]];

            if (!jcxmData.ContainsKey(s[1]))
            {
                throw new Exception(s[1] + "表数据不存在");
            }

            var tableData = jcxmData[s[1]];
            if (tableData.Count > condition)
            {
                var fieldData = tableData[condition];
                if (!fieldData.ContainsKey(s[2]))
                {
                    throw new Exception(s[1] + "表不存在" + s[2] + "字段");
                }
                return fieldData[s[2]].Trim();
            }
            else
            {
                throw new Exception("序号:" + condition + ";超过表存在的条数");
            }
        }

        /// <summary>
        /// 设置上传表数据
        /// </summary>
        /// <param name="format">参数</param>
        /// <param name="condition">序列</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public void SetData(string format, string value, int condition = 0)
        {
            var s = format.Split(new char[] { '.' });
            if (s.Length != 3)
            {
                throw new Exception(format + "格式错误");
            }
            if (!retDataTmp.ContainsKey(s[0]))
            {
                throw new Exception(s[0] + "检测项目不存在");
            }
            var jcxmData = retDataTmp[s[0]];

            if (!jcxmData.ContainsKey(s[1]))
            {
                throw new Exception(s[1] + "表数据不存在");
            }

            var tableData = jcxmData[s[1]];
            if (tableData.Count > condition)
            {
                var fieldData = tableData[condition];
                if (!fieldData.ContainsKey(s[2]))
                {
                    fieldData.Add(s[2], value);
                }
                else
                {
                    fieldData[s[2]] = value;
                }
            }
            else
            {
                throw new Exception("序号:" + condition + ";超过表存在的条数");
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="format"></param>
        /// <param name="t">0：字符串（默认）；1：数字；2：日期</param>
        /// <param name="asc"></param>
        public void SortData(string format, int t = 0, bool asc = true)
        {
            var s = format.Split(new char[] { '.' });
            if (s.Length != 3)
            {
                throw new Exception(format + "格式错误");
            }
            if (!retDataTmp.ContainsKey(s[0]))
            {
                throw new Exception(s[0] + "检测项目不存在");
            }
            var jcxmData = retDataTmp[s[0]];

            if (!jcxmData.ContainsKey(s[1]))
            {
                throw new Exception(s[1] + "表数据不存在");
            }

            var tableData = jcxmData[s[1]];
            if (tableData.Count > 0)
            {
                var fieldData = tableData[0];
                if (!fieldData.ContainsKey(s[2]))
                {
                    throw new Exception(s[1] + "表不存在" + s[2] + "字段");
                }
                if (asc)
                {
                    if (t == 1)
                    {
                        tableData = tableData.OrderBy(x => Convert.ToDouble(x[s[2]])).ToList();
                    }
                    else if (t == 2)
                    {
                        tableData = tableData.OrderBy(x => Convert.ToDateTime(x[s[2]])).ToList();
                    }
                    else
                    {
                        tableData = tableData.OrderBy(x => x[s[2]]).ToList();
                    }
                }
                else
                {
                    if (t == 1)
                    {
                        tableData = tableData.OrderByDescending(x => Convert.ToDouble(x[s[2]])).ToList();
                    }
                    else if (t == 2)
                    {
                        tableData = tableData.OrderByDescending(x => Convert.ToDateTime(x[s[2]])).ToList();
                    }
                    else
                    {
                        tableData = tableData.OrderByDescending(x => x[s[2]]).ToList();
                    }
                }
                jcxmData[s[1]] = tableData;
            }
        }
        #endregion

        #region 数字
        /// <summary>
        /// 保留小数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        private double Round(double d, int digits)
        {
            return Math.Round(d, digits);
        }
        /// <summary>
        /// 强制保留小数
        /// </summary>
        /// <param name="d"></param>
        /// <param name="digits"></param>
        /// <returns></returns>
        private string RoundEx(double d, int digits)
        {
            var s = Math.Round(d, digits).ToString();
            if (!s.Contains("."))
            {
                s += ".";
                for (int i = 0; i < digits; i++)
                {
                    s += "0";
                }
                return s;
            }
            else
            {
                var ss = s.Split('.')[1];
                if (ss.Length < digits)
                {
                    for (int i = ss.Length; i < digits; i++)
                    {
                        s += "0";
                    }
                }
                return s;
            }
        }

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="asc">True 顺序 False倒叙</param>
        /// <param name="t">参数</param>
        /// <returns></returns>
        private List<T> GetSort<T>(bool asc = true, params T[] t)
        {
            List<T> s = new List<T>();
            for (int i = 0; i < t.Length; i++)
            {
                s.Add(t[i]);
            }
            if (asc)
                s = s.OrderBy(x => x).ToList();
            else
                s = s.OrderByDescending(x => x).ToList();
            return s;
        }

        /// <summary>
        /// 取中间值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private T GetMiddle<T>(params T[] t)
        {
            var s = GetSort(true, t);
            return s[(t.Length + 1) / 2 - 1];
        }
        /// <summary>
        /// 取最小值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private T GetMin<T>(params T[] t)
        {
            var s = GetSort(true, t);
            return s[0];
        }
        /// <summary>
        /// 取最大值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="t"></param>
        /// <returns></returns>
        private T GetMax<T>(params T[] t)
        {
            var s = GetSort(true, t);
            return s[s.Count - 1];
        }

        /// <summary>
        /// 获取数字
        /// </summary>
        /// <param name="s"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        private string GetNum(string s, string desc = "")
        {
            Regex reg = new Regex("\\d+\\.?\\d*");
            var match = reg.Match(s);
            if (match.Success)
            {
                return match.Value;
            }
            else
            {
                if (desc != "")
                    throw new Exception(s + "中未找到数字");
                else
                    return "";
            }
        }
        #endregion

        #region  共用函数
        /// <summary>
        /// 校验字符串是否为空
        /// </summary>
        /// <param name="s"></param>
        /// <param name="desc"></param>
        public static void CheckEmpty(string s, string desc)
        {
            if (String.IsNullOrEmpty(s))
            {
                throw new Exception(desc);
            }
        }

        /// <summary>
        /// 转成整型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int GetInt(string s)
        {
            try
            {
                return Convert.ToInt32(s);
            }
            catch
            {
                throw new Exception(s + "转成Int型失败");
            }
        }

        /// <summary>
        /// 转成Double型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double GetDouble(string s)
        {
            try
            {
                return Convert.ToDouble(s);
            }
            catch
            {
                throw new Exception(s + "转成Double型失败");
            }
        }

        /// <summary>
        /// 转成DateTime型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime GetDate(string s)
        {
            try
            {
                return Convert.ToDateTime(s).Date;
            }
            catch
            {
                throw new Exception(s + "转成DateTime型失败");
            }
        }

        /// <summary>
        /// 转成DateTime型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(string s)
        {
            try
            {
                return Convert.ToDateTime(s);
            }
            catch
            {
                throw new Exception(s + "转成DateTime型失败");
            }
        }

        /// <summary>
        /// 转成整型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int GetSafeInt(string s, int def = 0)
        {
            try
            {
                return Convert.ToInt32(s);
            }
            catch
            {
                return def;
            }
        }

        /// <summary>
        /// 转成Double型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static double GetSafeDouble(string s, double def = 0.0)
        {
            try
            {
                return Convert.ToDouble(s);
            }
            catch
            {
                return def;
            }
        }

        /// <summary>
        /// 转成DateTime型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime GetSafeDate(string s, string def = "1900-1-1")
        {
            try
            {
                return Convert.ToDateTime(s).Date;
            }
            catch
            {
                try
                {
                    return DateTime.Parse(def);
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
        }

        /// <summary>
        /// 转成DateTime型
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static DateTime GetSafeDateTime(string s, string def = "1900-1-1")
        {
            try
            {
                return Convert.ToDateTime(s);
            }
            catch
            {
                try
                {
                    return DateTime.Parse(def);
                }
                catch
                {
                    return DateTime.MinValue;
                }
            }
        }

        /// <summary>
        /// 转义
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string EncodeCode(string s)
        {
            return s.Replace("-", "&hbr;").Replace("#", "&wno;").Replace("|", "&vbr;");
        }

        /// <summary>
        /// 转义
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string DecodeCode(string s)
        {
            s = Regex.Replace(s, "&hbr;", "-", RegexOptions.IgnoreCase);
            s = Regex.Replace(s, "&wno;", "#", RegexOptions.IgnoreCase);
            s = Regex.Replace(s, "&vbr;", "|", RegexOptions.IgnoreCase);
            return s;
        }
        #endregion

        #region 变量
        /// <summary>
        /// 1900年1月1号时间
        /// </summary>
        private DateTime Date19000101
        {
            get
            {
                return DateTime.Parse("1900-1-1");
            }
        }
        #endregion
    }
}
