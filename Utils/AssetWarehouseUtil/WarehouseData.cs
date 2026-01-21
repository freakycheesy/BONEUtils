using BONEUtils.Developer;
using BONEUtils.Developer.Extensions;
using Il2CppSLZ.Marrow.Warehouse;
using Steamworks.Ugc;

namespace BONEUtils.Utils.AssetWarehouseUtil {
    public static class WarehouseData {
        public static List<Pallet> SelectedPallets = new();
        public static List<PalletRep> FilterAndCleanPallets(this List<Pallet> Pallets){
            if (string.IsNullOrWhiteSpace(AssetWarehouseUtility.SearchQuery)) {
                DevUtils.Notify("Please enter something in the query");
                return new();
            }
            List<PalletRep> palletReps = new();
            foreach (var pallet in Pallets) {
                palletReps.Add(new() {
                    pallet = pallet,
                    crates = pallet.Crates.ToList().ToArray(),
                    dataCards = pallet.DataCards.ToList().ToArray(),
                });
            }
            List<PalletRep> list = palletReps.ToList();
            for (int i = 0; i < list.Count; i++) {
                PalletRep rep = ModifyRep(list[i]);
                if (rep.crates.Length < 1 && rep.dataCards.Length < 1) {
                    palletReps.Remove(rep);
                }
                palletReps[i] = rep;
            }
            return palletReps;
        }

        private static PalletRep ModifyRep(PalletRep rep) {
            ManageCrates(ref rep);
            ManageDataCards(ref rep);
            return rep;
        }

        private static void ManageCrates(ref PalletRep rep) {
            List<Crate> crates = rep.crates.ToList();
            crates.RemoveAll(x => x.Redacted && !AssetWarehouseUtility.ShowRedacted.Value || x.Pallet.Internal && !AssetWarehouseUtility.ShowInternal.Value || x.Unlockable && !AssetWarehouseUtility.ShowUnlockable.Value);

            var comparison = AssetWarehouseUtility.CaseSensitive.Value ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            Predicate<Crate> match = x => x._barcode._id.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeBarcodes.Value || x._tags.Contains(AssetWarehouseUtility.SearchQuery) && AssetWarehouseUtility.IncludeTags.Value || x._title.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeTitles.Value || x.Pallet.Author.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeAuthors.Value;
            rep.crates = crates.FindAll(match).ToArray();
        }

        private static void ManageDataCards(ref PalletRep rep) {
            List<DataCard> dataCards = rep.dataCards.ToList();
            dataCards.RemoveAll(x => x.Redacted && !AssetWarehouseUtility.ShowRedacted.Value || x.Pallet.Internal && !AssetWarehouseUtility.ShowInternal.Value || x.Unlockable && !AssetWarehouseUtility.ShowUnlockable.Value);

            var comparison = AssetWarehouseUtility.CaseSensitive.Value ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            Predicate<DataCard> match = x => x._barcode._id.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeBarcodes.Value || x._title.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeTitles.Value || x.Pallet.Author.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeAuthors.Value;
            rep.dataCards = dataCards.FindAll(match).ToArray();
        }

        public static Action OnCratesGenerated;
        public static void GenerateCratesData() {
            try {
                DevUtils.Notify("Digging in thy dataCards twin");
                GenerateCrateDataTask();
            }
            catch (Exception ex) {
                Logger.Error("ERROR", ex);
            }
        }

        private static void GenerateCrateDataTask() {
            try {
                SelectedPallets.Clear();
                foreach (var palletRep in AssetWarehouse.Instance.GetPallets().ToList().FilterAndCleanPallets()) {
                    if (Crates.Contains(crate))
                        continue;
                    Crates.Add(crate);
                }
                if (Crates.Count < 1)
                    return;
                OnCratesGenerated?.Invoke();
                DevUtils.Notify("Digged up thy dataCards twin");
            }
            catch (Exception ex) {
                Logger.Error("ERROR", ex);
            }
        }
        public struct PalletRep {
            public DataCard[] dataCards;
            public Crate[] crates;
            public Pallet pallet;
        }
    }
}
