using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class JSBridge : MonoBehaviour
{
    [SerializeField] private SceneSetter sceneSetter;

    private string id = "";



    //Initial Setter\\

    /// <summary>
    /// First function that gets called by the web component
    /// Sets the string ID so when the other variables get output they have the right name
    /// </summary>
    /// <param name="_id"></param>
    public void SetInstance(string _id)
    {
        id = _id;

        //since this needs to be called first, output the initialization
        GetInitialized();
    }



    //Scene Setter\\

    /// <summary>
    /// Set the scene by taking in a node array and either loading in all the assets or
    /// modifying the current scene to match the nodeArrayString
    /// </summary>
    /// <param name="urlString"></param>
    public void SetScene(string urlString)
    {
        WebGLPluginJS.SendConsoleLog(urlString);

        //turn the json into a node array
        //NodeArray nodeArray = JsonUtility.FromJson<NodeArray>(urlString);

        //send the data off to the scene
        sceneSetter.LoadScene(urlString);
    }



    //Inputs\\
    
    /// <summary>
    /// Set the rotation for the X axis
    /// </summary>
    /// <param name="rotationString"></param>
    public void SetRotationx(string rotationString)
    {
        //attribute change callback is going to throw a string
        float rotationX = float.Parse(rotationString);

        //Set the unity scene with the val
        sceneSetter.SetOrganRotationX(rotationX);

        //send the change in rotationX as an event
        JsonNum rot = new JsonNum();

        //set the data for the json
        rot.num = rotationX;

        //convert to string
        string json = JsonUtility.ToJson(rot);

        //output
        WebGLPluginJS.SendEvent(id, "rotationX", json);
    }

    /// <summary>
    /// Set the rotation for the Y axis
    /// </summary>
    /// <param name="rotationString"></param>
    public void SetRotationy(string rotationString)
    {
        //attribute change callback is going to throw a string
        float rotationY = float.Parse(rotationString);

        //Set the unity scene with the val
        sceneSetter.SetOrganRotationY(rotationY);

        //send the change in rotationX as an event
        JsonNum rot = new JsonNum();

        //set the data for the json
        rot.num = rotationY;

        //convert to string
        string json = JsonUtility.ToJson(rot);

        //output
        WebGLPluginJS.SendEvent(id, "rotation", json);
    }

    /// <summary>
    /// Set the zoom for the camera
    /// </summary>
    /// <param name="zoomString"></param>
    public void SetZoom(string zoomString)
    {
        //attribute change callback is going to throw a string
        float zoom = float.Parse(zoomString);

        //Set the unity scene with the val
        sceneSetter.SetCameraZoom(zoom);

        //convert the data to json and output it as an event
        JsonNum zNum = new JsonNum();

        //set the data for the json
        zNum.num = zoom;

        //convert to string
        string json = JsonUtility.ToJson(zNum);

        //output
        WebGLPluginJS.SendEvent(id, "zoom", json);
    }

    /// <summary>
    /// NOT IMPLEMENTED
    /// should recenter the root gameobject for the organs
    /// </summary>
    /// <param name="organString"></param>
    public void SetTarget(string organString)
    {

        //Vector3 xyz

        //set the center of the scene to rotate around

        //Set the unity scene with the val

        //convert the data to json and output it as an event

        int i = int.Parse(organString);

        //sceneManager.SetOrgan();

        //output
        WebGLPluginJS.SendEvent(id, "target", organString);
    }

    /// <summary>
    /// NOT IMPLEMENTED
    /// Set the bounds of the camera
    /// </summary>
    /// <param name="xyz"></param>
    public void SetBounds(Vector3 xyz)
    {
        //set the perspective bounds of the camera

        //Set the unity scene with the val
        
        //convert the data to json and output it as an event
    }

    /// <summary>
    /// Set the camera type based off the name of the string input
    /// </summary>
    /// <param name="camera"></param>
    public void SetCamera(string camera)
    {
        //Set the unity scene with the val
        sceneSetter.SetCameraType(camera);

        //convert the data to json and output it as an event
        JsonString cam = new JsonString();

        //set the data for the json
        cam.str = camera;

        //convert to string
        string json = JsonUtility.ToJson(cam);

        //output
        WebGLPluginJS.SendEvent(id, "camera", json);
    }

    /// <summary>
    /// Set whether the screen is interactable or not
    /// </summary>
    /// <param name="interactivityString"></param>
    public void SetInteractive(string interactivityString)
    {
        bool interactive = Convert.ToBoolean(interactivityString);

        //Set the unity scene with the val
        sceneSetter.SetCameraInteractivity(interactive);

        //convert the data to json and output it as an event
        JsonBool interactivity = new JsonBool();

        //set the data for the json
        interactivity.boolean = interactive;

        //convert to string
        string json = JsonUtility.ToJson(interactivity);

        //output
        WebGLPluginJS.SendEvent(id, "interactive", json);
    }


                    //Outputs\\

    /// <summary>
    /// Output to JS that Unity initialized
    /// </summary>
    public void GetInitialized()
    {
        //Send the string initialized to the WebGL pluggin
        WebGLPluginJS.SendOutput(id, "initialized", null);
    }

    /// <summary>
    /// Output the Rotation of the organ to JS
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public void GetRotationChange(float x, float y)
    {
        //Create a new rotation object
        Rotation rot = new Rotation();

        //Set the new rotation for the rotation obj
        rot.rotationX = x;
        rot.rotationY = y;

        //Convert the obj to a jason
        string json = JsonUtility.ToJson(rot);

        //Output to the webgl script
        WebGLPluginJS.SendOutput(id, "rotationChange", json);
    }

    /// <summary>
    /// Output that the organ is being dragged by the user
    /// </summary>
    public void GetNodeDrag()
    {
        //Create a new node drag event
        NodeDragEvent node = new NodeDragEvent();

        //convert the obj to a json string
        string json = JsonUtility.ToJson(node);

        //output the node drag event to the webgl script
        WebGLPluginJS.SendOutput(id, "nodeDrag", json);
    }

    /// <summary>
    /// Output that the organ got clicked
    /// </summary>
    public void GetNodeClick()
    {
        //create a node click event
        NodeClickEvent node = new NodeClickEvent();

        //convert the obj to a json string
        string json = JsonUtility.ToJson(node);

        //output the node click event to the webgl script
        WebGLPluginJS.SendOutput(id, "nodeClick", json);
    }

    /// <summary>
    /// NOT IMPLEMENTED
    /// Output when the user starts hovering over the organ
    /// </summary>
    public void GetNodeHoverStart()
    {
        // Need to pass in the spatial scene node data
        SpatialSceneNode node = new SpatialSceneNode();

        //conver it to a json string
        string json = JsonUtility.ToJson(node);

        //output the data to the webgl script
        //WebGLPluginJS.SendOutput(id, "nodeHoverStart", json);
    }

    /// <summary>
    /// NOT IMPLEMENTED
    /// Output when teh user stops hovering over the organ
    /// </summary>
    public void GetNodeHoverStop()
    {
        // Need to pass in the actual spatial scene node
        SpatialSceneNode node = new SpatialSceneNode();

        //convert to json
        string json = JsonUtility.ToJson(node);

        //output the data to the webgl script
        WebGLPluginJS.SendOutput(id, "nodeHoverStop", json);
    }
}
