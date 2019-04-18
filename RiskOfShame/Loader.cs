using System.Reflection;
using System.Linq;
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
            UnityEngine.Object.DontDestroyOnLoad(BaseObject);
            BaseObject.SetActive(false);
            var types = Assembly.GetExecutingAssembly().GetTypes().ToList().Where(t => t.BaseType == typeof(UnityEngine.MonoBehaviour) && !t.IsNested);
            foreach(var type in types)
            {
                var component = (UnityEngine.MonoBehaviour)BaseObject.AddComponent(type);
                component.enabled = false;
            }
            BaseObject.GetComponent<Menu>().enabled = true;
            BaseObject.SetActive(true);
        }

        public static void Unload()
        {
            UnityEngine.Object.Destroy(BaseObject);
        }
    }
}
