using UnityEngine;
using System.Runtime.InteropServices;

/// <summary>
/// Class with a JS Plugin functions for WebGL.
/// </summary>
public class WebGLPluginJS : MonoBehaviour {

    [DllImport("__Internal")]
    public static extern void SendConsoleLog(string str);

    [DllImport("__Internal")]
    public static extern void SendEvent(string _id, string eventName, string jsonP);

    [DllImport("__Internal")]
    public static extern void SendOutput(string _id, string eventName, string jsonP);
}
