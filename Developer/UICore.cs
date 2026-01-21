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
            RootPage = Page.Root.CreatePage("BONE Utils", OverrideColor.lightBlue);
            UtilitiesPage = RootPage.CreatePage("Utilities", OverrideColor.lightBlue);
            DangerousPage = RootPage.CreatePage("Dangerous", OverrideColor.red);
            SettingsPage = RootPage.CreatePage("Settings", Color.yellow);
        }
    }
}
