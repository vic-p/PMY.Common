using Microsoft.Practices.Unity.InterceptionExtension;
using PMY.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMY.CommonUnity.AOP.Handler
{
    public class BeforeHandler : ICallHandler
    {
        public int Order { get; set; }

        private Logger logger = new Logger(typeof(BeforeHandler));

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var para in input.Inputs)
            {
                sb.AppendFormat("Para={0} ", para == null ? "null" : para.ToString());
            }
            //Console.WriteLine("日志已记录，开始请求{0}", sb.ToString());
            logger.Info($"日志已记录，开始请求{sb.ToString()}");
            return getNext()(input, getNext);
        }
    }
}
