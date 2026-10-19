using Alexandria.ItemAPI;
using ArchiGungeon.ArchipelagoServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ArchiGungeon.DebugTools;
using Archipelago.MultiClient.Net.Models;
using ArchiGungeon.Data;

namespace ArchiGungeon.ItemArchipelago
{

    public class NpcCellLocationItem : PassiveItem
    {
        #region Item Init
        public static int SpawnItemID = -1;
        public static int TotalNpcCells = 15;

        private static string displayName = "NPC Cell Item";
        private static string spriteDirectory = "ArchiGungeon/Resources/archipelago.png";

        public static void RegisterItemBase()
        {
            ArchDebugPrint.DebugLog(DebugCategory.PluginStartup, "Registering NpcCellLocationItem");

            GameObject obj = new GameObject(displayName);
            var item = obj.AddComponent<NpcCellLocationItem>();

            ItemBuilder.AddSpriteToObject(displayName, spriteDirectory, obj);

            //TODO Rename
            ItemBuilder.SetupItem(item, "A crossover!", "A cool item from another world", ArchipelaGunPlugin.MOD_ITEM_PREFIX);

            item.ShouldBeExcludedFromShops = true;
            item.CanBeDropped = false;
            item.CanBeSold = false;
            item.IgnoredByRat = true;
            item.quality = ItemQuality.EXCLUDED;

            SpawnItemID = PickupObjectDatabase.GetId(item);

            //ArchipelagoGUI.ConsoleLog("APItem spawn ID: " + SpawnItemID);

            return;
        }

        #endregion

        #region Item Behavior

        /*
         * potential use for adding extra NPC cell unlocks?
         */
        public static int GetBaseAPNpcCellAmount(int userOptionCase)
        {
            switch (userOptionCase)
            {
                default:
                    return 16;
            }
        }

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

            /*
            if (CountGoalManager.GetCountStat(CountStats.NpcCellsOpened) == TotalNpcCells - 1)
            {
                // TODO unlock ledge goblin
            }
            else
            {
                SessionHandler.DataSender.SendFoundLocationCheck(APLocations.NPCCell + CountGoalManager.GetCountStat(CountStats.NpcCellsOpened));
            }
            */
            if (CountGoalManager.GetCountStat(CountStats.NpcCellsOpened) < TotalNpcCells)
            {
                SessionHandler.DataSender.SendFoundLocationCheck(APLocations.NPCCell + CountGoalManager.GetCountStat(CountStats.NpcCellsOpened));
                ArchipelaGunLocalDataManager.AddToCountSaveDataEntry(CountStats.NpcCellsOpened, 1);
            }
        }

        public static bool HasAPItemChecksRemaining()
        {
            return (CountGoalManager.GetCountStat(CountStats.NpcCellsOpened) < TotalNpcCells);
        }

        #endregion
    }
}
