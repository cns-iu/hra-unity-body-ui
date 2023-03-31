using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Disable capturing whole input on the page
/// Read more: https://docs.unity3d.com/ScriptReference/WebGLInput-captureAllKeyboardInput.html
/// </summary>
public class DisableWebGLInputCapture : MonoBehaviour
{
    void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        // disable WebGLInput.captureAllKeyboardInput so elements in web page can handle keyboard inputs
        WebGLInput.captureAllKeyboardInput = false;
#endif
    }
}