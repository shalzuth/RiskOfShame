using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono.Cecil;

namespace Loader
{
    class Program
    {
        static void Main(string[] args)
        {
            var dll = AssemblyDefinition.ReadAssembly("RiskOfShame.dll");
            var rand = Guid.NewGuid().ToString().Replace("-", "");
            dll.Name.Name += rand;
            dll.MainModule.Name += rand;
            dll.MainModule.Types.ToList().ForEach(t => t.Namespace += rand);
            var dllBytes = new Byte[0];
            using (var newDll = new MemoryStream())
            {
                dll.Write(newDll);
                dllBytes = newDll.ToArray();
            }

            var ror2 = System.Diagnostics.Process.GetProcessesByName("Risk of Rain 2");
            var injector = new SharpMonoInjector.Injector("Risk of Rain 2");
            injector.Inject(dllBytes, dll.Name.Name, "Loader", "Load");
            //Thread.Sleep(10000);
            Environment.Exit(0);
        }
    }
}
