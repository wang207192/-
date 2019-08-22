using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MaterialEvaluationCal.Base
{
    public class SqlBase: BaseDal
    {
        /// <summary>
        /// 实例化一个Dal对象
        /// </summary>
        /// <param name="dalType">需实例化的类</param>
        /// <returns></returns>
        public SqlBase() : base(ESqlConnType.ConnectionStringMain)
        {

        }

        public SqlBase(ESqlConnType eSqlConnType)
            : base(eSqlConnType)
        {

        }
    }
}
