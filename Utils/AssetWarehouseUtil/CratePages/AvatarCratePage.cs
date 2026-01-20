using BoneLib;
using BoneLib.BoneMenu;
using BONEUtils.Developer;
using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow.Warehouse;
using UnityEngine;
namespace BONEUtils.Utils.AssetWarehouseUtil.CratePages {
    public class AvatarCratePage : CratePage {
        public AvatarCratePage(BoneLib.BoneMenu.Page palletPage, Crate crate) : base(palletPage, crate) {
            Developer.Logger.Log("Avatar");
        }

        public static void Init() {
            CreateListener += OnCreate;
        }

        private static void OnCreate(BoneLib.BoneMenu.Page page, Crate crate) {
            if (crate is not AvatarCrate)
                return;
            CratePages.Add(new AvatarCratePage(page, crate));
        }

        public override void OnPageCreated() {
            MyPage.CreateFunction("Swap Avatar", Color.cyan, DirectSpawn);
        }

        private void DirectSpawn() {
            Player.RigManager.SwapAvatarCrate(Crate.Barcode);
        }
    }
}
