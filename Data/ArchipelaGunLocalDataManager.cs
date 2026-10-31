using ArchiGungeon.Data;
using ArchiGungeon.DebugTools;
using ArchiGungeon.ItemArchipelago;
using Archipelago.MultiClient.Net.Models;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static DungeonTileStampData;

namespace ArchiGungeon.Data
{
    public class ArchipelaGunLocalDataManager
    {
        public static HashSet<GungeonFlags> locationsCheckedFlags = new HashSet<GungeonFlags>(new GungeonFlagsComparer());
        public static HashSet<long> FoundLocationsAPIds = new HashSet<long>();
        public static Dictionary<long, string> FoundHints = new Dictionary<long, string>();
        public static int RecievedItemsIndex = 0;
        public static int CreditMultiplier = 1;
        public static int MetaShopProgCount = 0;
        public static int TrorcShopProgCount = 0;
        public static int DougShopProgCount = 0;
        public static int GoopShopProgCount = 0;
        public static int StarterChestProgress = 0;
        public static bool HasOublietteAccess = false;
        public static bool HasAbbeyAccess = false;
        public static bool HasRatLairAccess = false;
        public static bool HasRnGAccess = false;

        public static void TryPreviousSaveLoad()
        {
            if(SaveDataWriter.InitSaveFilenameAndCheckPrevious() == true)
            {
                // TODO FUTURE: define other dicts for multiple save data types to consider
                ArchipelaGunFileData data = SaveDataWriter.RetrieveSaveData();

                CountGoalManager.SetFullCountSaveData(data.countSaveData);
                locationsCheckedFlags = data.locationsCheckedFlagsData == null ? new HashSet<GungeonFlags>(new GungeonFlagsComparer()) : data.locationsCheckedFlagsData;
                RecievedItemsIndex = data.RecievedItemsIndex;
                FoundLocationsAPIds = data.FoundLocationsAPIds == null ? new HashSet<long>() : data.FoundLocationsAPIds;
                FoundHints = data.FoundHints == null ? new Dictionary<long, string>() : data.FoundHints;
                CreditMultiplier = data.CreditMultiplier;
                MetaShopProgCount = data.MetaShopProgCount;
                TrorcShopProgCount = data.TrorcShopProgCount;
                DougShopProgCount = data.DougShopProgCount;
                GoopShopProgCount = data.GoopShopProgCount;
                StarterChestProgress = data.StarterChestProgress;
                HasOublietteAccess = data.HasOublietteAccess;
                HasAbbeyAccess = data.HasAbbeyAccess;
                HasRatLairAccess = data.HasRatLairAccess;
                HasRnGAccess = data.HasRnGAccess;
            }
        }

        public static void AddFoundLocationIds(long[] ids)
        {
            if(FoundLocationsAPIds == null)
            {
                FoundLocationsAPIds = new HashSet<long>();
            }

            foreach (long id in ids)
            {
                FoundLocationsAPIds.Add(id);
            }
        }

        public static void AddFoundHint(long locationId, string hint)
        {
            if (FoundHints.ContainsKey(locationId))
            {
                ArchDebugPrint.DebugLog(DebugCategory.Warning, "WARN: Attempting to save hint that was already found for location: " + locationId);
                return;
            }
            FoundHints.Add(locationId, hint);
        }

        private static void HandleSaveValidationAndWrite()
        {
            Dictionary<CountStats, int> countSaveDataToWrite = new Dictionary<CountStats, int>();

            foreach (CountStats countStat in (CountStats[])Enum.GetValues(typeof(CountStats)))
            {
                int statData = CountGoalManager.GetCountStat(countStat);

                if (statData > 0)
                {
                    countSaveDataToWrite[countStat] = statData;

                    ArchDebugPrint.DebugLog(DebugCategory.LocalSaveData, $"Saving count for {countStat}: {statData}");
                }
                else
                {
                    countSaveDataToWrite[countStat] = 0;
                }

            }
            ArchipelaGunFileData dataToSave = new ArchipelaGunFileData();

            // TODO does locationsCheckedFlags need to be validated??
            dataToSave.countSaveData = countSaveDataToWrite;
            dataToSave.locationsCheckedFlagsData = locationsCheckedFlags;
            dataToSave.RecievedItemsIndex = RecievedItemsIndex;
            dataToSave.FoundLocationsAPIds = FoundLocationsAPIds;
            dataToSave.FoundHints = FoundHints;
            dataToSave.CreditMultiplier = CreditMultiplier;
            dataToSave.MetaShopProgCount = MetaShopProgCount;
            dataToSave.DougShopProgCount = DougShopProgCount;
            dataToSave.GoopShopProgCount = GoopShopProgCount;
            dataToSave.TrorcShopProgCount = TrorcShopProgCount;
            dataToSave.StarterChestProgress = StarterChestProgress;
            dataToSave.HasOublietteAccess = HasOublietteAccess;
            dataToSave.HasAbbeyAccess = HasAbbeyAccess;
            dataToSave.HasRatLairAccess = HasRatLairAccess;
            dataToSave.HasRnGAccess = HasRnGAccess;

            SaveDataWriter.WriteSaveFile(dataToSave);
        }

        //TODO ...refactor this please
        public static void SaveCurrentRandomizerProgress()
        {
            HandleSaveValidationAndWrite();
        }

        public static void AddToCountSaveDataEntry(CountStats statToAdd, int numberToAdd)
        {
            if(numberToAdd > 0)
            {
                ArchDebugPrint.DebugLog(DebugCategory.LocalSaveData, $"{statToAdd} goal adding: {numberToAdd}");
            }
            int goalsMet = CountGoalManager.AddToGoalCount(statToAdd, numberToAdd);

            if (goalsMet >= 1)
            {
                ArchDebugPrint.DebugLog(DebugCategory.CountingGoal, $"[{statToAdd}] Goal handling {goalsMet} completions");

                AchievementLocationCheckHandler.SendStatLocationChecks(statToAdd, goalsMet);
                CountGoalManager.RemoveClearedGoals(statToAdd, goalsMet);

                SaveCurrentRandomizerProgress();
            }
        }

        public static void CheckFullCountStatsForGoals()
        {
            ArchDebugPrint.DebugLog(DebugCategory.CountingGoal, $"Checking save data for cleared goals");

            List<CountStats> countStatList = CountGoalManager.GetFullCountSaveData().Keys.ToList<CountStats>();
            foreach(CountStats countStat in countStatList)
            {
                AddToCountSaveDataEntry(countStat, 0);
            }
        }

        #region Harmony Patching
        /*
         * Hook ArchipelaGun data management functions into main data management functions
         */
        [HarmonyPatch]
        internal class ArchipelaGunSaveDataPatches
        {
            /*
             * Patch - GameStatsManager.Load
             * Load ArchipelaGun local data whenever GameStatsManager.Load runs
             */
            [HarmonyPatch(typeof(GameStatsManager), nameof(GameStatsManager.Load))]
            [HarmonyPostfix]
            public static void ArchipelaGunLoadOnMainLoad()
            {
                TryPreviousSaveLoad();
            }

            /*
             * Patch - GameStatsManager.Save
             * Save ArchipelaGun local data whenever GameStatsManager.Save runs
             */
            [HarmonyPatch(typeof(GameStatsManager), nameof(GameStatsManager.Save))]
            [HarmonyPostfix]
            public static void ArchipelaGunSaveOnMainSave()
            {
                SaveCurrentRandomizerProgress();
            }

            [HarmonyPatch(typeof(GameStatsManager), nameof(GameStatsManager.DANGEROUS_ResetAllStats))]
            [HarmonyPostfix]
            public static void ArchipelaGunDeleteSave()
            {
                //TODO
                SaveDataWriter.DANGEROUS_DeleteArchipelaGunDataFile();
                CountGoalManager.DANGEROUS_ClearData();
                locationsCheckedFlags.Clear();
                RecievedItemsIndex = 0;
            }
        }
        #endregion
    }

    public class ArchipelaGunFileData
    {
        public Dictionary<CountStats, int> countSaveData;
        public HashSet<GungeonFlags> locationsCheckedFlagsData;
        public HashSet<long> FoundLocationsAPIds;
        public Dictionary<long, string> FoundHints;
        public int RecievedItemsIndex;
        public int CreditMultiplier;
        public int MetaShopProgCount = 0;
        public int TrorcShopProgCount = 0;
        public int DougShopProgCount = 0;
        public int GoopShopProgCount = 0;
        public int StarterChestProgress = 0;
        public bool HasOublietteAccess = false;
        public bool HasAbbeyAccess = false;
        public bool HasRatLairAccess = false;
        public bool HasRnGAccess = false;
    }
}
