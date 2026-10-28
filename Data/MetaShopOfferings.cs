using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ArchiGungeon.Data
{
    public enum ShopPage
    {
        OX_SHOP_PAGE_1,
        OX_SHOP_PAGE_2,
        OX_SHOP_PAGE_3,
        OX_SHOP_PAGE_4,
        OX_SHOP_PAGE_5,
        OX_SHOP_PAGE_6,
        OX_SHOP_PAGE_7,
        OX_SHOP_PAGE_8,
        TRORC_SHOP_PAGE_1,
        TRORC_SHOP_PAGE_2,
        GOOPTON_SHOP_PAGE_1,
        GOOPTON_SHOP_PAGE_2,
        DOUG_SHOP_PAGE_1,
        DOUG_SHOP_PAGE_2,
        DOUG_SHOP_PAGE_3,
        DOUG_SHOP_PAGE_4,
        DOUG_SHOP_PAGE_5,
        DOUG_SHOP_PAGE_6,
    }

    public struct LocationInfo
    {
        public string ItemName;
        public ShopPage Page;

        public LocationInfo(string itemName, ShopPage page)
        {
            ItemName = itemName;
            Page = page;
        }
    }

    public static class MetaShopOfferings
    {
        public static readonly Dictionary<GungeonFlags, LocationInfo> ShopOfferings =
            new Dictionary<GungeonFlags, LocationInfo>
            {
                // OX PAGE 1
                { GungeonFlags.BLUEPRINTMETA_THOMPSON, new LocationInfo("thompson", ShopPage.OX_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTMETA_HEARTBOTTLE, new LocationInfo("heart_bottle", ShopPage.OX_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTMETA_GAMMARAY, new LocationInfo("gamma_ray", ShopPage.OX_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTMETA_RPG, new LocationInfo("rpg", ShopPage.OX_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTMETA_ROCKETBULLETS, new LocationInfo("rocket_powered_bullets", ShopPage.OX_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTMETA_HEGEMONYRIFLE, new LocationInfo("hegemony_carbine", ShopPage.OX_SHOP_PAGE_1) },
                // OX PAGE 2
                { GungeonFlags.BLUEPRINTMETA_HEARTLOCKET, new LocationInfo("heart_locket", ShopPage.OX_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTMETA_FIREBULLETS, new LocationInfo("hot_lead", ShopPage.OX_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTMETA_FREEZERAY, new LocationInfo("freeze_ray", ShopPage.OX_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTMETA_HEARTLUNCHBOX, new LocationInfo("heart_lunchbox", ShopPage.OX_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTMETA_BOX, new LocationInfo("box", ShopPage.OX_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTMETA_MAILBOX, new LocationInfo("mailbox", ShopPage.OX_SHOP_PAGE_2) },
                // OX PAGE 3
                { GungeonFlags.BLUEPRINTMETA_FATBULLETS, new LocationInfo("fat_bullets", ShopPage.OX_SHOP_PAGE_3) },
                { GungeonFlags.BLUEPRINTMETA_BETTERBULLETS, new LocationInfo("+1_bullets", ShopPage.OX_SHOP_PAGE_3) },
                { GungeonFlags.BLUEPRINTMETA_ANGRYBULLETS, new LocationInfo("angry_bullets", ShopPage.OX_SHOP_PAGE_3) },
                { GungeonFlags.BLUEPRINTMETA_HEARTPURSE, new LocationInfo("heart_purse", ShopPage.OX_SHOP_PAGE_3) },
                { GungeonFlags.BLUEPRINTMETA_ORIGUNI, new LocationInfo("origuni", ShopPage.OX_SHOP_PAGE_3) },
                // OX PAGE 4
                { GungeonFlags.BLUEPRINTMETA_EYEPATCH, new LocationInfo("eyepatch", ShopPage.OX_SHOP_PAGE_4) },
                { GungeonFlags.BLUEPRINTMETA_BLOODYEYE, new LocationInfo("bloody_eye", ShopPage.OX_SHOP_PAGE_4) },
                { GungeonFlags.BLUEPRINTMETA_SPICE, new LocationInfo("spice", ShopPage.OX_SHOP_PAGE_4) },
                { GungeonFlags.BLUEPRINTMETA_GRASSCHOPPER, new LocationInfo("grasschopper", ShopPage.OX_SHOP_PAGE_4) },
                { GungeonFlags.BLUEPRINTMETA_SUNGLASSES, new LocationInfo("sunglasses", ShopPage.OX_SHOP_PAGE_4) },
                { GungeonFlags.BLUEPRINTMETA_FLASHRAYGUN, new LocationInfo("flash_ray", ShopPage.OX_SHOP_PAGE_4) },
                // OX PAGE 5
                { GungeonFlags.BLUEPRINTMETA_SINGULARITY, new LocationInfo("singularity", ShopPage.OX_SHOP_PAGE_5) },
                { GungeonFlags.BLUEPRINTMETA_SCIENCECANNON, new LocationInfo("science_cannon", ShopPage.OX_SHOP_PAGE_5) },
                { GungeonFlags.BLUEPRINTMETA_NANOMACHINES, new LocationInfo("nanomachines", ShopPage.OX_SHOP_PAGE_5) },
                { GungeonFlags.BLUEPRINTMETA_ICEBREAKER, new LocationInfo("ice_breaker", ShopPage.OX_SHOP_PAGE_5) },
                { GungeonFlags.BLUEPRINTMETA_PORTABLETURRET, new LocationInfo("portable_turret", ShopPage.OX_SHOP_PAGE_5) },
                { GungeonFlags.BLUEPRINTMETA_DUCTTAPE, new LocationInfo("duct_tape", ShopPage.OX_SHOP_PAGE_5) },
                // OX PAGE 6
                { GungeonFlags.BLUEPRINTMETA_TANGLER, new LocationInfo("tangler", ShopPage.OX_SHOP_PAGE_6) },
                { GungeonFlags.BLUEPRINTMETA_LAMP, new LocationInfo("magic_lamp", ShopPage.OX_SHOP_PAGE_6) },
                { GungeonFlags.BLUEPRINTMETA_FACEMELTER, new LocationInfo("face_melter", ShopPage.OX_SHOP_PAGE_6) },
                { GungeonFlags.BLUEPRINTMETA_HEROINE, new LocationInfo("heroine", ShopPage.OX_SHOP_PAGE_6) },
                { GungeonFlags.BLUEPRINTMETA_MASSSHOTGUN, new LocationInfo("mass_shotgun", ShopPage.OX_SHOP_PAGE_6) },
                { GungeonFlags.BLUEPRINTMETA_CEREBRALBORE, new LocationInfo("bullet_bore", ShopPage.OX_SHOP_PAGE_6) },
                // OX PAGE 7
                { GungeonFlags.BLUEPRINTMETA_RAIDENCOIL, new LocationInfo("raiden_coil", ShopPage.OX_SHOP_PAGE_7) },
                { GungeonFlags.BLUEPRINTMETA_BLACKHOLEGUN, new LocationInfo("black_hole_gun", ShopPage.OX_SHOP_PAGE_7) },
                { GungeonFlags.BLUEPRINTMETA_SPACEFRIEND, new LocationInfo("space_friend", ShopPage.OX_SHOP_PAGE_7) },
                { GungeonFlags.BLUEPRINTMETA_YARI, new LocationInfo("yari_launcher", ShopPage.OX_SHOP_PAGE_7) },
                { GungeonFlags.BLUEPRINTMETA_RAILGUN, new LocationInfo("railgun", ShopPage.OX_SHOP_PAGE_7) },
                { GungeonFlags.BLUEPRINTMETA_BROCCOLI, new LocationInfo("broccoli", ShopPage.OX_SHOP_PAGE_7) },
                // OX PAGE 8
                { GungeonFlags.BLUEPRINTMETA_MTXGUN, new LocationInfo("microtransaction_gun", ShopPage.OX_SHOP_PAGE_8) },
                { GungeonFlags.BLUEPRINTMETA_PLATINUMBULLETS, new LocationInfo("platinum_bullets", ShopPage.OX_SHOP_PAGE_8) },
                // GOOPTON PAGE 1
                { GungeonFlags.BLUEPRINTGOOP_BUGBOOTS, new LocationInfo("bug_boots", ShopPage.GOOPTON_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTGOOP_FOSSILIZEDGUN, new LocationInfo("fossilized_gun", ShopPage.GOOPTON_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTGOOP_MONSTERBLOOD, new LocationInfo("monster_blood", ShopPage.GOOPTON_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTGOOP_PLAGUEPISTOL, new LocationInfo("plague_pistol", ShopPage.GOOPTON_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTGOOP_MEMBRANE, new LocationInfo("the_membrane", ShopPage.GOOPTON_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTGOOP_PLUNGER, new LocationInfo("plunger", ShopPage.GOOPTON_SHOP_PAGE_1) },
                // GOOPTON PAGE 2
                { GungeonFlags.BLUEPRINTGOOP_SPONGE, new LocationInfo("sponge", ShopPage.GOOPTON_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTGOOP_TENTACLEARM, new LocationInfo("abyssal_tentacle", ShopPage.GOOPTON_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTGOOP_NAPALMSTRIKE, new LocationInfo("napalm_strike", ShopPage.GOOPTON_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTGOOP_GUNDROMEDASTRAIN, new LocationInfo("gundromeda_strain", ShopPage.GOOPTON_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTGOOP_EVOLVER, new LocationInfo("evolver", ShopPage.GOOPTON_SHOP_PAGE_2) },
                // TRORC PAGE 1
                { GungeonFlags.BLUEPRINTTRUCK_CLUSTERMINE, new LocationInfo("cluster_mine", ShopPage.TRORC_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTTRUCK_MSIXTEEN, new LocationInfo("m16", ShopPage.TRORC_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTTRUCK_RATION, new LocationInfo("ration", ShopPage.TRORC_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTTRUCK_AIRSTRIKE, new LocationInfo("air_strike", ShopPage.TRORC_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTTRUCK_COMMANDO, new LocationInfo("commando", ShopPage.TRORC_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTTRUCK_AWP, new LocationInfo("awp", ShopPage.TRORC_SHOP_PAGE_1) },
                // TRORC PAGE 2
                { GungeonFlags.BLUEPRINTTRUCK_NIKITA, new LocationInfo("rc_rocket", ShopPage.TRORC_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTTRUCK_BIGBOY, new LocationInfo("big_boy", ShopPage.TRORC_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTTRUCK_CHAFFGRENADE, new LocationInfo("chaff_grenade", ShopPage.TRORC_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTTRUCK_PATRIOT, new LocationInfo("patriot", ShopPage.TRORC_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTTRUCK_MAGAZINERACK, new LocationInfo("magazine_rack", ShopPage.TRORC_SHOP_PAGE_2) },
                // DOUG PAGE 1
                { GungeonFlags.BLUEPRINTBEETLE_DEVOLVER, new LocationInfo("devolver", ShopPage.DOUG_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTBEETLE_BASEBALLBAT, new LocationInfo("casey", ShopPage.DOUG_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTBEETLE_DAWNHAMMER, new LocationInfo("mourning_star", ShopPage.DOUG_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTBEETLE_STRAFEGUN, new LocationInfo("strafe_gun", ShopPage.DOUG_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTBEETLE_HYPERLIGHT, new LocationInfo("hyper_light_blaster", ShopPage.DOUG_SHOP_PAGE_1) },
                { GungeonFlags.BLUEPRINTBEETLE_STARPEW, new LocationInfo("starpew", ShopPage.DOUG_SHOP_PAGE_1) },
                // DOUG PAGE 2
                { GungeonFlags.BLUEPRINTBEETLE_DUELINGLASER, new LocationInfo("dueling_pistol", ShopPage.DOUG_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTBEETLE_TELEPORTERPROTOTYPE, new LocationInfo("teleporter_prototype", ShopPage.DOUG_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTBEETLE_VORPALGUN, new LocationInfo("vorpal_gun", ShopPage.DOUG_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTBEETLE_STOUTIES, new LocationInfo("stout_bullets", ShopPage.DOUG_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTBEETLE_CHARMERS, new LocationInfo("charming_rounds", ShopPage.DOUG_SHOP_PAGE_2) },
                { GungeonFlags.BLUEPRINTBEETLE_FLAKTUALLY, new LocationInfo("flak_bullets", ShopPage.DOUG_SHOP_PAGE_2) },
                // DOUG PAGE 3
                { GungeonFlags.BLUEPRINTBEETLE_MAGICITES, new LocationInfo("magic_bullets", ShopPage.DOUG_SHOP_PAGE_3) },
                { GungeonFlags.BLUEPRINTBEETLE_METROIDBOMBS, new LocationInfo("roll_bomb", ShopPage.DOUG_SHOP_PAGE_3) },
                { GungeonFlags.BLUEPRINTBEETLE_SILVALLIES, new LocationInfo("silver_bullets", ShopPage.DOUG_SHOP_PAGE_3) },
                { GungeonFlags.BLUEPRINTBEETLE_HELIX, new LocationInfo("helix_bullets", ShopPage.DOUG_SHOP_PAGE_3) },
                // FIXME: The locations.py says that "GOLDIES" should be here? This might be the Guilded Bullets?
                { GungeonFlags.BLUEPRINTBEETLE_GOLDIES, new LocationInfo("gilded_bullets", ShopPage.DOUG_SHOP_PAGE_3) },
                { GungeonFlags.BLUEPRINTBEETLE_BEEBULLETS, new LocationInfo("bumbullets", ShopPage.DOUG_SHOP_PAGE_3) },
                // DOUG PAGE 4
                { GungeonFlags.BLUEPRINTBEETLE_DEVOLVERBULLETS, new LocationInfo("devolver_rounds", ShopPage.DOUG_SHOP_PAGE_4) },
                { GungeonFlags.BLUEPRINTBEETLE_CRITICALBULLETS, new LocationInfo("vorpal_bullets", ShopPage.DOUG_SHOP_PAGE_4) },
                { GungeonFlags.BLUEPRINTBEETLE_SNOWBALLETS, new LocationInfo("snowballets", ShopPage.DOUG_SHOP_PAGE_4) },
                { GungeonFlags.BLUEPRINTBEETLE_BIGGIANTHEAD, new LocationInfo("stone_dome", ShopPage.DOUG_SHOP_PAGE_4) },
                { GungeonFlags.BLUEPRINTBEETLE_COMBINERIFLE, new LocationInfo("combined_rifle", ShopPage.DOUG_SHOP_PAGE_4) },
                { GungeonFlags.BLUEPRINTBEETLE_EXOTICGUN, new LocationInfo("the_exotic", ShopPage.DOUG_SHOP_PAGE_4) },
                // DOUG PAGE 5
                { GungeonFlags.BLUEPRINTBEETLE_BIGSHOTGUN, new LocationInfo("big_shotgun", ShopPage.DOUG_SHOP_PAGE_5) },
                { GungeonFlags.BLUEPRINTBEETLE_GUNDERFURY, new LocationInfo("gunderfury", ShopPage.DOUG_SHOP_PAGE_5) },
                { GungeonFlags.BLUEPRINTBEETLE_GLAIVE, new LocationInfo("kruller_glaive", ShopPage.DOUG_SHOP_PAGE_5) },
                { GungeonFlags.BLUEPRINTBEETLE_LUTE, new LocationInfo("really_special_lute", ShopPage.DOUG_SHOP_PAGE_5) },
                { GungeonFlags.BLUEPRINTBEETLE_TETRIS, new LocationInfo("tetrominator", ShopPage.DOUG_SHOP_PAGE_5) },
                { GungeonFlags.BLUEPRINTBEETLE_TRIGUN, new LocationInfo("triple_gun", ShopPage.DOUG_SHOP_PAGE_5) },
                // DOUG PAGE 6
                { GungeonFlags.BLUEPRINTBEETLE_SHOVEL, new LocationInfo("knights_gun", ShopPage.DOUG_SHOP_PAGE_6) },
                { GungeonFlags.BLUEPRINTBEETLE_PREDATOR, new LocationInfo("the_predator", ShopPage.DOUG_SHOP_PAGE_6) },
                { GungeonFlags.BLUEPRINTBEETLE_VOIDCORELAUNCHER, new LocationInfo("void_core_cannon", ShopPage.DOUG_SHOP_PAGE_6) },
                { GungeonFlags.BLUEPRINTBEETLE_BLANKBULLETS, new LocationInfo("blank_bullets", ShopPage.DOUG_SHOP_PAGE_6) },
                { GungeonFlags.BLUEPRINTBEETLE_ORBITALBULLETS, new LocationInfo("orbital_bullets", ShopPage.DOUG_SHOP_PAGE_6) },
                { GungeonFlags.BLUEPRINTBEETLE_SCOUTER, new LocationInfo("scouter", ShopPage.DOUG_SHOP_PAGE_6) },
                { GungeonFlags.BLUEPRINTBEETLE_CONTROLLER, new LocationInfo("3rd_party_controller", ShopPage.DOUG_SHOP_PAGE_6) },
                { GungeonFlags.BLUEPRINTBEETLE_GUNNER, new LocationInfo("gunner", ShopPage.DOUG_SHOP_PAGE_6) },
            };
    }
}
