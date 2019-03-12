using Microsoft.Practices.Unity.InterceptionExtension;
using PMY.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMY.CommonUnity.AOP.Handler
{
    public class ExceptionHandler : ICallHandler
    {
        public int Order { get; set; }

        private Logger logger = new Logger(typeof(BeforeHandler));

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            IMethodReturn methodReturn = getNext()(input, getNext);
            if (methodReturn.Exception == null)
            {
                logger.Info("无异常。");
            }
            else
            {
                //Console.WriteLine("异常:{0}", methodReturn.Exception.Message);
                logger.Error($"异常:{methodReturn.Exception.Message}", methodReturn.Exception);
            }
            return methodReturn;
        }
    }
}
