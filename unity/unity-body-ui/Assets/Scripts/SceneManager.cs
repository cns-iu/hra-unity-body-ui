using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    [Header("JSBridge")]
    public JSBridge jsBridge;

    [Header("Organs")]
    public GameObject organReference;
    public OrganControlScript organControlScript;

    [Header("Camera")]
    public Camera cam;

    [Header("Interactivity")]
    public bool interactivity = true;



    public void SetCameraInteractivity(bool interacive)
    {
        interactivity = interacive;
    }

    public void SetOrganRotationX(float xRotation)
    {
        organReference.transform.Rotate(new Vector3(xRotation, 0, 0));

        jsBridge.GetRotationChange(organReference.transform.rotation.x, organReference.transform.rotation.y);
    }

    public void SetOrganRotationY(float yRotation)
    {
        organReference.transform.Rotate(new Vector3(0, yRotation, 0));

        jsBridge.GetRotationChange(organReference.transform.rotation.x, organReference.transform.rotation.y);
    }

    public void SetCameraZoom(float zoom)
    {
        cam.fieldOfView = zoom;
    }

    public void SetCameraType(string cameraType)
    {
        if (cameraType == "orthographic")
        {
            cam.orthographic = true;
        }
        else if (cameraType == "perspective")
        {
            cam.orthographic = false;
        }
        else
        {
            Debug.Log("wrong camera type");
        }
    }

    public void SetOrgan(int i)
    {
        organControlScript.IsolateOrgan(i);
    }
}
