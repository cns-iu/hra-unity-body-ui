using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetter
    : MonoBehaviour
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

    [Header("Scene varaibles")]
    public SpatialSceneManager spatialSceneManager;
    public bool sceneSet = false;



    public void SetCameraInteractivity(bool interacive)
    {
        interactivity = interacive;
    }

    public void SetOrganRotationX(float xRotation)
    {
        float yRot = organReference.transform.eulerAngles.y;
        float zRot = organReference.transform.eulerAngles.z;

        Vector3 newRotation = new Vector3(xRotation, yRot, zRot);

        organReference.transform.eulerAngles = newRotation;

        //organReference.transform.Rotate(new Vector3(xRotation, 0, 0));

        //get local rotation
        jsBridge.GetRotationChange(xRotation, organReference.transform.rotation.y);
        Debug.Log(organReference.transform.rotation.x + " " + organReference.transform.rotation.y);
    }

    public void SetOrganRotationY(float yRotation)
    {
        float xRot = organReference.transform.eulerAngles.x;
        float zRot = organReference.transform.eulerAngles.z;


        Vector3 newRotation = new Vector3(xRot, yRotation, zRot);

        organReference.transform.eulerAngles = newRotation;

        //organReference.transform.Rotate(new Vector3(0, yRotation, 0));

        jsBridge.GetRotationChange(organReference.transform.rotation.x, yRotation);

        Debug.Log(organReference.transform.eulerAngles.x + " " + organReference.transform.eulerAngles.y);
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

    public void LoadScene(NodeArray nodeArray)
    {
        if (sceneSet)
        {



            spatialSceneManager.ChangeScene(nodeArray);
        }
        else
        {
            spatialSceneManager.SetScene(nodeArray);
            sceneSet = true;
        }
    }
}
