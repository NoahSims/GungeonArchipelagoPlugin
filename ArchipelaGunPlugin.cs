using Alexandria.DungeonAPI;
using ArchiGungeon.ArchipelagoServer;
using ArchiGungeon.Character;
using ArchiGungeon.Data;
using ArchiGungeon.DebugTools;
using ArchiGungeon.EnemyHandlers;
using ArchiGungeon.GungeonEventHandlers;
using ArchiGungeon.ItemArchipelago;
using ArchiGungeon.UserInterface;
using BepInEx;
using Dungeonator;
using HarmonyLib;
using UnityEngine;

namespace ArchiGungeon
{
    [BepInDependency(Alexandria.Alexandria.GUID)] // this mod depends on the Alexandria API: https://enter-the-gungeon.thunderstore.io/package/Alexandria/Alexandria/
    [BepInDependency(ETGModMainBehaviour.GUID)]
    [BepInPlugin(GUID, NAME, VERSION)]
    
    ///<summary>
    /// Class <c>ArchipelaGunPlugin</c> Main plugin class called by BepInEx to initialize all behavior
    ///</summary>
    public class ArchipelaGunPlugin : BaseUnityPlugin
    {
        public const string GUID = "maoboulve.etg.archipelagogungeon";
        public const string NAME = "Archipelago Gungeon Randomizer";
        public const string VERSION = "0.1.5";
        public const string TEXT_COLOR = "#B6FFB8";

        public const string MOD_ITEM_PREFIX = "arch";
        public const string AP_WORLD_VERSION = "0.1.1";

        private readonly Harmony _harmony = new Harmony(GUID);

        #region Plugin Startup
        public static ArchipelagoGUI ArchipelagoModMenu { get; protected set; }
        private static bool isInit = false;


        private void Awake()
        {
            _harmony.PatchAll();
        }

        public void Start()
        {
            ArchDebugPrint.DebugLog(DebugCategory.PluginStartup, "Starting ArchipelaGunPlugin");

            
            ETGModMainBehaviour.WaitForGameManagerStart(GMStart);
            //DungeonHooks.OnPreDungeonGeneration += PredungeonGen;
        }

        public void PredungeonGen(LoopDungeonGenerator generator, Dungeon dungeon, DungeonFlow flow, int dungeonSeed)
        {
            ArchDebugPrint.DebugLog(DebugCategory.PlayerEventListener, "Gungeon pregen: " + dungeon.lockedDoorObjects.ToString());
        }

        public void GMStart(GameManager g)
        {
            ArchDebugPrint.DebugLog(DebugCategory.PluginStartup, "GameManager started");

            InitItemHooks();
            RegisterItems();
            InitEnemyHooks();
            InitModMenu();
            

            Log($"{NAME} v{VERSION} started successfully.", TEXT_COLOR);
            isInit = true;

            StartPostInitializationSetup();

            return;
        }

        private void InitItemHooks()
        {
            //CustomBlankModItem.InitHooks();
        }

        private void RegisterItems()
        {
            //Archipelagun.Register();
            APPickUpItem.RegisterItemBase();
            ReverseCurse.RegisterItem();
            ReverseCurseReversal.RegisterItem();
            
            BulletParadoxItem.RegisterItem();
            PilotParadoxItem.RegisterItem();
            ConvictParadoxItem.RegisterItem();
            MarineParadoxItem.RegisterItem();
            RobotParadoxItem.RegisterItem();
            HunterParadoxItem.RegisterItem();
        }

        private void InitEnemyHooks()
        {
            EnemySwapping.InitializeEnemySwapper();
        }

        private void InitModMenu()
        {
            ArchipelagoModMenu = new ArchipelagoGUI();
            ArchipelagoModMenu.Start();
        }

        // Setup functions after instancing all needed functions to prevent missing reference runtime errors
        private void StartPostInitializationSetup()
        {
            StartGungeonPlayerListener();

            //ArchipelagoGUI.ConsoleLog($"===** Debug text log at {Paths.ConfigPath} **=== \n\n");
            // Print all directories BepInEx will allow
            //LocalSaveDataHandler.TDD_PrintAllPathsDirectory();

            InitExceptionCatcher();

            // TODO - Move this to a proper implementation elsewhere. But this is a (jank) example of how you could disable a character on selection by providing a prereq they can't complete.
            // Seemed to only work on initial foyer load. Reloads they returned.

            //FoyerCharacterSelectFlag[] array = UnityEngine.Object.FindObjectsOfType<FoyerCharacterSelectFlag>();
            //foreach (var charSelect in array)
            //{
            //    ArchDebugPrint.DebugLog(DebugCategory.ItemHandling, "Character select named: " + charSelect.name);
            //    if (charSelect.name.Contains("convict", true))
            //    {
            //        ArchDebugPrint.DebugLog(DebugCategory.ItemHandling, "Disabling Convict from character select");
            //        foreach (var prerequisite in charSelect.prerequisites)
            //        {
            //            ArchDebugPrint.DebugLog(DebugCategory.ItemHandling, "Has prereq of type: " + prerequisite.prerequisiteType.ToString());
            //        }
            //        var boogeyPrereq = new DungeonPrerequisite();
            //        boogeyPrereq.prerequisiteType = DungeonPrerequisite.PrerequisiteType.COMPARISON;
            //        boogeyPrereq.comparisonValue = -1;
            //        var boogeyPrereq2 = new DungeonPrerequisite();
            //        boogeyPrereq2.prerequisiteType = DungeonPrerequisite.PrerequisiteType.FLAG;
            //        boogeyPrereq2.requireFlag = true;
            //        boogeyPrereq2.saveFlagToCheck = GungeonFlags.ACHIEVEMENT_COMPLETE_FOUR_PASTS;
            //        charSelect.prerequisites = new DungeonPrerequisite[2] { boogeyPrereq, boogeyPrereq2 };
            //    }
            //}

            return;
        }


        private void StartGungeonPlayerListener()
        {
            GungeonPlayerEventListener.StartSystemEventListens();
            return;
        }

        private void InitExceptionCatcher()
        {
            ArchDebugPrint.InitDebugLog();
            ArchDebugPrint.DebugLog(DebugCategory.PluginStartup, "Init Exception Catcher");
            Application.logMessageReceived += ArchDebugPrint.OnCatchException;
        }

        private static void Log(string text, string color = "#FFFFFF")
        {
            ETGModConsole.Log($"<color={color}>{text}</color>");
        }

        #endregion



        #region Unity Tick Management
        public void Update()
        {   
            if ( !isInit )
            {
                return;
            }

            ArchipelagoModMenu.Update();
            SessionHandler.TickCheckItemQueue();

            return;
        }

        #endregion
    }
}
