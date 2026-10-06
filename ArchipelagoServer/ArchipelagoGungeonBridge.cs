using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using ArchiGungeon.GungeonEventHandlers;
using ArchiGungeon.ItemArchipelago;
using ArchiGungeon.DebugTools;
using ArchiGungeon.Character;
using ArchiGungeon.Data;
using HutongGames.PlayMaker.Actions;
using Archipelago.MultiClient.Net.Models;

namespace ArchiGungeon.ArchipelagoServer
{
    public class ArchipelagoGungeonBridge
    {
        #region APWorld Data
        private static readonly long _baseItemID = 8754000;
        private static readonly long _gunAndItemIdRange = 1000;
        private static readonly long _npcRange = 1100;
        private static readonly long _fillerRange = 1200;

        //private static readonly long consumableCategoryItemID = 8754100;
        //private static readonly long trapCategoryItemID = 8754200;
        //private static readonly long progressionItemID = 8754300;
        //private static readonly long paradoxCharacterItemID = 8754400;
        //private static readonly long undoCurseItemID = 8754500;

        #endregion

        #region Player References
        private static PlayerController playerOne;
        private static PlayerController playerTwo;

        public static void SetPlayerOne(PlayerController controller)
        {
            ArchDebugPrint.DebugLog(DebugCategory.PlayerEventListener, "Setting palyer One");
            playerOne = controller;
            return;
        }

        public static void SetPlayerTwo(PlayerController controller) 
        {
            playerTwo = controller;
            return;
        }
        #endregion

        #region Server Receive Events
        public static void DeathlinkKillPlayer(string causeOfDeath = "Deathlink")
        {
            
            if(playerTwo != null)
            {
                if(playerTwo.healthHaver.IsAlive && playerOne.healthHaver.IsDead)
                {
                    ArchDebugPrint.DebugLog(DebugCategory.ServerReceive, "Attempting to kill Player Two");

                    playerTwo.healthHaver.lastIncurredDamageSource = causeOfDeath;
                    playerTwo.healthHaver.Die(Vector2.zero);
                    return;
                }
                else if(playerTwo.healthHaver.IsAlive)
                {
                    ArchDebugPrint.DebugLog(DebugCategory.ServerReceive, "Soft killing Player Two");

                    //playerTwo.healthHaver.ManualDeathHandling = true;
                    playerTwo.healthHaver.currentHealth = 0f;
                    playerTwo.healthHaver.lastIncurredDamageSource = causeOfDeath;
                    playerTwo.Fall();

                    return;
                }

            }

            ArchDebugPrint.DebugLog(DebugCategory.ServerReceive, "Killing Player One");
            playerOne.healthHaver.lastIncurredDamageSource = causeOfDeath;
            playerOne.Die(Vector2.zero);

            return;
        }

        /// <summary>
        /// Unlocks the item or spawns it if appropriate.
        /// </summary>
        /// <param name="receivedItem">Item id from AP of item to give. This is a custom set Id that will match whats set in the APWorld.</param>
        public static void GiveGungeonItem(ItemInfo receivedItem)
        {
            // TODO: Add item recieve banner here.
            // TODO: remove this weird id range logic to more defined flags.
            // Reduce the AP Item Id down to an Id that matches the internal item ids for guns/items.
            long adjustedItemId = receivedItem.ItemId - _baseItemID;
            if(adjustedItemId < _gunAndItemIdRange)
            {
                var spawneditem = SpecificItemSpawnHandler.GivePlayerSpecificItem((int)adjustedItemId);
                GameStatsManager.Instance.ForceUnlock(spawneditem.GetComponent<EncounterTrackable>()?.EncounterGuid);
            }
            else if(adjustedItemId < _npcRange)
            {
                UnlockNPC((int)adjustedItemId - (int)_gunAndItemIdRange);
            }
            else if(adjustedItemId < _fillerRange)
            {
                TrapSpawnHandler.SpawnTrapByCase((int)adjustedItemId - (int)_npcRange); //I will be changing this.
            }
        }

        /* Old giving of items based more on random items than specific ones.
        public static void GiveGungeonItem(long receivedItemID)
        {
            // Is it a curse reverse?
            if(CheckIDForSpecificItem(receivedItemID))
            {
                return;
            }

            // gun & passives
            long categoryAdjustedID = receivedItemID - (long)baseItemID;
            if (categoryAdjustedID < 100)
            {
                RandomizedByQualityItems.GiveRandomizedItemByCase((int)categoryAdjustedID);
                return;
            }

            // consumables
            categoryAdjustedID = receivedItemID - (long)consumableCategoryItemID;
            if(categoryAdjustedID < 100)
            {
                ConsumableSpawnHandler.SpawnConsumableByCase((int)categoryAdjustedID);
                return;
            }

            // traps
            categoryAdjustedID = receivedItemID - (long)trapCategoryItemID;
            if (categoryAdjustedID < 100)
            {
                TrapSpawnHandler.SpawnTrapByCase((int)categoryAdjustedID);
                return;
            }

            // progression
            categoryAdjustedID = receivedItemID - (long)progressionItemID;
            if (categoryAdjustedID < 100)
            {
                ProgressionItemSpawnHandler.SpawnProgressionItem((int)categoryAdjustedID);
                return;
            }

            // paradox mode
            categoryAdjustedID = receivedItemID - (long)paradoxCharacterItemID;
            if (categoryAdjustedID < 100)
            {
                CharSwap.ReceiveParadoxModeItem((int)categoryAdjustedID);
                return;
            }

            return;
        }
        */
        #endregion

        #region Item Spawning
        
        //private static bool CheckIDForSpecificItem(long itemIdToCheck)
        //{
        //    bool matchedItem = false;

        //    if(itemIdToCheck == undoCurseItemID)
        //    {
        //        GiveUndoReverseCurse(1);
        //        matchedItem = true;
        //    }


        //    return matchedItem;
        //}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NPCId"></param>
        public static void UnlockNPC(int NPCId)
        {
            ArchDebugPrint.DebugLog(DebugCategory.ServerReceive, "Unlocking NPC: " + NPCId);
            switch (NPCId)
            {
                case 0: // Ox and Candence
                    GameStatsManager.Instance.SetFlag(GungeonFlags.META_SHOP_ACTIVE_IN_FOYER, true);
                    break;
                case 1: // Prof Goop
                    GameStatsManager.Instance.SetFlag(GungeonFlags.SHOP_GOOP_ACTIVE, true);
                    GameStatsManager.Instance.SetFlag(GungeonFlags.SHOP_HAS_MET_GOOP, true);
                    break;
                case 2: // Trorc
                    GameStatsManager.Instance.SetFlag(GungeonFlags.SHOP_TRUCK_ACTIVE, true);
                    break;
                case 3: // Doug
                    GameStatsManager.Instance.SetFlag(GungeonFlags.SHOP_BEETLE_ACTIVE, true);
                    break;
                case 4: // Tinker
                    GameStatsManager.Instance.SetFlag(GungeonFlags.SHERPA_ACTIVE_IN_ELEVATOR_ROOM, true);
                    GameStatsManager.Instance.SetFlag(GungeonFlags.SHERPA_READY_FOR_UNLOCKS, true);
                    break;
                default:
                    break;
            }
        }

        public static void SpawnAPItem(int numberToSpawn)
        {
            PlayerController playerToSpawnOn;

            if(playerOne.healthHaver.IsAlive)
            {
                playerToSpawnOn = playerOne;
            }

            else if (playerTwo != null)
            {
                if(playerTwo.healthHaver.IsDead)
                {
                    return;
                }
                playerToSpawnOn = playerTwo;
            }

            else
            {
                return;
            }

            for(int i=0; i < numberToSpawn; i++)
            {
                GameObject archipelItem = PickupObjectDatabase.GetById(APPickUpItem.SpawnItemID).gameObject;
                LootEngine.SpawnItem(archipelItem, playerToSpawnOn.CenterPosition, Vector2.zero, 0);
            }

            return;
        }

        public static void GiveReverseCurse(int numberToSpawn)
        {
            PlayerController playerToSpawnOn;

            if (playerOne.healthHaver.IsAlive)
            {
                playerToSpawnOn = playerOne;
            }

            else if (playerTwo != null)
            {
                if (playerTwo.healthHaver.IsDead)
                {
                    return;
                }
                playerToSpawnOn = playerTwo;
            }
            else
            {
                return;
            }

            for (int i = 0; i < numberToSpawn; i++)
            {
                PickupObject cursePassive = PickupObjectDatabase.GetById(ReverseCurse.SpawnItemID);
                playerToSpawnOn.AcquirePassiveItemPrefabDirectly((PassiveItem)cursePassive);
            }

            return;
        }

        public static void GiveUndoReverseCurse(int numberToSpawn)
        {
            PlayerController playerToSpawnOn;

            if (playerOne.healthHaver.IsAlive)
            {
                playerToSpawnOn = playerOne;
            }

            else if (playerTwo != null)
            {
                if (playerTwo.healthHaver.IsDead)
                {
                    return;
                }
                playerToSpawnOn = playerTwo;
            }
            else
            {
                return;
            }

            for (int i = 0; i < numberToSpawn; i++)
            {
                PickupObject undoCursePassive = PickupObjectDatabase.GetById(ReverseCurseReversal.SpawnItemID);
                playerToSpawnOn.AcquirePassiveItemPrefabDirectly((PassiveItem)undoCursePassive);
            }

            return;
        }
        #endregion
    }

}

