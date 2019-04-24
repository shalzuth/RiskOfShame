using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using RiskOfShame.Loader.Core;

namespace RiskOfShame.Loader
{
    public class Status
    {
        private static String _text;
        public static String Text
        {
            get { return _text; }
            set
            {
                _text = value;
                OnTextChanged(EventArgs.Empty);
            }
        }
        public static event EventHandler TextChanged;
        protected static void OnTextChanged(EventArgs e)
        {
            TextChanged?.Invoke(null, e);
        }
        static Status()
        {
            TextChanged += (sender, e) => { return; };
        }
    }
    public partial class TempMainWindow
    {
        public TempMainWindow()
        {
            InitializeComponent();
            Task.Run(() => { Init(); });
        }
        public void Init()
        {
            Status.Text = "Waiting for Risk of Rain 2 to open";
            var ror2 = System.Diagnostics.Process.GetProcessesByName("Risk of Rain 2");
            if (ror2.Length == 0)
                Console.WriteLine("Waiting for Risk of Rain 2 to open...");
            while (ror2.Length == 0)
            {
                Console.WriteLine("Waiting for Risk of Rain 2 to open...");
                Thread.Sleep(5000);
                ror2 = System.Diagnostics.Process.GetProcessesByName("Risk of Rain 2");
            }
            Status.Text = "Game Open - Injecting";
            try
            {
                String randString = "aa" + Guid.NewGuid().ToString().Substring(0, 8);
                var gameDir = System.IO.Path.GetDirectoryName(ror2[0].MainModule.FileName);
                var gameName = System.IO.Path.GetFileName(gameDir);
                var unityDllPath = gameDir + @"\" + gameName + @"_Data\Managed\";
                Compiler.UnityDllPath = unityDllPath;
                Status.Text = "Injecting - Game @ " + unityDllPath;
                Compiler.UpdateSources();
                Injector.Inject("Risk of Rain 2", Compiler.CompileDll(randString), randString, "Loader", "Load");
                System.IO.File.Delete(randString + ".dll");
                if (System.IO.File.Exists(randString + ".pdb"))
                    System.IO.File.Delete(randString + ".pdb");
                Status.Text = "Injected, closing app shortly";
                //Thread.Sleep(10000);
                Environment.Exit(0);
            }
            catch (Exception e)
            {
                Status.Text = e.Message;
            }
        }
    }
}
