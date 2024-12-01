using HarmonyLib;
using System.Linq;

namespace CurseOfEmbrion.Patches
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal class StartOfRoundPatch
    {
        [HarmonyPatch("StartGame")]
        [HarmonyPrefix]
        static void StartGamePatch(StartOfRound __instance)
        {
            int curseChance = ConfigManager.curseChance.Value;
            if (curseChance < 0 || curseChance > 100)
                curseChance = (int)ConfigManager.curseChance.DefaultValue;
            if (UnityEngine.Random.Range(1, 101) <= curseChance)
            {
                bool foundRadMech = false;
                CurseOfEmbrionMod.outsidePowerCount = __instance.currentLevel.maxOutsideEnemyPowerCount;
                __instance.currentLevel.maxOutsideEnemyPowerCount = __instance.levels[12].maxOutsideEnemyPowerCount;
                CurseOfEmbrionMod.outsideEnemies = __instance.currentLevel.OutsideEnemies.ToList();
                for (int i = 0; i < __instance.currentLevel.OutsideEnemies.Count; i++)
                {
                    if (__instance.currentLevel.OutsideEnemies[i].enemyType.name == "RadMech")
                    {
                        foundRadMech = true;
                        __instance.currentLevel.OutsideEnemies[i] = __instance.levels[12].OutsideEnemies[3];
                    }
                    else
                        __instance.currentLevel.OutsideEnemies[i].rarity = __instance.currentLevel.OutsideEnemies[i].rarity / 4;
                }
                if (!foundRadMech)
                    __instance.currentLevel.OutsideEnemies.Add(__instance.levels[12].OutsideEnemies[3]);
                if (CurseOfEmbrionMod.bundle != null && ConfigManager.enableCustomAudio.Value)
                    __instance.speakerAudioSource.PlayOneShot(CurseOfEmbrionMod.curseClips[UnityEngine.Random.Range(0, 4)]);
            }
        }

        [HarmonyPatch("EndOfGame")]
        [HarmonyPrefix]
        static void EndOfGamePatch(StartOfRound __instance)
        {
            if (CurseOfEmbrionMod.outsideEnemies != null)
            {
                __instance.currentLevel.OutsideEnemies = CurseOfEmbrionMod.outsideEnemies;
                __instance.currentLevel.maxOutsideEnemyPowerCount = CurseOfEmbrionMod.outsidePowerCount;
                CurseOfEmbrionMod.outsideEnemies = null;
            }
        }
    }
}