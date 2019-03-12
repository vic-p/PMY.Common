using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMY.Common.SQLHelper
{
    public interface ISqlHelper
    {
        /// <summary>
        /// 提交sql语句执行（增删改）
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="spms"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string sql, params IDataParameter[] spms);

        /// <summary>
        /// 提交sql语句返回首行首列的值
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="spms"></param>
        /// <returns></returns>
        object ExecuteScalar(string sql, params IDataParameter[] spms);

        /// <summary>
        /// 提交sql语句返回读取器
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="spms"></param>
        /// <returns></returns>
        IDataReader ExecuteReader(string sql, params IDataParameter[] spms);

        /// <summary>
        /// 查询实体类对象集合
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="spms"></param>
        /// <returns></returns>
        List<T> GetList<T>(string sql, params IDataParameter[] spms) where T : new();

        /// <summary>
        /// 查询单个实体类对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="spms"></param>
        /// <returns></returns>
        T GetSingle<T>(string sql, params IDataParameter[] spms) where T : new();

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="spms">参数</param>
        /// <returns></returns>
        DataTable GetDataTable(string sql, params IDataParameter[] spms);

        /// <summary>
        /// 查询记录条数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="spms"></param>
        /// <returns></returns>
        int GetCount(string sql, params IDataParameter[] spms);
    }
}
