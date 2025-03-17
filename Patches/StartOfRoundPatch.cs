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
            if (!__instance.IsHost)
                return;
            int curseChance = ConfigManager.curseChance.Value;
            if (curseChance < 0 || curseChance > 100)
                curseChance = (int)ConfigManager.curseChance.DefaultValue;
            if (__instance.currentLevel.PlanetName != "71 Gordion" && UnityEngine.Random.Range(1, 101) <= curseChance)
            {
                bool foundRadMech = false;
                SelectableLevel embrion = __instance.levels.Where(x => x.PlanetName == "5 Embrion").First();
                SpawnableEnemyWithRarity emRadMech = embrion.OutsideEnemies.Where(x => x.enemyType.name == "RadMech").First();
                CurseOfEmbrionMod.levelDescription = __instance.currentLevel.LevelDescription;
                __instance.currentLevel.LevelDescription += "\n\n<color=red>Curse of Embrion</color>";
                CurseOfEmbrionMod.outsidePowerCount = __instance.currentLevel.maxOutsideEnemyPowerCount;
                __instance.currentLevel.maxOutsideEnemyPowerCount = embrion.maxOutsideEnemyPowerCount;
                CurseOfEmbrionMod.outsideEnemies = __instance.currentLevel.OutsideEnemies.ToList();
                for (int i = 0; i < __instance.currentLevel.OutsideEnemies.Count; i++)
                {
                    if (__instance.currentLevel.OutsideEnemies[i].enemyType.name == "RadMech")
                    {
                        foundRadMech = true;
                        __instance.currentLevel.OutsideEnemies[i] = emRadMech;
                    }
                    else
                        __instance.currentLevel.OutsideEnemies[i].rarity = ConfigManager.onlyBirdsOnCurse.Value ? 0 : __instance.currentLevel.OutsideEnemies[i].rarity / 4;
                }
                if (!foundRadMech)
                    __instance.currentLevel.OutsideEnemies.Add(emRadMech);
            }
        }

        [HarmonyPatch("EndOfGame")]
        [HarmonyPrefix]
        static void EndOfGamePatch(StartOfRound __instance)
        {
            if (!__instance.IsHost)
                return;
            if (CurseOfEmbrionMod.outsideEnemies != null)
            {
                __instance.currentLevel.OutsideEnemies = CurseOfEmbrionMod.outsideEnemies;
                __instance.currentLevel.maxOutsideEnemyPowerCount = CurseOfEmbrionMod.outsidePowerCount;
                __instance.currentLevel.LevelDescription = CurseOfEmbrionMod.levelDescription;
                CurseOfEmbrionMod.outsideEnemies = null;
            }
        }
    }
}