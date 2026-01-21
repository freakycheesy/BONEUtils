using BoneLib;
using BoneLib.BoneMenu;
using BONEUtils.Developer;
using Il2CppSLZ.Bonelab;
using Il2CppSLZ.Marrow.Warehouse;
using UnityEngine;
namespace BONEUtils.Utils.AssetWarehouseUtil.CratePages {
    public class SpawnableCratePage : ScannablePage {
        public SpawnableCratePage(BoneLib.BoneMenu.Page palletPage, Crate crate) : base(palletPage, crate) {
            Developer.Logger.Log("Spawnable");
        }
        public static void Init() {
            CreateListener += OnCreate;
        }

        private static void OnCreate(BoneLib.BoneMenu.Page page, Scannable crate) {
            if (crate is not SpawnableCrate)
                return;
            Instances.Add(new SpawnableCratePage(page, (Crate)crate));
        }

        public override void OnPageCreated() {
            MyPage.CreateFunction("Patch Spawngun", Color.cyan, PatchSpawngun);
            MyPage.CreateFunction("Direct Spawn", Color.cyan, DirectSpawn);
        }

        private void DirectSpawn() {
            var position = Player.Head.transform.position + (Player.Head.transform.forward * 5);
            SpawnableCrateReference crateRef = new(Scannable.Barcode);
            HelperMethods.SpawnCrate(crateRef, position);
        }

        private void PatchSpawngun() {
            foreach (var instance in UnityEngine.Object.FindObjectsOfType<SpawnGun>()) {
                instance._lastSelectedCrate = (SpawnableCrate)Scannable;
                instance._selectedCrate = (SpawnableCrate)Scannable;
            }
        }
    }
}
