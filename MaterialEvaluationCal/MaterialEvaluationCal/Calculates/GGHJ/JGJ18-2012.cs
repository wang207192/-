using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace MaterialEvaluationCal.Calculates
{
    public partial class JGJ18_2012 : BaseMethods
    {
        public static bool Calc(IDictionary<string, IList<IDictionary<string, string>>> dataExtra, ref IDictionary<string, IDictionary<string, IList<IDictionary<string, string>>>> retData, ref string err)
        {
            /************************ 代码开始 *********************/
            return true;
        }


        public static string GetExtraDataJson(string work,string tablename)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"calcData\":{\"" + work + "\":{\"" + tablename + "\":[");
            string sql = string.Format(@"select * from dbo.GBT_700-2006");
            Base.SqlBase sqlbase = new Base.SqlBase();
            DataSet ds = sqlbase.ExecuteDataset(sql);
            string json = MaterialEvaluationCal.Base.JsonHelper.SerializeObject(ds);
            return sb.ToString();
        }

    }
}
