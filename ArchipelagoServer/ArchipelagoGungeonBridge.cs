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

namespace ArchiGungeon.ArchipelagoServer
{
    public class ArchipelagoGungeonBridge
    {
        #region APWorld Data
        private static readonly long _baseItemID = 8754000;
        private static readonly long _gunAndItemIdRange = 1000;
        private static readonly long _progressiveRange = 1000;
        private static readonly long _fillerRange = 1000;

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
        /// <param name="receivedItemID">Item id from AP of item to give. This is a custom set Id that will match whats set in the APWorld.</param>
        public static void GiveGungeonItem(long receivedItemID)
        {
            long adjustedItemId = receivedItemID - _baseItemID;
            if(adjustedItemId < _gunAndItemIdRange)
            {
                //TODO: Also unlock the item in the ammonomicon/loot pool
                var spawneditem = SpecificItemSpawnHandler.GivePlayerSpecificItem((int)adjustedItemId);
                GameStatsManager.Instance.ForceUnlock(spawneditem.GetComponent<EncounterTrackable>()?.EncounterGuid);
            }
            else if(adjustedItemId < _progressiveRange)
            {

            }
            else if(adjustedItemId < _fillerRange)
            {

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
        /// <summary>
        /// Just checks if the item Id is the undoCurseItemID
        /// </summary>
        /// <param name="itemIdToCheck"></param>
        /// <returns>true if its an undo Curse item</returns>
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

