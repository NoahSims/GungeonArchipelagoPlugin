# Archipelago Mod -- Enter the Gungeon
Archipelago Multiworld Randomizer mod for **Enter the Gungeon**. 

> [!WARNING]
> The Current version, v0.2.0, is not in a state that we recommend playing. This is mostly a proof of concept build of a new implementation of the mod and is still a work in progress.

## APWorld Dependency
APWorld releases found here (https://github.com/arsylvester/EtG-AP/releases)
- **v0.2.0 - current**: Enter the Gungeon APWorld v0.2.0
- **v0.1.1 - v0.1.7**: Enter the Gungeon APWorld v0.1.1 (https://github.com/MaoBoulve/ArchipelaGunAPWorld/releases)
- **v0.0.4 - v0.0.8**: Enter the Gungeon APWorld v0.0.1
- **v0.0.1 - 0.0.3**: KinTheInfinite Gungeon APWorld (https://github.com/KinTheInfinite/Archipelago/releases)

## Dependencies
Easiest to handle through a mod loader like R2ModMan or Thunderstore.
https://enter-the-gungeon.thunderstore.io/

 - EtG.Alexandria version **0.5.10**
 - ModTheGungeon version **1.9.2**
 - BepInEx version **5.4.21**

 ## Setup
 Enter the Gungeon is in a preview state for Archipelago and thus requires more involved setup. Please see https://archipelago.gg/tutorial/Archipelago/setup/en for details.

 Once an Archipelago server with EtG is setup, install the above mods. They are version dependent and will cause issues with mismatching APIs.

 Extract the contents of **ArchipelagoGungeon_vX.X.X** into the BepInEx plugin folder where the other mods are located:
 
 ![image](https://github.com/user-attachments/assets/f6368910-eebf-4152-990f-6257b5b74d6c)

 **ArchiGungeon.dll** should be within the first folder of ArchipelagoGungeon:
 
 ![image](https://github.com/user-attachments/assets/1dd46d36-5831-4f91-a232-85cecce09111)

> [!NOTE]
> This version of the mod is designed to be played from a new save of gungeon. Make sure to **back up your save slots** and make a **new save** before connecting.
> To connect to the archipelago server, press F4 to open the debug console and connect using the command: 
> ```connect <server-url> <port> <slot-Name>```

![2025-04-01 21-01-03 - frame at 0m14s](https://github.com/user-attachments/assets/293226a0-f645-4b03-bcb2-64ceeb34478c)
