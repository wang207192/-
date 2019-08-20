using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEvaluationCal.Common
{
    public class BaseData
    {
        /* 字段：标准表（计算中使用计算标准用到的对应表数据）
           参数：表 字段（键值对）
        */
        public IDictionary<string, IList<IDictionary<string, string>>> dataExtraTmp;

        /* 字段：传入或传出参数
           参数：检测项目  表 字段（键值对）
        */
        public IDictionary<string, IDictionary<string, IList<IDictionary<string, string>>>> retDataTmp;
    }
}
