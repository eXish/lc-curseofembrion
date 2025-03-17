using BepInEx;
using HarmonyLib;
using System.Collections.Generic;

namespace CurseOfEmbrion
{
    [BepInPlugin(mGUID, mName, mVersion)]
    public class CurseOfEmbrionMod : BaseUnityPlugin
    {
        const string mGUID = "eXish.CurseOfEmbrion";
        const string mName = "CurseOfEmbrion";
        const string mVersion = "1.1.2";

        readonly Harmony harmony = new Harmony(mGUID);

        internal static CurseOfEmbrionMod instance;
        internal static List<SpawnableEnemyWithRarity> outsideEnemies = null;
        internal static string levelDescription;
        internal static int outsidePowerCount; 

        void Awake()
        {
            if (instance == null)
                instance = this;

            ConfigManager.Init();
            if (ConfigManager.curseChance.Value < 0 || ConfigManager.curseChance.Value > 100)
                instance.Logger.LogWarning($"The value \"{ConfigManager.curseChance.Value}\" is not valid for setting \"curseChance\"! The default will be used instead.");

            harmony.PatchAll();

            instance.Logger.LogInfo($"{mName}-{mVersion} loaded!");
        }
    }
}
