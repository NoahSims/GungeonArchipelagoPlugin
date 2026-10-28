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
using Alexandria;

namespace ArchiGungeon.ArchipelagoServer
{
    public class ArchipelagoGungeonBridge
    {
        #region Player References
        private static PlayerController playerOne;
        private static PlayerController playerTwo;

        public static void SetPlayerOne(PlayerController controller)
        {
            ArchDebugPrint.DebugLog(DebugCategory.PlayerEventListener, "Setting player One");
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
            if(apItem != null)
            { 
                ArchipelagoUIHelper.ArchipelagoUINotification("Received " + apItem.DisplayName, "Found in " + apItem.LocationGame);

                switch (apItem.Type)
                {
                    case ReceivedItemType.ETGBaseItemFiller:
                        // TODO refactor this now that we get the pickup obj earlier
                        var spawneditem = SpecificItemSpawnHandler.GivePlayerSpecificItem(apItem.PickupObj.PickupObjectId);
                        GameStatsManager.Instance.ForceUnlock(spawneditem.GetComponent<EncounterTrackable>()?.EncounterGuid);
                        break;
                    case ReceivedItemType.FoyerShop:
                    case ReceivedItemType.NPC:
                        UnlockNPC(apItem.ItemId);
                        break;
                    case ReceivedItemType.CustomProgressive:
                        CustomProgressiveUnlocks(apItem);
                        break;
                    case ReceivedItemType.ConsumableFiller:
                        GiveCurrency(apItem);
                        break;
                    case ReceivedItemType.CharacterUnlock:
                        // TODO
                        break;
                    case ReceivedItemType.CosmeticFiller:
                        // TODO
                        break;
                    case ReceivedItemType.KeyUnlock:
                    case ReceivedItemType.CoreProgression:
                        UnlockProgressFlag(apItem);
                        break;
                    case ReceivedItemType.Trap:
                        // TODO
                        break;
                    default:
                        break;
                }
            }
            else
            {
                ArchDebugPrint.DebugLog(DebugCategory.Error, "Error giving gungeon item, apitem is null!");
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
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.META_SHOP_ACTIVE_IN_FOYER, true);
                    ArchipelaGunLocalDataManager.MetaShopProgCount++;
                    break;
                case 1: // Prof Goop
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHOP_GOOP_ACTIVE, true);
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHOP_HAS_MET_GOOP, true);
                    // Goopton needs a special flag set on any character that they purchased at least once from the in run shop.
                    GameStats goopStats = new GameStats();
                    goopStats.SetStat(TrackedStats.MERCHANT_PURCHASES_GOOP, 1);
                    GameStatsManager.Instance.m_characterStats.GetFirst().Value.AddStats(goopStats);
                    ArchipelaGunLocalDataManager.GoopShopProgCount++;
                    break;
                case 2: // Trorc
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHOP_TRUCK_ACTIVE, true);
                    ArchipelaGunLocalDataManager.TrorcShopProgCount++;
                    break;
                case 3: // Doug
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHOP_BEETLE_ACTIVE, true);
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHOP_HAS_MET_BEETLE, true);
                    ArchipelaGunLocalDataManager.DougShopProgCount++;
                    break;
                case 4: // Tinker
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHERPA_ACTIVE_IN_ELEVATOR_ROOM, true);
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHERPA_READY_FOR_UNLOCKS, true);
                    break;
                case 5: // Sorceress
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SORCERESS_ACTIVE_IN_FOYER, true);
                    break;
                case 6: // Daisuke
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.DAISUKE_ACTIVE_IN_FOYER, true);
                    break;
                case 7: // Frifle and the Grey Mauser
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.FRIFLE_ACTIVE_IN_FOYER, true);
                    break;
                case 8: // Gunsling King and Manservantes
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.GUNSLING_KING_RESCUED_FROM_CELL, true);
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.GUNSLING_KING_ACTIVE_IN_FOYER, true);
                    break;
                case 9: // The Lost Adventurer
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.LOST_ADVENTURER_RESCUED_FROM_CELL, true);
                    break;
                case 10: // Ledge Goblin
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.LEDGEGOBLIN_ACTIVE_IN_FOYER, true);
                    break;
                case 11: // Tonic
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.TONIC_ACTIVE_IN_FOYER, true);
                    break;
                case 12: // Bowler
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.BOWLER_ACTIVE_IN_FOYER, true);
                    break;
                case 13: // Old Red
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHOP_BLANK_ACTIVE, true);
                    break;
                case 14: // Flynt
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHOP_KEY_ACTIVE, true);
                    break;
                case 15: // Cursula
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHOP_CURSE_ACTIVE, true);
                    break;
                case 16: // Synergrace
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SYNERGRACE_UNLOCKED, true);
                    break;
                case 45: // Vampire
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.VAMPIRE_RELEASED, true);
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// All non-npc progress items that are unlocked just through setting GameStatsMangager flags.
        /// </summary>
        /// <param name="item"></param>
        public static void UnlockProgressFlag(ReceivedAPItem item)
        {
            ArchDebugPrint.DebugLog(DebugCategory.ServerReceive, "Unlocking Core Progression: " + item.DisplayName);
            switch (item.DisplayName)
            {
                case "Prime Primer":
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.BLACKSMITH_ELEMENT1, true);
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.BLACKSMITH_MET_PREVIOUSLY, true);
                    break;
                case "Planar Lead":
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.BLACKSMITH_ELEMENT2, true);
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.BLACKSMITH_MET_PREVIOUSLY, true);
                    break;
                case "Obsidian Shell Casing":
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.BLACKSMITH_ELEMENT3, true);
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.BLACKSMITH_MET_PREVIOUSLY, true);
                    break;
                case "Arcane Powder":
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.BLACKSMITH_ELEMENT4, true);
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.BLACKSMITH_MET_PREVIOUSLY, true);
                    break;
                case "Shortcut elevator - Level 2":
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHERPA_UNLOCK1_COMPLETE, true);
                    break;
                case "Shortcut elevator - Level 3":
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHERPA_UNLOCK2_COMPLETE, true);
                    break;
                case "Shortcut elevator - Level 4":
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHERPA_UNLOCK3_COMPLETE, true);
                    break;
                case "Shortcut elevator - Level 5":
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHERPA_UNLOCK4_COMPLETE, true);
                    break;
                case "Boss Rush":
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHERPA_HAS_UNLOCKED_BOSSRUSH, true);
                    break;
                case "Payday Items Available to Steal":
                    GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.ACHIEVEMENT_BIGGEST_WALLET, true);
                    break;
                default:
                    break;
            }
        }

        public static void CustomProgressiveUnlocks(ReceivedAPItem unlockItem)
        {
            switch (unlockItem.DisplayName)
            {
                case "Tinker Progressive":
                    // Go in reverse order and set the next flag true
                    if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK4_COMPLETE))
                    {
                        GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHERPA_HAS_UNLOCKED_BOSSRUSH, true);
                    }
                    else if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK3_COMPLETE))
                    {
                        GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHERPA_UNLOCK4_COMPLETE, true);
                    }
                    else if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK2_COMPLETE))
                    {
                        GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHERPA_UNLOCK3_COMPLETE, true);
                    }
                    else if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_UNLOCK1_COMPLETE))
                    {
                        GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHERPA_UNLOCK2_COMPLETE, true);
                    }
                    else if (GameStatsManager.Instance.GetFlag(GungeonFlags.SHERPA_ACTIVE_IN_ELEVATOR_ROOM))
                    {
                        GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHERPA_UNLOCK1_COMPLETE, true);
                    }
                    else // Do we want tinker unlock to be first? Or just go to the first shortcut?
                    {
                       GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHERPA_ACTIVE_IN_ELEVATOR_ROOM, true);
                       GungeonFlagHandler.SafelySetGungeonFlag(GungeonFlags.SHERPA_READY_FOR_UNLOCKS, true);
                    }
                    break;
                case "Progressive Casing Multiplier":
                    //ArchipelaGunLocalDataManager.locationsCheckedFlags.Add(null);
                    break;
                case "Progressive Hedgemony Credit Multiplier":
                    if(ArchipelaGunLocalDataManager.CreditMultiplier < 4) // Magic number hard cap to 3 progressive items.
                    {
                        ArchipelaGunLocalDataManager.CreditMultiplier++;
                        SetMetaCreditMultiplier();
                    }
                    break;
                case "Progressive Curse Reduction":
                    //ArchipelaGunLocalDataManager.locationsCheckedFlags.Add(null);
                    break;
                case "Progressive Coolness":
                    //ArchipelaGunLocalDataManager.locationsCheckedFlags.Add(null);
                    break;
                case "Progressive Start Chest":
                    //ArchipelaGunLocalDataManager.locationsCheckedFlags.Add(null);
                    break;
                default:
                    break;
            }
        }

        public static void GiveCurrency(ReceivedAPItem unlockItem)
        {
            int count = 0;
            string itemId = "hegemony_credit"; // Move to within the switch statement if this is used for other currencies.
            switch (unlockItem.DisplayName)
            {
                case "5 Hegemony Credits":
                    count = 5;
                    break;
                case "10 Hegemony Credits":
                    count = 10;
                    break;
                case "20 Hegemony Credits":
                    count = 20;
                    break;
            }

            for (int i = 0; i < count; i++)
            {
                GameManager.Instance.PrimaryPlayer.GiveItem(itemId);
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

        public static void RunStartCustomProgressives()
        {
            SetMetaCreditMultiplier();
        }

        public static void SetMetaCreditMultiplier()
        {
            GameManager.Instance.RewardManager.CurrentRewardData.MinMetaCurrencyFromBoss = 2 * ArchipelaGunLocalDataManager.CreditMultiplier;
            GameManager.Instance.RewardManager.CurrentRewardData.MinMetaCurrencyFromBoss = 5 * ArchipelaGunLocalDataManager.CreditMultiplier;
        }

        #endregion
    }

}

