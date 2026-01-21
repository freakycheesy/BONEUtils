using BoneLib;
using BoneLib.BoneMenu;
using BONEUtils.Developer;
using BONEUtils.Utils.AssetWarehouseUtil.CratePages;
using Il2CppSLZ.Marrow.Warehouse;
using MelonLoader;
using UnityEngine;

namespace BONEUtils.Utils.AssetWarehouseUtil {
    public class AssetWarehouseUtility : Utility {
        public static Page Page;
        public static Page PalletsPage;
        public static Page CratesPage;

        public static MelonPreferences_Entry<bool> ShowRedacted { get; set; } = Core.Preferences.CreateEntry<bool>("ShowRedacted", true);
        public static MelonPreferences_Entry<bool> ShowInternal { get; set; } = Core.Preferences.CreateEntry<bool>("ShowInternal", true);
        public static MelonPreferences_Entry<bool> ShowUnlockable { get; set; } = Core.Preferences.CreateEntry<bool>("ShowUnlockable", true);

        public static MelonPreferences_Entry<bool> IncludeBarcodes { get; set; } = Core.Preferences.CreateEntry<bool>("IncBarcodes", true);
        public static MelonPreferences_Entry<bool> IncludeTags { get; set; } = Core.Preferences.CreateEntry<bool>("IncTags", true);
        public static MelonPreferences_Entry<bool> IncludeAuthors { get; set; } = Core.Preferences.CreateEntry<bool>("IncAuthors", true);
        public static MelonPreferences_Entry<bool> IncludeTitles { get; set; } = Core.Preferences.CreateEntry<bool>("IncTitles", true);
        public static MelonPreferences_Entry<bool> CaseSensitive { get; set; } = Core.Preferences.CreateEntry<bool>("CaseSensitive", false);
        public const int MaxElements = 10;
        public static string SearchQuery;

        private void BoneMenuCreator() {
            Page = UICore.UtilitiesPage.CreatePage("Asset Warehouse", OverrideColor.red);
            Page.CreateFunction("Refresh", OverrideColor.green, Refresh);
            Page.CreateString("Search Query", OverrideColor.green, SearchQuery, Search);
            FitlerOptions();
            QueryOptions();
            PalletsPage = Page.CreatePage("Pallets", OverrideColor.green, MaxElements);
        }

        private static void QueryOptions() {
            var queryOptions = Page.CreatePage("Query Options", Color.white);
            queryOptions.CreateBool("Include Barcodes", Color.white, IncludeBarcodes.Value, (a) => IncludeBarcodes.Value = a);
            queryOptions.CreateBool("Include Tags", Color.white, IncludeTags.Value, (a) => IncludeTags.Value = a);
            queryOptions.CreateBool("Include Authors", Color.white, IncludeAuthors.Value, (a) => IncludeAuthors.Value = a);
            queryOptions.CreateBool("Include Titles", Color.white, IncludeTitles.Value, (a) => IncludeTitles.Value = a);
            queryOptions.CreateBool("Case Sensitive", Color.white, CaseSensitive.Value, (a) => CaseSensitive.Value = a);
        }

        private static void FitlerOptions() {
            var filters = Page.CreatePage("Filters", Color.white);
            filters.CreateBool("Show Redacted", Color.white, ShowRedacted.Value, (a) => ShowRedacted.Value = a);
            filters.CreateBool("Show Internal", Color.white, ShowInternal.Value, (a) => ShowInternal.Value = a);
            filters.CreateBool("Show Unlockable", Color.white, ShowUnlockable.Value, (a) => ShowUnlockable.Value = a);
        }
        public void Refresh() {
            _RefreshThread();
        }
        private async void _RefreshThread() {
            RemovePallets();
            WarehouseData.GenerateCratesData();
        }

        private static void RemovePallets() {
            ScannablePage.Instances.Clear();
            WarehouseData.SelectedPallets.Clear();
            PalletsPage.RemoveAll();
        }

        public void Search(string query) {
            SearchQuery = query;
            Refresh();
        }

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
            foreach (var item in WarehouseData.SelectedPallets) {
                ScannablePage.CreatePage(item);
            }
        }
    }
}
