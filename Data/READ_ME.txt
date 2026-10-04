SaveManagerPatch.cs currently is not working and causes the game to crash on load. 
Best assessment right now is that it has to do with MTG ExtendedEnumCache -> https://github.com/SpecialAPI/ModTheGungeonAPI/blob/main/ModTheGungeonAPI/ETGMod/Compatibility/ExtendedEnumCache.cs
MTG is already doing some shenanigans with Harmony to patch the ETG SaveManager that's conflicting with this SavePatch. MTG's changes are done in such a way that I don't know if I can make the SaveManagerPatch compatible.
Recomendation as of now is to abandon this approach for the time being.