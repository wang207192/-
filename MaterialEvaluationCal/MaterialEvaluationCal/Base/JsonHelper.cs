using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEvaluationCal.Base
{
    /// <summary>
    /// Json帮助类
    /// </summary>
    public class JsonHelper
    {
        /// <summary>
        /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
        public static string SerializeObject(object o)
        {
            // string json = JsonConvert.SerializeObject(o, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });//空数据不格式化
            string json = JsonConvert.SerializeObject(o);//默认空数据json会出来“null”字符串

            return json;
        }
        /// <summary>
        /// 解析JSON字符串生成对象实体
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
        /// <returns>对象实体</returns>
        public static T DeserializeJsonToObject<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }
        /// <summary>
        /// 解析JSON数组生成对象实体集合
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
        /// <returns>对象实体集合</returns>
        public static List<T> DeserializeJsonToList<T>(string json) where T : class
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
            List<T> list = o as List<T>;
            return list;
        }
        /// <summary>
        /// 反序列化JSON到给定的匿名对象.
        /// </summary>
        /// <typeparam name="T">匿名对象类型</typeparam>
        /// <param name="json">json字符串</param>
        /// <param name="anonymousTypeObject">匿名对象</param>
        /// <returns>匿名对象</returns>
        public static T DeserializeAnonymousType<T>(string json, T anonymousTypeObject)
        {
            T t = JsonConvert.DeserializeAnonymousType(json, anonymousTypeObject);
            return t;
        }
    }
}
/*
 https://www.cnblogs.com/freshman0216/p/4161800.html
 /// <summary>
    /// Json测试
    /// </summary>
    public class JsonTest : IRun
    {
        public void Run()
        {
            Student sdudent = new Student();
            sdudent.ID = 1;
            sdudent.Name = "陈晨";
            sdudent.NickName = "石子儿";
            sdudent.Class = new Class() { Name = "CS0216", ID = 0216 };

            //实体序列化和反序列化
            string json1 = JsonHelper.SerializeObject(sdudent);
            //json1 : {"ID":1,"Name":"陈晨","NickName":"石子儿","Class":{"ID":216,"Name":"CS0216"}}
            Student sdudent1 = JsonHelper.DeserializeJsonToObject<Student>(json1);

            //实体集合序列化和反序列化
            List<Student> sdudentList = new List<Student>() { sdudent, sdudent1 };
            string json2 = JsonHelper.SerializeObject(sdudentList);
            //json: [{"ID":1,"Name":"陈晨","NickName":"石子儿","Class":{"ID":216,"Name":"CS0216"}},{"ID":1,"Name":"陈晨","NickName":"石子儿","Class":{"ID":216,"Name":"CS0216"}}]
            List<Student> sdudentList2 = JsonHelper.DeserializeJsonToList<Student>(json2);

            //DataTable序列化和反序列化
            DataTable dt = new DataTable();
            dt.TableName = "Student";
            dt.Columns.Add("ID", typeof(int));
            dt.Columns.Add("Name");
            dt.Columns.Add("NickName");
            DataRow dr = dt.NewRow();
            dr["ID"] = 112;
            dr["Name"] = "战三";
            dr["NickName"] = "小三";
            dt.Rows.Add(dr);
            string json3 = JsonHelper.SerializeObject(dt);
            //json3 : [{"ID":112,"Name":"战三","NickName":"小三"}]
            DataTable sdudentDt3 = JsonHelper.DeserializeJsonToObject<DataTable>(json3);
            List<Student> sdudentList3 = JsonHelper.DeserializeJsonToList<Student>(json3);

            //验证对象和数组
            Student sdudent4 = JsonHelper.DeserializeJsonToObject<Student>("{\"ID\":\"112\",\"Name\":\"石子儿\"}");
            List<Student> sdudentList4 = JsonHelper.DeserializeJsonToList<Student>("[{\"ID\":\"112\",\"Name\":\"石子儿\"}]");

            //匿名对象解析
            var tempEntity = new { ID = 0, Name = string.Empty };
            string json5 = JsonHelper.SerializeObject(tempEntity);
            //json5 : {"ID":0,"Name":""}
            tempEntity = JsonHelper.DeserializeAnonymousType("{\"ID\":\"112\",\"Name\":\"石子儿\"}", tempEntity);
            var tempStudent = new Student();
            tempStudent = JsonHelper.DeserializeAnonymousType("{\"ID\":\"112\",\"Name\":\"石子儿\"}", tempStudent);

            Console.Read();
        }

    }

    /// <summary>
    /// 学生信息实体
    /// </summary>
    public class Student
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public Class Class { get; set; }
    }

    /// <summary>
    /// 学生班级实体
    /// </summary>
    public class Class
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
 */

