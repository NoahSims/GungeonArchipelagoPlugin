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
            { PickupObjectIdRange + 16, new ReceivedAPItem(PickupObjectIdRange + 16, "Synergrace", ReceivedItemType.NPC) },
            { PickupObjectIdRange + 17, new ReceivedAPItem(PickupObjectIdRange + 17, "Progressive Cassing Multiplier", ReceivedItemType.CustomProgressive) },
            { PickupObjectIdRange + 18, new ReceivedAPItem(PickupObjectIdRange + 18, "Progressive Hedgemony Multiplier", ReceivedItemType.CustomProgressive) },
            { PickupObjectIdRange + 19, new ReceivedAPItem(PickupObjectIdRange + 19, "Progressive Curse Reduction", ReceivedItemType.CustomProgressive) },
            { PickupObjectIdRange + 20, new ReceivedAPItem(PickupObjectIdRange + 20, "Progressive Coolness", ReceivedItemType.CustomProgressive) },
            { PickupObjectIdRange + 21, new ReceivedAPItem(PickupObjectIdRange + 21, "Progressive Start Chest", ReceivedItemType.CustomProgressive) },
            { PickupObjectIdRange + 22, new ReceivedAPItem(PickupObjectIdRange + 22, "5 Hedgemoney Credits", ReceivedItemType.ConsumableFiller) }, // Consider one object to represent all numbers of consumables that can vary.
            { PickupObjectIdRange + 23, new ReceivedAPItem(PickupObjectIdRange + 23, "50 Casings", ReceivedItemType.ConsumableFiller) },
            { PickupObjectIdRange + 24, new ReceivedAPItem(PickupObjectIdRange + 24, "2 Keys", ReceivedItemType.ConsumableFiller) },
            { PickupObjectIdRange + 25, new ReceivedAPItem(PickupObjectIdRange + 25, "Alt Costume - Knight", ReceivedItemType.CosmeticFiller) },
            { PickupObjectIdRange + 26, new ReceivedAPItem(PickupObjectIdRange + 26, "Alt Costume - Beastmaster", ReceivedItemType.CosmeticFiller) },
            { PickupObjectIdRange + 27, new ReceivedAPItem(PickupObjectIdRange + 27, "Alt Costume - Jailbird", ReceivedItemType.CosmeticFiller) },
            { PickupObjectIdRange + 28, new ReceivedAPItem(PickupObjectIdRange + 28, "Alt Costume - Rogue", ReceivedItemType.CosmeticFiller) },
            { PickupObjectIdRange + 29, new ReceivedAPItem(PickupObjectIdRange + 29, "Alt Costume - Termbot", ReceivedItemType.CosmeticFiller) },
            { PickupObjectIdRange + 30, new ReceivedAPItem(PickupObjectIdRange + 30, "Alt Costume - Rabbit", ReceivedItemType.CosmeticFiller) },
            { PickupObjectIdRange + 31, new ReceivedAPItem(PickupObjectIdRange + 31, "Alt Costume - Shell", ReceivedItemType.CosmeticFiller) },
            { PickupObjectIdRange + 32, new ReceivedAPItem(PickupObjectIdRange + 32, "Playable Character - The Pilot", ReceivedItemType.CharacterUnlock) },
            { PickupObjectIdRange + 33, new ReceivedAPItem(PickupObjectIdRange + 33, "Playable Character - The Marine", ReceivedItemType.CharacterUnlock) },
            { PickupObjectIdRange + 34, new ReceivedAPItem(PickupObjectIdRange + 34, "Playable Character - The Convict", ReceivedItemType.CharacterUnlock) },
            { PickupObjectIdRange + 35, new ReceivedAPItem(PickupObjectIdRange + 35, "Playable Character - The Hunter", ReceivedItemType.CharacterUnlock) },
            { PickupObjectIdRange + 36, new ReceivedAPItem(PickupObjectIdRange + 36, "Playable Character - The Robot", ReceivedItemType.CharacterUnlock) },
            { PickupObjectIdRange + 37, new ReceivedAPItem(PickupObjectIdRange + 37, "Playable Character - The Bullet", ReceivedItemType.CharacterUnlock) },
            { PickupObjectIdRange + 38, new ReceivedAPItem(PickupObjectIdRange + 38, "Playable Character - The Gunslinger", ReceivedItemType.CharacterUnlock) },
            { PickupObjectIdRange + 39, new ReceivedAPItem(PickupObjectIdRange + 39, "Shortcut elevator - Level 2", ReceivedItemType.KeyUnlock) },
            { PickupObjectIdRange + 40, new ReceivedAPItem(PickupObjectIdRange + 40, "Shortcut elevator - Level 3", ReceivedItemType.KeyUnlock) },
            { PickupObjectIdRange + 41, new ReceivedAPItem(PickupObjectIdRange + 41, "Shortcut elevator - Level 4", ReceivedItemType.KeyUnlock) },
            { PickupObjectIdRange + 42, new ReceivedAPItem(PickupObjectIdRange + 42, "Shortcut elevator - Level 5", ReceivedItemType.KeyUnlock) },
            { PickupObjectIdRange + 43, new ReceivedAPItem(PickupObjectIdRange + 43, "Boss Rush", ReceivedItemType.KeyUnlock) },
            { PickupObjectIdRange + 44, new ReceivedAPItem(PickupObjectIdRange + 44, "Payday Items Available to Steal", ReceivedItemType.KeyUnlock) },
            { PickupObjectIdRange + 45, new ReceivedAPItem(PickupObjectIdRange + 45, "Vampire", ReceivedItemType.NPC) },
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
        Trap,
        /// <summary>
        /// Items that set flags allowing access to goals, new areas, npc quest progression, etc.
        /// </summary>
        KeyUnlock
    }
}
