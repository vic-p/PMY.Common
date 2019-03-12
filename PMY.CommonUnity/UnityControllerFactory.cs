using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace PMY.CommonUnity
{
    public class UnityControllerFactory : DefaultControllerFactory
    {
        private string _containerName;
        private string _configPath;
        public UnityControllerFactory(string containerName = "MyContainer", string configPath = "Config\\unity.Config")
        {
            _containerName = containerName;
            _configPath = configPath;
        }
        private IUnityContainer UnityContainer
        {
            get
            {
                return UnitySingleton.CreateUnitySingleton(_containerName, _configPath).container;
            }
        }

        /// <summary>
        /// 创建控制器对象
        /// </summary>
        /// <param name="requestContext"></param>
        /// <param name="controllerType"></param>
        /// <returns></returns>
        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (null == controllerType)
            {
                return null;
            }
            if (UnityContainer == null)
            {
                return null;
            }
            IController controller = (IController)this.UnityContainer.Resolve(controllerType);
            return controller;
        }
        /// <summary>
        /// 释放
        /// </summary>
        /// <param name="controller"></param>
        public override void ReleaseController(IController controller)
        {
            this.UnityContainer.Teardown(controller);//释放对象
        }
    }
}
