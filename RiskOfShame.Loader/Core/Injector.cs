using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskOfShame.Loader.Core
{
    public static class Injector
    {
        public static void Inject(String GameName, Byte[] Assembly, String Namespace, String Class, String Method)
        {
            var injector = new SharpMonoInjector.Injector(GameName);
            injector.Inject(Assembly, Namespace, Class, Method);
        }
    }
}
