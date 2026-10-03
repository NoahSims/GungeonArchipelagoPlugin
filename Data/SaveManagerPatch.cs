using FullInspector;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using WebSocketSharp;
using static HutongGames.PlayMaker.Actions.TestSaveStat;
using static SaveManager;

namespace ArchiGungeon.Data
{
    [HarmonyPatch]
    internal class SaveManagerPatch
    {
        public static string ARCH_SAVE_SLOT = "ArchipelaGun";

        static readonly Type[] SaveFileTypes =
        {
            typeof(MidGameSaveData),
            typeof(GameOptions),
            typeof(GameStatsManager)
        };

        [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.Init))]
        [HarmonyPrefix]
        public static bool Patch_Init()
        {
            ETGModConsole.Log("SaveManagerPatch: Entering Init");
            ETGModConsole.Log("SaveManagerPatch: Persistent SavePath: " + SavePath);
            if (s_hasBeenInitialized)
            {
                ETGModConsole.Log("SaveManagerPatch: has been initialized");
                return false;
            }
            ETGModConsole.Log("SaveManagerPatch: Test1");
            if (string.IsNullOrEmpty(SavePath))
            {
                ETGModConsole.Log("Application.persistentDataPath FAILED! " + SavePath);
                //Debug.LogError("Application.persistentDataPath FAILED! " + SavePath);
                SavePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "../LocalLow/Dodge Roll/Enter the Gungeon");
            }
            ETGModConsole.Log("SaveManagerPatch: Test2");
            if (!Directory.Exists(SavePath))
            {
                try
                {
                    ETGModConsole.Log("Manually create default save directory!");
                    //Debug.LogWarning("Manually create default save directory!");
                    Directory.CreateDirectory(SavePath);
                }
                catch (Exception ex)
                {
                    ETGModConsole.Log("Failed to create default save directory: " + ex.Message);
                    //Debug.LogError("Failed to create default save directory: " + ex.Message);
                }
            }
            ETGModConsole.Log("SaveManagerPatch: Test3");
            int num = 4;
            Brave.PlayerPrefs.SetInt("saveslot", num);
            ETGModConsole.Log("SaveManagerPatch: Test4");

            SafeMove(Path.Combine(OldSavePath, string.Format(GameSave.legacyFilePattern, ARCH_SAVE_SLOT)), Path.Combine(OldSavePath, string.Format(GameSave.filePattern, ARCH_SAVE_SLOT)));
            SafeMove(Path.Combine(OldSavePath, string.Format(OptionsSave.legacyFilePattern, ARCH_SAVE_SLOT)), Path.Combine(OldSavePath, string.Format(OptionsSave.filePattern, ARCH_SAVE_SLOT)));
            SafeMove(Path.Combine(OldSavePath, string.Format(GameSave.filePattern, ARCH_SAVE_SLOT)), Path.Combine(SavePath, string.Format(GameSave.filePattern, ARCH_SAVE_SLOT)));
            SafeMove(Path.Combine(OldSavePath, string.Format(OptionsSave.filePattern, ARCH_SAVE_SLOT)), Path.Combine(SavePath, string.Format(OptionsSave.filePattern, ARCH_SAVE_SLOT)));
            SafeMove(PathCombine(SavePath, "01", string.Format(GameSave.filePattern, ARCH_SAVE_SLOT)), Path.Combine(SavePath, string.Format(GameSave.filePattern, ARCH_SAVE_SLOT)), allowOverwritting: true);
            SafeMove(PathCombine(SavePath, "01", string.Format(OptionsSave.filePattern, ARCH_SAVE_SLOT)), Path.Combine(SavePath, string.Format(OptionsSave.filePattern, ARCH_SAVE_SLOT)), allowOverwritting: true);

            ETGModConsole.Log("SaveManagerPatch: Test5");
            s_hasBeenInitialized = true;

            ETGModConsole.Log("SaveManagerPatch: EXITING INIT");
            return false;
        }

        [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.ChangeSlot))]
        [HarmonyPrefix]
        public static bool Patch_ChangeSlot()
        {
            // do not change slot
            ETGModConsole.Log("SaveManagerPatch: CANNOT CHANGE SAVE SLOT IN ARCHIPELAGUN");
            return false;
        }

        [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.DeleteCurrentSlotMidGameSave))]
        [HarmonyPrefix]
        public static bool Patch_DeleteCurrentSlotMidGameSave(SaveSlot? overrideSaveSlot = null)
        {
            ETGModConsole.Log("DELETING CURRENT MID GAME SAVE");
            //Debug.LogError("DELETING CURRENT MID GAME SAVE");
            if (GameStatsManager.HasInstance)
            {
                GameStatsManager.Instance.midGameSaveGuid = null;
            }
            string path = string.Format(MidGameSave.filePattern, ARCH_SAVE_SLOT);
            string path2 = Path.Combine(SavePath, path);
            if (File.Exists(path2))
            {
                File.Delete(path2);
            }
            return false;
        }

        [HarmonyPatch]
        internal class SaveManager_SavePatch
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                ETGModConsole.Log("SaveManager_SavePatch: Entering TargetMethods");
                var open = AccessTools.Method(typeof(SaveManager), nameof(SaveManager.Save));
                foreach (var t in SaveFileTypes)
                    yield return open.MakeGenericMethod(t);
            }

            [HarmonyPrefix]
            public static bool Patch_Save(ref bool __result, object obj, SaveManager.SaveType saveType, int playTimeMin, uint versionNumber = 0u, SaveSlot? overrideSaveSlot = null)
            {
                ETGModConsole.Log("SaveManagerPatch: Entering Patch_Save");
                bool encrypted = saveType.encrypted;
                if (!s_hasBeenInitialized)
                {
                    ETGModConsole.Log(String.Format("Tried to save data before SaveManager was initialized! {0} {1}", obj.GetType(), saveType.filePattern));
                    //Debug.LogErrorFormat("Tried to save data before SaveManager was initialized! {0} {1}", obj.GetType(), saveType.filePattern);
                    __result = false;
                    return false;
                }
                string path = string.Format(saveType.filePattern, ARCH_SAVE_SLOT);
                string text = Path.Combine(SavePath, path);
                string text2;
                try
                {
                    bool prettyPrintSerializedJson = fiSettings.PrettyPrintSerializedJson;
                    fiSettings.PrettyPrintSerializedJson = !encrypted;
                    if (obj.GetType() == typeof(MidGameSaveData))
                        text2 = SerializationHelpers.SerializeToContent<MidGameSaveData, FullSerializerSerializer>((MidGameSaveData)obj);
                    else if (obj.GetType() == typeof(GameOptions))
                        text2 = SerializationHelpers.SerializeToContent<GameOptions, FullSerializerSerializer>((GameOptions)obj);
                    else if (obj.GetType() == typeof(GameStatsManager))
                        text2 = SerializationHelpers.SerializeToContent<GameStatsManager, FullSerializerSerializer>((GameStatsManager)obj);
                    else
                    {
                        ETGModConsole.Log("Failed to serialize save data: " + obj.GetType() + " not a recognized SaveFileType");
                        __result = false;
                        return false;
                    }
                    fiSettings.PrettyPrintSerializedJson = prettyPrintSerializedJson;
                }
                catch (Exception ex)
                {
                    ETGModConsole.Log("Failed to serialize save data: " + ex.Message);
                    //Debug.LogError("Failed to serialize save data: " + ex.Message);
                    __result = false;
                    return false;
                }
                if (encrypted)
                {
                    text2 = Encrypt(text2);
                }
                text2 = $"version: {versionNumber}\n{text2}";
                if (!Directory.Exists(SavePath))
                {
                    Directory.CreateDirectory(SavePath);
                }
                bool flag = false;
                if (File.Exists(text))
                {
                    try
                    {
                        File.Copy(text, text + ".temp", overwrite: true);
                        flag = true;
                    }
                    catch (Exception ex2)
                    {
                        ETGModConsole.Log("Failed to create a temporary copy of current save: " + ex2.Message);
                        //Debug.LogError("Failed to create a temporary copy of current save: " + ex2.Message);
                        __result = false;
                        return false;
                    }
                }
                try
                {
                    WriteAllText(text, text2);
                }
                catch (Exception ex3)
                {
                    ETGModConsole.Log("Failed to write new save data: " + ex3.Message);
                    //Debug.LogError("Failed to write new save data: " + ex3.Message);
                    try
                    {
                        File.Delete(text);
                        File.Move(text + ".temp", text);
                    }
                    catch (Exception ex4)
                    {
                        ETGModConsole.Log("Failed to restore temp save data: " + ex4.Message);
                        //Debug.LogError("Failed to restore temp save data: " + ex4.Message);
                    }
                    return false;
                }
                if (flag)
                {
                    try
                    {
                        if (File.Exists(text + ".temp"))
                        {
                            File.Delete(text + ".temp");
                        }
                    }
                    catch (Exception ex5)
                    {
                        ETGModConsole.Log("Failed to replace temp save file: " + ex5.Message);
                        //Debug.LogError("Failed to replace temp save file: " + ex5.Message);
                    }
                }
                if (saveType.backupCount > 0)
                {
                    int latestBackupPlaytimeMinutes = GetLatestBackupPlaytimeMinutes(saveType, overrideSaveSlot);
                    if (playTimeMin >= latestBackupPlaytimeMinutes + saveType.backupMinTimeMin)
                    {
                        string arg = $"{playTimeMin / 60}h{playTimeMin % 60}m";
                        string path2 = string.Format(saveType.backupPattern, ARCH_SAVE_SLOT);
                        string path3 = Path.Combine(SavePath, path2);
                        try
                        {
                            WriteAllText(path3, text2);
                        }
                        catch (Exception ex6)
                        {
                            ETGModConsole.Log("Failed to create new save backup: " + ex6.Message);
                            //Debug.LogError("Failed to create new save backup: " + ex6.Message);
                        }
                        DeleteOldBackups(saveType, overrideSaveSlot);
                    }
                }
                return true;
            }
        }

        [HarmonyPatch]
        internal class SaveManager_LoadPatch_MidGameSave
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                ETGModConsole.Log("SaveManager_LoadPatch_MidGameSave: Entering TargetMethods");
                var open = AccessTools.Method(typeof(SaveManager), nameof(SaveManager.Load));
                yield return open.MakeGenericMethod(typeof(MidGameSaveData));
            }

            [HarmonyPrefix]
            public static bool Patch_Load(ref bool __result, SaveManager.SaveType saveType, out MidGameSaveData obj, bool allowDecrypted, uint expectedVersion = 0u, Func<string, uint, string> versionUpdater = null, SaveSlot? overrideSaveSlot = null)
            {
                ETGModConsole.Log("SaveManagerPatch: Entering Patch_Load MidGameSaveData");
                obj = default;
                if (!s_hasBeenInitialized)
                {
                    ETGModConsole.Log(String.Format("Tried to load data before SaveManager was initialized! {0} {1}", saveType.filePattern, typeof(MidGameSaveData)));
                    //Debug.LogErrorFormat("Tried to load data before SaveManager was initialized! {0} {1}", saveType.filePattern, typeof(T));
                    __result = false;
                    return false;
                }
                string text = string.Format(saveType.filePattern, ARCH_SAVE_SLOT);
                string text2 = Path.Combine(SavePath, text);
                if (!File.Exists(text2))
                {
                    ETGModConsole.Log("Save data doesn't exist: " + text);
                    //Debug.LogWarning("Save data doesn't exist: " + text);
                    __result = false;
                    return false;
                }
                string text3;
                try
                {
                    text3 = ReadAllText(text2);
                }
                catch (Exception ex)
                {
                    ETGModConsole.Log("Failed to read save data: " + ex.Message);
                    //Debug.LogError("Failed to read save data: " + ex.Message);
                    __result |= false;
                    return false;
                }
                uint num = 0u;
                if (text3.StartsWith("version: "))
                {
                    StringReader stringReader = new StringReader(text3);
                    string text4 = stringReader.ReadLine();
                    if (!uint.TryParse(text4.Substring(9), out var result))
                    {
                        ETGModConsole.Log(String.Format("Failed to read save version number (expected [{0}], got [{1}]", expectedVersion, text4.Substring(9)));
                        //Debug.LogErrorFormat("Failed to read save version number (expected [{0}], got [{1}]", expectedVersion, text4.Substring(9));
                        __result = false;
                        return false;
                    }
                    num = result;
                    text3 = stringReader.ReadToEnd();
                }
                if (IsDataEncrypted(text3))
                {
                    text3 = Decrypt(text3);
                }
                else if (!allowDecrypted)
                {
                    ETGModConsole.Log("Save file corrupted!  Copying to a new file");
                    //Debug.LogError("Save file corrupted!  Copying to a new file");
                    text3 = $"version: {num}\n{text3}";
                    try
                    {
                        WriteAllText(text2 + ".corrupt", text3);
                    }
                    catch (Exception ex2)
                    {
                        ETGModConsole.Log("Failed to save off the corrupted file: " + ex2.Message);
                        //Debug.LogError("Failed to save off the corrupted file: " + ex2.Message);
                    }
                    __result |= false;
                    return false;
                }
                if (num < expectedVersion && versionUpdater != null)
                {
                    text3 = versionUpdater(text3, num);
                }

                obj = SerializationHelpers.DeserializeFromContent<MidGameSaveData, FullSerializerSerializer>(text3);

                if (obj == null)
                {
                    ETGModConsole.Log("Save file corrupted!  Copying to a new file");
                    //Debug.LogError("Save file corrupted!  Copying to a new file");
                    try
                    {
                        text3 = ReadAllText(text2);
                    }
                    catch (Exception ex3)
                    {
                        ETGModConsole.Log("Failed to read corrupted save data: " + ex3.Message);
                        //Debug.LogError("Failed to read corrupted save data: " + ex3.Message);
                    }
                    try
                    {
                        WriteAllText(text2 + ".corrupt", text3);
                    }
                    catch (Exception ex4)
                    {
                        ETGModConsole.Log("Failed to save off the corrupted file: " + ex4.Message);
                        //Debug.LogError("Failed to save off the corrupted file: " + ex4.Message);
                    }
                    __result = false;
                    return false;
                }
                __result = true;
                return false;
            }
        }

        [HarmonyPatch]
        internal class SaveManager_LoadPatch_GameOptions
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                ETGModConsole.Log("SaveManager_LoadPatch_GameOptions: Entering TargetMethods");
                var open = AccessTools.Method(typeof(SaveManager), nameof(SaveManager.Load));
                yield return open.MakeGenericMethod(typeof(GameOptions));
            }

            [HarmonyPrefix]
            public static bool Patch_Load(ref bool __result, SaveManager.SaveType saveType, out GameOptions obj, bool allowDecrypted, uint expectedVersion = 0u, Func<string, uint, string> versionUpdater = null, SaveSlot? overrideSaveSlot = null)
            {
                ETGModConsole.Log("SaveManagerPatch: Entering Patch_Load GameOptions");
                obj = default;
                if (!s_hasBeenInitialized)
                {
                    ETGModConsole.Log(String.Format("Tried to load data before SaveManager was initialized! {0} {1}", saveType.filePattern, typeof(GameOptions)));
                    //Debug.LogErrorFormat("Tried to load data before SaveManager was initialized! {0} {1}", saveType.filePattern, typeof(T));
                    __result = false;
                    return false;
                }
                string text = string.Format(saveType.filePattern, ARCH_SAVE_SLOT);
                string text2 = Path.Combine(SavePath, text);
                if (!File.Exists(text2))
                {
                    ETGModConsole.Log("Save data doesn't exist: " + text);
                    //Debug.LogWarning("Save data doesn't exist: " + text);
                    __result = false;
                    return false;
                }
                string text3;
                try
                {
                    text3 = ReadAllText(text2);
                }
                catch (Exception ex)
                {
                    ETGModConsole.Log("Failed to read save data: " + ex.Message);
                    //Debug.LogError("Failed to read save data: " + ex.Message);
                    __result |= false;
                    return false;
                }
                uint num = 0u;
                if (text3.StartsWith("version: "))
                {
                    StringReader stringReader = new StringReader(text3);
                    string text4 = stringReader.ReadLine();
                    if (!uint.TryParse(text4.Substring(9), out var result))
                    {
                        ETGModConsole.Log(String.Format("Failed to read save version number (expected [{0}], got [{1}]", expectedVersion, text4.Substring(9)));
                        //Debug.LogErrorFormat("Failed to read save version number (expected [{0}], got [{1}]", expectedVersion, text4.Substring(9));
                        __result = false;
                        return false;
                    }
                    num = result;
                    text3 = stringReader.ReadToEnd();
                }
                if (IsDataEncrypted(text3))
                {
                    text3 = Decrypt(text3);
                }
                else if (!allowDecrypted)
                {
                    ETGModConsole.Log("Save file corrupted!  Copying to a new file");
                    //Debug.LogError("Save file corrupted!  Copying to a new file");
                    text3 = $"version: {num}\n{text3}";
                    try
                    {
                        WriteAllText(text2 + ".corrupt", text3);
                    }
                    catch (Exception ex2)
                    {
                        ETGModConsole.Log("Failed to save off the corrupted file: " + ex2.Message);
                        //Debug.LogError("Failed to save off the corrupted file: " + ex2.Message);
                    }
                    __result |= false;
                    return false;
                }
                if (num < expectedVersion && versionUpdater != null)
                {
                    text3 = versionUpdater(text3, num);
                }

                obj = SerializationHelpers.DeserializeFromContent<GameOptions, FullSerializerSerializer>(text3);

                if (obj == null)
                {
                    ETGModConsole.Log("Save file corrupted!  Copying to a new file");
                    //Debug.LogError("Save file corrupted!  Copying to a new file");
                    try
                    {
                        text3 = ReadAllText(text2);
                    }
                    catch (Exception ex3)
                    {
                        ETGModConsole.Log("Failed to read corrupted save data: " + ex3.Message);
                        //Debug.LogError("Failed to read corrupted save data: " + ex3.Message);
                    }
                    try
                    {
                        WriteAllText(text2 + ".corrupt", text3);
                    }
                    catch (Exception ex4)
                    {
                        ETGModConsole.Log("Failed to save off the corrupted file: " + ex4.Message);
                        //Debug.LogError("Failed to save off the corrupted file: " + ex4.Message);
                    }
                    __result = false;
                    return false;
                }
                __result = true;
                return false;
            }
        }

        [HarmonyPatch]
        internal class SaveManager_LoadPatch_GameStatsManager
        {
            static IEnumerable<MethodBase> TargetMethods()
            {
                ETGModConsole.Log("SaveManager_LoadPatch_GameStatsManager: Entering TargetMethods");
                var open = AccessTools.Method(typeof(SaveManager), nameof(SaveManager.Load));
                yield return open.MakeGenericMethod(typeof(GameStatsManager));
            }

            [HarmonyPrefix]
            public static bool Patch_Load(ref bool __result, SaveManager.SaveType saveType, out GameStatsManager obj, bool allowDecrypted, uint expectedVersion = 0u, Func<string, uint, string> versionUpdater = null, SaveSlot? overrideSaveSlot = null)
            {
                ETGModConsole.Log("SaveManagerPatch: Entering Patch_Load GameStatsManager");
                obj = default;
                if (!s_hasBeenInitialized)
                {
                    ETGModConsole.Log(String.Format("Tried to load data before SaveManager was initialized! {0} {1}", saveType.filePattern, typeof(GameStatsManager)));
                    //Debug.LogErrorFormat("Tried to load data before SaveManager was initialized! {0} {1}", saveType.filePattern, typeof(T));
                    __result = false;
                    return false;
                }
                string text = string.Format(saveType.filePattern, ARCH_SAVE_SLOT);
                string text2 = Path.Combine(SavePath, text);
                if (!File.Exists(text2))
                {
                    ETGModConsole.Log("Save data doesn't exist: " + text);
                    //Debug.LogWarning("Save data doesn't exist: " + text);
                    __result = false;
                    return false;
                }
                string text3;
                try
                {
                    text3 = ReadAllText(text2);
                }
                catch (Exception ex)
                {
                    ETGModConsole.Log("Failed to read save data: " + ex.Message);
                    //Debug.LogError("Failed to read save data: " + ex.Message);
                    __result |= false;
                    return false;
                }
                uint num = 0u;
                if (text3.StartsWith("version: "))
                {
                    StringReader stringReader = new StringReader(text3);
                    string text4 = stringReader.ReadLine();
                    if (!uint.TryParse(text4.Substring(9), out var result))
                    {
                        ETGModConsole.Log(String.Format("Failed to read save version number (expected [{0}], got [{1}]", expectedVersion, text4.Substring(9)));
                        //Debug.LogErrorFormat("Failed to read save version number (expected [{0}], got [{1}]", expectedVersion, text4.Substring(9));
                        __result = false;
                        return false;
                    }
                    num = result;
                    text3 = stringReader.ReadToEnd();
                }
                if (IsDataEncrypted(text3))
                {
                    text3 = Decrypt(text3);
                }
                else if (!allowDecrypted)
                {
                    ETGModConsole.Log("Save file corrupted!  Copying to a new file");
                    //Debug.LogError("Save file corrupted!  Copying to a new file");
                    text3 = $"version: {num}\n{text3}";
                    try
                    {
                        WriteAllText(text2 + ".corrupt", text3);
                    }
                    catch (Exception ex2)
                    {
                        ETGModConsole.Log("Failed to save off the corrupted file: " + ex2.Message);
                        //Debug.LogError("Failed to save off the corrupted file: " + ex2.Message);
                    }
                    __result |= false;
                    return false;
                }
                if (num < expectedVersion && versionUpdater != null)
                {
                    text3 = versionUpdater(text3, num);
                }

                obj = SerializationHelpers.DeserializeFromContent<GameStatsManager, FullSerializerSerializer>(text3);

                if (obj == null)
                {
                    ETGModConsole.Log("Save file corrupted!  Copying to a new file");
                    //Debug.LogError("Save file corrupted!  Copying to a new file");
                    try
                    {
                        text3 = ReadAllText(text2);
                    }
                    catch (Exception ex3)
                    {
                        ETGModConsole.Log("Failed to read corrupted save data: " + ex3.Message);
                        //Debug.LogError("Failed to read corrupted save data: " + ex3.Message);
                    }
                    try
                    {
                        WriteAllText(text2 + ".corrupt", text3);
                    }
                    catch (Exception ex4)
                    {
                        ETGModConsole.Log("Failed to save off the corrupted file: " + ex4.Message);
                        //Debug.LogError("Failed to save off the corrupted file: " + ex4.Message);
                    }
                    __result = false;
                    return false;
                }
                __result = true;
                return false;
            }
        }

        [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.GetLatestBackupPlaytimeMinutes))]
        [HarmonyPrefix]
        private static bool Patch_GetLatestBackupPlaytimeMinutes(ref int __result, SaveManager.SaveType saveType, SaveSlot? overrideSaveSlot = null)
        {
            ETGModConsole.Log("SaveManagerPatch: Patch_GetLatestBackupPlaytimeMinutes");
            string text = string.Format(saveType.backupPattern, ARCH_SAVE_SLOT, string.Empty);
            string pattern = text + "(?<hour>\\d+)h(?<min>\\d+)m";
            string[] files = Directory.GetFiles(SavePath);
            int num = 0;
            for (int i = 0; i < files.Length; i++)
            {
                Match match = Regex.Match(files[i], pattern, RegexOptions.Multiline);
                if (match.Success)
                {
                    int num2 = Convert.ToInt32(match.Groups["hour"].Captures[0].Value) * 60;
                    num2 += Convert.ToInt32(match.Groups["min"].Captures[0].Value);
                    if (num2 > num)
                    {
                        num = num2;
                    }
                }
            }
            __result = num;
            return false;
        }

        [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.DeleteAllBackups))]
        [HarmonyPrefix]
        public static bool Patch_DeleteAllBackups(SaveManager.SaveType saveType, SaveSlot? overrideSaveSlot = null)
        {
            string text = string.Format(saveType.backupPattern, ARCH_SAVE_SLOT, string.Empty);
            string pattern = text + "(?<hour>\\d+)h(?<min>\\d+)m";
            string[] files = Directory.GetFiles(SavePath);
            for (int i = 0; i < files.Length; i++)
            {
                Match match = Regex.Match(files[i], pattern, RegexOptions.Multiline);
                if (match.Success)
                {
                    try
                    {
                        File.Delete(files[i]);
                    }
                    catch (Exception ex)
                    {
                        ETGModConsole.Log("Failed to remove backup file: " + ex.Message);
                        //Debug.LogError("Failed to remove backup file: " + ex.Message);
                        break;
                    }
                }
            }
            return false;
        }

        [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.SafeMoveBackups))]
        [HarmonyPrefix]
        private static bool Patch_SafeMoveBackups(SaveManager.SaveType saveType, string oldPath, string newPath, SaveSlot? overrideSaveSlot = null)
        {
            string text = string.Format(saveType.backupPattern, ARCH_SAVE_SLOT, string.Empty);
            string pattern = text + "(?<hour>\\d+)h(?<min>\\d+)m";
            string[] files = Directory.GetFiles(oldPath);
            for (int i = 0; i < files.Length; i++)
            {
                Match match = Regex.Match(files[i], pattern, RegexOptions.Multiline);
                if (match.Success)
                {
                    SafeMove(files[i], Path.Combine(newPath, Path.GetFileName(files[i])));
                }
            }
            return false;
        }

        [HarmonyPatch(typeof(SaveManager), nameof(SaveManager.DeleteOldBackups))]
        [HarmonyPrefix]
        private static bool Patch_DeleteOldBackups(SaveManager.SaveType saveType, SaveSlot? overrideSaveSlot = null)
        {
            string text = string.Format(saveType.backupPattern, ARCH_SAVE_SLOT, string.Empty);
            string pattern = text + "(?<hour>\\d+)h(?<min>\\d+)m";
            List<Tuple<string, int>> list = new List<Tuple<string, int>>();
            string[] files = Directory.GetFiles(SavePath);
            for (int i = 0; i < files.Length; i++)
            {
                Match match = Regex.Match(files[i], pattern, RegexOptions.Multiline);
                if (match.Success)
                {
                    int num = Convert.ToInt32(match.Groups["hour"].Captures[0].Value) * 60;
                    num += Convert.ToInt32(match.Groups["min"].Captures[0].Value);
                    list.Add(Tuple.Create(files[i], num));
                }
            }
            list.Sort((Tuple<string, int> a, Tuple<string, int> b) => b.Second - a.Second);
            while (list.Count > saveType.backupCount && list.Count > 0)
            {
                try
                {
                    File.Delete(list[list.Count - 1].First);
                }
                catch (Exception ex)
                {
                    ETGModConsole.Log("Failed to remove backup file: " + ex.Message);
                    //Debug.LogError("Failed to remove backup file: " + ex.Message);
                    break;
                }
                list.RemoveAt(list.Count - 1);
            }

            return false;
        }
    }
}
