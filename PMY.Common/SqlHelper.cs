using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PMY.Common
{
    public class SqlHelper
    {
        private static string strConnection = "";
         static SqlHelper()
        {
            strConnection = ConfigHelper.GetAppSettings("SQLConnString");
            if (string.IsNullOrEmpty(strConnection))
            {
                throw new Exception("未在配置文件的<AppSettings>下配置SQLConnString eg:<add key=\"SQLConnString\" value=\"server =.; database = 数据库名; uid = 用户名; pwd = 密码\" /> ");
            }
        }
        /// <summary>
        /// 非查询命令（增删改）操作
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns>影响条数</returns>
        public static int ExecuteNonQuery(IDbCommand cmd)
        {
            return cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 查询操作
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="cmd"></param>
        /// <returns>查询结果</returns>
        public static List<T> QueryList<T>(IDbCommand cmd) where T : new()
        {
            return TransList<T>(cmd.ExecuteReader());
        }

        /// <summary>
        /// 把DataReader的数据转成List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static List<T> TransList<T>(IDataReader reader) where T : new()
        {
            List<T> tList = new List<T>();
            Type type = typeof(T);
            var properties = type.GetProperties();
            if (reader.Read())
            {
                do
                {
                    T t = new T();
                    //foreach (PropertyInfo p in properties)
                    //{
                    //    p.SetValue(t, Convert.ChangeType(reader[p.Name], p.PropertyType));
                    //}
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        var propertyInfo = properties.Where(p => p.Name == reader.GetName(i));
                        if (propertyInfo.Count() > 0)
                        {
                            propertyInfo.First().SetValue(t, Convert.ChangeType(reader[i], propertyInfo.First().PropertyType));
                        }

                    }
                    tList.Add(t);
                }
                while (reader.Read());
            }
            return tList;
        }

        /// <summary>
        /// 执行SQL命令
        /// </summary>
        /// <typeparam name="T">返回结果类型</typeparam>
        /// <param name="sql">SQL命令</param>
        /// <param name="func">执行操作</param>
        /// <returns></returns>
        public static T ExcuteSql<T>(string sql, Func<IDbCommand, T> func)
        {
            using (SqlConnection conn = new SqlConnection(strConnection))
            {
                conn.Open();
                //还可以扩展事务
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                cmd.CommandType = CommandType.Text;
                return func(cmd);
            }
        }
    }
}
