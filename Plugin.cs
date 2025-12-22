using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace CurseOfEmbrion
{
    [BepInPlugin(mGUID, mName, mVersion)]
    public class CurseOfEmbrionMod : BaseUnityPlugin
    {
        const string mGUID = "eXish.CurseOfEmbrion";
        const string mName = "CurseOfEmbrion";
        const string mVersion = "1.2.0";

        readonly Harmony harmony = new Harmony(mGUID);

        internal static CurseOfEmbrionMod instance;
        internal static ManualLogSource log;
        internal static AssetBundle bundle;
        internal static AudioClip alertNoise;
        internal static List<SpawnableEnemyWithRarity> outsideEnemies = null;
        internal static string levelDescription;
        internal static int outsidePowerCount; 

        void Awake()
        {
            if (instance == null)
                instance = this;

            log = Logger;

            ConfigManager.Init();
            if (ConfigManager.curseChance.Value < 0 || ConfigManager.curseChance.Value > 100)
                log.LogWarning($"The value \"{ConfigManager.curseChance.Value}\" is not valid for setting \"curseChance\"! The default will be used instead.");

            string modLocation = instance.Info.Location.TrimEnd("CurseOfEmbrion.dll".ToCharArray());
            bundle = AssetBundle.LoadFromFile(modLocation + "curseofembrion");
            if (bundle != null)
                alertNoise = bundle.LoadAsset<AudioClip>("Assets/CurseOfEmbrion/AlertNoise.wav");
            else
                log.LogError("Unable to locate the asset file! The alert noise will not play!");

            harmony.PatchAll();

            log.LogInfo($"{mName}-{mVersion} loaded!");
        }
    }
}
