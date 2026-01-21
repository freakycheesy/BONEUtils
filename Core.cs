using BONEUtils.Developer;
using MelonLoader;

[assembly: MelonInfo(typeof(BONEUtils.Core), "BONE-Utils", "1.0.0", "freakycheesy", "https://github.com/freakycheesy/BONEUtils")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace BONEUtils
{
    public class Core : MelonMod
    {
        public static MelonPreferences_Category Preferences;
        public static Core Instance;
        public override void OnInitializeMelon()
        {
            Preferences = MelonPreferences.CreateCategory("BONE-UTILS");
            LoggerInstance.Msg("Loaded BONE-Utils.");
            Instance = this;
            UICore.Init();
            Utility.Init();
        }
    }
}