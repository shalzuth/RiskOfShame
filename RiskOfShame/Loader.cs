namespace RiskOfShame
{
    public class Loader
    {
        public static UnityEngine.GameObject BaseObject;

        public static void Load()
        {
            //RoR2.RoR2Application.isModded = true;
            while (BaseObject = UnityEngine.GameObject.Find("Risk Of Shame"))
                UnityEngine.GameObject.Destroy(BaseObject);
            BaseObject = new UnityEngine.GameObject("Risk Of Shame");
            //BaseObject.AddComponent<Testing>();
            //BaseObject.AddComponent<EnemySurvivors>();//.enabled = false;

            BaseObject.AddComponent<DropItem>().enabled = false;
            BaseObject.AddComponent<ItemNotification>().enabled = false;
            BaseObject.AddComponent<AdvancedTooltips>().enabled = false;
            BaseObject.AddComponent<AlwaysSprint>().enabled = false;
            BaseObject.AddComponent<RemoveEmptyObjects>().enabled = false;
            BaseObject.AddComponent<RevealChests>().enabled = false;
            BaseObject.AddComponent<RevealLoot>().enabled = false;
            BaseObject.AddComponent<RevealTeleporter>().enabled = false;
            BaseObject.AddComponent<BigParty>().enabled = false;
            // BaseObject.AddComponent<SkipLevel>().enabled = false;
            //BaseObject.AddComponent<Unlock>().enabled = false;
            //BaseObject.AddComponent<Logger>().enabled = false;
            BaseObject.AddComponent<Menu>();
            UnityEngine.Object.DontDestroyOnLoad(BaseObject);
            //UnityEngine.Networking.NetworkServer.Spawn(BaseObject);
        }

        public static void Unload()
        {
            UnityEngine.Object.Destroy(BaseObject);
        }
    }
}
