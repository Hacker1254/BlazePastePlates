using TMPro;
using UnityEngine;
using VRC;
using Object = UnityEngine.Object;

namespace BE4v.PastePlates
{
    internal class VRCNameplate
    {
        internal TextMeshProUGUI TMProComp;
        internal Player VRCPlayer;

        internal NameplateSettings Settings { get; set; }

        internal bool IsReady;
        internal Vector3 OgPoz;
        internal Vector3 NewPoz;

        private byte frames;
        private byte ping;
        private int noUpdateCount = 0;

        // i dunno why i have this :3
        internal VRCNameplate (Player player) : this(player, new NameplateSettings()) { }

        internal VRCNameplate(Player player, NameplateSettings settings)
        {
            $"Making Nameplate For {player.user.displayName} <3".GreenPrefix("NamePlates");
            VRCPlayer = player;
            Transform OgObj = player.transform.Find("Player Nameplate/Canvas/Layout/NameplateGroup/Nameplate/Contents/Quick Stats");
            OgPoz = OgObj.transform.localPosition;
            Transform transform = Object.Instantiate<Transform>(OgObj, OgObj.parent, false);
            transform.Clean();
            NewPoz = new Vector3(transform.transform.localPosition.x, transform.transform.localPosition.y * 2, transform.transform.localPosition.z);

            if (transform == null)
            {
                "Quick Stats Transform Is Null!".RedPrefix("ERROR");
                return;
            }

            transform.gameObject.active = true;

            var TextOBJ = Object.Instantiate<GameObject>(transform.Find("Performance Text").gameObject, transform, false);
            TextOBJ.active = true;
            TMProComp = TextOBJ.GetComponent<TextMeshProUGUI>();
            Settings = settings;
            IsReady = true;
        }

        internal void Update() {
            if (!IsReady) return;

            TMProComp.text =
                 (Settings.ShowRank ? $"[{VRCPlayer.GetRankColorAndFormatted(true)}]" : "")
                            + (Settings.ShowMaster ? (VRCPlayer.playerApi.isMaster ? " [M]".UnityColor("#0059b3") : "") : "")
                            + (Settings.ShowFriend ? (VRCPlayer.IsLocalPlayerFriend() ? " [F]".UnityColor("#ffe135") : "") : "")
                            + (Settings.ShowAviStatus ? (VRCPlayer.Components.AvatarModel.releaseStatus == "public" ? " [Public]".UnityColor("#00ffa6") : " [Private]".UnityColor("#ff0064")) : "")
                            + (Settings.ShowFPS ? VRCPlayer.GetFramesColored(true) : "")
                            + (Settings.ShowPing ? VRCPlayer.GetPingColored(true) : "")
                            + (Settings.ShowPlatform ? $" [{VRCPlayer.Platform(true)}]" : "")
                            ;
            string Pref = "";
            if (Settings.ShowCrashed)
            {
                if (frames == VRCPlayer.playerNet.ApproxDeltaTimeMS && ping == VRCPlayer.playerNet.Ping)
                    noUpdateCount++;
                else
                    noUpdateCount = 0;

                frames = VRCPlayer.playerNet.ApproxDeltaTimeMS;
                ping = (byte)VRCPlayer.playerNet.Ping;
                if (noUpdateCount < 30)
                    Pref = "";
                else if (noUpdateCount > 150)
                    Pref = "| [<color=red>Crashed</color>]";
                else if (noUpdateCount > 30)
                    Pref = "| [<color=yellow>Lagging</color>]";
            }

            TMProComp.text += Pref;

            // This is a pretty bad way to do this, but it works i guess lmao
            if (VRCPlayer.transform.Find("Player Nameplate/Canvas/Layout/NameplateGroup/Nameplate/Contents/Quick Stats").gameObject.active == false)
                TMProComp.transform.parent.transform.localPosition = OgPoz;
            else TMProComp.transform.parent.transform.localPosition = NewPoz;
        }
    }
}
