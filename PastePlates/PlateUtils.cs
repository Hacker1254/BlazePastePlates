using System.Linq;
using UnityEngine;
using VRC;
using VRC.Core;

namespace BE4v.PastePlates
{
    internal static class PlateUtils
    {
        internal static string GetRankColorAndFormatted(this Player player, bool Short = false) =>
            $"<color={player.user.GetRankColor()}>{player.GetRankFormatted(Short)}</color>";

        internal static string GetRankFormatted(this Player player, bool Short = false) =>
            GetRankFormatted(player.user, Short);

        public static float GetFrames(this Player player) =>
            (player.playerNet.ApproxDeltaTimeMS != 0) ? Mathf.Floor(1000f / (float)player.playerNet.ApproxDeltaTimeMS) : -1f;

        internal static short GetPing(this Player instance) =>
             instance.playerNet.Ping;

        public static string GetRankFormatted(this APIUser player, bool Short = false)
        {
            bool MOD = player.hasModerationPowers || player.tags.Contains("admin_moderator");
            bool ADMIN = player.hasScriptingAccess || player.tags.Contains("admin_");
            if (ADMIN)
                return "[Admin User]";
            else if (MOD)
                return "[Moderation User]";
            else if (player.hasVeteranTrustLevel)
                return Short ? "T" : "Trusted";
            else if (player.hasTrustedTrustLevel)
                return Short ? "K" : "Known";
            else if (player.hasKnownTrustLevel)
                return Short ? "U" : "User";
            else if (player.hasBasicTrustLevel)
                return Short ? "N" : "New";
            else
                return Short ? "V" : "Vistor";
        }

        internal static string GetRankColor(this APIUser instance)
        {
            string a = instance.GetRankFormatted().ToLower();
            switch (a)
            {
                case "staff": return "#5e0000";
                case "trusted": return "#a621ff";
                case "known": return "#ffa200";
                case "user": return "#00e62a";
                case "new": return "blue";
                case "visitor": return "#00aeff";
                default: return "#bababa";
            }
        }

        internal static bool IsFriended(this Player player, string UsrID) =>
            player.user.IsFriendsWith(UsrID);

        internal static bool IsLocalPlayerFriend(this Player player) =>
            player.user.IsFriendsWith(APIUser.CurrentUser.id);

        internal static string GetRankColor(this Player instance) =>
            instance.user.GetRankColor();

        /// <summary>
        ///  Works with Hex Too
        /// </summary>
        /// <param name="text"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static string UnityColor(this string text, string color) =>
            $"<color={color}>{text}</color>";

        internal static string Platform(this Player player, bool Short = false)
        {
            if (player.user.IsOnMobile) return Short ? "Q" : "Quest";
            else if (player.playerApi.IsUserInVR()) return "Vr";

            return Short ? "P" : "PC";
        }

        public static string GetPingColored(this Player instance, bool AddPrefix = false)
        {
            short ping = instance.GetPing();
            string result;
            string Fix = "";
            if (AddPrefix) Fix = " [P] ";
            if (ping >= 80)
                result = $"<color=red>{Fix}{ping}</color>";
            else
                result = ((ping <= 35) ? $"<color=green>{Fix}{ping}</color>" : $"<color=yellow>{Fix}{ping}</color>");

            return result;
        }

        internal static void Clean(this Transform transform)
        {
            transform.Find("Trust Icon").gameObject.SetActive(false);
            transform.Find("Trust Text").gameObject.SetActive(false);
            transform.Find("Performance Text").gameObject.SetActive(false);
            transform.Find("Friend Anchor Stats").gameObject.SetActive(false);
            transform.Find("Performance Icon").gameObject.SetActive(false);
        }

        public static string GetFramesColored(this Player instance, bool AddPrefix = false)
        {
            var frames = instance.GetFrames();
            string result;
            string Fix = "";
            if (AddPrefix) Fix = " [F] ";
            if (frames >= 65)
                result = $"<color=green>{Fix}{frames}</color>";
            else
                result = ((frames <= 15) ? $"<color=red>{Fix}{frames}</color>" : $"<color=yellow>{Fix}{frames}</color>");

            return result;
        }
    }
}
