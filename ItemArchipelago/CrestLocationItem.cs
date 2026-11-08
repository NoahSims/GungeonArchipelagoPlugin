using Alexandria.ItemAPI;
using ArchiGungeon.ArchipelagoServer;
using ArchiGungeon.Data;
using ArchiGungeon.DebugTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArchiGungeon.ItemArchipelago
{
    public class CrestLocationItem : PassiveItem
    {
        #region Item Init
        public static int SpawnItemID = -1;

        private static string displayName = "Old Crest Location Item";
        private static string spriteDirectory = "ArchiGungeon/Resources/archipelago.png";

        public static void RegisterItemBase()
        {
            ArchDebugPrint.DebugLog(DebugCategory.PluginStartup, "Registering CrestLocationItem");

            GameObject obj = new GameObject(displayName);
            var item = obj.AddComponent<CrestLocationItem>();

            ItemBuilder.AddSpriteToObject(displayName, spriteDirectory, obj);

            //TODO Rename
            ItemBuilder.SetupItem(item, "Old Crest Location", "Item to represent the location for getting the Old Crest.", ArchipelaGunPlugin.MOD_ITEM_PREFIX);

            item.ShouldBeExcludedFromShops = true;
            item.CanBeDropped = false;
            item.CanBeSold = false;
            item.IgnoredByRat = true;
            item.quality = ItemQuality.EXCLUDED;

            SpawnItemID = PickupObjectDatabase.GetId(item);

            return;
        }

        #endregion

        #region Item Behavior

        public override void Pickup(PlayerController player)
        {
            base.Pickup(player);

            SessionHandler.DataSender.SendFoundLocationCheck(APLocations.OldCrestPickup);
        }

        #endregion
    }
}
