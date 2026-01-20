using MelonLoader;

[assembly: MelonInfo(typeof(BONEUtils.Core), "BONEUtils", "1.0.0", "freakycheesy", null)]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace BONEUtils
{
    public class Core : MelonMod
    {
        public static Core Instance;
        public override void OnInitializeMelon()
        {
            Instance = this;
            LoggerInstance.Msg("Initialized.");
        }
    }
}