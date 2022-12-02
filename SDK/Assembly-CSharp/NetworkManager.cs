using System;
using System.Linq;
using IL2CPP_Core.Objects;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    static NetworkManager()
    {
        var methodsPlayer = Instance_Class.GetMethods(x => x.GetParameters().Length == 1 && x.GetParameters()[0].ReturnType.Name == VRC.Player.Instance_Class.FullName);
        try
        {
            methodsPlayer[1].Name = "OnPlayerJoined";
        }
        catch { }

        try
        {
            methodsPlayer[3].Name = "OnPlayerLeft";
        }
        catch { }
    }
    public NetworkManager(IntPtr ptr) : base(ptr) { }



    public static new IL2Class Instance_Class = IL2CPP.AssemblyList["Assembly-CSharp"].GetClasses().FirstOrDefault(x => x.GetMethod("OnCustomAuthenticationResponse") != null && x.GetMethod("Awake") != null);
}