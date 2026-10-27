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

    public static class MetaShopOfferings
    {
        public static readonly Dictionary<string, ShopPage> ShopOfferings =
            new Dictionary<string, ShopPage>
            {
                // OX PAGE 1
                { "thompson", ShopPage.OX_SHOP_PAGE_1 },
                { "heart_bottle", ShopPage.OX_SHOP_PAGE_1 },
                { "gamma_ray", ShopPage.OX_SHOP_PAGE_1 },
                { "rpg", ShopPage.OX_SHOP_PAGE_1 },
                { "rocket_powered_bullets", ShopPage.OX_SHOP_PAGE_1 },
                { "hegemony_carbine", ShopPage.OX_SHOP_PAGE_1 },
                // OX PAGE 2
                { "heart_locket", ShopPage.OX_SHOP_PAGE_2 },
                { "hot_lead", ShopPage.OX_SHOP_PAGE_2 },
                { "freeze_ray", ShopPage.OX_SHOP_PAGE_2 },
                { "heart_lunchbox", ShopPage.OX_SHOP_PAGE_2 },
                { "box", ShopPage.OX_SHOP_PAGE_2 },
                { "mailbox", ShopPage.OX_SHOP_PAGE_2 },
                // OX PAGE 3
                { "fat_bullets", ShopPage.OX_SHOP_PAGE_3 },
                { "+1_bullets", ShopPage.OX_SHOP_PAGE_3 },
                { "angry_bullets", ShopPage.OX_SHOP_PAGE_3 },
                { "heart_purse", ShopPage.OX_SHOP_PAGE_3 },
                { "origuni", ShopPage.OX_SHOP_PAGE_3 },
                // OX PAGE 4
                { "eyepatch", ShopPage.OX_SHOP_PAGE_4 },
                { "bloody_eye", ShopPage.OX_SHOP_PAGE_4 },
                { "spice", ShopPage.OX_SHOP_PAGE_4 },
                { "grasschopper", ShopPage.OX_SHOP_PAGE_4 },
                { "sunglasses", ShopPage.OX_SHOP_PAGE_4 },
                { "flash_ray", ShopPage.OX_SHOP_PAGE_4 },
                // OX PAGE 5
                { "singularity", ShopPage.OX_SHOP_PAGE_5 },
                { "science_cannon", ShopPage.OX_SHOP_PAGE_5 },
                { "nanomachines", ShopPage.OX_SHOP_PAGE_5 },
                { "ice_breaker", ShopPage.OX_SHOP_PAGE_5 },
                { "portable_turret", ShopPage.OX_SHOP_PAGE_5 },
                { "duct_tape", ShopPage.OX_SHOP_PAGE_5 },
                // OX PAGE 6
                { "tangler", ShopPage.OX_SHOP_PAGE_6 },
                { "magic_lamp", ShopPage.OX_SHOP_PAGE_6 },
                { "face_melter", ShopPage.OX_SHOP_PAGE_6 },
                { "heroine", ShopPage.OX_SHOP_PAGE_6 },
                { "mass_shotgun", ShopPage.OX_SHOP_PAGE_6 },
                { "bullet_bore", ShopPage.OX_SHOP_PAGE_6 },
                // OX PAGE 7
                { "raiden_coil", ShopPage.OX_SHOP_PAGE_7 },
                { "black_hole_gun", ShopPage.OX_SHOP_PAGE_7 },
                { "space_friend", ShopPage.OX_SHOP_PAGE_7 },
                { "yari_launcher", ShopPage.OX_SHOP_PAGE_7 },
                { "railgun", ShopPage.OX_SHOP_PAGE_7 },
                { "broccoli", ShopPage.OX_SHOP_PAGE_7 },
                // OX PAGE 8
                { "microtransaction_gun", ShopPage.OX_SHOP_PAGE_8 },
                { "platinum_bullets", ShopPage.OX_SHOP_PAGE_8 },
                // GOOPTON PAGE 1
                { "bug_boots", ShopPage.GOOPTON_SHOP_PAGE_1 },
                { "fossilized_gun", ShopPage.GOOPTON_SHOP_PAGE_1 },
                { "monster_blood", ShopPage.GOOPTON_SHOP_PAGE_1 },
                { "plague_pistol", ShopPage.GOOPTON_SHOP_PAGE_1 },
                { "the_membrane", ShopPage.GOOPTON_SHOP_PAGE_1 },
                { "plunger", ShopPage.GOOPTON_SHOP_PAGE_1 },
                // GOOPTON PAGE 2
                { "sponge", ShopPage.GOOPTON_SHOP_PAGE_2 },
                { "abyssal_tentacle", ShopPage.GOOPTON_SHOP_PAGE_2 },
                { "napalm_strike", ShopPage.GOOPTON_SHOP_PAGE_2 },
                { "gundromeda_strain", ShopPage.GOOPTON_SHOP_PAGE_2 },
                { "evolver", ShopPage.GOOPTON_SHOP_PAGE_2 },
                // TRORC PAGE 1
                { "cluster_mine", ShopPage.TRORC_SHOP_PAGE_1 },
                { "m16", ShopPage.TRORC_SHOP_PAGE_1 },
                { "ration", ShopPage.TRORC_SHOP_PAGE_1 },
                { "air_strike", ShopPage.TRORC_SHOP_PAGE_1 },
                { "commando", ShopPage.TRORC_SHOP_PAGE_1 },
                { "awp", ShopPage.TRORC_SHOP_PAGE_1 },
                // TRORC PAGE 2
                { "rc_rocket", ShopPage.TRORC_SHOP_PAGE_2 },
                { "big_boy", ShopPage.TRORC_SHOP_PAGE_2 },
                { "chaff_grenade", ShopPage.TRORC_SHOP_PAGE_2 },
                { "patriot", ShopPage.TRORC_SHOP_PAGE_2 },
                { "magazine_rack", ShopPage.TRORC_SHOP_PAGE_2 },
                // DOUG PAGE 1
                { "devolver", ShopPage.DOUG_SHOP_PAGE_1 },
                { "casey", ShopPage.DOUG_SHOP_PAGE_1 },
                { "mourning_star", ShopPage.DOUG_SHOP_PAGE_1 },
                { "strafe_gun", ShopPage.DOUG_SHOP_PAGE_1 },
                { "hyper_light_blaster", ShopPage.DOUG_SHOP_PAGE_1 },
                { "starpew", ShopPage.DOUG_SHOP_PAGE_1 },
                // DOUG PAGE 2
                { "dueling_pistol", ShopPage.DOUG_SHOP_PAGE_2 },
                { "teleporter_prototype", ShopPage.DOUG_SHOP_PAGE_2 },
                { "vorpal_gun", ShopPage.DOUG_SHOP_PAGE_2 },
                { "stout_bullets", ShopPage.DOUG_SHOP_PAGE_2 },
                { "charming_rounds", ShopPage.DOUG_SHOP_PAGE_2 },
                { "flak_bullets", ShopPage.DOUG_SHOP_PAGE_2 },
                // DOUG PAGE 3
                { "magic_bullets", ShopPage.DOUG_SHOP_PAGE_3 },
                { "roll_bomb", ShopPage.DOUG_SHOP_PAGE_3 },
                { "silver_bullets", ShopPage.DOUG_SHOP_PAGE_3 },
                { "helix_bullets", ShopPage.DOUG_SHOP_PAGE_3 },
                // FIXME: The locations.py says that "GOLDIES" should be here? This might be the Guilded Bullets?
                { "bumbullets", ShopPage.DOUG_SHOP_PAGE_3 },
                // DOUG PAGE 4
                { "devolver_rounds", ShopPage.DOUG_SHOP_PAGE_4 },
                { "vorpal_bullets", ShopPage.DOUG_SHOP_PAGE_4 },
                { "snowballets", ShopPage.DOUG_SHOP_PAGE_4 },
                { "stone_dome", ShopPage.DOUG_SHOP_PAGE_4 },
                { "combined_rifle", ShopPage.DOUG_SHOP_PAGE_4 },
                { "the_exotic", ShopPage.DOUG_SHOP_PAGE_4 },
                // DOUG PAGE 5
                { "big_shotgun", ShopPage.DOUG_SHOP_PAGE_5 },
                { "gunderfury", ShopPage.DOUG_SHOP_PAGE_5 },
                { "kruller_glaive", ShopPage.DOUG_SHOP_PAGE_5 },
                { "really_special_lute", ShopPage.DOUG_SHOP_PAGE_5 },
                { "tetrominator", ShopPage.DOUG_SHOP_PAGE_5 },
                { "triple_gun", ShopPage.DOUG_SHOP_PAGE_5 },
                // DOUG PAGE 6
                { "knights_gun", ShopPage.DOUG_SHOP_PAGE_6 },
                { "the_predator", ShopPage.DOUG_SHOP_PAGE_6 },
                { "void_core_cannon", ShopPage.DOUG_SHOP_PAGE_6 },
                { "blank_bullets", ShopPage.DOUG_SHOP_PAGE_6 },
                { "orbital_bullets", ShopPage.DOUG_SHOP_PAGE_6 },
                { "scouter", ShopPage.DOUG_SHOP_PAGE_6 },
                { "3rd_party_controller", ShopPage.DOUG_SHOP_PAGE_6 },
                { "gunner", ShopPage.DOUG_SHOP_PAGE_6 },
            };
    }
}
