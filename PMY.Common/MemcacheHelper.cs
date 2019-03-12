using Memcached.ClientLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMY.Common
{
    public class MemcacheHelper
    {
        private static readonly MemcachedClient mc = null;

        static MemcacheHelper()
        {
            //最好放在配置文件中
            //string[] serverlist = { "127.0.0.1:11211", "10.0.0.132:11211" };
            string strMemcacheServerlist = ConfigHelper.GetAppSettings("MemcacheServerlist");
            if (string.IsNullOrEmpty(strMemcacheServerlist))
            {
                throw new Exception("未在配置文件的<AppSettings>下配置MemcacheServerlist；eg:<add key=\"MemcacheServerlist\" value=\"127.0.0.1:11211, 10.0.0.132:11211\" />");
            }
            string[] serverlist = strMemcacheServerlist.Split(new char[]{ ','},StringSplitOptions.RemoveEmptyEntries);

            //初始化池
            SockIOPool pool = SockIOPool.GetInstance();
            pool.SetServers(serverlist);

            pool.InitConnections = 3;
            pool.MinConnections = 3;
            pool.MaxConnections = 5;

            pool.SocketConnectTimeout = 1000;
            pool.SocketTimeout = 3000;

            pool.MaintenanceSleep = 30;
            pool.Failover = true;

            pool.Nagle = false;
            pool.Initialize();

            // 获得客户端实例
            mc = new MemcachedClient();
            mc.EnableCompression = false;
        }
        /// <summary>
        /// 存储数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Set(string key, object value)
        {
            return mc.Set(key, value);
        }

        /// <summary>
        /// 在过期时刻前存储数据
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="time">过期时刻，这里可以用DateTime.Now.AddMinutes(过期时间段)表示多久过期</param>
        /// <returns></returns>
        public static bool Set(string key, object value, DateTime time)
        {
            return mc.Set(key, value, time);
        }

        /// <summary>
        /// 随机生成主键存储数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="time"></param>
        /// <returns>key</returns>
        public static string Set(object value, DateTime time)
        {
            string key = Guid.NewGuid().ToString();
            bool isSet = Set(key, value, time);
            if (isSet)
            {
                return key;
            }
            else
            {
                return null;
            }

        }

        /// <summary>
        /// 获取数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object Get(string key)
        {
            return mc.Get(key);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool Delete(string key)
        {
            if (mc.KeyExists(key))
            {
                return mc.Delete(key);

            }
            return false;

        }
    }
}
