using BoneLib.BoneMenu;
using Il2CppSLZ.Marrow.Warehouse;
using UnityEngine;

namespace BONEUtils.Utils.AssetWarehouseUtil {
    public abstract class CratePage {
        public static List<CratePage> CratePages = new();
        public static Action<Page, Crate> CreateListener;
        public static void CreatePage(Crate crate) {
            var pallet = crate.Pallet;
            var palletPage = AssetWarehouseUtility.PalletsPage.CreatePage($"{pallet._title}\n({pallet._barcode._id})", Color.green, AssetWarehouseUtility.MaxElements);
            CreateListener.Invoke(palletPage, crate);
        }
        public Page MyPage {
            get; private set;
        }
        public Crate Crate {
            get; private set;
        }
        protected CratePage(Page palletPage, Crate crate) {
            Crate = crate;
            MyPage = palletPage.CreatePage($"{crate._title}\n({crate._barcode._id})", Color.cyan, AssetWarehouseUtility.MaxElements);

            AssetWarehouseUtility.CratesPage.CreatePageLink(MyPage);
            OnPageCreated();
        }
        public abstract void OnPageCreated();

    }
}
