using ArchiGungeon.DebugTools;
using Archipelago.MultiClient.Net.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArchiGungeon.Data
{
    /// <summary>
    /// Container to represent items that will be expected to receive in AP. Contains a full dictionary of all non-base game items that should match the apworld implementation. Use GetReceivedIteminfo() to get details on a custom item id.
    /// </summary>
    public class ReceivedAPItem
    {
        public long ItemId { get; private set; }
        public string DisplayName { get; private set; }
        public ReceivedItemType Type { get; private set; }

        /// <summary>
        /// If there is a physical pickupable object that represents this item in game. Ie. can it be spawned? Else this will be null.
        /// </summary>
        public PickupObject PickupObj { get; private set; } = null;
        /// <summary>
        /// The object sent from the multiworld server with details on the item. Will be null on locally spawned items.
        /// </summary>
        public ItemInfo NetworkItemInfo { get; private set; }
        /// <summary>
        /// Name of the location the item was found in the multiworld.
        /// </summary>
        public string LocationDisplayName => NetworkItemInfo?.LocationDisplayName ?? "Debug Location";

        private ReceivedAPItem(long id, string name, ReceivedItemType type, PickupObject pickup = null)
        {
            ItemId = id;
            DisplayName = name;
            Type = type;
            PickupObj = pickup;
        }

        /// <summary>
        /// Base Id being used by the apworld logic for all EtG items.
        /// </summary>
        public const long BaseItemId = 8754000;

        /// <summary>
        /// Ids between the _baseItemId and this are Pickup objects in game.
        /// </summary>
        public const long PickupObjectIdRange = 8755000;

        private static Dictionary<long, ReceivedAPItem> _fullCustomItemDictionary = new Dictionary<long, ReceivedAPItem>()
        {
            { PickupObjectIdRange, new ReceivedAPItem(PickupObjectIdRange, "Ox and Cadence", ReceivedItemType.MetaShop) },
            { PickupObjectIdRange + 1, new ReceivedAPItem(PickupObjectIdRange + 1, "Professor Goopton", ReceivedItemType.MetaShop) },
            { PickupObjectIdRange + 2, new ReceivedAPItem(PickupObjectIdRange + 2, "Professor Trorc", ReceivedItemType.MetaShop) },
            { PickupObjectIdRange + 3, new ReceivedAPItem(PickupObjectIdRange + 3, "Doug", ReceivedItemType.MetaShop) },
            { PickupObjectIdRange + 4, new ReceivedAPItem(PickupObjectIdRange + 4, "Tinker", ReceivedItemType.NPC) },
            { PickupObjectIdRange + 5, new ReceivedAPItem(PickupObjectIdRange + 5, "Sorceress", ReceivedItemType.NPC) },
            { PickupObjectIdRange + 6, new ReceivedAPItem(PickupObjectIdRange + 6, "Daisuke", ReceivedItemType.NPC) },
            { PickupObjectIdRange + 7, new ReceivedAPItem(PickupObjectIdRange + 7, "Frifle and the Grey Mauser", ReceivedItemType.NPC) },
            { PickupObjectIdRange + 8, new ReceivedAPItem(PickupObjectIdRange + 8, "Gunsling King and Manservantes", ReceivedItemType.NPC) },
            { PickupObjectIdRange + 9, new ReceivedAPItem(PickupObjectIdRange + 9, "The Lost Adventurer", ReceivedItemType.NPC) },
            { PickupObjectIdRange + 10, new ReceivedAPItem(PickupObjectIdRange + 10, "Ledge Goblin", ReceivedItemType.NPC) },
            { PickupObjectIdRange + 11, new ReceivedAPItem(PickupObjectIdRange + 11, "Tonic", ReceivedItemType.NPC) },
            { PickupObjectIdRange + 12, new ReceivedAPItem(PickupObjectIdRange + 12, "Bowler", ReceivedItemType.NPC) },
            { PickupObjectIdRange + 13, new ReceivedAPItem(PickupObjectIdRange + 13, "Old Red", ReceivedItemType.NPC) },
            { PickupObjectIdRange + 14, new ReceivedAPItem(PickupObjectIdRange + 14, "Flynt", ReceivedItemType.NPC) },
            { PickupObjectIdRange + 15, new ReceivedAPItem(PickupObjectIdRange + 15, "Cursula", ReceivedItemType.NPC) },
            { PickupObjectIdRange + 16, new ReceivedAPItem(PickupObjectIdRange + 16, "Synergrace", ReceivedItemType.NPC) }
        };

        /// <summary>
        /// Given a the AP item id of a EtG AP item, returns an object with details on said item.
        /// </summary>
        /// <remarks>Expected base id at 8754000.</remarks>
        /// <param name="id">Internal AP id to look up.</param>
        /// <returns>ReceivedAPItem - Item class with details on the received item.</returns>
        public static ReceivedAPItem GetReceivedAPItem(long id)
        {
            _fullCustomItemDictionary.TryGetValue(id, out ReceivedAPItem item);

            // If value is not in the dictionary it may be a base EtG item, which we can subtract our base id from to get the EtG item id.
            if(item == null && id >= BaseItemId && id < PickupObjectIdRange)
            {
                try
                {
                    var pickUpObj = PickupObjectDatabase.GetById((int)(id - BaseItemId));
                    if (pickUpObj != null)
                    {
                        item = new ReceivedAPItem(id, pickUpObj.DisplayName, ReceivedItemType.ETGBaseItemFiller, pickUpObj);
                    }
                }
                catch (Exception e)
                {
                    //print message, but otherwise ignore and return null item.
                    ArchDebugPrint.DebugLog(DebugCategory.ItemHandling, $"Issue getting pickup object for id: {id}");
                }
            }

            return item;
        }
    }

    public enum ReceivedItemType
    { 
        /// <summary>
        /// Regular gun, active item, or passive item drops. Including in base pool or not.
        /// </summary>
        ETGBaseItemFiller,
        /// <summary>
        /// Unlocks access to a shop in the foyer or new stock for an existing shop.
        /// </summary>
        MetaShop,
        /// <summary>
        /// Non-foyer shop NPCs either unlocked or progress in their quests.
        /// </summary>
        NPC,
        /// <summary>
        /// Any custom progression thats newly added to speed up AP progression (ie. random start chest, hedgemony credits multiplier, base coolness increase, etc.)
        /// </summary>
        CustomProgressive,
        /// <summary>
        /// Consumables that will be used in a run and thats it. Hearts, casings, keys, etc.
        /// </summary>
        ConsumableFiller,
        /// <summary>
        /// Unlocks access to player as another Gungeneer
        /// </summary>
        CharacterUnlock,
        /// <summary>
        /// Alt skin unlocks for characters or guns
        /// </summary>
        CosmeticFiller,
        /// <summary>
        /// Items needed to access new areas in the gungeon or get to completing the AP goal. (ie. bullet parts, infuriating note, old crest, etc.)
        /// </summary>
        CoreProgression,
        /// <summary>
        /// Will spawn a trap.
        /// </summary>
        Trap
    }
}
