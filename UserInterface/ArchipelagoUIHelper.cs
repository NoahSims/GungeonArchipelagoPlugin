using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArchiGungeon.UserInterface
{
    public class ArchipelagoUIHelper : MonoBehaviour
    {
        // Helper method for on screen notifications
        public static void ArchipelagoUINotification(string lineOne, string lineTwo)
        {
            GameUIRoot gameUIRoot = FindObjectOfType<GameUIRoot>();
            gameUIRoot.notificationController?.DoCustomNotification(lineOne, lineTwo, new tk2dSpriteCollectionData(), 1);
        }
    }
}
