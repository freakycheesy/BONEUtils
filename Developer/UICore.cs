using BoneLib.BoneMenu;
using UnityEngine;

namespace BONEUtils.Developer {
    public static class UICore {
        public static Page RootPage { get; private set; }
        internal static void Init() {
            RootPage = Page.Root.CreatePage("BONE Utils", Color.blue);
        }
    }
}
