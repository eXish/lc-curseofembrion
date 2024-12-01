using BepInEx;
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
        const string mVersion = "1.0.0";

        readonly Harmony harmony = new Harmony(mGUID);

        internal static CurseOfEmbrionMod instance;
        internal static AssetBundle bundle;
        internal static AudioClip[] curseClips = new AudioClip[4];
        internal static List<SpawnableEnemyWithRarity> outsideEnemies = null;
        internal static int outsidePowerCount; 

        void Awake()
        {
            if (instance == null)
                instance = this;

            ConfigManager.Init();
            if (ConfigManager.curseChance.Value < 0 || ConfigManager.curseChance.Value > 100)
                instance.Logger.LogWarning($"The value \"{ConfigManager.curseChance.Value}\" is not valid for setting \"curseChance\"! The default will be used instead.");

            string modLocation = instance.Info.Location.TrimEnd("CurseOfEmbrion.dll".ToCharArray());
            bundle = AssetBundle.LoadFromFile(modLocation + "curseofembrion");
            if (bundle != null)
            {
                curseClips[0] = bundle.LoadAsset<AudioClip>("Assets/CurseOfEmbrion/CurseOfEmbrion1.mp3");
                curseClips[1] = bundle.LoadAsset<AudioClip>("Assets/CurseOfEmbrion/CurseOfEmbrion2.mp3");
                curseClips[2] = bundle.LoadAsset<AudioClip>("Assets/CurseOfEmbrion/CurseOfEmbrion3.mp3");
                curseClips[3] = bundle.LoadAsset<AudioClip>("Assets/CurseOfEmbrion/CurseOfEmbrion4.mp3");
            }
            else
                instance.Logger.LogError($"Unable to locate the asset file! Audio clips for the curse will not play.");

            harmony.PatchAll();

            instance.Logger.LogInfo($"{mName}-{mVersion} loaded!");
        }
    }
}
