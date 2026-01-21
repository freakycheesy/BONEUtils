using BoneLib.BoneMenu;
using BONEUtils.Developer;
using Il2CppSLZ.Marrow.Warehouse;
using UnityEngine;
using static BONEUtils.Utils.AssetWarehouseUtil.WarehouseData;

namespace BONEUtils.Utils.AssetWarehouseUtil {
    public abstract class ScannablePage {
        public static List<ScannablePage> Instances = new();
        public static Action<Page, Scannable> CreateListener;
        public static void CreatePage(PalletRep rep) {
            var pallet = rep.Pallet;
            var palletPage = AssetWarehouseUtility.PalletsPage.CreatePage($"{pallet._title}\n({pallet._barcode._id})", Color.green, AssetWarehouseUtility.MaxElements);
            var cratesPage = palletPage.CreatePage("Crates", Color.blue);
            var datacardsPage = palletPage.CreatePage("Data Cards", Color.magenta);
            rep.Crates?.ToList()?.ForEach(x=>CreateListener?.Invoke(cratesPage, x));
            rep.DataCards?.ToList()?.ForEach(x => CreateListener?.Invoke(datacardsPage, x));
        }
        public Page MyPage {
            get; private set;
        }
        public Scannable Scannable {
            get; private set;
        }
        protected ScannablePage(Page parent, Scannable scannable) {
            Scannable = scannable;
            MyPage = parent.CreatePage($"{scannable._title}\n({scannable._barcode._id})", Color.cyan, AssetWarehouseUtility.MaxElements);

            AssetWarehouseUtility.CratesPage.CreatePageLink(MyPage);
            Developer.Logger.Log($"Created Scannable Page: {scannable.Barcode.ID}");
            OnPageCreated();
        }
        public abstract void OnPageCreated();

    }
}
