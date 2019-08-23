using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Linq;

namespace MaterialEvaluationCal.Calculates
{
    public partial class JGJ18_2012 : BaseMethods
    {


        public static bool Calc(IDictionary<string, IList<IDictionary<string, string>>> dataExtra, ref IDictionary<string, IDictionary<string, IList<IDictionary<string, string>>>> retData, ref string err)
        {

            /************************ 代码开始 *********************/
            
            //单行数据进行验证
            foreach (var projectItems in retData)
            {
                foreach (var tableItems in projectItems.Value)
                {
                    foreach(var dicFields in tableItems.Value)
                    {
                        Check_JGG(dataExtra, dicFields, ref retData,ref err);
                    }
                }

            }
            return true;
        }


        #region
        public static void GetdataExtra(ref IDictionary<string, IDictionary<string, IList<IDictionary<string, string>>>> retData)
        {
            #region 数据组装
            var basedate = new Dictionary<string, string>();
            //从表RECID
            basedate.Add("RECID", "");
            //主表RECID
            basedate.Add("BYZBRECID", "");
            //牌号
            basedate.Add("GCLX_PH", "");
            //等级
            basedate.Add("GCLX_JB", "");
            //规格
            basedate.Add("GGXH", "");
            //屈服荷重（KN）
            basedate.Add("QFHZ1", "");
            basedate.Add("QFHZ2", "");
            basedate.Add("QFHZ3", "");
            //屈服强度
            basedate.Add("QFQD1", "");
            basedate.Add("QFQD2", "");
            basedate.Add("QFQD3", "");
            //厚度或直径
            basedate.Add("ZJ", "");
            //伸长值
            basedate.Add("SCZ1", "");
            basedate.Add("SCZ2", "");
            basedate.Add("SCZ3", "");
            //冷弯
            basedate.Add("LW1", "");
            basedate.Add("LW2", "");
            basedate.Add("LW3", "");
            //抗拉强度
            basedate.Add("KLQD1", "");
            basedate.Add("KLQD2", "");
            basedate.Add("KLQD3", "");
            //伸长率
            basedate.Add("SCL1", "");
            basedate.Add("SCL2", "");
            basedate.Add("SCL3", "");
            //抗拉荷重（KN）
            basedate.Add("KLHZ1", "");
            basedate.Add("KLHZ2", "");
            basedate.Add("KLHZ3", "");
            //屈服合格
            basedate.Add("HG_QF1", "");
            basedate.Add("HG_QF2", "");
            basedate.Add("HG_QF3", "");
            //屈服合格个数
            basedate.Add("HG_QF", "1");
            //抗拉合格
            basedate.Add("HG_KL1", "");
            basedate.Add("HG_KL2", "");
            basedate.Add("HG_KL3", "");
            //抗拉合格个数
            basedate.Add("HG_KL", "");
            //伸长率合格
            basedate.Add("HG_SC1", "");
            basedate.Add("HG_SC2", "");
            basedate.Add("HG_SC3", "");
            //伸长率合格个数
            basedate.Add("HG_SC", "");
            //冷弯合格
            basedate.Add("HG_LW1", "");
            basedate.Add("HG_LW2", "");
            basedate.Add("HG_LW3", "");
            //冷弯合格个数
            basedate.Add("HG_LW", "");
            #endregion
        }

        /// <summary>
        /// 结构钢检验
        /// </summary>
        /// <param name="dataExtra">标准数据</param>
        /// <param name="rowDate">待检验数据</param>
        /// <returns></returns>
        public static bool Check_JGG(IDictionary<string, IList<IDictionary<string, string>>> dataExtra, IDictionary<string, string> rowDate,ref IDictionary<string, IDictionary<string, IList<IDictionary<string, string>>>> retData, ref string err)
        {
            //筛选牌号
            if (string.IsNullOrEmpty(rowDate["GCLX_PH"]))
            {
                throw new Exception("钢材牌号不存在");
            }
            if (!dataExtra.ContainsKey("BZ_JGG_DJ"))
            {
                throw new Exception("BZ_JGG_DJ 表数据不存在");
            }
            var tableData = dataExtra["BZ_JGG_DJ"];
            if (!tableData.Any(u => u.ContainsKey("PH")))
            {
                err = "【BZ_JGG_DJ】表不存在【PH】字段";
                return false;
            }
            if (!tableData.Where(u => u.Values.Contains(rowDate["GCLX_PH"].ToString())).Any())
                return false;
            var fieldData = tableData.Where(u => u.Values.Contains(rowDate["GCLX_PH"].ToString()));
            //检验项目
            if(!string.IsNullOrEmpty(rowDate["JCXM"]))
            {
                var inspectionItems = rowDate["JCXM"].Split(',');
                foreach(var item in inspectionItems)
                {
                    switch(item)
                    {
                        case "拉伸":
                            DrawCheck(fieldData, rowDate);//伸长率+厚度
                            break;
                        case "冷弯":
                            BendingCheck(fieldData, rowDate);//弯心直径+厚度
                            break;
                        case "屈服强度":
                            CheckYield_Strength(fieldData, rowDate);  //屈服强度
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {

            }



            //筛选等级
            //if (!string.IsNullOrEmpty(rowDate["Grade"]) && rowDate["Grade"].Trim() != "-")
            //{
            //    fieldData = fieldData.Where(u => u.Values.Contains(rowDate["Grade"]));
            //}


            //检验屈服强度1,2,3
            //检验抗拉强度1,2,3
            //检验断后伸长率


            return true;
        }
        #endregion


        #region 拉伸检测
        /// <summary>
        /// 拉伸检测
        /// </summary>
        /// <param name="tableData"></param>
        /// <param name="rowDate"></param>
        public static void DrawCheck(IEnumerable<IDictionary<string, string>> tableData, IDictionary<string, string> rowDate)
        {
            //厚度、直径，伸长率 SCL1
            /*
             * 1.CD	长度(MM)  /  ZJ	直径  取值不确定暂定ZJ
             * 2.伸长率为“—”时是否需要判断
             */

            bool checkOK = false;
            //伸长率验证 如SCL1,SCL2,SCL3
            int baseNum = 1;
            var checkItems = rowDate.Where(u => u.Key.StartsWith("SCL")).ToArray();
            foreach (var val in checkItems)
            {
                var item = tableData.Where(u => u.Keys.ToString() == "SCLBZZ" && (u.Values == null || GetInt(u.Values.ToString()) <= GetInt(val.Value.ToString())));
                //指定长度验证 
                //Q195 的仅≤40的需要验证
                if (!rowDate.Any(u => (u.Key == "GCLX_PH" && u.Value.Contains("Q195") && (u.Key == "ZJ" && u.Value == "≤40"))))
                    CheckDiam(ref tableData, rowDate, "ZJM", "ZJ");
                if (item.Any())
                {
                    checkOK = true;
                }
                //更新指定的数据，如：伸长率1是否合格等
                if (string.IsNullOrEmpty(rowDate["HG_SC"]))
                    rowDate["HG_SC"] = "0";
                if (checkOK)
                {
                    rowDate["HG_SC" + baseNum] = "1";
                    rowDate["HG_SC"] = (GetInt(rowDate["HG_SC"]) + 1).ToString();
                }
                else
                {
                    rowDate["HG_SC" + baseNum] = "0";
                }

                baseNum++;
            }
        }
        #endregion

        #region 冷弯检测
        /// <summary>
        /// 冷弯检测
        /// </summary>
        /// <param name="tableData"></param>
        /// <param name="rowDate"></param>
        public static void BendingCheck(IEnumerable<IDictionary<string, string>> tableData, IDictionary<string, string> rowDate)
        {
            //CheckItem(fieldData, rowDate, "SCL", "LWBZZ", true);//弯心直径+厚度
            /*
             * LWZJ	弯心直径
                XGM	XGM
                LWJD	冷弯角度
                FFWQCS	反复弯曲次数
                WHICH	报表模板编号
                XLGS	拉根数
                XWGS	弯根数
                -判定标准待确定
             */
            return;
            bool checkOK = false;
            //冷弯验证 如LW1,LW2,LW3
            int baseNum = 1;
            var checkItems = rowDate.Where(u => u.Key.StartsWith("LW")).ToArray();
            foreach (var val in checkItems)
            {
                var item = tableData.Where(u => u.Keys.ToString() == "SCLBZZ" && (u.Values == null || GetInt(u.Values.ToString()) <= GetInt(val.Value.ToString())));
                //指定长度验证 
                //Q195 的仅≤40的需要验证
                if (!rowDate.Any(u => (u.Key == "GCLX_PH" && u.Value.Contains("Q195") && (u.Key == "ZJ" && u.Value == "≤40"))))
                    CheckDiam(ref tableData, rowDate, "ZJM", "ZJ");
                if (item.Any())
                {
                    checkOK = true;
                }
                //更新指定的数据，如：伸长率1是否合格等
                if (string.IsNullOrEmpty(rowDate["HG_SC"]))
                    rowDate["HG_SC"] = "0";
                if (checkOK)
                {
                    rowDate["HG_SC" + baseNum] = "1";
                    rowDate["HG_SC"] = (GetInt(rowDate["HG_SC"]) + 1).ToString();
                }
                else
                {
                    rowDate["HG_SC" + baseNum] = "0";
                }

                baseNum++;
            }
        }
        #endregion

        #region 验证指定长度的标准数据是否存在
        /// <summary>
        /// 验证指定长度的标准数据是否存在
        /// </summary>
        /// <param name="tableData">筛选标准表数据</param>
        /// <param name="rowDate">验证行数据</param>
        /// <param name="keyStr">标准表对应字段名</param>
        /// <param name="ValStr">验证行对应字段名</param>
        private static void CheckDiam(ref IEnumerable<IDictionary<string, string>> tableData, IDictionary<string, string> rowDate, string keyStr, string ValStr)
        {
            if (!rowDate.ContainsKey(ValStr))
                return;
            if (tableData.Any(u => (u.Keys.ToString() == keyStr && u.Values.ToString() == rowDate[ValStr])))
            {
                tableData = tableData.Where(u => (u.Keys.ToString() == keyStr && u.Values.ToString() == rowDate[ValStr]));
            }
            return;
        }
        #endregion


        //屈服强度验证
        private static void CheckYield_Strength(IEnumerable<IDictionary<string, string>> tableData, IDictionary<string, string> rowDate)
        {
            //厚度、直径，伸长率 SCL1
            /*CD    长度(MM)  /  ZJ    直径  取值不确定*/
            //获取需检验数据中指定元素的值
            bool checkOK = false;
            // 屈服强度验证 如QFQD1,QFQD2,QFQD3
            int baseNum = 1;
            var checkItems = rowDate.Where(u => u.Key.StartsWith("QFQD")).ToArray();
            foreach (var val in checkItems)
            {
                var item = tableData.Where(u => u.Keys.ToString() == "QFQDBZZ" && (u.Values == null || GetInt(u.Values.ToString()) <= GetInt(val.Value.ToString())));
                //指定长度验证
                if (tableData.Any(u => (u.Keys.ToString() == "ZJM" && u.Values.ToString() == rowDate["ZJ"])))
                {
                    item = tableData.Where(u => (u.Keys.ToString() == "ZJM" && u.Values.ToString() == rowDate["ZJ"]));
                }
                if (item.Any())
                {
                    checkOK = true;
                }
                //更新指定的数据，如：伸长率1是否合格等
                if (string.IsNullOrEmpty(rowDate["HG_QF"]))
                    rowDate["HG_QF"] = "0";
                if (checkOK)
                {
                    rowDate["HG_QF" + baseNum] = "1";
                    rowDate["HG_QF"] = (GetInt(rowDate["HG_QF"]) + 1).ToString();
                }
                else
                {
                    rowDate["HG_QF" + baseNum] = "0";
                }
                baseNum++;
            }
        }


        public static string GetExtraDataJson(string work, string tablename)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"calcData\":{\"" + work + "\":");
            string sql = string.Format(@"select * from dbo.[GBT_700-2006]");
            Base.SqlBase sqlbase = new Base.SqlBase();
            DataSet ds = sqlbase.ExecuteDataset(sql);
            ds.Tables[0].TableName = tablename;
            string json = MaterialEvaluationCal.Base.JsonHelper.SerializeObject(ds);
            sb.Append(json);
            sb.Append("},\"code\":1,\"message\":\"成功\"}");
            GetDictionary(sb.ToString(), work);
            return sb.ToString();
        }

        public static IDictionary<string, IList<IDictionary<string, string>>> GetDictionary(string json,string work)
        {
            IDictionary<string, IList<IDictionary<string, string>>> dataExtra = new Dictionary<string, IList<IDictionary<string, string>>>();
            var json_main = new { calcData = new object(), message = string.Empty };
            var stract_class = MaterialEvaluationCal.Base.JsonHelper.DeserializeAnonymousType(json, json_main);
            string json_str = stract_class.calcData.ToString().Replace("\"" + work + "\":", "").TrimStart('{').TrimEnd('}');
            DataSet ds = MaterialEvaluationCal.Base.JsonHelper.DeserializeJsonToObject<DataSet>(json_str);
            string name = ds.Tables[0].TableName;
            DataTable dt = ds.Tables[0];
            IList<IDictionary<string, string>> list = new List<IDictionary<string, string>>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                IDictionary<string, string> openWith = new Dictionary<string, string>();
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    openWith.Add(dt.Columns[j].ColumnName, dt.Rows[i][dt.Columns[j].ColumnName].ToString());
                }
                list.Add(openWith);
            }
            dataExtra.Add(name, list);
            return dataExtra;
        }


    }
}
