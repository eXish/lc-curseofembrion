using BepInEx.Configuration;

namespace CurseOfEmbrion
{
    internal class ConfigManager
    {
        public static ConfigEntry<int> curseChance;
        public static ConfigEntry<bool> onlyBirdsOnCurse;
        public static ConfigEntry<bool> playAlertNoise;
        public static ConfigEntry<float> alertNoiseVolume;

        public static void Init()
        {
            curseChance = CurseOfEmbrionMod.instance.Config.Bind("General Settings", "curseChance", 10, "The chance for the curse to apply each moon. Can be set to a minimum of 0 (never occurs) and a maximum of 100 (always occurs).");
            onlyBirdsOnCurse = CurseOfEmbrionMod.instance.Config.Bind("General Settings", "onlyBirdsOnCurse", false, "Forces all outside enemies to be Old Birds when the curse is present.");
            playAlertNoise = CurseOfEmbrionMod.instance.Config.Bind("General Settings", "playAlertNoise", true, "Plays an alert noise when the curse is active.");
            alertNoiseVolume = CurseOfEmbrionMod.instance.Config.Bind("General Settings", "alertNoiseVolume", 1f, "The volume of the alert noise. Can be set to a minimum of .1 and a maximum of 1.");
        }
    }
}
