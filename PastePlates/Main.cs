using System;
using System.Collections.Generic;
using VRC;

namespace BE4v.PastePlates
{
    internal static class Main
    {
        internal static Dictionary<Player, VRCNameplate> currentPlates = new Dictionary<Player, VRCNameplate>();
        private static DateTime LateDelay = DateTime.Now;

        // Add into ur Menu, and Config
        internal static NameplateSettings Settings = new NameplateSettings {
            ShowPlatform = true,
            ShowAviStatus = true,
            ShowPing = true,
            ShowFPS = true,
            ShowFriend = true,
            ShowMaster = true,
            ShowRank = true,
            ShowCrashed = true
        };

        // Add Onto ur OnPlayerJoin Patch
        internal static void OnPlayerJoin(Player player) => 
            currentPlates.Add(player, new VRCNameplate(player, Settings));

        // Add Onto ur OnPlayerLeave Patch (WARNING, THIS IS NEEDED OTHERWISE THE GAME WILL CRASH WHEN A PLAYER WITH A PLATE LEAVES, AS IT WILL TRY TO UPDATE A NULL PLAYERS PLATE)
        internal static void OnPlayerLeave(Player player) =>
            currentPlates.Remove(player);

        // You need to call this in Order for, Ya know, Plates to update, i put this into the OnPlayerUpdateSync Patch, but anywhere where it actively updates should be fine
        internal static void Update() {
            if (LateDelay < DateTime.Now) { // Small Delay to help with lag Ig
                LateDelay = DateTime.Now.AddSeconds(new Random().Next(1, 2));
                foreach (var plate in currentPlates.Values) plate.Update();
            }
        }
    }
}
