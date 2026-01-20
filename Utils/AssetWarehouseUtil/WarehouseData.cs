using BONEUtils.Developer;
using BONEUtils.Developer.Extensions;
using Il2CppSLZ.Marrow.Warehouse;
using Steamworks.Ugc;

namespace BONEUtils.Utils.AssetWarehouseUtil {
    public static class WarehouseData {
        public static List<Crate> Crates = new();
        public static List<T> FilterAndCleanCrates<T>(this List<T> Crates) where T : Crate {
            if (string.IsNullOrWhiteSpace(AssetWarehouseUtility.SearchQuery)) {
                DevUtils.Notify("Please enter something in the query");
                return new();
            }

            Crates.RemoveAll(x => x.Redacted && !AssetWarehouseUtility.ShowRedacted);
            Crates.RemoveAll(x => x.Pallet.Internal && !AssetWarehouseUtility.ShowInternal);
            Crates.RemoveAll(x => x.Unlockable && !AssetWarehouseUtility.ShowUnlockable);

            var comparison = AssetWarehouseUtility.CaseSensitive ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            Predicate<Crate> match = x => x._barcode._id.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeBarcodes || x._tags.Contains(AssetWarehouseUtility.SearchQuery) && AssetWarehouseUtility.IncludeTags || x._title.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeTitles || x.Pallet.Author.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeAuthors;
            Crates = Crates.ToList().FindAll(match);
            return Crates;
        }
        public static Action OnCratesGenerated;
        public static void GenerateCratesData() {
            try {
                DevUtils.Notify("Digging in thy crates twin");
                GenerateCrateDataTask();
            }
            catch (Exception ex) {
                Logger.Error("ERROR", ex);
            }
        }

        private static void GenerateCrateDataTask() {
            try {
                Crates = new();
                foreach (var crate in AssetWarehouse.Instance.GetCrates().ToList().FilterAndCleanCrates()) {
                    if (Crates.Contains(crate))
                        continue;
                    Crates.Add(crate);
                }
                if (Crates.Count < 1)
                    return;
                OnCratesGenerated?.Invoke();
                DevUtils.Notify("Digged up thy crates twin");
            }
            catch (Exception ex) {
                Logger.Error("ERROR", ex);
            }
        }
    }
}
