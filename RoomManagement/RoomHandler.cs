using Alexandria.DungeonAPI;
using ArchiGungeon.Data;
using ArchiGungeon.DebugTools;
using BepInEx;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using static Alexandria.DungeonAPI.RoomFactory;

namespace ArchiGungeon.RoomManagement
{
    internal class RoomHandler
    {
        public const string MOD_PREFIX = "ArchiGungeonMod";

        public static Dictionary<string, RoomData> customRooms = new Dictionary<string, RoomData>();

        public static void Init()
        {
            string[] directories = Directory.GetDirectories(Paths.PluginPath, "CustomRoomData", SearchOption.AllDirectories);
            foreach (string roomDirectory in directories)
            {
                customRooms = RoomFactory.LoadRoomsFromRoomDirectory(MOD_PREFIX, roomDirectory);
            }
        }

        public static PrototypeDungeonRoom getRoomByName(string name)
        {
            if (customRooms.ContainsKey(name))
            {
                return customRooms[name].room;
            }
            ArchDebugPrint.DebugLog(DebugCategory.Error, "RoomHandler: Room not found in custom room dictionary - " + name);
            return null;
        }
    }
}
