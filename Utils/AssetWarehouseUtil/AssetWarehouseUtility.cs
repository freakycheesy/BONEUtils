using BoneLib;
using BoneLib.BoneMenu;
using BONEUtils.Developer;
using BONEUtils.Utils.AssetWarehouseUtil.CratePages;
using Il2CppSLZ.Marrow.Warehouse;
using UnityEngine;

namespace BONEUtils.Utils.AssetWarehouseUtil {
    public class AssetWarehouseUtility : Utility {
        public static Page Page;
        public static Page PalletsPage;
        public static Page CratesPage;

        public static bool ShowRedacted { get; set; } = true;
        public static bool ShowInternal { get; set; } = true;
        public static bool ShowUnlockable { get; set; } = true;

        public static bool IncludeBarcodes { get; set; } = true;
        public static bool IncludeTags { get; set; } = true;
        public static bool IncludeAuthors { get; set; } = true;
        public static bool IncludeTitles { get; set; } = true;
        public static bool CaseSensitive { get; set; } = false;
        public const int MaxElements = 10;
        public static string SearchQuery;

        private void BoneMenuCreator() {
            Page = UICore.RootPage.CreatePage("Asset Warehouse", Color.red);
            Page.CreateFunction("Refresh", Color.green, Refresh);
            Page.CreateString("Search Query", Color.green, SearchQuery, Search);
            FitlerOptions();
            QueryOptions();
            PalletsPage = Page.CreatePage("Pallets", Color.green, MaxElements);
            CratesPage = Page.CreatePage("Crates", Color.blue, MaxElements);
        }

        private static void QueryOptions() {
            var queryOptions = Page.CreatePage("Query Options", Color.white);
            queryOptions.CreateBool("Include Barcodes", Color.white, IncludeBarcodes, (a) => IncludeBarcodes = a);
            queryOptions.CreateBool("Include Tags", Color.white, IncludeTags, (a) => IncludeTags = a);
            queryOptions.CreateBool("Include Authors", Color.white, IncludeAuthors, (a) => IncludeAuthors = a);
            queryOptions.CreateBool("Include Titles", Color.white, IncludeTitles, (a) => IncludeTitles = a);
            queryOptions.CreateBool("Case Sensitive", Color.white, CaseSensitive, (a) => CaseSensitive = a);
        }

        private static void FitlerOptions() {
            var filters = Page.CreatePage("Filters", Color.white);
            filters.CreateBool("Show Redacted", Color.white, ShowRedacted, (a) => ShowRedacted = a);
            filters.CreateBool("Show Internal", Color.white, ShowInternal, (a) => ShowInternal = a);
            filters.CreateBool("Show Unlockable", Color.white, ShowUnlockable, (a) => ShowUnlockable = a);
        }
        public void Refresh() {
            _RefreshThread();
        }
        private async void _RefreshThread() {
            RemovePallets();
            WarehouseData.GenerateCratesData();
        }

        private static void RemovePallets() {
            PalletsPage.RemoveAll();
            CratesPage.RemoveAll();
            CratePage.CratePages = new List<CratePage>();
        }

        public void Search(string query) {
            SearchQuery = query;
            Refresh();
        }

        /*
        public void LoadLevel(Crate value) {
            Logger.lo("Trying to Load Scene");
            SceneStreamer.Load(value.Barcode);
        }
        public void SelectSpawnable(Crate value) {
            MelonLogger.Msg("Trying to Select Spawnable");
            SpawnGunPatch.SwapSpawnGunCrate(value as SpawnableCrate);
        }

        public void SwapAvatar(Crate value) {
            MelonLogger.Msg("Trying to Swap Avatar");
            Player.RigManager.SwapAvatarCrate(value.Barcode);
        }*/

        protected override void OnLoad() {
            Developer.Logger.Log("Initialized.");
            WarehouseData.OnCratesGenerated += CreateCratePages;
            Hooking.OnLevelLoaded += (_) => RemovePallets();
            Hooking.OnLevelUnloaded += RemovePallets;
            Hooking.OnUIRigCreated += RemovePallets;
            BoneMenuCreator();

            SpawnableCratePage.Init();
            LevelCratePage.Init();
            AvatarCratePage.Init();
        }

        private static void CreateCratePages() {
            foreach (var item in WarehouseData.Crates) {
                CratePage.CreatePage(item);
            }
        }
    }
}
