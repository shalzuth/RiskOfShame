using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RiskOfShame.Loader.Core
{
    public class Config
    {
        public static Config Instance { get; } = Load();

        public ObservableCollection<Mod> Mods { get; set; } = new ObservableCollection<Mod>();

        public string Username { get; set; }

        public string PasswordHash { get; set; }


        public void Save()
        {
            //File.WriteAllText("Config.json", JsonConvert.SerializeObject(this, Formatting.Indented));
        }

        public static Config Load()
        {
            return new Config();
            //return !File.Exists("Config.json") ? new Config() : JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));
        }
    }
}
