using BoneLib;
using BoneLib.BoneMenu;
using BONEUtils.Developer;
using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow.SceneStreaming;
using Il2CppSLZ.Marrow.Warehouse;
using UnityEngine;
namespace BONEUtils.Utils.AssetWarehouseUtil.CratePages {
    public class LevelCratePage : ScannablePage {
        public LevelCratePage(BoneLib.BoneMenu.Page palletPage, Crate crate) : base(palletPage, crate) {
            Developer.Logger.Log("Level");
        }

        public static void Init() {
            CreateListener += OnCreate;
        }

        private static void OnCreate(BoneLib.BoneMenu.Page page, Scannable crate) {
            if (crate is not LevelCrate)
                return;
            Instances.Add(new LevelCratePage(page, (Crate)crate));
        }

        public override void OnPageCreated() {
            MyPage.CreateFunction("Load Level", Color.cyan, LoadLevel);
        }
        private void LoadLevel() {
            SceneStreamer.Load(Scannable.Barcode);
        }
    }
}
