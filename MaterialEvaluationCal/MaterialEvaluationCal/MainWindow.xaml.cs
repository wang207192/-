using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MaterialEvaluationCal
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            string type = "碳素结构钢";
            string tablename = "S_JGG";
            string err = "";
            //Calculates.JGJ18_2012.GetExtraDataJson(work, tablename);

            string sqlStr = "Select top 10 * from  S_JGG ";
            string JsonhelperData = GetDataJson(type, sqlStr, tablename);


            string extraDatajson = Calculates.JGJ18_2012.GetExtraDataJson("碳素结构钢", "GBT_700-2006");
            var listExtraData = Calculates.JGJ18_2012.GetDictionary(extraDatajson, "碳素结构钢");
            var retData1 = Calculates.JGJ18_2012.GetDictionary(JsonhelperData, type);

            IDictionary<string, IDictionary<string, IList<IDictionary<string, string>>>> retData = new Dictionary<string, IDictionary<string, IList<IDictionary<string, string>>>>();

            retData.Add("碳素结构钢", retData1);
            Calculates.JGJ18_2012.Calc(listExtraData, ref retData, ref err);

        }

        public static string GetDataJson(string type, string sqlstr, string tableName)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("{\"calcData\":{\"" + type + "\":");

            string sql = string.Format(@sqlstr);
            Base.SqlBase sqlbase = new Base.SqlBase("jcjt");
            DataSet ds = sqlbase.ExecuteDataset(sql);

            ds.Tables[0].TableName = tableName;
            string json = MaterialEvaluationCal.Base.JsonHelper.SerializeObject(ds);
            sb.Append(json);
            sb.Append("},\"code\":1,\"message\":\"成功\"}");
            Calculates.JGJ18_2012.GetDictionary(sb.ToString(), type);
            return sb.ToString();
        }
    }
}
