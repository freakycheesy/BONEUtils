using BONEUtils.Developer;
using BONEUtils.Developer.Extensions;
using Il2CppSLZ.Marrow.Warehouse;
using Steamworks.Ugc;

namespace BONEUtils.Utils.AssetWarehouseUtil {
    public static class WarehouseData {
        public static List<PalletRep> SelectedPallets = new();
        public static List<PalletRep> FilterAndCleanPallets(this List<Pallet> Pallets){
            if (string.IsNullOrWhiteSpace(AssetWarehouseUtility.SearchQuery)) {
                DevUtils.Notify("Please enter something in the query");
                return new();
            }
            List<PalletRep> palletReps = new();
            foreach (var pallet in Pallets) {
                palletReps.Add(new(pallet));
            }
            for (int i = 0; i < palletReps.Count; i++) {
                PalletRep rep = ModifyRep(palletReps[i]);
                if (rep.Crates.Length < 1 && rep.DataCards.Length < 1) {
                    palletReps.RemoveAt(i);
                }
                palletReps[i] = rep;
            }
            return palletReps;
        }

        private static PalletRep ModifyRep(PalletRep rep) {
            rep = ManageCrates(rep); rep = ManageDataCards(rep);

            return rep;
        }

        private static PalletRep ManageCrates(PalletRep rep) {
            List<Crate> crates = rep.Crates.ToList();
            crates.RemoveAll(x => x.Redacted && !AssetWarehouseUtility.ShowRedacted.Value || x.Pallet.Internal && !AssetWarehouseUtility.ShowInternal.Value || x.Unlockable && !AssetWarehouseUtility.ShowUnlockable.Value);

            var comparison = AssetWarehouseUtility.CaseSensitive.Value ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            Predicate<Crate> match = x => x._barcode._id.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeBarcodes.Value || x._tags.Contains(AssetWarehouseUtility.SearchQuery) && AssetWarehouseUtility.IncludeTags.Value || x._title.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeTitles.Value || x.Pallet.Author.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeAuthors.Value;
            rep.Crates = crates.FindAll(match).ToArray();
            return rep;
        }

        private static PalletRep ManageDataCards(PalletRep rep) {
            List<DataCard> dataCards = rep.DataCards.ToList();
            dataCards.RemoveAll(x => x.Redacted && !AssetWarehouseUtility.ShowRedacted.Value || x.Pallet.Internal && !AssetWarehouseUtility.ShowInternal.Value || x.Unlockable && !AssetWarehouseUtility.ShowUnlockable.Value);

            var comparison = AssetWarehouseUtility.CaseSensitive.Value ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
            Predicate<DataCard> match = x => x._barcode._id.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeBarcodes.Value || x._title.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeTitles.Value || x.Pallet.Author.Contains(AssetWarehouseUtility.SearchQuery, comparison) && AssetWarehouseUtility.IncludeAuthors.Value;
            rep.DataCards = dataCards.FindAll(match).ToArray();
            return rep;
        }

        public static Action OnCratesGenerated;
        public static void GenerateCratesData() {
            try {
                SelectedPallets.Clear();
                DevUtils.Notify("Digging in thy dataCards twin");
                GenerateCrateDataTask();
                OnCratesGenerated?.Invoke();
            }
            catch (Exception ex) {
                Logger.Error("ERROR", ex);
            }
        }

        private static void GenerateCrateDataTask() {
            try {
                SelectedPallets = AssetWarehouse.Instance.GetPallets().ToList().FilterAndCleanPallets();
                DevUtils.Notify("Digged up thy dataCards twin");
            }
            catch (Exception ex) {
                Logger.Error("ERROR", ex);
            }
        }
        public struct PalletRep {
            public DataCard[] DataCards {
                get; set;
            }
            public Crate[] Crates {
                get; set;
            }
            public Pallet Pallet {
                get; set;
            }

            public PalletRep(Pallet pallet) {
                Pallet = pallet;
                Crates = Pallet.Crates.ToList().ToArray();
                DataCards = Pallet.DataCards.ToList().ToArray();
            }
        }
    }
}
