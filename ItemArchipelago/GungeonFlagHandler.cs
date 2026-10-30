using ArchiGungeon.ArchipelagoServer;
using ArchiGungeon.Data;
using ArchiGungeon.UserInterface;
using Archipelago.MultiClient.Net.Models;
using Brave.BulletScript;
using Dungeonator;
using HarmonyLib;
using HutongGames.PlayMaker.Actions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace ArchiGungeon.ItemArchipelago
{
    [HarmonyPatch]
    internal class GungeonFlagHandler
    {
        // Map a flag to the NULL APlocation if you want to stop the flag from being set, but not unlock a location.
        public static Dictionary<GungeonFlags, APLocations> GungeonFlagsToAPLocations = new Dictionary<GungeonFlags, APLocations>(new GungeonFlagsComparer()) {
            // Shop flags
            { GungeonFlags.BLUEPRINTMETA_THOMPSON, APLocations.BLUEPRINTMETA_THOMPSON },
            { GungeonFlags.BLUEPRINTMETA_HEARTBOTTLE, APLocations.BLUEPRINTMETA_HEARTBOTTLE },
            { GungeonFlags.BLUEPRINTMETA_GAMMARAY, APLocations.BLUEPRINTMETA_GAMMARAY },
            { GungeonFlags.BLUEPRINTMETA_RPG, APLocations.BLUEPRINTMETA_RPG },
            { GungeonFlags.BLUEPRINTMETA_ROCKETBULLETS, APLocations.BLUEPRINTMETA_ROCKETBULLETS },
            { GungeonFlags.BLUEPRINTMETA_HEGEMONYRIFLE, APLocations.BLUEPRINTMETA_HEGEMONYRIFLE },
            { GungeonFlags.BLUEPRINTMETA_HEARTLOCKET, APLocations.BLUEPRINTMETA_HEARTLOCKET },
            { GungeonFlags.BLUEPRINTMETA_FIREBULLETS, APLocations.BLUEPRINTMETA_FIREBULLETS },
            { GungeonFlags.BLUEPRINTMETA_FREEZERAY, APLocations.BLUEPRINTMETA_FREEZERAY },
            { GungeonFlags.BLUEPRINTMETA_HEARTLUNCHBOX, APLocations.BLUEPRINTMETA_HEARTLUNCHBOX },
            { GungeonFlags.BLUEPRINTMETA_BOX, APLocations.BLUEPRINTMETA_BOX },
            { GungeonFlags.BLUEPRINTMETA_MAILBOX, APLocations.BLUEPRINTMETA_MAILBOX },
            { GungeonFlags.BLUEPRINTMETA_FATBULLETS, APLocations.BLUEPRINTMETA_FATBULLETS },
            { GungeonFlags.BLUEPRINTMETA_BETTERBULLETS, APLocations.BLUEPRINTMETA_BETTERBULLETS },
            { GungeonFlags.BLUEPRINTMETA_ANGRYBULLETS, APLocations.BLUEPRINTMETA_ANGRYBULLETS },
            { GungeonFlags.BLUEPRINTMETA_SIREN, APLocations.BLUEPRINTMETA_SIREN },
            { GungeonFlags.BLUEPRINTMETA_HEARTPURSE, APLocations.BLUEPRINTMETA_HEARTPURSE },
            { GungeonFlags.BLUEPRINTMETA_ORIGUNI, APLocations.BLUEPRINTMETA_ORIGUNI },
            { GungeonFlags.BLUEPRINTMETA_EYEPATCH, APLocations.BLUEPRINTMETA_EYEPATCH },
            { GungeonFlags.BLUEPRINTMETA_BLOODYEYE, APLocations.BLUEPRINTMETA_BLOODYEYE }, 
            { GungeonFlags.BLUEPRINTMETA_SPICE, APLocations.BLUEPRINTMETA_SPICE },
            { GungeonFlags.BLUEPRINTMETA_GRASSCHOPPER, APLocations.BLUEPRINTMETA_GRASSCHOPPER },
            { GungeonFlags.BLUEPRINTMETA_SUNGLASSES, APLocations.BLUEPRINTMETA_SUNGLASSES },
            { GungeonFlags.BLUEPRINTMETA_FLASHRAYGUN, APLocations.BLUEPRINTMETA_FLASHRAYGUN },
            { GungeonFlags.BLUEPRINTMETA_SINGULARITY, APLocations.BLUEPRINTMETA_SINGULARITY },
            { GungeonFlags.BLUEPRINTMETA_SCIENCECANNON, APLocations.BLUEPRINTMETA_SCIENCECANNON },
            { GungeonFlags.BLUEPRINTMETA_NANOMACHINES, APLocations.BLUEPRINTMETA_NANOMACHINES },
            { GungeonFlags.BLUEPRINTMETA_ICEBREAKER, APLocations.BLUEPRINTMETA_ICEBREAKER },
            { GungeonFlags.BLUEPRINTMETA_PORTABLETURRET, APLocations.BLUEPRINTMETA_PORTABLETURRET },
            { GungeonFlags.BLUEPRINTMETA_DUCTTAPE, APLocations.BLUEPRINTMETA_DUCTTAPE },
            { GungeonFlags.BLUEPRINTMETA_TANGLER, APLocations.BLUEPRINTMETA_TANGLER },
            { GungeonFlags.BLUEPRINTMETA_LAMP, APLocations.BLUEPRINTMETA_LAMP },
            { GungeonFlags.BLUEPRINTMETA_FACEMELTER, APLocations.BLUEPRINTMETA_FACEMELTER },
            { GungeonFlags.BLUEPRINTMETA_HEROINE, APLocations.BLUEPRINTMETA_HEROINE },
            { GungeonFlags.BLUEPRINTMETA_MASSSHOTGUN, APLocations.BLUEPRINTMETA_MASSSHOTGUN },
            { GungeonFlags.BLUEPRINTMETA_CEREBRALBORE, APLocations.BLUEPRINTMETA_CEREBRALBORE },
            { GungeonFlags.BLUEPRINTMETA_RAIDENCOIL, APLocations.BLUEPRINTMETA_RAIDENCOIL },
            { GungeonFlags.BLUEPRINTMETA_BLACKHOLEGUN, APLocations.BLUEPRINTMETA_BLACKHOLEGUN },
            { GungeonFlags.BLUEPRINTMETA_SPACEFRIEND, APLocations.BLUEPRINTMETA_SPACEFRIEND },
            { GungeonFlags.BLUEPRINTMETA_YARI, APLocations.BLUEPRINTMETA_YARI },
            { GungeonFlags.BLUEPRINTMETA_RAILGUN, APLocations.BLUEPRINTMETA_RAILGUN },
            { GungeonFlags.BLUEPRINTMETA_BROCCOLI, APLocations.BLUEPRINTMETA_BROCCOLI },
            //GungeonFlags.BLUEPRINTMETA_MTXGUN, // This causes weird loop on startup if you have the mtx dlc
            { GungeonFlags.BLUEPRINTMETA_PLATINUMBULLETS, APLocations.BLUEPRINTMETA_PLATINUMBULLETS },
            { GungeonFlags.BLUEPRINTGOOP_BUGBOOTS, APLocations.BLUEPRINTGOOP_BUGBOOTS },
            { GungeonFlags.BLUEPRINTGOOP_FOSSILIZEDGUN, APLocations.BLUEPRINTGOOP_FOSSILIZEDGUN },
            { GungeonFlags.BLUEPRINTGOOP_MONSTERBLOOD, APLocations.BLUEPRINTGOOP_MONSTERBLOOD },
            { GungeonFlags.BLUEPRINTGOOP_PLAGUEPISTOL, APLocations.BLUEPRINTGOOP_PLAGUEPISTOL },
            { GungeonFlags.BLUEPRINTGOOP_MEMBRANE, APLocations.BLUEPRINTGOOP_MEMBRANE },
            { GungeonFlags.BLUEPRINTGOOP_PLUNGER, APLocations.BLUEPRINTGOOP_PLUNGER },
            { GungeonFlags.BLUEPRINTGOOP_SPONGE, APLocations.BLUEPRINTGOOP_SPONGE },
            { GungeonFlags.BLUEPRINTGOOP_TENTACLEARM, APLocations.BLUEPRINTGOOP_TENTACLEARM },
            { GungeonFlags.BLUEPRINTGOOP_NAPALMSTRIKE, APLocations.BLUEPRINTGOOP_NAPALMSTRIKE },
            { GungeonFlags.BLUEPRINTGOOP_GUNDROMEDASTRAIN, APLocations.BLUEPRINTGOOP_GUNDROMEDASTRAIN },
            { GungeonFlags.BLUEPRINTGOOP_EVOLVER, APLocations.BLUEPRINTGOOP_EVOLVER },
            { GungeonFlags.BLUEPRINTTRUCK_CLUSTERMINE, APLocations.BLUEPRINTTRUCK_CLUSTERMINE },
            { GungeonFlags.BLUEPRINTTRUCK_MSIXTEEN, APLocations.BLUEPRINTTRUCK_MSIXTEEN },
            { GungeonFlags.BLUEPRINTTRUCK_RATION, APLocations.BLUEPRINTTRUCK_RATION },
            { GungeonFlags.BLUEPRINTTRUCK_AIRSTRIKE, APLocations.BLUEPRINTTRUCK_AIRSTRIKE },
            { GungeonFlags.BLUEPRINTTRUCK_COMMANDO, APLocations.BLUEPRINTTRUCK_COMMANDO },
            { GungeonFlags.BLUEPRINTTRUCK_AWP, APLocations.BLUEPRINTTRUCK_AWP },
            { GungeonFlags.BLUEPRINTTRUCK_NIKITA, APLocations.BLUEPRINTTRUCK_NIKITA },
            { GungeonFlags.BLUEPRINTTRUCK_BIGBOY, APLocations.BLUEPRINTTRUCK_BIGBOY },
            { GungeonFlags.BLUEPRINTTRUCK_CHAFFGRENADE, APLocations.BLUEPRINTTRUCK_CHAFFGRENADE },
            { GungeonFlags.BLUEPRINTTRUCK_PATRIOT, APLocations.BLUEPRINTTRUCK_PATRIOT },
            { GungeonFlags.BLUEPRINTTRUCK_MAGAZINERACK, APLocations.BLUEPRINTTRUCK_MAGAZINERACK },
            { GungeonFlags.BLUEPRINTBEETLE_DEVOLVER, APLocations.BLUEPRINTBEETLE_DEVOLVER },
            { GungeonFlags.BLUEPRINTBEETLE_BASEBALLBAT, APLocations.BLUEPRINTBEETLE_BASEBALLBAT },
            { GungeonFlags.BLUEPRINTBEETLE_DAWNHAMMER, APLocations.BLUEPRINTBEETLE_DAWNHAMMER },
            { GungeonFlags.BLUEPRINTBEETLE_STRAFEGUN, APLocations.BLUEPRINTBEETLE_STRAFEGUN },
            { GungeonFlags.BLUEPRINTBEETLE_HYPERLIGHT, APLocations.BLUEPRINTBEETLE_HYPERLIGHT },
            { GungeonFlags.BLUEPRINTBEETLE_STARPEW, APLocations.BLUEPRINTBEETLE_STARPEW },
            { GungeonFlags.BLUEPRINTBEETLE_DUELINGLASER, APLocations.BLUEPRINTBEETLE_DUELINGLASER },
            { GungeonFlags.BLUEPRINTBEETLE_TELEPORTERPROTOTYPE, APLocations.BLUEPRINTBEETLE_TELEPORTERPROTOTYPE },
            { GungeonFlags.BLUEPRINTBEETLE_VORPALGUN, APLocations.BLUEPRINTBEETLE_VORPALGUN },
            { GungeonFlags.BLUEPRINTBEETLE_STOUTIES, APLocations.BLUEPRINTBEETLE_STOUTIES },
            { GungeonFlags.BLUEPRINTBEETLE_CHARMERS, APLocations.BLUEPRINTBEETLE_CHARMERS },
            { GungeonFlags.BLUEPRINTBEETLE_FLAKTUALLY, APLocations.BLUEPRINTBEETLE_FLAKTUALLY },
            { GungeonFlags.BLUEPRINTBEETLE_MAGICITES, APLocations.BLUEPRINTBEETLE_MAGICITES },
            { GungeonFlags.BLUEPRINTBEETLE_METROIDBOMBS, APLocations.BLUEPRINTBEETLE_METROIDBOMBS },
            { GungeonFlags.BLUEPRINTBEETLE_SILVALLIES, APLocations.BLUEPRINTBEETLE_SILVALLIES },
            { GungeonFlags.BLUEPRINTBEETLE_HELIX, APLocations.BLUEPRINTBEETLE_HELIX },
            { GungeonFlags.BLUEPRINTBEETLE_GOLDIES, APLocations.BLUEPRINTBEETLE_GOLDIES },
            { GungeonFlags.BLUEPRINTBEETLE_BEEBULLETS, APLocations.BLUEPRINTBEETLE_BEEBULLETS },
            { GungeonFlags.BLUEPRINTBEETLE_DEVOLVERBULLETS, APLocations.BLUEPRINTBEETLE_DEVOLVERBULLETS },
            { GungeonFlags.BLUEPRINTBEETLE_CRITICALBULLETS, APLocations.BLUEPRINTBEETLE_CRITICALBULLETS },
            { GungeonFlags.BLUEPRINTBEETLE_SNOWBALLETS, APLocations.BLUEPRINTBEETLE_SNOWBALLETS },
            { GungeonFlags.BLUEPRINTBEETLE_BIGGIANTHEAD, APLocations.BLUEPRINTBEETLE_BIGGIANTHEAD },
            { GungeonFlags.BLUEPRINTBEETLE_COMBINERIFLE, APLocations.BLUEPRINTBEETLE_COMBINERIFLE },
            { GungeonFlags.BLUEPRINTBEETLE_EXOTICGUN, APLocations.BLUEPRINTBEETLE_EXOTICGUN },
            { GungeonFlags.BLUEPRINTBEETLE_BIGSHOTGUN, APLocations.BLUEPRINTBEETLE_BIGSHOTGUN },
            { GungeonFlags.BLUEPRINTBEETLE_GUNDERFURY, APLocations.BLUEPRINTBEETLE_GUNDERFURY },
            { GungeonFlags.BLUEPRINTBEETLE_GLAIVE, APLocations.BLUEPRINTBEETLE_GLAIVE },
            { GungeonFlags.BLUEPRINTBEETLE_LUTE, APLocations.BLUEPRINTBEETLE_LUTE },
            { GungeonFlags.BLUEPRINTBEETLE_TETRIS, APLocations.BLUEPRINTBEETLE_TETRIS },
            { GungeonFlags.BLUEPRINTBEETLE_TRIGUN, APLocations.BLUEPRINTBEETLE_TRIGUN },
            { GungeonFlags.BLUEPRINTBEETLE_SHOVEL, APLocations.BLUEPRINTBEETLE_SHOVEL },
            { GungeonFlags.BLUEPRINTBEETLE_PREDATOR, APLocations.BLUEPRINTBEETLE_PREDATOR },
            { GungeonFlags.BLUEPRINTBEETLE_VOIDCORELAUNCHER, APLocations.BLUEPRINTBEETLE_VOIDCORELAUNCHER },
            { GungeonFlags.BLUEPRINTBEETLE_BLANKBULLETS, APLocations.BLUEPRINTBEETLE_BLANKBULLETS },
            { GungeonFlags.BLUEPRINTBEETLE_ORBITALBULLETS, APLocations.BLUEPRINTBEETLE_ORBITALBULLETS },
            { GungeonFlags.BLUEPRINTBEETLE_SCOUTER, APLocations.BLUEPRINTBEETLE_SCOUTER },
            { GungeonFlags.BLUEPRINTBEETLE_CONTROLLER, APLocations.BLUEPRINTBEETLE_CONTROLLER },
            { GungeonFlags.BLUEPRINTBEETLE_GUNNER, APLocations.BLUEPRINTBEETLE_GUNNER },
            // Sherpa/Tinker flags
            { GungeonFlags.SHERPA_ACTIVE_IN_ELEVATOR_ROOM, APLocations.SHERPA_ACTIVE_IN_ELEVATOR_ROOM },
            { GungeonFlags.SHERPA_UNLOCK1_COMPLETE, APLocations.NULL },
            { GungeonFlags.SHERPA_UNLOCK2_COMPLETE, APLocations.NULL },
            { GungeonFlags.SHERPA_UNLOCK3_COMPLETE, APLocations.NULL },
            { GungeonFlags.SHERPA_UNLOCK4_COMPLETE, APLocations.NULL },
            { GungeonFlags.SHERPA_HAS_UNLOCKED_BOSSRUSH, APLocations.NULL },
            { GungeonFlags.SHERPA_UNLOCK1_ELEMENT1, APLocations.SHERPA_UNLOCK1_ELEMENT1 },
            { GungeonFlags.SHERPA_UNLOCK1_ELEMENT2, APLocations.SHERPA_UNLOCK1_ELEMENT2 },
            { GungeonFlags.SHERPA_UNLOCK1_ELEMENT3, APLocations.SHERPA_UNLOCK1_ELEMENT3 },
            { GungeonFlags.SHERPA_UNLOCK1_ELEMENT4, APLocations.SHERPA_UNLOCK1_ELEMENT4 },
            { GungeonFlags.SHERPA_UNLOCK2_ELEMENT1, APLocations.SHERPA_UNLOCK2_ELEMENT1 },
            { GungeonFlags.SHERPA_UNLOCK2_ELEMENT2, APLocations.SHERPA_UNLOCK2_ELEMENT2 },
            { GungeonFlags.SHERPA_UNLOCK2_ELEMENT3, APLocations.SHERPA_UNLOCK2_ELEMENT3 },
            { GungeonFlags.SHERPA_UNLOCK2_ELEMENT4, APLocations.SHERPA_UNLOCK2_ELEMENT4 },
            { GungeonFlags.SHERPA_UNLOCK3_ELEMENT1, APLocations.SHERPA_UNLOCK3_ELEMENT1 },
            { GungeonFlags.SHERPA_UNLOCK3_ELEMENT2, APLocations.SHERPA_UNLOCK3_ELEMENT2 },
            { GungeonFlags.SHERPA_UNLOCK3_ELEMENT3, APLocations.SHERPA_UNLOCK3_ELEMENT3 },
            { GungeonFlags.SHERPA_UNLOCK3_ELEMENT4, APLocations.SHERPA_UNLOCK3_ELEMENT4 },
            { GungeonFlags.SHERPA_UNLOCK4_ELEMENT1, APLocations.SHERPA_UNLOCK4_ELEMENT1 },
            { GungeonFlags.SHERPA_UNLOCK4_ELEMENT2, APLocations.SHERPA_UNLOCK4_ELEMENT2 },
            { GungeonFlags.SHERPA_UNLOCK4_ELEMENT3, APLocations.SHERPA_UNLOCK4_ELEMENT3 },
            { GungeonFlags.SHERPA_UNLOCK4_ELEMENT4, APLocations.SHERPA_UNLOCK4_ELEMENT4 },
            // Other NPCs
            { GungeonFlags.WINCHESTER_MET_PREVIOUSLY, APLocations.WINCHESTER_MET_PREVIOUSLY },
            { GungeonFlags.SHOP_TRUCK_ACTIVE, APLocations.SHOP_TRUCK_ACTIVE },
            // Bullet Parts
            { GungeonFlags.BLACKSMITH_ELEMENT1, APLocations.BLACKSMITH_ELEMENT1 },
            { GungeonFlags.BLACKSMITH_ELEMENT2, APLocations.BLACKSMITH_ELEMENT2 },
            { GungeonFlags.BLACKSMITH_ELEMENT3, APLocations.BLACKSMITH_ELEMENT3 },
            { GungeonFlags.BLACKSMITH_ELEMENT4, APLocations.BLACKSMITH_ELEMENT4 },
            { GungeonFlags.BLACKSMITH_BULLET_COMPLETE, APLocations.BLACKSMITH_BULLET_COMPLETE },
            // Bosses
            { GungeonFlags.BOSSKILLED_GATLING_GULL, APLocations.BOSSKILLED_GATLING_GULL },
            { GungeonFlags.BOSSKILLED_BULLET_KING, APLocations.BOSSKILLED_BULLET_KING },
            { GungeonFlags.BOSSKILLED_BULLET_BROS, APLocations.BOSSKILLED_BULLET_BROS },
            { GungeonFlags.BOSSKILLED_MEDUZI, APLocations.BOSSKILLED_MEDUZI },
            { GungeonFlags.BOSSKILLED_BASHELLISK, APLocations.BOSSKILLED_BASHELLISK },
            { GungeonFlags.BOSSKILLED_BEHOLSTER, APLocations.BOSSKILLED_BEHOLSTER },
            { GungeonFlags.BOSSKILLED_TANK, APLocations.BOSSKILLED_TANK },
            { GungeonFlags.BOSSKILLED_POWDERSKULL, APLocations.BOSSKILLED_POWDERSKULL },
            { GungeonFlags.BOSSKILLED_MINEFLAYER, APLocations.BOSSKILLED_MINEFLAYER },
            { GungeonFlags.BOSSKILLED_HIGH_PRIEST, APLocations.BOSSKILLED_HIGH_PRIEST },
            { GungeonFlags.BOSSKILLED_STATUES, APLocations.BOSSKILLED_STATUES },
            { GungeonFlags.BOSSKILLED_DEMONWALL, APLocations.BOSSKILLED_DEMONWALL },
            { GungeonFlags.BOSSKILLED_DOORMIMIC, APLocations.BOSSKILLED_DOORMIMIC },
            { GungeonFlags.BOSSKILLED_DRAGUN, APLocations.BOSSKILLED_DRAGUN },
            { GungeonFlags.BOSSKILLED_MINI_FUSELIER, APLocations.BOSSKILLED_MINI_FUSELIER },
            { GungeonFlags.BOSSKILLED_BLOBULORD, APLocations.BOSSKILLED_BLOBULORD },
            { GungeonFlags.BOSSKILLED_OLDKING, APLocations.BOSSKILLED_OLDKING },
            { GungeonFlags.BOSSKILLED_RESOURCEFULRAT, APLocations.BOSSKILLED_RESOURCEFULRAT },
            { GungeonFlags.BOSSKILLED_HELICOPTER, APLocations.BOSSKILLED_HELICOPTER },
            { GungeonFlags.BOSSKILLED_LICH, APLocations.BOSSKILLED_LICH },
            { GungeonFlags.BOSSKILLED_HIGHDRAGUN, APLocations.BOSSKILLED_HIGHDRAGUN },
            { GungeonFlags.TUTORIAL_KILLED_MANFREDS_RIVAL, APLocations.TUTORIAL_KILLED_MANFREDS_RIVAL },
            // Pasts
            { GungeonFlags.BOSSKILLED_ROGUE_PAST, APLocations.BOSSKILLED_ROGUE_PAST },
            { GungeonFlags.BOSSKILLED_CONVICT_PAST, APLocations.BOSSKILLED_CONVICT_PAST },
            { GungeonFlags.BOSSKILLED_SOLDIER_PAST, APLocations.BOSSKILLED_SOLDIER_PAST },
            { GungeonFlags.BOSSKILLED_GUIDE_PAST, APLocations.BOSSKILLED_GUIDE_PAST },
            { GungeonFlags.BOSSKILLED_ROBOT_PAST, APLocations.BOSSKILLED_ROBOT_PAST },
            { GungeonFlags.BOSSKILLED_BULLET_PAST, APLocations.BOSSKILLED_BULLET_PAST },
            { GungeonFlags.GUNSLINGER_PAST_KILLED, APLocations.GUNSLINGER_PAST_KILLED },
            // Master Rounds
            { GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_CASTLE, APLocations.ACHIEVEMENT_NOBOSSDAMAGE_CASTLE },
            { GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_GUNGEON, APLocations.ACHIEVEMENT_NOBOSSDAMAGE_GUNGEON },
            { GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_MINES, APLocations.ACHIEVEMENT_NOBOSSDAMAGE_MINES },
            { GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_HOLLOW, APLocations.ACHIEVEMENT_NOBOSSDAMAGE_HOLLOW },
            { GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_FORGE, APLocations.ACHIEVEMENT_NOBOSSDAMAGE_FORGE },
            // Achievements
            { GungeonFlags.ACHIEVEMENT_ACCESS_OUBLIETTE, APLocations.ACHIEVEMENT_ACCESS_OUBLIETTE },
            { GungeonFlags.ACHIEVEMENT_ACCESS_ABBEY, APLocations.ACHIEVEMENT_ACCESS_ABBEY },
            { GungeonFlags.ACHIEVEMENT_CASTLE_MANYTIMES, APLocations.ACHIEVEMENT_CASTLE_MANYTIMES },
            { GungeonFlags.ACHIEVEMENT_GUNGEON_MANYTIMES, APLocations.ACHIEVEMENT_GUNGEON_MANYTIMES },
            { GungeonFlags.ACHIEVEMENT_MINES_MANYTIMES, APLocations.ACHIEVEMENT_MINES_MANYTIMES },
            { GungeonFlags.ACHIEVEMENT_HOLLOW_MANYTIMES, APLocations.ACHIEVEMENT_HOLLOW_MANYTIMES },
            { GungeonFlags.ACHIEVEMENT_FORGE_MANYTIMES, APLocations.ACHIEVEMENT_FORGE_MANYTIMES },
            { GungeonFlags.ACHIEVEMENT_STEAL_THINGS, APLocations.ACHIEVEMENT_STEAL_THINGS },
            { GungeonFlags.ACHIEVEMENT_TABLE_PIT, APLocations.ACHIEVEMENT_TABLE_PIT },
            { GungeonFlags.ACHIEVEMENT_BEASTMODE, APLocations.ACHIEVEMENT_BEASTMODE },
            { GungeonFlags.ACHIEVEMENT_TABLES_FLIPPED, APLocations.ACHIEVEMENT_TABLES_FLIPPED },
            { GungeonFlags.ACHIEVEMENT_SURPRISE_MIMIC, APLocations.ACHIEVEMENT_SURPRISE_MIMIC },
            { GungeonFlags.ACHIEVEMENT_KILL_JAMMED_BOSS, APLocations.ACHIEVEMENT_KILL_JAMMED_BOSS },
            { GungeonFlags.ACHIEVEMENT_BIGGEST_WALLET, APLocations.ACHIEVEMENT_BIGGEST_WALLET },
            // Other
            { GungeonFlags.TUTORIAL_COMPLETED, APLocations.TUTORIAL_COMPLETED },
            { GungeonFlags.TUTORIAL_UNLOCK_HELMET, APLocations.TUTORIAL_UNLOCK_HELMET },
            { GungeonFlags.TUTORIAL_UNLOCK_SHIELD, APLocations.TUTORIAL_UNLOCK_SHIELD },
            { GungeonFlags.ITEMSPECIFIC_STOLE_DRILL, APLocations.ITEMSPECIFIC_STOLE_DRILL },
            { GungeonFlags.ITEMSPECIFIC_STOLE_BANKBAG, APLocations.ITEMSPECIFIC_STOLE_BANKBAG },
            { GungeonFlags.ITEMSPECIFIC_STOLE_BANKMASK, APLocations.ITEMSPECIFIC_STOLE_BANKMASK }
        };

        // Used to determine if a flag should be checked for locations, but still set like a typical gungeon flag
        // TODO this is fine for MVP, but I'd like to redo this at some point
        public static HashSet<GungeonFlags> _flagsToNotCapture = new HashSet<GungeonFlags>{
            GungeonFlags.BOSSKILLED_GATLING_GULL,
            GungeonFlags.BOSSKILLED_BULLET_KING,
            GungeonFlags.BOSSKILLED_BULLET_BROS,
            GungeonFlags.BOSSKILLED_MEDUZI,
            GungeonFlags.BOSSKILLED_BASHELLISK,
            GungeonFlags.BOSSKILLED_BEHOLSTER,
            GungeonFlags.BOSSKILLED_TANK,
            GungeonFlags.BOSSKILLED_POWDERSKULL,
            GungeonFlags.BOSSKILLED_MINEFLAYER,
            GungeonFlags.BOSSKILLED_HIGH_PRIEST,
            GungeonFlags.BOSSKILLED_STATUES,
            GungeonFlags.BOSSKILLED_DEMONWALL,
            GungeonFlags.BOSSKILLED_DOORMIMIC,
            GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_CASTLE,
            GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_GUNGEON,
            GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_MINES,
            GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_HOLLOW,
            GungeonFlags.ACHIEVEMENT_NOBOSSDAMAGE_FORGE,
            GungeonFlags.TUTORIAL_COMPLETED,
            GungeonFlags.TUTORIAL_UNLOCK_HELMET,
            GungeonFlags.TUTORIAL_UNLOCK_SHIELD,
            GungeonFlags.ITEMSPECIFIC_STOLE_DRILL,
            GungeonFlags.ITEMSPECIFIC_STOLE_BANKBAG,
            GungeonFlags.ITEMSPECIFIC_STOLE_BANKMASK
        };

        public static HashSet<GungeonFlags> _pastKilledFlags = new HashSet<GungeonFlags>{
            GungeonFlags.BOSSKILLED_ROGUE_PAST,
            GungeonFlags.BOSSKILLED_CONVICT_PAST,
            GungeonFlags.BOSSKILLED_SOLDIER_PAST,
            GungeonFlags.BOSSKILLED_GUIDE_PAST,
            GungeonFlags.BOSSKILLED_ROBOT_PAST,
            GungeonFlags.BOSSKILLED_BULLET_PAST,
            GungeonFlags.GUNSLINGER_PAST_KILLED
        };

        /// <summary>
        /// Flag that we are setting ourselves, so if it gets picked up by our interjection of SetFlag(), let it through.
        /// </summary>
        private static GungeonFlags _expectedSetFlag = GungeonFlags.NONE;

        /// <summary>
        /// Sets a Gungeon flag without it getting caught by a Harmony Prefix injection. Use instead of GameStatsManager.Instance.SetFlag().
        /// </summary>
        /// <param name="flag">Gungeon flag to set</param>
        /// <param name="value">Set flag to true or false</param>
        public static void SafelySetGungeonFlag(GungeonFlags flag, bool value)
        {
            _expectedSetFlag = flag;
            GameStatsManager.Instance.SetFlag(flag, value);
        }

        /*
         * Patch - GameStatsManager.SetFlag
         * Interject when a Gungeon Flag is set, if its in APLocationFlags don't have it set.
         * For example with meta shops, capture location check without unlocking associated item.
         * Any time an item is purchased from the metashop, the associated flag is set for the save file.
         * This patch takes over that SetFlag function, checks if the flag being set is a metashop location,
         * then captures that flag as an archipelago location without unlocking the item.
         */
        [HarmonyPatch(typeof(GameStatsManager), nameof(GameStatsManager.SetFlag))]
        [HarmonyPrefix]
        public static bool CaptureGungeonFlagSet(GungeonFlags flag, bool value)
        {
            // TODO - this is just for MVP, remove this once we implement other win conditions
            if (_pastKilledFlags.Contains(flag)) {
                ETGModConsole.Log("GameStatsManager: Sending game completion!!: " + flag);
                SessionHandler.DataSender.SendGameCompletion();
                return false;
            }

            //TODO make collection of all meta shop flags to compare against.
            if (GungeonFlagsToAPLocations.ContainsKey(flag) && flag != _expectedSetFlag)
            {
                ETGModConsole.Log("GameStatsManagerPatch: Location checked - Send item here: " + flag);
                SessionHandler.DataSender.SendFoundLocationCheck(GungeonFlagsToAPLocations[flag]);
                ArchipelaGunLocalDataManager.locationsCheckedFlags.Add(flag); // TODO update this for all location checks probably
                if (_flagsToNotCapture.Contains(flag)) // TODO This is kind of ugly jank, but it's fine for mvp
                {
                    return true;
                }
                return false;
            }

            if (flag == _expectedSetFlag)
            {
                // Now that the flag has been bypassed, reset the var
                _expectedSetFlag = GungeonFlags.NONE;
                ETGModConsole.Log("GameStatsManagerPatch: Expected flag was set: " + flag + " - " + value);
            }
            else
            {
                ETGModConsole.Log("GameStatsManagerPatch: Something has set a save flag: " + flag + " - " + value);
            }
            
            return true;
        }

        /*
         * Patch - MetaShopController.GetFlagFromTargetItem
         * Specific to Ox and Cadence shop; Prevent locations checked from reappearing in shop after purchase
         */
        [HarmonyPatch(typeof(MetaShopController), nameof(MetaShopController.GetFlagFromTargetItem))]
        [HarmonyPrefix]
        public static bool Patch_GetFlagFromTargetItem(ref GungeonFlags __result, int shopItemId)
        {
            //ETGModConsole.Log("Patch_GetFlagFromTargetItem: " + shopItemId);
            GungeonFlags flag = GungeonFlags.NONE;
            PickupObject byId = PickupObjectDatabase.GetById(shopItemId);
            foreach (var prereq in byId.encounterTrackable.prerequisites)
            {
                if(prereq.prerequisiteType == DungeonPrerequisite.PrerequisiteType.FLAG)
                {
                    flag = prereq.saveFlagToCheck;
                }
            }
            if (ArchipelaGunLocalDataManager.locationsCheckedFlags.Contains(flag))
            {
                //ETGModConsole.Log("GameStatsManagerPatch: MetaShopController attempting to put heart bottle in shop");
                __result = GungeonFlags.BLUEPRINTMETA_MTXGUN; // TODO this needs to be not the mtx gun, lol
                return false;
            }
            __result = flag;
            return false;
        }

        /*
         * Patch - EncounterTrackable.PrerequisitesMet
         * Controls whether to add items to meta shops (but not ox and cadence).
         * Needed for tracking locations purchased from shops but not unlocked as items yet.
         */
        [HarmonyPatch(typeof(EncounterTrackable), nameof(EncounterTrackable.PrerequisitesMet))]
        [HarmonyPrefix]
        public static bool Patch_PrerequisitesMet(EncounterTrackable __instance, ref bool __result)
        {
            if (GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
            {
                for (int i = 0; i < __instance.m_prerequisites.Length; i++)
                {
                    if (__instance.m_prerequisites[i].prerequisiteType == DungeonPrerequisite.PrerequisiteType.FLAG)
                    {
                        bool hasLocationBeenChecked = ArchipelaGunLocalDataManager.locationsCheckedFlags.Contains(__instance.m_prerequisites[i].saveFlagToCheck);
                        ShopPage shopPageForItem = MetaShopOfferings.ShopOfferings.GetValueSafe(__instance.m_prerequisites[i].saveFlagToCheck).Page;

                        // The result should be *FALSE* if the item *SHOULD* spawn in the shop
                        __result = !(!hasLocationBeenChecked && MetaShopOfferings.hasAccessToShopPage(shopPageForItem));

                        return false;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// Before the Foyer can check if Doug should spawn or not, intercept to run custom logic if we have the ap items to spawn Doug or not. Force the spawn if so, or 0 chance if not.
        /// </summary>
        /// <returns></returns>
        [HarmonyPatch(typeof(Dungeon), nameof(Dungeon.StartupFoyerChecks))]
        [HarmonyPrefix]
        public static bool Patch_StartupFoyerChecks()
        {
            // Doug locations ids are between 240000 and 240220, with no other ids should be in that range. 
            // So we can count how many location ids found are between that range and prevent Doug from spawning back in the foyer if we've reached max bought items for the progressive pages available to us.
            // Max count of 6 pages, so if we have all 6 then we just gaurentee Doug is always there.
            if (ArchipelaGunLocalDataManager.DougShopProgCount < 6 && ArchipelaGunLocalDataManager.FoundLocationsAPIds.Count(id => id > 240000 && id < 240220) >= ArchipelaGunLocalDataManager.DougShopProgCount * 6)
            {
                // We have all items we have access to, don't run StartupFoyerChecks() to spawn Doug
                return false;
            }
            else
            {
                // Move on to StartupFoyerChecks() and set the chance to spawn Doug to 100.
                GameStatsManager.Instance.AccumulatedBeetleMerchantChance = 100;
                GameStatsManager.Instance.AccumulatedUsedBeetleMerchantChance = 100;
                return true;
            }
        }

        /// <summary>
        /// Interjects before GetRandomLockedPaydayItem() is ran to return a random not unlocked payday item. If we don't have the AP Item (which unlocks the biggest wallet achievement), then force it to be null.
        /// Once we have the item let the game run the random as normal.
        /// </summary>
        /// <returns></returns>
        [HarmonyPatch(typeof(BaseShopController), nameof(BaseShopController.GetRandomLockedPaydayItem))]
        [HarmonyPrefix]
        public static bool Patch_GetRandomLockedPaydayItem(ref PickupObject __result)
        {
            if(!GameStatsManager.Instance.GetFlag(GungeonFlags.ACHIEVEMENT_BIGGEST_WALLET))
            {
                __result = null;
                return false;
            }
            else
            {
                return true;
            }
        }

        /*
         * Patch - ShopItemController.OnEnteredRange
         * On entering range of a shop item, if it is an AP item, change the UI text to show the AP item being bought rather than the guneon item.
         */
        [HarmonyPatch(typeof(ShopItemController), nameof(ShopItemController.OnEnteredRange))]
        [HarmonyPostfix]
        public static void Patch_ShopOnEnteredRange(ShopItemController __instance)
        {
            //ETGModConsole.Log("GungeonFlagHandler: Entering OnEnteredRange - " + __instance.item.SaveFlagToSetOnAcquisition.ToString());
            // m_parentShop = metaShopController = Ox and Cadence. m_baseParentShop = all other shops
            if ((__instance.m_parentShop != null || __instance.m_baseParentShop) && GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
            {
                //ETGModConsole.Log("GungeonFlagHandler: Attempting to change label");
                GameUIRoot.Instance.DeregisterDefaultLabel(__instance.transform);
                long locationId = (long)__instance.item.SaveFlagToSetOnAcquisition;
                string locationText = ArchipelaGunLocalDataManager.FoundHints.ContainsKey(locationId) ? ArchipelaGunLocalDataManager.FoundHints[locationId] : "Not connected to Archipelago...";
                string priceText = __instance.ModifiedPrice.ToString() + "[sprite \"hbux_text_icon\"]";
                string text = $"{locationText}: {priceText}";
                Vector3 offset = new Vector3(__instance.sprite.GetBounds().max.x + 0.1875f, __instance.sprite.GetBounds().min.y, 0f);
                GameObject gameObject = GameUIRoot.Instance.RegisterDefaultLabel(__instance.transform, offset, text);
                dfLabel componentInChildren = gameObject.GetComponentInChildren<dfLabel>();
                componentInChildren.ColorizeSymbols = false;
                componentInChildren.ProcessMarkup = true;

                if (__instance.m_parentShop != null && __instance.m_parentShop is MetaShopController)
                {
                    MetaShopController metaShopController = __instance.m_parentShop as MetaShopController;
                    if ((bool)metaShopController.Hologramophone && __instance.item is ItemBlueprintItem)
                    {
                        PickupObject archItem = PickupObjectDatabase.GetById(NpcCellLocationItem.SpawnItemID);
                        metaShopController.Hologramophone.ChangeToSprite(__instance.gameObject, archItem.sprite.collection, archItem.sprite.spriteId);
                    }
                }
                
            }
        }

        /*
         * Patch - MetaShopController.DoSetup
         * After Ox and Cadence shop is instantiated, get hints for the items they currently have available.
         * Also checks for valid item shop pools
         */
        [HarmonyPatch(typeof(MetaShopController), nameof(MetaShopController.DoSetup))]
        [HarmonyPostfix]
        public static void MetaShopDoSetupPostfix(MetaShopController __instance)
        {
            ShopDoSetupPostfix(__instance.m_itemControllers);
        }

        /*
         * Patch - BaseShopController.DoSetup
         * After Non-Ox and Cadence shops are instantiated, get hints for the items they currently have available.
         * Also checks for valid item shop pools
         */
        [HarmonyPatch(typeof(BaseShopController), nameof(BaseShopController.DoSetup))]
        [HarmonyPostfix]
        public static void BaseShopDoSetupPostfix(BaseShopController __instance)
        {
            if(GameManager.Instance.CurrentLevelOverrideState == GameManager.LevelOverrideState.FOYER)
            {
                ShopDoSetupPostfix(__instance.m_itemControllers);
            }
        }

        public static void ShopDoSetupPostfix(List<ShopItemController> itemControllers)
        {
            List<long> locationIds = new List<long>();
            List<GungeonFlags> itemGungeonFlags = new List<GungeonFlags>();

            foreach (var item in itemControllers)
            {
                itemGungeonFlags.Add(item.item.SaveFlagToSetOnAcquisition);

                //// Preform check for finding hints ////
                if (!ArchipelaGunLocalDataManager.FoundHints.ContainsKey((long)item.item.SaveFlagToSetOnAcquisition) &&
                    item.item.SaveFlagToSetOnAcquisition != GungeonFlags.BLUEPRINTMETA_MTXGUN) //TODO this check is only neccessary because I'm using MTXGUN in Patch_GetFlagFromTargetItem
                {
                    locationIds.Add((long)item.item.SaveFlagToSetOnAcquisition);
                }

                //// Stop shop item from spawning if the current shop page isn't unlocked ////
                GungeonFlags itemFlag = item.item.SaveFlagToSetOnAcquisition;

                if (!MetaShopOfferings.ShopOfferings.ContainsKey(itemFlag))
                {
                    // TODO: Freak out and panic!
                    Debug.Log("ALERT: Couldn't find a shop offering with the tag " + itemFlag);
                }
                ShopPage shopPageForItem = MetaShopOfferings.ShopOfferings.GetValueSafe(itemFlag).Page;

                // Figure out what shop pages the player should have access to...
                bool hasAccess = MetaShopOfferings.hasAccessToShopPage(shopPageForItem);

                // If the item shouldn't be accessed, stop the item from spawning
                if (!hasAccess)
                {
                    item.ForceOutOfStock();
                }
            }

            // get hints for current shop locations
            SessionHandler.DataSender.ScoutLocationHints(locationIds.ToArray());
        }


        // This function will trigger any time the game tries to generate the loot pool and read what it thinks are in the pool. Could be useful for debugging.
        // I don't know where else to put this, so this can hang out here for now. Uncomment if you want, I guess
        /*
        [HarmonyPatch(typeof(GenericLootTable), nameof(GenericLootTable.GetCompiledRawItems))]
        [HarmonyPostfix]
        public static void Patch_getCompiledRawItems(ref List<WeightedGameObject> __result)
        {
            String list = "";
            foreach (WeightedGameObject item in __result)
            {
                list += item.pickupId + " ";
            }
            ETGModConsole.Log("GameStatsManagerPatch: getting GenericLootTable: " + list);
        }
        */
    }
}
