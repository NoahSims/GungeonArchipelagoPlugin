using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ArchiGungeon.UserInterface
{
    public class AchripelagoUIHelper : MonoBehaviour
    {
        // Helper method for on screen notifications
        public static void ArchipelagoUINotification(string bigMessage, string smallMessage)
        {
            GameUIRoot gameUIRoot = FindObjectOfType<GameUIRoot>();
            gameUIRoot.notificationController?.DoCustomNotification(bigMessage, smallMessage, new tk2dSpriteCollectionData(), 1);
        }
    }
}
