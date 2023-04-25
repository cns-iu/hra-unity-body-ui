using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSetter : MonoBehaviour
{
    [Header("JSBridge")]
    [SerializeField] private JSBridge _jsBridge;

    [Header("Organs")]
    [SerializeField] private GameObject _organReference;

    [Header("Camera")]
    [SerializeField] private Camera _cam;

    [Header("Scene varaibles")]
    [SerializeField] private SpatialSceneManager _spatialSceneManager;

    [Header("Public bools")]
    public bool interactivity = true;
    public bool sceneSet = false;


    /// <summary>
    /// Sets the interactivity for the camera (whether it can be moved or not)
    /// </summary>
    /// <param name="interactive"></param>
    public void SetScreenInteractivity(bool interactive)
    {
        interactivity = interactive;
    }

    /// <summary>
    /// Set the x rotation equal to the passed value
    /// </summary>
    /// <param name="xRotation"></param>
    public void SetOrganRotationX(float xRotation)
    {
        //get current y and z rotion 
        float yRot = _organReference.transform.eulerAngles.y;
        float zRot = _organReference.transform.eulerAngles.z;

        //create a new rotation using the passed xrotation
        Vector3 newRotation = new Vector3(xRotation, yRot, zRot);

        //set the euler angle equal to the new rotation
        _organReference.transform.eulerAngles = newRotation;

        //pass data to JSBridge for output
        _jsBridge.GetRotationChange(xRotation, _organReference.transform.rotation.y);
    }

    /// <summary>
    /// Set the y rotation equal to the passed value
    /// </summary>
    /// <param name="yRotation"></param>
    public void SetOrganRotationY(float yRotation)
    {
        //get current x and z rotation
        float xRot = _organReference.transform.eulerAngles.x;
        float zRot = _organReference.transform.eulerAngles.z;

        //create a new rotation using the passed yrotation
        Vector3 newRotation = new Vector3(xRot, yRotation, zRot);

        //set the euler angle equal to the new rotation
        _organReference.transform.eulerAngles = newRotation;

        //pass data to JSBridge for output
        _jsBridge.GetRotationChange(_organReference.transform.rotation.x, yRotation);
    }

    public void SetCameraZoom(float zoom)
    {
        _cam.fieldOfView = zoom;
    }

    /// <summary>
    /// Sets the camera type to either orthographic or perspective
    /// </summary>
    /// <param name="cameraType"></param>
    public void SetCameraType(string cameraType)
    {
        if (cameraType == "orthographic")
        {
            _cam.orthographic = true;
        }
        else if (cameraType == "perspective")
        {
            _cam.orthographic = false;
        }
        else
        {
            Debug.Log("wrong camera type");
        }
    }

    /// <summary>
    /// Loads the scene based off the passed url from the JS bridge
    /// </summary>
    /// <param name="url"></param>
    public void LoadScene(string url)
    {
        if (sceneSet)
        {
            //if the scene has been loaded already just change it
            _spatialSceneManager.ChangeScene(url);
        }
        else
        {
            //if the scene has not been loaded then generate all the models and load the scene
            WebGLPluginJS.SendConsoleLog("Load Scene: " + url);
            _spatialSceneManager.SetScene(url);
            sceneSet = true;
        }
    }
}
