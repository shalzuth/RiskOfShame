using System;
using System.IO;
using System.Linq;
using UnityEngine;

namespace RiskOfShame
{
    public class LogBookMenu : MonoBehaviour
    {
        static RoR2.UI.LogBook.EntryStatus IsAvailable(RoR2.UserProfile userProfile, RoR2.UI.LogBook.Entry entry)
        {
            return RoR2.UI.LogBook.EntryStatus.Available;
        }
        static RoR2.UI.TooltipContent Tooltip(RoR2.UserProfile userProfile, RoR2.UI.LogBook.Entry entry, RoR2.UI.LogBook.EntryStatus entryStatus)
        {
            var tooltip = new RoR2.UI.TooltipContent();
            tooltip.titleToken = entry.nameToken;
            tooltip.titleColor = Color.blue;
            tooltip.bodyToken = ((ExtraData)entry.extraData).Data;
            return tooltip;
        }
        void Content(RoR2.UI.LogBook.PageBuilder builder)
        {
            builder.AddSimpleTextPanel(((ExtraData)builder.entry.extraData).Data);
            builder.AddNotesPanel(((ExtraData)builder.entry.extraData).Notes);
        }
        class ExtraData
        {
            public String Data;
            public String Notes;
        }
        void OnEnable()
        {
            var categories = RoR2.UI.LogBook.LogBookController.categories.ToList();
            var category = new RoR2.UI.LogBook.CategoryDef();
            category.nameToken = "Mods";
            category.iconPrefab = Resources.Load<GameObject>("Prefabs/UI/Logbook/MonsterEntryIcon");
            var icon = new Texture2D(1, 1);
            var bytes = new System.Net.WebClient().DownloadData("https://avatars0.githubusercontent.com/u/5614665?s=460&v=4");
            icon.LoadImage(bytes);
            var sprite = Sprite.Create(icon, new Rect(0, 0, icon.width, icon.height), new Vector2(0, 0));

            var types = System.Reflection.Assembly.GetExecutingAssembly().GetTypes().ToList().Where(t => t.BaseType == typeof(MonoBehaviour));
            category.entries = types.Select(t => new RoR2.UI.LogBook.Entry()
            {
                nameToken = t.Name,
                categoryTypeToken = "By Shalzuth",
                color = RoR2.ColorCatalog.GetColor(RoR2.ColorCatalog.ColorIndex.NormalDifficulty),
                iconTexture = sprite.texture,
                extraData = new ExtraData { Data = "example data", Notes = "example notes" },
                modelPrefab = null,
                getStatus = new Func<RoR2.UserProfile, RoR2.UI.LogBook.Entry, RoR2.UI.LogBook.EntryStatus>(IsAvailable),
                getTooltipContent = new Func<RoR2.UserProfile, RoR2.UI.LogBook.Entry, RoR2.UI.LogBook.EntryStatus, RoR2.UI.TooltipContent>(Tooltip),
                addEntries = new Action<RoR2.UI.LogBook.PageBuilder>(Content)
            }).ToArray();
            categories.Add(category);
            RoR2.UI.LogBook.LogBookController.categories = categories.ToArray();

            RoR2.ViewablesCatalog.Node logBook = new RoR2.ViewablesCatalog.Node("Logbook", true, null);
            RoR2.ViewablesCatalog.Node ModsFolder = new RoR2.ViewablesCatalog.Node(category.nameToken, true, logBook);
            category.viewableNode = ModsFolder;
            for (int j = 0; j < category.entries.Length; j++)
            {
                var modEntry = new RoR2.ViewablesCatalog.Node(category.entries[j].nameToken, false, ModsFolder);
                modEntry.shouldShowUnviewed = ((RoR2.UserProfile userProfile) => true);
                category.entries[j].viewableNode = modEntry;
            }
        }
    }
}