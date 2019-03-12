using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMY.CommonUnity
{
    public class UnitySingleton
    {
        private UnitySingleton() { }

        private static UnitySingleton _unitySingleton;

        //ioc容器
        public IUnityContainer container;

        public static UnitySingleton CreateUnitySingleton(string containerName = "MyContainer",string configPath= "Config\\unity.Config")
        {
            if (!File.Exists(Path.Combine(AppDomain.CurrentDomain.BaseDirectory + configPath)))
                return null;
            if (_unitySingleton == null || _unitySingleton.container == null)
            {
                ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
                fileMap.ExeConfigFilename = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + configPath);
                Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
                UnityConfigurationSection section = (UnityConfigurationSection)configuration.GetSection(UnityConfigurationSection.SectionName);
                _unitySingleton = new UnitySingleton()
                {
                    container = new UnityContainer().LoadConfiguration(section, containerName)
                };
            }
            return _unitySingleton;
        }

        //IOC注入实体
        public T GetInstance<T>()
        {
            return _unitySingleton.container.Resolve<T>();
        }
    }
}
