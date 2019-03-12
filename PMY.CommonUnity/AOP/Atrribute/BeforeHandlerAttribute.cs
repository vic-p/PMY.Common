using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using PMY.CommonUnity.AOP.Handler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PMY.CommonUnity.AOP.Atrribute
{
    public class BeforeHandlerAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return new BeforeHandler() { Order = this.Order };
        }
    }
}
