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
            if (flag == GungeonFlags.BLUEPRINTMETA_HEARTBOTTLE)
            {
                //ETGModConsole.Log("GameStatsManagerPatch: Heartbottle purchased - Location checked. Send item here");
                //TODO - send items from location check
                AchripelagoUIHelper.ArchipelagoUINotification("Sent <item> to <example>", "TODO - this is a location");
                SaveDataManagement.locationsCheckedFlags.Add(flag);
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
            ETGModConsole.Log("Patch_GetFlagFromTargetItem: " + shopItemId);
            GungeonFlags flag = GungeonFlags.NONE;
            PickupObject byId = PickupObjectDatabase.GetById(shopItemId);
            foreach (var prereq in byId.encounterTrackable.prerequisites)
            {
                if(prereq.prerequisiteType == DungeonPrerequisite.PrerequisiteType.FLAG)
                {
                    flag = prereq.saveFlagToCheck;
                }
            }
            if (SaveDataManagement.locationsCheckedFlags.Contains(flag))
            {
                //ETGModConsole.Log("GameStatsManagerPatch: MetaShopController attempting to put heart bottle in shop");
                __result = GungeonFlags.BLUEPRINTMETA_MTXGUN;
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
