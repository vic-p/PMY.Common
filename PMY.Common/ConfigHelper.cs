using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMY.Common
{
    public class ConfigHelper
    {
        public static string GetAppSettings(string key)
        {
            //根据并不存在的Key值访问<add>元素，不会导致异常，会返回null
            string strValue = ConfigurationManager.AppSettings[key];
            return strValue;
            
        }

        public static string GetConnectionString(string key)
        {
            //根据并不存在的Key值访问<add>元素，不会导致异常，会返回null
            string strValue = ConfigurationManager.ConnectionStrings[key].ConnectionString;
            return strValue;

        }
    }
}
