using BoneLib.BoneMenu;
using UnityEngine;

namespace BONEUtils.Developer {
    public static class UICore {
        public static Page RootPage { get; private set; }
        public static Page UtilitiesPage {
            get; private set;
        }
        public static Page DangerousPage {
            get; private set;
        }
        public static Page SettingsPage {
            get; private set;
        }
        internal static void Init() {
            RootPage = Page.Root.CreatePage("BONE Utils", Color.blue);
            UtilitiesPage = RootPage.CreatePage("Utilities", Color.cyan);
            DangerousPage = RootPage.CreatePage("Dangerous", Color.red);
            SettingsPage = RootPage.CreatePage("Settings", Color.yellow);
        }
    }
}
