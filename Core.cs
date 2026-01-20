using BONEUtils.Developer;
using MelonLoader;

[assembly: MelonInfo(typeof(BONEUtils.Core), "BONE-Utils", "1.0.0", "freakycheesy", "https://github.com/freakycheesy/BONEUtils")]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace BONEUtils
{
    public class Core : MelonMod
    {
        public static Core Instance;
        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Loaded BONE-Utils.");
            Instance = this;
            UICore.Init();
            Utility.Init();
        }
    }
}