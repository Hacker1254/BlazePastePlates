using System;
using System.Diagnostics;
using IL2CPP_Core.Objects;
using BE4v.SDK;
using BE4v.Patch.Core;
using BE4v.PastePlates;

namespace BE4v.Patch.List
{
    public class Logger_JoinLeft : IPatch
    {
        public delegate void _(IntPtr _instance, IntPtr _player);
        public void Start()
        {
            try
            {
                IL2Method method = NetworkManager.Instance_Class.GetMethod("OnPlayerJoined");
                if (method == null)
                    throw new NullReferenceException();

                patch_Join = new IL2Patch(method, (_)OnPlayerJoined);
                __OnPlayerJoined = patch_Join.CreateDelegate<_>();
            }
            catch { "1th method error!".RedPrefix("Logger_JoinLeft"); }

            try
            {
                IL2Method method = NetworkManager.Instance_Class.GetMethod("OnPlayerLeft");
                if (method == null)
                    throw new NullReferenceException();

                patch_Left = new IL2Patch(method, (_)OnPlayerLeft);
                __OnPlayerLeft = patch_Left.CreateDelegate<_>();
            }
            catch { "2th method error!".RedPrefix("Logger_JoinLeft"); }
        }

        public static void OnPlayerJoined(IntPtr _instance, IntPtr _player)
        {
            if (_instance == IntPtr.Zero) return;
            if (_player == IntPtr.Zero) return;
            try
            {
                VRC.Player player = new VRC.Player(_player);
                $"{player.user.displayName}".GreenPrefix("OnPlayerJoined");
                Main.OnPlayerJoin(player);
            }
            catch { }

            __OnPlayerJoined(_instance, _player);
        }

        public static void OnPlayerLeft(IntPtr _instance, IntPtr _player)
        {
            if (_instance == IntPtr.Zero) return;
            if (_player == IntPtr.Zero) return;
            try
            {
                VRC.Player player = new VRC.Player(_player);
                $"{player.user.displayName}".RedPrefix("OnPlayerLeave");
                Main.OnPlayerLeave(player);
            }
            catch { }

            __OnPlayerLeft(_instance, _player);
        }

        public static IL2Patch patch_Connect;
        public static _ __OnPlayerConnect;

        public static IL2Patch patch_Join;
        public static _ __OnPlayerJoined;

        public static IL2Patch patch_Left;
        public static _ __OnPlayerLeft;
    }
}
