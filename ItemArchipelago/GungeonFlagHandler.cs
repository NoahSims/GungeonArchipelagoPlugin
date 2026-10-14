using ArchiGungeon.ArchipelagoServer;
using ArchiGungeon.Data;
using ArchiGungeon.UserInterface;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ArchiGungeon.ItemArchipelago
{
    [HarmonyPatch]
    internal class GungeonFlagHandler
    {
        // Map a flag to the NULL APlocation if you want to stop the flag from being set, but not unlock a location.
        public static Dictionary<GungeonFlags, APLocations> GungeonFlagsToAPLocations = new Dictionary<GungeonFlags, APLocations>(new GungeonFlagsComparer()) {
            // Shop flags
            { GungeonFlags.BLUEPRINTMETA_THOMPSON, APLocations.BLUEPRINTMETA_THOMPSON },
            { GungeonFlags.BLUEPRINTMETA_HEARTBOTTLE, APLocations.BLUEPRINTMETA_HEARTBOTTLE },
            { GungeonFlags.BLUEPRINTMETA_GAMMARAY, APLocations.BLUEPRINTMETA_GAMMARAY },
            { GungeonFlags.BLUEPRINTMETA_RPG, APLocations.BLUEPRINTMETA_RPG },
            { GungeonFlags.BLUEPRINTMETA_ROCKETBULLETS, APLocations.BLUEPRINTMETA_ROCKETBULLETS },
            { GungeonFlags.BLUEPRINTMETA_HEGEMONYRIFLE, APLocations.BLUEPRINTMETA_HEGEMONYRIFLE },
            { GungeonFlags.BLUEPRINTMETA_HEARTLOCKET, APLocations.BLUEPRINTMETA_HEARTLOCKET },
            { GungeonFlags.BLUEPRINTMETA_FIREBULLETS, APLocations.BLUEPRINTMETA_FIREBULLETS },
            { GungeonFlags.BLUEPRINTMETA_FREEZERAY, APLocations.BLUEPRINTMETA_FREEZERAY },
            { GungeonFlags.BLUEPRINTMETA_HEARTLUNCHBOX, APLocations.BLUEPRINTMETA_HEARTLUNCHBOX },
            { GungeonFlags.BLUEPRINTMETA_BOX, APLocations.BLUEPRINTMETA_BOX },
            { GungeonFlags.BLUEPRINTMETA_MAILBOX, APLocations.BLUEPRINTMETA_MAILBOX },
            { GungeonFlags.BLUEPRINTMETA_FATBULLETS, APLocations.BLUEPRINTMETA_FATBULLETS },
            { GungeonFlags.BLUEPRINTMETA_BETTERBULLETS, APLocations.BLUEPRINTMETA_BETTERBULLETS },
            { GungeonFlags.BLUEPRINTMETA_ANGRYBULLETS, APLocations.BLUEPRINTMETA_ANGRYBULLETS },
            { GungeonFlags.BLUEPRINTMETA_SIREN, APLocations.BLUEPRINTMETA_SIREN },
            { GungeonFlags.BLUEPRINTMETA_HEARTPURSE, APLocations.BLUEPRINTMETA_HEARTPURSE },
            { GungeonFlags.BLUEPRINTMETA_ORIGUNI, APLocations.BLUEPRINTMETA_ORIGUNI },
            { GungeonFlags.BLUEPRINTMETA_EYEPATCH, APLocations.BLUEPRINTMETA_EYEPATCH },
            { GungeonFlags.BLUEPRINTMETA_BLOODYEYE, APLocations.BLUEPRINTMETA_BLOODYEYE }, 
            { GungeonFlags.BLUEPRINTMETA_SPICE, APLocations.BLUEPRINTMETA_SPICE },
            { GungeonFlags.BLUEPRINTMETA_GRASSCHOPPER, APLocations.BLUEPRINTMETA_GRASSCHOPPER },
            { GungeonFlags.BLUEPRINTMETA_SUNGLASSES, APLocations.BLUEPRINTMETA_SUNGLASSES },
            { GungeonFlags.BLUEPRINTMETA_FLASHRAYGUN, APLocations.BLUEPRINTMETA_FLASHRAYGUN },
            { GungeonFlags.BLUEPRINTMETA_SINGULARITY, APLocations.BLUEPRINTMETA_SINGULARITY },
            { GungeonFlags.BLUEPRINTMETA_SCIENCECANNON, APLocations.BLUEPRINTMETA_SCIENCECANNON },
            { GungeonFlags.BLUEPRINTMETA_NANOMACHINES, APLocations.BLUEPRINTMETA_NANOMACHINES },
            { GungeonFlags.BLUEPRINTMETA_ICEBREAKER, APLocations.BLUEPRINTMETA_ICEBREAKER },
            { GungeonFlags.BLUEPRINTMETA_PORTABLETURRET, APLocations.BLUEPRINTMETA_PORTABLETURRET },
            { GungeonFlags.BLUEPRINTMETA_DUCTTAPE, APLocations.BLUEPRINTMETA_DUCTTAPE },
            { GungeonFlags.BLUEPRINTMETA_TANGLER, APLocations.BLUEPRINTMETA_TANGLER },
            { GungeonFlags.BLUEPRINTMETA_LAMP, APLocations.BLUEPRINTMETA_LAMP },
            { GungeonFlags.BLUEPRINTMETA_FACEMELTER, APLocations.BLUEPRINTMETA_FACEMELTER },
            { GungeonFlags.BLUEPRINTMETA_HEROINE, APLocations.BLUEPRINTMETA_HEROINE },
            { GungeonFlags.BLUEPRINTMETA_MASSSHOTGUN, APLocations.BLUEPRINTMETA_MASSSHOTGUN },
            { GungeonFlags.BLUEPRINTMETA_CEREBRALBORE, APLocations.BLUEPRINTMETA_CEREBRALBORE },
            { GungeonFlags.BLUEPRINTMETA_RAIDENCOIL, APLocations.BLUEPRINTMETA_RAIDENCOIL },
            { GungeonFlags.BLUEPRINTMETA_BLACKHOLEGUN, APLocations.BLUEPRINTMETA_BLACKHOLEGUN },
            { GungeonFlags.BLUEPRINTMETA_SPACEFRIEND, APLocations.BLUEPRINTMETA_SPACEFRIEND },
            { GungeonFlags.BLUEPRINTMETA_YARI, APLocations.BLUEPRINTMETA_YARI },
            { GungeonFlags.BLUEPRINTMETA_RAILGUN, APLocations.BLUEPRINTMETA_RAILGUN },
            { GungeonFlags.BLUEPRINTMETA_BROCCOLI, APLocations.BLUEPRINTMETA_BROCCOLI },
            //GungeonFlags.BLUEPRINTMETA_MTXGUN, // This causes weird loop on startup if you have the mtx dlc
            { GungeonFlags.BLUEPRINTMETA_PLATINUMBULLETS, APLocations.BLUEPRINTMETA_PLATINUMBULLETS },
            { GungeonFlags.BLUEPRINTGOOP_BUGBOOTS, APLocations.BLUEPRINTGOOP_BUGBOOTS },
            { GungeonFlags.BLUEPRINTGOOP_FOSSILIZEDGUN, APLocations.BLUEPRINTGOOP_FOSSILIZEDGUN },
            { GungeonFlags.BLUEPRINTGOOP_MONSTERBLOOD, APLocations.BLUEPRINTGOOP_MONSTERBLOOD },
            { GungeonFlags.BLUEPRINTGOOP_PLAGUEPISTOL, APLocations.BLUEPRINTGOOP_PLAGUEPISTOL },
            { GungeonFlags.BLUEPRINTGOOP_MEMBRANE, APLocations.BLUEPRINTGOOP_MEMBRANE },
            { GungeonFlags.BLUEPRINTGOOP_PLUNGER, APLocations.BLUEPRINTGOOP_PLUNGER },
            { GungeonFlags.BLUEPRINTGOOP_SPONGE, APLocations.BLUEPRINTGOOP_SPONGE },
            { GungeonFlags.BLUEPRINTGOOP_TENTACLEARM, APLocations.BLUEPRINTGOOP_TENTACLEARM },
            { GungeonFlags.BLUEPRINTGOOP_NAPALMSTRIKE, APLocations.BLUEPRINTGOOP_NAPALMSTRIKE },
            { GungeonFlags.BLUEPRINTGOOP_GUNDROMEDASTRAIN, APLocations.BLUEPRINTGOOP_GUNDROMEDASTRAIN },
            { GungeonFlags.BLUEPRINTGOOP_EVOLVER, APLocations.BLUEPRINTGOOP_EVOLVER },
            { GungeonFlags.BLUEPRINTTRUCK_CLUSTERMINE, APLocations.BLUEPRINTTRUCK_CLUSTERMINE },
            { GungeonFlags.BLUEPRINTTRUCK_MSIXTEEN, APLocations.BLUEPRINTTRUCK_MSIXTEEN },
            { GungeonFlags.BLUEPRINTTRUCK_RATION, APLocations.BLUEPRINTTRUCK_RATION },
            { GungeonFlags.BLUEPRINTTRUCK_AIRSTRIKE, APLocations.BLUEPRINTTRUCK_AIRSTRIKE },
            { GungeonFlags.BLUEPRINTTRUCK_COMMANDO, APLocations.BLUEPRINTTRUCK_COMMANDO },
            { GungeonFlags.BLUEPRINTTRUCK_AWP, APLocations.BLUEPRINTTRUCK_AWP },
            { GungeonFlags.BLUEPRINTTRUCK_NIKITA, APLocations.BLUEPRINTTRUCK_NIKITA },
            { GungeonFlags.BLUEPRINTTRUCK_BIGBOY, APLocations.BLUEPRINTTRUCK_BIGBOY },
            { GungeonFlags.BLUEPRINTTRUCK_CHAFFGRENADE, APLocations.BLUEPRINTTRUCK_CHAFFGRENADE },
            { GungeonFlags.BLUEPRINTTRUCK_PATRIOT, APLocations.BLUEPRINTTRUCK_PATRIOT },
            { GungeonFlags.BLUEPRINTTRUCK_MAGAZINERACK, APLocations.BLUEPRINTTRUCK_MAGAZINERACK },
            { GungeonFlags.BLUEPRINTBEETLE_DEVOLVER, APLocations.BLUEPRINTBEETLE_DEVOLVER },
            { GungeonFlags.BLUEPRINTBEETLE_BASEBALLBAT, APLocations.BLUEPRINTBEETLE_BASEBALLBAT },
            { GungeonFlags.BLUEPRINTBEETLE_DAWNHAMMER, APLocations.BLUEPRINTBEETLE_DAWNHAMMER },
            { GungeonFlags.BLUEPRINTBEETLE_STRAFEGUN, APLocations.BLUEPRINTBEETLE_STRAFEGUN },
            { GungeonFlags.BLUEPRINTBEETLE_HYPERLIGHT, APLocations.BLUEPRINTBEETLE_HYPERLIGHT },
            { GungeonFlags.BLUEPRINTBEETLE_STARPEW, APLocations.BLUEPRINTBEETLE_STARPEW },
            { GungeonFlags.BLUEPRINTBEETLE_DUELINGLASER, APLocations.BLUEPRINTBEETLE_DUELINGLASER },
            { GungeonFlags.BLUEPRINTBEETLE_TELEPORTERPROTOTYPE, APLocations.BLUEPRINTBEETLE_TELEPORTERPROTOTYPE },
            { GungeonFlags.BLUEPRINTBEETLE_VORPALGUN, APLocations.BLUEPRINTBEETLE_VORPALGUN },
            { GungeonFlags.BLUEPRINTBEETLE_STOUTIES, APLocations.BLUEPRINTBEETLE_STOUTIES },
            { GungeonFlags.BLUEPRINTBEETLE_CHARMERS, APLocations.BLUEPRINTBEETLE_CHARMERS },
            { GungeonFlags.BLUEPRINTBEETLE_FLAKTUALLY, APLocations.BLUEPRINTBEETLE_FLAKTUALLY },
            { GungeonFlags.BLUEPRINTBEETLE_MAGICITES, APLocations.BLUEPRINTBEETLE_MAGICITES },
            { GungeonFlags.BLUEPRINTBEETLE_METROIDBOMBS, APLocations.BLUEPRINTBEETLE_METROIDBOMBS },
            { GungeonFlags.BLUEPRINTBEETLE_SILVALLIES, APLocations.BLUEPRINTBEETLE_SILVALLIES },
            { GungeonFlags.BLUEPRINTBEETLE_HELIX, APLocations.BLUEPRINTBEETLE_HELIX },
            { GungeonFlags.BLUEPRINTBEETLE_GOLDIES, APLocations.BLUEPRINTBEETLE_GOLDIES },
            { GungeonFlags.BLUEPRINTBEETLE_BEEBULLETS, APLocations.BLUEPRINTBEETLE_BEEBULLETS },
            { GungeonFlags.BLUEPRINTBEETLE_DEVOLVERBULLETS, APLocations.BLUEPRINTBEETLE_DEVOLVERBULLETS },
            { GungeonFlags.BLUEPRINTBEETLE_CRITICALBULLETS, APLocations.BLUEPRINTBEETLE_CRITICALBULLETS },
            { GungeonFlags.BLUEPRINTBEETLE_SNOWBALLETS, APLocations.BLUEPRINTBEETLE_SNOWBALLETS },
            { GungeonFlags.BLUEPRINTBEETLE_BIGGIANTHEAD, APLocations.BLUEPRINTBEETLE_BIGGIANTHEAD },
            { GungeonFlags.BLUEPRINTBEETLE_COMBINERIFLE, APLocations.BLUEPRINTBEETLE_COMBINERIFLE },
            { GungeonFlags.BLUEPRINTBEETLE_EXOTICGUN, APLocations.BLUEPRINTBEETLE_EXOTICGUN },
            { GungeonFlags.BLUEPRINTBEETLE_BIGSHOTGUN, APLocations.BLUEPRINTBEETLE_BIGSHOTGUN },
            { GungeonFlags.BLUEPRINTBEETLE_GUNDERFURY, APLocations.BLUEPRINTBEETLE_GUNDERFURY },
            { GungeonFlags.BLUEPRINTBEETLE_GLAIVE, APLocations.BLUEPRINTBEETLE_GLAIVE },
            { GungeonFlags.BLUEPRINTBEETLE_LUTE, APLocations.BLUEPRINTBEETLE_LUTE },
            { GungeonFlags.BLUEPRINTBEETLE_TETRIS, APLocations.BLUEPRINTBEETLE_TETRIS },
            { GungeonFlags.BLUEPRINTBEETLE_TRIGUN, APLocations.BLUEPRINTBEETLE_TRIGUN },
            { GungeonFlags.BLUEPRINTBEETLE_SHOVEL, APLocations.BLUEPRINTBEETLE_SHOVEL },
            { GungeonFlags.BLUEPRINTBEETLE_PREDATOR, APLocations.BLUEPRINTBEETLE_PREDATOR },
            { GungeonFlags.BLUEPRINTBEETLE_VOIDCORELAUNCHER, APLocations.BLUEPRINTBEETLE_VOIDCORELAUNCHER },
            { GungeonFlags.BLUEPRINTBEETLE_BLANKBULLETS, APLocations.BLUEPRINTBEETLE_BLANKBULLETS },
            { GungeonFlags.BLUEPRINTBEETLE_ORBITALBULLETS, APLocations.BLUEPRINTBEETLE_ORBITALBULLETS },
            { GungeonFlags.BLUEPRINTBEETLE_SCOUTER, APLocations.BLUEPRINTBEETLE_SCOUTER },
            { GungeonFlags.BLUEPRINTBEETLE_CONTROLLER, APLocations.BLUEPRINTBEETLE_CONTROLLER },
            { GungeonFlags.BLUEPRINTBEETLE_GUNNER, APLocations.BLUEPRINTBEETLE_GUNNER },
            // Sherpa/Tinker flags
            { GungeonFlags.SHERPA_ACTIVE_IN_ELEVATOR_ROOM, APLocations.NULL },
            { GungeonFlags.SHERPA_UNLOCK1_COMPLETE, APLocations.NULL },
            { GungeonFlags.SHERPA_UNLOCK2_COMPLETE, APLocations.NULL },
            { GungeonFlags.SHERPA_UNLOCK3_COMPLETE, APLocations.NULL },
            { GungeonFlags.SHERPA_UNLOCK4_COMPLETE, APLocations.NULL },
            { GungeonFlags.SHERPA_HAS_UNLOCKED_BOSSRUSH, APLocations.NULL },
            { GungeonFlags.SHERPA_UNLOCK1_ELEMENT1, APLocations.ShortcutUnlock1Element1 },
            { GungeonFlags.SHERPA_UNLOCK1_ELEMENT2, APLocations.ShortcutUnlock1Element2 },
            { GungeonFlags.SHERPA_UNLOCK1_ELEMENT3, APLocations.ShortcutUnlock1Element3 },
            { GungeonFlags.SHERPA_UNLOCK1_ELEMENT4, APLocations.ShortcutUnlock1Element4 },
            { GungeonFlags.SHERPA_UNLOCK2_ELEMENT1, APLocations.ShortcutUnlock2Element1 },
            { GungeonFlags.SHERPA_UNLOCK2_ELEMENT2, APLocations.ShortcutUnlock2Element2 },
            { GungeonFlags.SHERPA_UNLOCK2_ELEMENT3, APLocations.ShortcutUnlock2Element3 },
            { GungeonFlags.SHERPA_UNLOCK2_ELEMENT4, APLocations.ShortcutUnlock2Element4 },
            { GungeonFlags.SHERPA_UNLOCK3_ELEMENT1, APLocations.ShortcutUnlock3Element1 },
            { GungeonFlags.SHERPA_UNLOCK3_ELEMENT2, APLocations.ShortcutUnlock3Element2 },
            { GungeonFlags.SHERPA_UNLOCK3_ELEMENT3, APLocations.ShortcutUnlock3Element3 },
            { GungeonFlags.SHERPA_UNLOCK3_ELEMENT4, APLocations.ShortcutUnlock3Element4 },
            { GungeonFlags.SHERPA_UNLOCK4_ELEMENT1, APLocations.ShortcutUnlock4Element1 },
            { GungeonFlags.SHERPA_UNLOCK4_ELEMENT2, APLocations.ShortcutUnlock4Element2 },
            { GungeonFlags.SHERPA_UNLOCK4_ELEMENT3, APLocations.ShortcutUnlock4Element3 },
            { GungeonFlags.SHERPA_UNLOCK4_ELEMENT4, APLocations.ShortcutUnlock4Element4 },
            // Bullet Parts
            { GungeonFlags.BLACKSMITH_ELEMENT1, APLocations.NULL },
            { GungeonFlags.BLACKSMITH_ELEMENT2, APLocations.NULL },
            { GungeonFlags.BLACKSMITH_ELEMENT3, APLocations.NULL },
            { GungeonFlags.BLACKSMITH_ELEMENT4, APLocations.NULL },
            // Bosses
            { GungeonFlags.BOSSKILLED_GATLING_GULL, APLocations.GatlingGullDefeated },
            { GungeonFlags.BOSSKILLED_BULLET_KING, APLocations.BulletKingDefeated },
            { GungeonFlags.BOSSKILLED_BULLET_BROS, APLocations.TriggerTwinsDefeated },
        };

        /// <summary>
        /// Flag that we are setting ourselves, so if it gets picked up by our interjection of SetFlag(), let it through.
        /// </summary>
        private static GungeonFlags _expectedSetFlag = GungeonFlags.NONE;

        /// <summary>
        /// Sets a Gungeon flag without it getting caught by a Harmony Prefix injection. Use instead of GameStatsManager.Instance.SetFlag().
        /// </summary>
        /// <param name="flag">Gungeon flag to set</param>
        /// <param name="value">Set flag to true or false</param>
        public static void SafelySetGungeonFlag(GungeonFlags flag, bool value)
        {
            _expectedSetFlag = flag;
            GameStatsManager.Instance.SetFlag(flag, value);
        }

        /*
         * Patch - GameStatsManager.SetFlag
         * Interject when a Gungeon Flag is set, if its in APLocationFlags don't have it set.
         * For example with meta shops, capture location check without unlocking associated item.
         * Any time an item is purchased from the metashop, the associated flag is set for the save file.
         * This patch takes over that SetFlag function, checks if the flag being set is a metashop location,
         * then captures that flag as an archipelago location without unlocking the item.
         */
        [HarmonyPatch(typeof(GameStatsManager), nameof(GameStatsManager.SetFlag))]
        [HarmonyPrefix]
        public static bool CaptureGungeonFlagSet(GungeonFlags flag, bool value)
        {
            //TODO make collection of all meta shop flags to compare against.
            if (GungeonFlagsToAPLocations.ContainsKey(flag) && flag != _expectedSetFlag)
            {
                ETGModConsole.Log("GameStatsManagerPatch: Location checked - Send item here: " + flag);
                SessionHandler.DataSender.SendFoundLocationCheck(GungeonFlagsToAPLocations[flag]);
                ArchipelaGunLocalDataManager.locationsCheckedFlags.Add(flag); // TODO update this for all location checks probably
                return false;
            }

            if (flag == _expectedSetFlag)
            {
                // Now that the flag has beened bypassed, reset the var
                _expectedSetFlag = GungeonFlags.NONE;
                ETGModConsole.Log("GameStatsManagerPatch: Expected flag was set: " + flag + " - " + value);
            }
            else
            {
                ETGModConsole.Log("GameStatsManagerPatch: Something has set a save flag: " + flag + " - " + value);
            }
            
            return true;
        }

        /*
         * Patch - MetaShopController.GetFlagFromTargetItem
         * Specific to Ox and Cadence shop; Prevent locations checked from reappearing in shop after purchase
         */
        [HarmonyPatch(typeof(MetaShopController), nameof(MetaShopController.GetFlagFromTargetItem))]
        [HarmonyPrefix]
        public static bool Patch_GetFlagFromTargetItem(ref GungeonFlags __result, int shopItemId)
        {
            //ETGModConsole.Log("Patch_GetFlagFromTargetItem: " + shopItemId);
            GungeonFlags flag = GungeonFlags.NONE;
            PickupObject byId = PickupObjectDatabase.GetById(shopItemId);
            foreach (var prereq in byId.encounterTrackable.prerequisites)
            {
                if(prereq.prerequisiteType == DungeonPrerequisite.PrerequisiteType.FLAG)
                {
                    flag = prereq.saveFlagToCheck;
                }
            }
            if (ArchipelaGunLocalDataManager.locationsCheckedFlags.Contains(flag))
            {
                //ETGModConsole.Log("GameStatsManagerPatch: MetaShopController attempting to put heart bottle in shop");
                __result = GungeonFlags.BLUEPRINTMETA_MTXGUN; // this needs to be not the mtx gun, lol
                return false;
            }
            __result = flag;
            return false;
        }

        /*
         * Patch - EncounterTrackable.PrerequisitesMet
         * Controls whether to add items to meta shops (but not ox and cadence).
         * Needed for tracking locations purchased from shops but not unlocked as items yet.
         */
        [HarmonyPatch(typeof(EncounterTrackable), nameof(EncounterTrackable.PrerequisitesMet))]
        [HarmonyPrefix]
        public static bool Patch_PrerequisitesMet(EncounterTrackable __instance, ref bool __result)
        {
            if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
            {
                for (int i = 0; i < __instance.m_prerequisites.Length; i++)
                {
                    if (__instance.m_prerequisites[i].prerequisiteType == DungeonPrerequisite.PrerequisiteType.FLAG)
                    {
                        __result = ArchipelaGunLocalDataManager.locationsCheckedFlags.Contains(__instance.m_prerequisites[i].saveFlagToCheck);
                        return false;
                    }
                }
            }
            return true;
        }

        // This function will trigger any time the game tries to generate the loot pool and read what it thinks are in the pool. Could be useful for debugging.
        // I don't know where else to put this, so this can hang out here for now. Uncomment if you want, I guess
        /*
        [HarmonyPatch(typeof(GenericLootTable), nameof(GenericLootTable.GetCompiledRawItems))]
        [HarmonyPostfix]
        public static void Patch_getCompiledRawItems(ref List<WeightedGameObject> __result)
        {
            String list = "";
            foreach (WeightedGameObject item in __result)
            {
                list += item.pickupId + " ";
            }
            ETGModConsole.Log("GameStatsManagerPatch: getting GenericLootTable: " + list);
        }
        */
    }
}
