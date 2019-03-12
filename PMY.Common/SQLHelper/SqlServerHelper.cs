using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMY.Common.SQLHelper
{
    public class SqlServerHelper : ISqlHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private string strConn = ConfigurationManager.ConnectionStrings["sqlConn"].ConnectionString;

        /// <summary>
        /// 准备Command对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="spms"></param>
        /// <returns></returns>
        private SqlCommand PrepareCommand(string sql, params IDataParameter[] spms)
        {
            SqlConnection conn = new SqlConnection(strConn);
            SqlCommand cmd = new SqlCommand(sql, conn);
            if (spms != null)
                cmd.Parameters.AddRange(spms);
            return cmd;
        }

        /// <summary>
        /// 提交sql语句执行（增删改）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="spms"></param>
        /// <returns>影响行数</returns>
        public int ExecuteNonQuery(string sql, params IDataParameter[] spms)
        {
            int result = 0;
            SqlCommand cmd = PrepareCommand(sql, spms);
            try
            {
                cmd.Connection.Open();
                result = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
            return result;
        }

        /// <summary>
        /// 提交sql语句返回首行首列的值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="spms"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql, params IDataParameter[] spms)
        {
            object result = null;
            SqlCommand cmd = PrepareCommand(sql, spms);
            try
            {
                cmd.Connection.Open();
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
                throw new Exception(ex.Message);
            }
            finally
            {
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            }
            return result;
        }

        /// <summary>
        /// 提交sql语句返回读取器
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="spms"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string sql, params IDataParameter[] spms)
        {
            SqlDataReader reader = null;
            SqlCommand cmd = PrepareCommand(sql, spms);
            try
            {
                cmd.Connection.Open();
                //关闭reader对象，其对应的连接对象自动关闭
                reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                if (reader != null)
                    reader.Close();
                if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
                throw new Exception(ex.Message);
            }
            return reader;
        }

        /// <summary>
        /// 查询实体类对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="spms"></param>
        /// <returns></returns>
        public List<T> GetList<T>(string sql, params IDataParameter[] spms) where T : new()
        {
            List<T> list = new List<T>();
            Type type = typeof(T);
            var properties = type.GetProperties();
            SqlDataReader reader = (SqlDataReader)ExecuteReader(sql, spms);
            while (reader.Read())
            {
                T t = new T();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var propertyInfo = properties.Where(p => p.Name == reader.GetName(i));
                    if (propertyInfo.Count() > 0 && reader[i]!=DBNull.Value)
                    {
                        Type PropertyType = propertyInfo.First().PropertyType;
                        Type underlyingType = Nullable.GetUnderlyingType(PropertyType);
                        propertyInfo.First().SetValue(t, Convert.ChangeType(reader[i], underlyingType ?? PropertyType));
                    }

                }
                list.Add(t);
            }
            reader.Close();
            return list;
        }

        /// <summary>
        /// 查询单个实体类对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="spms"></param>
        /// <returns></returns>
        public T GetSingle<T>(string sql, params IDataParameter[] spms) where T : new()
        {
            T t = default(T);
            Type type = typeof(T);
            var properties = type.GetProperties();
            SqlDataReader reader = (SqlDataReader)ExecuteReader(sql, spms);
            if (reader.Read())
            {
                t = new T();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var propertyInfo = properties.Where(p => p.Name == reader.GetName(i));
                    if (propertyInfo.Count() > 0)
                    {
                        propertyInfo.First().SetValue(t, Convert.ChangeType(reader[i], propertyInfo.First().PropertyType));
                    }

                }
            }
            reader.Close();
            return t;
        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="spms">参数</param>
        /// <returns></returns>
        public DataTable GetDataTable(string sql, params IDataParameter[] spms)
        {
            SqlCommand cmd = PrepareCommand(sql, spms);

            SqlDataAdapter da = new SqlDataAdapter(cmd); //创建DataAdapter数据适配器实例
            DataSet ds = new DataSet();//创建DataSet实例
            da.Fill(ds, "tables");//使用DataAdapter的Fill方法(填充)，调用SELECT命令
            if (cmd.Connection.State != ConnectionState.Closed)
                    cmd.Connection.Close();
            return ds.Tables[0];
        }
        
        /// <summary>
        /// 查询记录条数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="spms"></param>
        /// <returns></returns>
        public int GetCount(string sql, params IDataParameter[] spms)
        {
            return (int)ExecuteScalar(sql, spms);
        }

    }
}
