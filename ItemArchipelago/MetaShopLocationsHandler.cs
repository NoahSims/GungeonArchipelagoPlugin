using ArchiGungeon.ArchipelagoServer;
using ArchiGungeon.Data;
using ArchiGungeon.UserInterface;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ArchiGungeon.ItemArchipelago
{
    [HarmonyPatch]
    internal class MetaShopLocationsHandler
    {
        public static HashSet<GungeonFlags> metaShopLocationFlags = new HashSet<GungeonFlags>(new GungeonFlagsComparer()) {
            GungeonFlags.BLUEPRINTMETA_THOMPSON,
            GungeonFlags.BLUEPRINTMETA_HEARTBOTTLE,
            GungeonFlags.BLUEPRINTMETA_GAMMARAY,
            GungeonFlags.BLUEPRINTMETA_RPG,
            GungeonFlags.BLUEPRINTMETA_ROCKETBULLETS,
            GungeonFlags.BLUEPRINTMETA_HEGEMONYRIFLE,
            GungeonFlags.BLUEPRINTMETA_HEARTLOCKET,
            GungeonFlags.BLUEPRINTMETA_FIREBULLETS,
            GungeonFlags.BLUEPRINTMETA_FREEZERAY,
            GungeonFlags.BLUEPRINTMETA_HEARTLUNCHBOX,
            GungeonFlags.BLUEPRINTMETA_BOX,
            GungeonFlags.BLUEPRINTMETA_MAILBOX,
            GungeonFlags.BLUEPRINTMETA_FATBULLETS,
            GungeonFlags.BLUEPRINTMETA_BETTERBULLETS,
            GungeonFlags.BLUEPRINTMETA_ANGRYBULLETS,
            GungeonFlags.BLUEPRINTMETA_SIREN,
            GungeonFlags.BLUEPRINTMETA_HEARTPURSE,
            GungeonFlags.BLUEPRINTMETA_ORIGUNI,
            GungeonFlags.BLUEPRINTMETA_EYEPATCH,
            GungeonFlags.BLUEPRINTMETA_BLOODYEYE,
            GungeonFlags.BLUEPRINTMETA_SPICE,
            GungeonFlags.BLUEPRINTMETA_GRASSCHOPPER,
            GungeonFlags.BLUEPRINTMETA_SUNGLASSES,
            GungeonFlags.BLUEPRINTMETA_FLASHRAYGUN,
            GungeonFlags.BLUEPRINTMETA_SINGULARITY,
            GungeonFlags.BLUEPRINTMETA_SCIENCECANNON,
            GungeonFlags.BLUEPRINTMETA_NANOMACHINES,
            GungeonFlags.BLUEPRINTMETA_ICEBREAKER,
            GungeonFlags.BLUEPRINTMETA_PORTABLETURRET,
            GungeonFlags.BLUEPRINTMETA_DUCTTAPE,
            GungeonFlags.BLUEPRINTMETA_TANGLER,
            GungeonFlags.BLUEPRINTMETA_LAMP,
            GungeonFlags.BLUEPRINTMETA_FACEMELTER,
            GungeonFlags.BLUEPRINTMETA_HEROINE,
            GungeonFlags.BLUEPRINTMETA_MASSSHOTGUN,
            GungeonFlags.BLUEPRINTMETA_CEREBRALBORE,
            GungeonFlags.BLUEPRINTMETA_RAIDENCOIL,
            GungeonFlags.BLUEPRINTMETA_BLACKHOLEGUN,
            GungeonFlags.BLUEPRINTMETA_SPACEFRIEND,
            GungeonFlags.BLUEPRINTMETA_YARI,
            GungeonFlags.BLUEPRINTMETA_RAILGUN,
            GungeonFlags.BLUEPRINTMETA_BROCCOLI,
            //GungeonFlags.BLUEPRINTMETA_MTXGUN, // This causes weird loop on startup if you have the mtx dlc
            GungeonFlags.BLUEPRINTMETA_PLATINUMBULLETS,
            GungeonFlags.BLUEPRINTGOOP_BUGBOOTS,
            GungeonFlags.BLUEPRINTGOOP_FOSSILIZEDGUN,
            GungeonFlags.BLUEPRINTGOOP_MONSTERBLOOD,
            GungeonFlags.BLUEPRINTGOOP_PLAGUEPISTOL,
            GungeonFlags.BLUEPRINTGOOP_MEMBRANE,
            GungeonFlags.BLUEPRINTGOOP_PLUNGER,
            GungeonFlags.BLUEPRINTGOOP_SPONGE,
            GungeonFlags.BLUEPRINTGOOP_TENTACLEARM,
            GungeonFlags.BLUEPRINTGOOP_NAPALMSTRIKE,
            GungeonFlags.BLUEPRINTGOOP_GUNDROMEDASTRAIN,
            GungeonFlags.BLUEPRINTGOOP_EVOLVER,
            GungeonFlags.BLUEPRINTTRUCK_CLUSTERMINE,
            GungeonFlags.BLUEPRINTTRUCK_MSIXTEEN,
            GungeonFlags.BLUEPRINTTRUCK_RATION,
            GungeonFlags.BLUEPRINTTRUCK_AIRSTRIKE,
            GungeonFlags.BLUEPRINTTRUCK_COMMANDO,
            GungeonFlags.BLUEPRINTTRUCK_AWP,
            GungeonFlags.BLUEPRINTTRUCK_NIKITA,
            GungeonFlags.BLUEPRINTTRUCK_BIGBOY,
            GungeonFlags.BLUEPRINTTRUCK_CHAFFGRENADE,
            GungeonFlags.BLUEPRINTTRUCK_PATRIOT,
            GungeonFlags.BLUEPRINTTRUCK_MAGAZINERACK,
            GungeonFlags.BLUEPRINTBEETLE_DEVOLVER,
            GungeonFlags.BLUEPRINTBEETLE_BASEBALLBAT,
            GungeonFlags.BLUEPRINTBEETLE_DAWNHAMMER,
            GungeonFlags.BLUEPRINTBEETLE_STRAFEGUN,
            GungeonFlags.BLUEPRINTBEETLE_HYPERLIGHT,
            GungeonFlags.BLUEPRINTBEETLE_STARPEW,
            GungeonFlags.BLUEPRINTBEETLE_DUELINGLASER,
            GungeonFlags.BLUEPRINTBEETLE_TELEPORTERPROTOTYPE,
            GungeonFlags.BLUEPRINTBEETLE_VORPALGUN,
            GungeonFlags.BLUEPRINTBEETLE_STOUTIES,
            GungeonFlags.BLUEPRINTBEETLE_CHARMERS,
            GungeonFlags.BLUEPRINTBEETLE_FLAKTUALLY,
            GungeonFlags.BLUEPRINTBEETLE_MAGICITES,
            GungeonFlags.BLUEPRINTBEETLE_METROIDBOMBS,
            GungeonFlags.BLUEPRINTBEETLE_SILVALLIES,
            GungeonFlags.BLUEPRINTBEETLE_HELIX,
            GungeonFlags.BLUEPRINTBEETLE_GOLDIES,
            GungeonFlags.BLUEPRINTBEETLE_BEEBULLETS,
            GungeonFlags.BLUEPRINTBEETLE_DEVOLVERBULLETS,
            GungeonFlags.BLUEPRINTBEETLE_CRITICALBULLETS,
            GungeonFlags.BLUEPRINTBEETLE_SNOWBALLETS,
            GungeonFlags.BLUEPRINTBEETLE_BIGGIANTHEAD,
            GungeonFlags.BLUEPRINTBEETLE_COMBINERIFLE,
            GungeonFlags.BLUEPRINTBEETLE_EXOTICGUN,
            GungeonFlags.BLUEPRINTBEETLE_BIGSHOTGUN,
            GungeonFlags.BLUEPRINTBEETLE_GUNDERFURY,
            GungeonFlags.BLUEPRINTBEETLE_GLAIVE,
            GungeonFlags.BLUEPRINTBEETLE_LUTE,
            GungeonFlags.BLUEPRINTBEETLE_TETRIS,
            GungeonFlags.BLUEPRINTBEETLE_TRIGUN,
            GungeonFlags.BLUEPRINTBEETLE_SHOVEL,
            GungeonFlags.BLUEPRINTBEETLE_PREDATOR,
            GungeonFlags.BLUEPRINTBEETLE_VOIDCORELAUNCHER,
            GungeonFlags.BLUEPRINTBEETLE_BLANKBULLETS,
            GungeonFlags.BLUEPRINTBEETLE_ORBITALBULLETS,
            GungeonFlags.BLUEPRINTBEETLE_SCOUTER,
            GungeonFlags.BLUEPRINTBEETLE_CONTROLLER,
            GungeonFlags.BLUEPRINTBEETLE_GUNNER
        };

        /*
         * Patch - GameStatsManager.SetFlag
         * For meta shops, capture location check without unlocking associated item.
         * Any time an item is purchased from the metashop, the associated flag is set for the save file.
         * This patch takes over that SetFlag function, checks if the flag being set is a metashop location,
         * then captures that flag as an archipelago location without unlocking the item.
         */
        [HarmonyPatch(typeof(GameStatsManager), nameof(GameStatsManager.SetFlag))]
        [HarmonyPrefix]
        public static bool CaptureMetaShopLocationChecks(GungeonFlags flag, bool value)
        {
            //TODO make collection of all meta shop flags to compare against.
            if (metaShopLocationFlags.Contains(flag))
            {
                ETGModConsole.Log("GameStatsManagerPatch: Location checked - Send item here: " + flag);
                //TODO - send items from location check
                ArchipelagoUIHelper.ArchipelagoUINotification("Sent <item> to <example>", "TODO - this is a location");
                ArchipelaGunLocalDataManager.locationsCheckedFlags.Add(flag);
                return false;
            }
            ETGModConsole.Log("GameStatsManagerPatch: Something has set a save flag: " + flag + " - " + value);
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
