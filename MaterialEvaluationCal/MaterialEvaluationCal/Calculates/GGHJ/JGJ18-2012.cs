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
            //单个数据组装
            //var basedateList = retData.Values;
            //if (basedateList == null)
            //    basedateList = new ;
            //for (int i = 0;i<10;i++)
            //{


            //    basedateList.Add(basedate);
            //}
            //return retData.Add("1",basedateList);
        }

        /// <summary>
        /// 结构钢检验
        /// </summary>
        /// <param name="dataExtra">标准数据</param>
        /// <param name="rowDate">待检验数据</param>
        /// <returns></returns>
        public static bool Check_JGG(IDictionary<string, IList<IDictionary<string, string>>> dataExtra, IDictionary<string, string> rowDate,ref IDictionary<string, IDictionary<string, IList<IDictionary<string, string>>>> retData, ref string err)
        {
            //{1,["Brand_No","Q195"|"Grade","-"|"Land_Diam_Min","0"|"Land_Diam_Max","16"|"Grade","-"|"Width","NULL"|"Yield_Strength","195"|"Tensile_Strength","315-430"]}
            //{"GBT_700-2006",[{"id":"1"},{"no":"195"}],[{"id":"1"},{"no":"195"}]}
            //筛选牌号
            var Brand_No = rowDate["Brand_No"];
            if (string.IsNullOrEmpty(rowDate["Brand_No"]))
            {
                throw new Exception("钢材牌号不存在");
            }
            //var fileDate = GetExtraDataList("GBT_700-2006.GCLX_PH", Brand_No);
            if (!dataExtra.ContainsKey("GBT_700-2006"))
            {
                throw new Exception("GBT_700-2006 表数据不存在");
            }
            var tableData = dataExtra["GBT_700-2006"];
            if (!tableData.Any(u => u.ContainsKey("GCLX_PH")))
            {
                throw new Exception("表不存在 'GCLX_PH' 字段");
            }
            var fieldData = tableData.Where(u => u.ContainsKey("GCLX_PH"));
            //检验项目
            if(!string.IsNullOrEmpty(rowDate["JCXM"]))
            {
                var inspectionItems = rowDate["JCXM"].Split(',');
                foreach(var item in inspectionItems)
                {
                    switch(item)
                    {
                        case "拉伸":
                            CheckItem(ref tableData, rowDate, "SCL", "Elongation_After",true);//伸长率+厚度
                            break;
                        case "冷弯":
                            CheckItem(ref tableData, rowDate, "SCL", "Elongation_After", true);//弯心直径+厚度
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
            if (!string.IsNullOrEmpty(rowDate["Grade"]) && rowDate["Grade"].Trim() != "-")
            {
                fieldData = fieldData.Where(u => u.Values.Contains(rowDate["Grade"]));
            }


            //检验屈服强度1,2,3
            var Land_DiamItems = rowDate.Where(u => u.Key.Contains("QFQD"));
            if (Land_DiamItems.Any())
            {
                foreach (KeyValuePair<string, string> item in Land_DiamItems)
                {
                    //QFQD1,195
                    if (item.Value.Any() && fieldData.Any(u => u.Keys.ToString() == "Yield_Strength" && u.Values.ToString() == item.Value.ToString()))
                    {
                        //获取最小值

                    }
                }
            }
            //检验抗拉强度
            //检验断后伸长率


            return true;
        }
        /// <summary>
        /// 拉伸检测
        /// </summary>
        /// <param name="tableData"></param>
        /// <param name="rowDate"></param>
        private static void CheckItem(ref IList<IDictionary<string, string>> tableData,IDictionary<string,string> rowDate,string checkItemstr,string tableItemStr,bool hasLand = false)
        {
            if (string.IsNullOrEmpty(checkItemstr) || string.IsNullOrEmpty(tableItemStr))
                return;
            //厚度、直径，伸长率 SCL1
            /*CD	长度(MM)  /  ZJ	直径  取值不确定*/
            //获取需检验数据中指定元素的值
            string Land_Diam = "";
            int minNum = 0;
            int maxNum = 0;
            Land_Diam = rowDate["ZJ"];
            if (hasLand && !string.IsNullOrWhiteSpace(Land_Diam))
            {
                var s = Land_Diam.Split('>', '≤');
                if (Land_Diam.Contains(">"))
                {
                    if (Land_Diam.Contains("≤"))
                    {
                        maxNum = GetInt(s[1]);
                    } 
                    else
                        minNum = GetInt(s[0]);
                }
                else
                    maxNum = GetInt(s[0]);
            }

            bool checkOK = false;
            //伸长率验证 如SCL1,SCL2,SCL3
            int baseNum = 1;
            foreach (var val in rowDate.Where(u => u.Key.StartsWith(checkItemstr)))
            {
                var item = tableData.Where(u => u.Keys.ToString() == tableItemStr && (u.Values == null || GetInt(u.Values.ToString()) <= GetInt(val.Value.ToString())));
                //指定长度验证
                if (hasLand && tableData.Any(u => (u.Keys.ToString() == "Land_Diam_Min" && GetInt(u.Values.ToString()) > minNum) && (u.Keys.ToString() == "Land_Diam_Max" && GetInt(u.Values.ToString()) <= maxNum)))
                {
                    item = tableData.Where(u => (u.Keys.ToString() == "Land_Diam_Min" && GetInt(u.Values.ToString()) > minNum) && (u.Keys.ToString() == "Land_Diam_Max" && GetInt(u.Values.ToString()) <= maxNum));
                }
                if (item.Any())
                {
                    checkOK = true;
                }
                switch (checkItemstr)
                {
                    case "SCL":
                        rowDate["HG_SC" + baseNum] = checkOK ? "1" : "0";
                        break;
                    case "":
                        break;
                }
                baseNum++;
            }


        }
        /// <summary>
        /// 冷弯检测
        /// </summary>
        /// <param name="tableData"></param>
        /// <param name="rowDate"></param>
        private void CheckBending(ref IList<IDictionary<string, string>> tableData, IDictionary<string, string> rowDate)
        {

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
