using ArchiGungeon.Character;
using ArchiGungeon.Data;
using ArchiGungeon.DebugTools;
using ArchiGungeon.GungeonEventHandlers;
using ArchiGungeon.ItemArchipelago;
using ArchiGungeon.UserInterface;
using Archipelago.MultiClient.Net.Models;
using Archipelago.MultiClient.Net.Enums;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArchiGungeon.ArchipelagoServer
{
    public class ArchipelagoGungeonBridge
    {
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
        /// <param name="apItem">Item id from AP of item to give. This is a custom set Id that will match whats set in the APWorld.</param>
        public static void GiveGungeonItem(ReceivedAPItem apItem)
        {
            ArchipelagoUIHelper.ArchipelagoUINotification("Received " + apItem.DisplayName, "Found in " + apItem.LocationDisplayName);

            switch (apItem.Type)
            {
                case ReceivedItemType.ETGBaseItemFiller:
                    // TODO refactor this now that we get the pickup obj earlier
                    var spawneditem = SpecificItemSpawnHandler.GivePlayerSpecificItem(apItem.PickupObj.PickupObjectId);
                    GameStatsManager.Instance.ForceUnlock(spawneditem.GetComponent<EncounterTrackable>()?.EncounterGuid);
                    break;
                case ReceivedItemType.MetaShop:
                case ReceivedItemType.NPC:
                    UnlockNPC(apItem.ItemId);
                    break;
                case ReceivedItemType.CustomProgressive:
                    break;
                case ReceivedItemType.ConsumableFiller:
                    break;
                case ReceivedItemType.CharacterUnlock:
                    break;
                case ReceivedItemType.CosmeticFiller:
                    break;
                case ReceivedItemType.CoreProgression:
                    break;
                case ReceivedItemType.Trap:
                    break;
                default:
                    break;
            }
        }

        private static void GiveFiller()
        {

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
        public static void UnlockNPC(long NPCId)
        {
            var simplifiedId = NPCId - ReceivedAPItem.PickupObjectIdRange;
            ArchDebugPrint.DebugLog(DebugCategory.ServerReceive, "Unlocking NPC: " + NPCId);
            switch (simplifiedId)
            {
                case 0: // Ox and Candence
                    GameStatsManager.Instance.SetFlag(GungeonFlags.META_SHOP_ACTIVE_IN_FOYER, true);
                    break;
                case 1: // Prof Goop
                    GameStatsManager.Instance.SetFlag(GungeonFlags.SHOP_GOOP_ACTIVE, true);
                    GameStatsManager.Instance.SetFlag(GungeonFlags.SHOP_HAS_MET_GOOP, true);
                    // Goopton needs a special flag set on any character that they purchased at least once from the in run shop.
                    GameStats goopStats = new GameStats();
                    goopStats.SetStat(TrackedStats.MERCHANT_PURCHASES_GOOP, 1);
                    GameStatsManager.Instance.m_characterStats[PlayableCharacters.Pilot].AddStats(goopStats);
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
                case 5: // Sorceress
                    GameStatsManager.Instance.SetFlag(GungeonFlags.SORCERESS_ACTIVE_IN_FOYER, true);
                    break;
                case 6: // Daisuke
                    GameStatsManager.Instance.SetFlag(GungeonFlags.DAISUKE_ACTIVE_IN_FOYER, true);
                    break;
                case 7: // Frifle and the Grey Mauser
                    GameStatsManager.Instance.SetFlag(GungeonFlags.FRIFLE_ACTIVE_IN_FOYER, true);
                    break;
                case 8: // Gunsling King and Manservantes
                    GameStatsManager.Instance.SetFlag(GungeonFlags.GUNSLING_KING_RESCUED_FROM_CELL, true);
                    GameStatsManager.Instance.SetFlag(GungeonFlags.GUNSLING_KING_ACTIVE_IN_FOYER, true);
                    break;
                case 9: // The Lost Adventurer
                    GameStatsManager.Instance.SetFlag(GungeonFlags.LOST_ADVENTURER_RESCUED_FROM_CELL, true);
                    break;
                case 10: // Ledge Goblin
                    GameStatsManager.Instance.SetFlag(GungeonFlags.LEDGEGOBLIN_ACTIVE_IN_FOYER, true);
                    break;
                case 11: // Tonic
                    GameStatsManager.Instance.SetFlag(GungeonFlags.TONIC_ACTIVE_IN_FOYER, true);
                    break;
                case 12: // Bowler
                    GameStatsManager.Instance.SetFlag(GungeonFlags.BOWLER_ACTIVE_IN_FOYER, true);
                    break;
                case 13: // Old Red
                    GameStatsManager.Instance.SetFlag(GungeonFlags.SHOP_BLANK_ACTIVE, true);
                    break;
                case 14: // Flynt
                    GameStatsManager.Instance.SetFlag(GungeonFlags.SHOP_KEY_ACTIVE, true);
                    break;
                case 15: // Cursula
                    GameStatsManager.Instance.SetFlag(GungeonFlags.SHOP_CURSE_ACTIVE, true);
                    break;
                case 16: // Synergrace
                    GameStatsManager.Instance.SetFlag(GungeonFlags.SYNERGRACE_UNLOCKED, true);
                    break;
                case 45: // Vampire
                    GameStatsManager.Instance.SetFlag(GungeonFlags.VAMPIRE_RELEASED, true);
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

