using ArchiGungeon.Data;
using ArchiGungeon.DebugTools;
using ArchiGungeon.ItemArchipelago;
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

        public static void TryPreviousSaveLoad()
        {
            if(SaveDataWriter.InitSaveFilenameAndCheckPrevious() == true)
            {
                // TODO FUTURE: define other dicts for multiple save data types to consider
                ArchipelaGunFileData data = SaveDataWriter.RetrieveSaveData();

                CountGoalManager.SetFullCountSaveData(data.countSaveData);
                locationsCheckedFlags = data.locationsCheckedFlagsData;
            }
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
            }
        }
        #endregion
    }

    public class ArchipelaGunFileData
    {
        public Dictionary<CountStats, int> countSaveData;
        public HashSet<GungeonFlags> locationsCheckedFlagsData;

    }
}
