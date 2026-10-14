using ArchiGungeon.ItemArchipelago;
using Dungeonator;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebSocketSharp;
using static Alexandria.DungeonAPI.SpecialComponents;
using static Alexandria.DungeonAPI.SpecialComponents.PedestalSetter;

namespace ArchiGungeon.RoomManagement
{
    [HarmonyPatch]
    internal class NpcCellReplacer
    {
        // this maybe shouldn't be hard coded, but oh well
        public const string NPC_CELL_REPLACEMENT_ROOM_KEY = RoomHandler.MOD_PREFIX + ":\\archipelagoJail.newroom";

        /*
         * Patch - PedestalSetter.returnItemID
         * Patches the Alexandria custom level loader to set the AP item in the new NPC cell
         * TODO: This is some real jank and I'd like to find a better solution to this at some point
         */
        [HarmonyPatch(typeof(PedestalSetter), nameof(PedestalSetter.returnItemID))]
        [HarmonyPrefix]
        public static bool Patch_PedestalSetter(ref int __result, PedestalSetter __instance)
        {
            ETGModConsole.Log("NpcCellReplacer: item test");
            if (__instance.myLootType == LootType.N_A)
            {
                ETGModConsole.Log("NpcCellReplacer: item test 2");
                __result = NpcCellLocationItem.SpawnItemID;
                return false;
            }
            return true;
        }

        /*
         * Patch - LoopFlowBuilder.HandleNodeInjection
         * When the Dungeonator is building the level, if it tries to add any of the NPC cell rooms, replace the room with our custom room
         */
        [HarmonyPatch(typeof(LoopFlowBuilder), nameof(LoopFlowBuilder.HandleNodeInjection), new Type[] { typeof(BuilderFlowNode), typeof(RuntimeInjectionMetadata), typeof(RuntimeInjectionFlags), typeof(FlowCompositeMetastructure) })]
        [HarmonyPrefix]
        public static bool Patch_HandleNodeInjection(BuilderFlowNode root, RuntimeInjectionMetadata sourceMetadata, RuntimeInjectionFlags injectionFlags, FlowCompositeMetastructure metastructure)
        {
            string temp = "";
            SharedInjectionData injectionData = sourceMetadata.injectionData;
            if (injectionData != null)
            {
                foreach (var modData in injectionData.InjectionData)
                {
                    if (modData.exactRoom != null && modData.exactRoom.name.Contains("Cell"))
                    {
                        if (!NpcCellLocationItem.HasAPItemChecksRemaining())
                        {
                            modData.chanceToSpawn = 0; // disable cells spawning once all locations checked
                            ETGModConsole.Log("NpcCellReplacer: All cells location checked - disabling cell spawn.");
                        }
                        modData.exactRoom = RoomHandler.getRoomByName(NPC_CELL_REPLACEMENT_ROOM_KEY);
                        return true;
                    }
                }
            }
            return true;
        }

        /*
         * Patch - LoopFlowBuilder.ProcessSingleNodeInjection
         * When the Dungeonator is building the level, if it tries to add any of the NPC cell rooms, replace the room with our custom room
         */
        [HarmonyPatch(typeof(LoopFlowBuilder), nameof(LoopFlowBuilder.ProcessSingleNodeInjection))]
        [HarmonyPrefix]
        public static bool Patch_ProcessSingleNodeInjection(ref bool __result, ProceduralFlowModifierData currentInjectionData, BuilderFlowNode root, RuntimeInjectionFlags injectionFlags, FlowCompositeMetastructure metastructure, RuntimeInjectionMetadata optionalMetadata = null)
        {
            if (currentInjectionData != null)
            {
                if (currentInjectionData.exactRoom != null && currentInjectionData.exactRoom.name.Contains("Cell"))
                {
                    if (!NpcCellLocationItem.HasAPItemChecksRemaining())
                    {
                        currentInjectionData.chanceToSpawn = 0; // disable cells spawning once all locations checked
                        ETGModConsole.Log("NpcCellReplacer: All cells location checked - disabling cell spawn.");
                    }
                    currentInjectionData.exactRoom = RoomHandler.getRoomByName(NPC_CELL_REPLACEMENT_ROOM_KEY);
                }
            }
            return true;
        }
    }
}
