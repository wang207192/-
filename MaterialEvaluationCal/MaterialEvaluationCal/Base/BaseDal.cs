using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace MaterialEvaluationCal.Base
{
    /// <summary>
    /// 多个数据库链接库
    /// </summary>
    public enum ESqlConnType
    {
        ConnectionStringMain
    }

    /// <summary>
    /// 数据库连接类型
    /// </summary>
    public enum DB
    {
        Read, ReadAndWrite
    }

    public class BaseDal
    {
        private string sqlConnectionString; //当前读数据库链接字符串
        private string sqlConnectionStringWrite; //当前写数据库链接字符串

        public BaseDal(ESqlConnType eSqlConnType)
        {
            switch (eSqlConnType)
            {
                case ESqlConnType.ConnectionStringMain:
                    sqlConnectionString = ConfigurationManager.ConnectionStrings["ConnectionStringMain"].ConnectionString;    //汽配摘要数据数据库连接
                    sqlConnectionStringWrite = ConfigurationManager.ConnectionStrings["ConnectionStringMain"].ConnectionString;  //汽配摘要数据数据库连接
                    break;
            }
        }

        #region ExecuteNonQuery

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="commandText">需要执行的语句</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText)
        {
            return ExecuteNonQuery(commandText, DB.Read);

        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="commandText">需要执行的语句</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, DB sqlDbType)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteNonQuery(sqlConnectionString, CommandType.Text, commandText);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteNonQuery(sqlConnectionStringWrite, CommandType.Text, commandText);
            }
        }



        /// <summary>
        /// 执行带参数的SQL语句
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, params SqlParameter[] parameters)
        {
            return ExecuteNonQuery(commandText, DB.Read, parameters);
        }

        /// <summary>
        /// 执行带参数的SQL语句
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string commandText, DB sqlDbType, params SqlParameter[] parameters)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteNonQuery(sqlConnectionString, CommandType.Text, commandText, parameters);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteNonQuery(sqlConnectionStringWrite, CommandType.Text, commandText, parameters);

            }
        }





        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <returns></returns>
        public int SP_ExecuteNonQuery(string spName)
        {
            return SP_ExecuteNonQuery(spName, DB.Read);
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="sqlDbType">数据库连接类型</param>
        /// <returns></returns>
        public int SP_ExecuteNonQuery(string spName, DB sqlDbType)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteNonQuery(sqlConnectionString, CommandType.StoredProcedure, spName);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteNonQuery(sqlConnectionStringWrite, CommandType.StoredProcedure, spName);
            }
        }



        /// <summary>
        /// 执行带参数的存储过程
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public int SP_ExecuteNonQuery(string spName, params SqlParameter[] parameters)
        {
            return SP_ExecuteNonQuery(spName, DB.Read, parameters);
        }


        /// <summary>
        /// 执行带参数的存储过程
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public int SP_ExecuteNonQuery(string spName, DB sqlDbType, params SqlParameter[] parameters)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteNonQuery(sqlConnectionString, CommandType.StoredProcedure, spName, parameters);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteNonQuery(sqlConnectionStringWrite, CommandType.StoredProcedure, spName, parameters);
            }
        }

        #endregion

        #region ExecuteDataset

        /// <summary>
        /// 执行SQL语句返回列表信息
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <returns></returns>
        public DataSet ExecuteDataset(string commandText)
        {
            return ExecuteDataset(commandText, DB.Read);
        }


        /// <summary>
        /// 语句返回列表信息
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <returns></returns>
        public DataSet ExecuteDataset(string commandText, DB sqlDbType)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteDataset(sqlConnectionString, CommandType.Text, commandText);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteDataset(sqlConnectionStringWrite, CommandType.Text, commandText);
            }
        }


        /// <summary>
        /// 执行带参数的SQL语句并返回列表信息
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public DataSet ExecuteDataset(string commandText, params SqlParameter[] parameters)
        {
            return ExecuteDataset(commandText, DB.Read, parameters);
        }


        /// <summary>
        /// 执行带参数的SQL语句并返回列表信息
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public DataSet ExecuteDataset(string commandText, DB sqlDbType, params SqlParameter[] parameters)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteDataset(sqlConnectionString, CommandType.Text, commandText, parameters);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteDataset(sqlConnectionStringWrite, CommandType.Text, commandText, parameters);
            }
        }

        /// <summary>
        /// 执行存储过程返回列表信息
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <returns></returns>
        public DataSet SP_ExecuteDataset(string spName)
        {
            return SP_ExecuteDataset(spName, DB.Read);
        }

        /// <summary>
        /// 执行存储过程返回列表信息
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <returns></returns>
        public DataSet SP_ExecuteDataset(string spName, DB sqlDbType)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteDataset(sqlConnectionString, CommandType.StoredProcedure, spName);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteDataset(sqlConnectionStringWrite, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 执行带参数的存储过程并返回列表信息
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public DataSet SP_ExecuteDataset(string spName, params SqlParameter[] parameters)
        {
            return SP_ExecuteDataset(spName, DB.Read, parameters);
        }


        /// <summary>
        /// 执行带参数的存储过程并返回列表信息
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public DataSet SP_ExecuteDataset(string spName, DB sqlDbType, params SqlParameter[] parameters)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteDataset(sqlConnectionString, CommandType.StoredProcedure, spName, parameters);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteDataset(sqlConnectionStringWrite, CommandType.StoredProcedure, spName, parameters);
            }
        }


        #endregion

        #region ExecuteScalar

        /// <summary>
        /// 执行SQL语句获取第一行的第一个字段
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, DB.Read);
        }


        /// <summary>
        /// 执行SQL语句获取第一行的第一个字段
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, DB sqlDbType)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteScalar(sqlConnectionString, CommandType.Text, commandText);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteScalar(sqlConnectionStringWrite, CommandType.Text, commandText);
            }
        }


        /// <summary>
        /// 执行带参数的SQL语句获取第一行的第一个字段
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, params SqlParameter[] parameters)
        {
            return ExecuteScalar(commandText, DB.Read, parameters);
        }


        /// <summary>
        /// 执行带参数的SQL语句获取第一行的第一个字段
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, DB sqlDbType, params SqlParameter[] parameters)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteScalar(sqlConnectionString, CommandType.Text, commandText, parameters);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteScalar(sqlConnectionStringWrite, CommandType.Text, commandText, parameters);
            }
        }


        /// <summary>
        /// 执行存储过程获取第一行的第一个字段
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <returns></returns>
        public object SP_ExecuteScalar(string spName)
        {
            return SP_ExecuteScalar(spName, DB.Read);
        }


        /// <summary>
        /// 执行存储过程获取第一行的第一个字段
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <returns></returns>
        public object SP_ExecuteScalar(string spName, DB sqlDbType)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteScalar(sqlConnectionString, CommandType.StoredProcedure, spName);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteScalar(sqlConnectionStringWrite, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 执行带参数的存储过程获取第一行的第一个字段
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public object SP_ExecuteScalar(string spName, params SqlParameter[] parameters)
        {
            return SP_ExecuteScalar(spName, DB.Read, parameters);
        }


        /// <summary>
        /// 执行带参数的存储过程获取第一行的第一个字段
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public object SP_ExecuteScalar(string spName, DB sqlDbType, params SqlParameter[] parameters)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteScalar(sqlConnectionString, CommandType.StoredProcedure, spName, parameters);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteScalar(sqlConnectionStringWrite, CommandType.StoredProcedure, spName, parameters);
            }
        }


        #endregion

        #region ExecuteReader


        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, DB.Read);
        }


        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string commandText, DB sqlDbType)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteReader(sqlConnectionString, CommandType.Text, commandText);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteReader(sqlConnectionStringWrite, CommandType.Text, commandText);
            }
        }



        /// <summary>
        /// 执行带参数的SQL语句 
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string commandText, params SqlParameter[] parameters)
        {
            return ExecuteReader(commandText, DB.Read, parameters);
        }

        /// <summary>
        /// 执行带参数的SQL语句
        /// </summary>
        /// <param name="commandText">SQL语句</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string commandText, DB sqlDbType, params SqlParameter[] parameters)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteReader(sqlConnectionString, CommandType.Text, commandText, parameters);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteReader(sqlConnectionStringWrite, CommandType.Text, commandText, parameters);
            }
        }


        /// <summary>
        /// 执行存储过程 
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <returns></returns>
        public SqlDataReader SP_ExecuteReader(string spName)
        {
            return SP_ExecuteReader(spName, DB.Read);
        }


        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <returns></returns>
        public SqlDataReader SP_ExecuteReader(string spName, DB sqlDbType)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteReader(sqlConnectionString, CommandType.StoredProcedure, spName);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteReader(sqlConnectionStringWrite, CommandType.StoredProcedure, spName);
            }
        }

        /// <summary>
        /// 执行带参数的存储过程
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public SqlDataReader SP_ExecuteReader(string spName, params SqlParameter[] parameters)
        {
            return SP_ExecuteReader(spName, DB.Read, parameters);
        }

        /// <summary>
        /// 执行带参数的存储过程
        /// </summary>
        /// <param name="spName">存储过程名</param>
        /// <param name="sqlDbType">数据库链接类型</param>
        /// <param name="parameters">参数</param>
        /// <returns></returns>
        public SqlDataReader SP_ExecuteReader(string spName, DB sqlDbType, params SqlParameter[] parameters)
        {
            if (sqlDbType == DB.Read)
            {
                return DBUtility.SqlHelper.ExecuteReader(sqlConnectionString, CommandType.StoredProcedure, spName, parameters);
            }
            else
            {
                return DBUtility.SqlHelper.ExecuteReader(sqlConnectionStringWrite, CommandType.StoredProcedure, spName, parameters);
            }
        }


        #endregion

        #region 存储过程分页
        /// <summary>
        /// 分页1 wsd_page_1： 根据唯一字段唯一值按大小排序，如ID 
        /// </summary>
        /// <param name="tb">表名</param>
        /// <param name="collist">要查询出的字段列表,*表示全部字段</param>
        /// <param name="condition">查询条件 ,不带where</param>
        /// <param name="col">排序列 例：ID</param>
        /// <param name="coltype">列的类型,0-数字类型,1-字符类型</param>
        /// <param name="orderby">--排序,FALSE-顺序,TRUE-倒序</param>
        /// <param name="pagesize">每页记录数</param>
        /// <param name="page">当前页</param>
        /// <param name="records">总记录数：为0则计算总记录数</param>
        /// <returns>分页记录</returns>
        public DataSet GetPageList1(string tb, string collist, string condition, string col, int coltype, bool orderby, int pagesize, int page, ref int records)
        {
            DataSet Datalist = new DataSet();
            SqlParameter[] parms;
            parms = new SqlParameter[]
            {
                new SqlParameter("@tb",SqlDbType.VarChar,200),
                new SqlParameter("@collist",SqlDbType.VarChar,800),
                new SqlParameter("@condition",SqlDbType.VarChar,800),
                new SqlParameter("@col",SqlDbType.VarChar,50),
                new SqlParameter("@coltype",SqlDbType.SmallInt,2),
                new SqlParameter("@orderby",SqlDbType.Bit,1),
                new SqlParameter("@pagesize",SqlDbType.Int,4),
                new SqlParameter("@page",SqlDbType.Int,4),
                new SqlParameter("@records",SqlDbType.Int,4)
            };
            parms[0].Value = tb;
            parms[1].Value = collist;
            parms[2].Value = condition;
            parms[3].Value = col;
            parms[4].Value = coltype;
            parms[5].Value = orderby;
            parms[6].Value = pagesize;
            parms[7].Value = page;
            parms[8].Value = records;
            parms[8].Direction = ParameterDirection.InputOutput;
            Datalist = DBUtility.SqlHelper.ExecuteDataset(sqlConnectionString, CommandType.StoredProcedure, "Sys_Page1", parms);
            records = Convert.ToInt32(parms[8].Value.ToString());
            return Datalist;

        }




        /// <summary>
        ///  分页2 wsd_page_2：单表任意排序 
        /// </summary>
        /// <param name="tb">表名  例: news</param>
        /// <param name="collist">要查询出的字段列表,*表示全部字段</param>
        /// <param name="where">查询条件，不带where 例：classid = 2</param>
        /// <param name="orderby">排序条件 例：order by tuijian desc,id desc</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="page">当前页码</param>
        /// <param name="records">总记录数：为0则重新计算</param>
        /// <returns>分页记录</returns>
        public DataSet GetPageList2(string tb, string collist, string where, string orderby, int pagesize, int page, ref int records)
        {
            DataSet Datalist = new DataSet();
            SqlParameter[] parms;
            parms = new SqlParameter[]
            {
                new SqlParameter("@tb",SqlDbType.VarChar,500),
                new SqlParameter("@collist",SqlDbType.VarChar,800),
                new SqlParameter("@where",SqlDbType.VarChar,800),
                new SqlParameter("@orderby",SqlDbType.VarChar,800),
                new SqlParameter("@pagesize",SqlDbType.Int,4),
                new SqlParameter("@page",SqlDbType.Int,4),
                new SqlParameter("@records",SqlDbType.Int,4)
            };
            parms[0].Value = tb;
            parms[1].Value = collist;
            parms[2].Value = where;
            parms[3].Value = orderby;
            parms[4].Value = pagesize;
            parms[5].Value = page;
            parms[6].Value = records;
            parms[6].Direction = ParameterDirection.InputOutput;
            Datalist = DBUtility.SqlHelper.ExecuteDataset(sqlConnectionString, CommandType.StoredProcedure, "Sys_Page2", parms);
            records = Convert.ToInt32(parms[6].Value.ToString());
            return Datalist;

        }




        /// <summary>
        /// 分页3： 单表/多表通用分页存储过程 wsd_page_3
        /// </summary>
        /// <param name="tb">表名 例： table1 inner join table2 on table1.xx=table2.xx </param>
        /// <param name="collist">需要获取字段 例: tabl1.xx,table2.*,注意，需要把排序列都选上</param>
        /// <param name="where">条件,不带where</param>
        /// <param name="orderby">最内层orderby(需要带上表前缀，注意asc 必须写上) 例: order by table1.xxx desc,table2.ad asc "</param>
        /// <param name="orderbyo">最外城orderby xxx.desc,ad asc</param>        
        /// <param name="pagesize">每页条数</param>
        /// <param name="page">页数</param>
        /// <param name="records">记录条数</param>
        /// <returns></returns>

        public DataSet GetPageList3(string tb, string collist, string where, string orderby, string orderbyo, int pagesize, int page, ref int records)
        {
            DataSet Datalist = new DataSet();
            SqlParameter[] parms;
            parms = new SqlParameter[]
            {
                new SqlParameter("@tb",SqlDbType.VarChar,800),
                new SqlParameter("@collist",SqlDbType.VarChar,800),
                new SqlParameter("@where",SqlDbType.VarChar,800),
                new SqlParameter("@orderby",SqlDbType.VarChar,800),
                new SqlParameter("@orderbyo",SqlDbType.VarChar,800),
                new SqlParameter("@pagesize",SqlDbType.Int,4),
                new SqlParameter("@page",SqlDbType.Int,4),
                new SqlParameter("@records",SqlDbType.Int,4)
            };
            parms[0].Value = tb;
            parms[1].Value = collist;
            parms[2].Value = where;
            parms[3].Value = orderby;
            parms[4].Value = orderbyo;
            parms[5].Value = pagesize;
            parms[6].Value = page;
            parms[7].Value = records;
            parms[7].Direction = ParameterDirection.InputOutput;
            Datalist = DBUtility.SqlHelper.ExecuteDataset(sqlConnectionString, CommandType.StoredProcedure, "Sys_Page3", parms);
            records = Convert.ToInt32(parms[7].Value.ToString());
            return Datalist;

        }

        /// <summary>
        ///  分页4 wsd_page_4：单表任意排序 
        /// </summary>
        /// <param name="tb">表名  例: news</param>
        /// <param name="collist">要查询出的字段列表,*表示全部字段</param>
        /// <param name="where">查询条件，不带where 例：classid = 2</param>
        /// <param name="orderby">排序条件 例：order by tuijian desc,id desc</param>
        /// <param name="pagesize">每页条数</param>
        /// <param name="page">当前页码</param>
        /// <param name="records">总记录数：为0则重新计算</param>
        /// <returns>分页记录</returns>
        public DataSet GetPageList4(string tb, string collist, string where, string orderby, int pagesize, int page, ref int records)
        {
            DataSet Datalist = new DataSet();
            SqlParameter[] parms;
            parms = new SqlParameter[]
            {
                new SqlParameter("@tb",SqlDbType.VarChar,500),
                new SqlParameter("@collist",SqlDbType.VarChar,800),
                new SqlParameter("@where",SqlDbType.VarChar,800),
                new SqlParameter("@orderby",SqlDbType.VarChar,800),
                new SqlParameter("@pagesize",SqlDbType.Int,4),
                new SqlParameter("@page",SqlDbType.Int,4),
                new SqlParameter("@records",SqlDbType.Int,4)
            };
            parms[0].Value = tb;
            parms[1].Value = collist;
            parms[2].Value = where;
            parms[3].Value = orderby;
            parms[4].Value = pagesize;
            parms[5].Value = page;
            parms[6].Value = records;
            parms[6].Direction = ParameterDirection.InputOutput;
            Datalist = DBUtility.SqlHelper.ExecuteDataset(sqlConnectionString, CommandType.StoredProcedure, "Sys_Page4", parms);
            records = Convert.ToInt32(parms[6].Value.ToString());
            return Datalist;

        }
        #endregion
    }
}
