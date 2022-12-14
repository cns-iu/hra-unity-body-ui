using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class JSBridge : MonoBehaviour
{
    [SerializeField] private SceneManager sceneManager;

    private string id = "placeHolder";

            //Initial Setter\\
    public void SetInstance(string _id)
    {
        id = _id;

        //since this needs to be called first, output the initialization
        GetInitialized();
    }

                //Inputs\\
    public void SetRotationX(float rotationX)
    {
        //Set the unity scene with the val
        sceneManager.SetOrganRotationX(rotationX);

        //send the change in rotationX as an event
        JsonNum rot = new JsonNum();

        //set the data for the json
        rot.num = rotationX;

        //convert to string
        string json = JsonUtility.ToJson(rot);

        //output
        WebGLPluginJS.SendEvent(id, "rotationX", json);
    }

    public void SetRotationY(float rotationY)
    {
        //Set the unity scene with the val
        sceneManager.SetOrganRotationY(rotationY);

        //send the change in rotationX as an event
        JsonNum rot = new JsonNum();

        //set the data for the json
        rot.num = rotationY;

        //convert to string
        string json = JsonUtility.ToJson(rot);

        //output
        WebGLPluginJS.SendEvent(id, "rotation", json);
    }

    public void SetZoom(float zoom)
    {
        //Set the unity scene with the val
        sceneManager.SetCameraZoom(zoom);

        //convert the data to json and output it as an event
        JsonNum zNum = new JsonNum();

        //set the data for the json
        zNum.num = zoom;

        //convert to string
        string json = JsonUtility.ToJson(zNum);

        //output
        WebGLPluginJS.SendEvent(id, "zoom", json);
    }

    public void SetTarget(Vector3 xyz)
    {
        //set the center of the scene to rotate around
        
        //Set the unity scene with the val

        //convert the data to json and output it as an event
    }

    public void SetBounds(Vector3 xyz)
    {
        //set the perspective bounds of the camera

        //Set the unity scene with the val

        //convert the data to json and output it as an event
    }

    public void SetCamera(string camera)
    {
        //Set the unity scene with the val
        sceneManager.SetCameraType(camera);

        //convert the data to json and output it as an event
        JsonString cam = new JsonString();

        //set the data for the json
        cam.str = camera;

        //convert to string
        string json = JsonUtility.ToJson(cam);

        //output
        WebGLPluginJS.SendEvent(id, "camera", json);
    }

    public void SetInteractivity(bool interactive)
    {
        //Set the unity scene with the val
        sceneManager.SetCameraInteractivity(interactive);

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
    public void GetInitialized()
    {
        WebGLPluginJS.SendOutput(id, "initialized", null);
    }


    public void GetRotationChange(float x, float y)
    {
        Rotation rot = new Rotation();
        rot.rotationX = x;
        rot.rotationY = y;
        string json = JsonUtility.ToJson(rot);
        WebGLPluginJS.SendOutput(id, "rotationChange", json);
    }

    public void GetNodeDrag()
    {
        NodeDragEvent node = new NodeDragEvent();

        string json = JsonUtility.ToJson(node);

        WebGLPluginJS.SendOutput(id, "nodeDrag", json);
    }

    public void GetNodeClick()
    {
        NodeClickEvent node = new NodeClickEvent();

        string json = JsonUtility.ToJson(node);

        WebGLPluginJS.SendOutput(id, "nodeClick", json);
    }

    public void GetNodeHoverStart()
    {
        SpatialSceneNode node = new SpatialSceneNode();

        string json = JsonUtility.ToJson(node);

        WebGLPluginJS.SendOutput(id, "nodeHoverStart", json);
    }

    public void GetNodeHoverStop()
    {
        SpatialSceneNode node = new SpatialSceneNode();

        string json = JsonUtility.ToJson(node);

        WebGLPluginJS.SendOutput(id, "nodeHoverStop", json);
    }


    //Json Funcs\\
    [Serializable]
    public class JsonNum
    {
        public float num;
    }

    [Serializable]
    public class JsonString
    {
        public string str;
    }

    [Serializable]
    public class JsonBool
    {
        public bool boolean;
    }

    [Serializable]
    public class Rotation
    {
        public float rotationX;
        public float rotationY;
    }

    [Serializable]
    public class NodeDragEvent
    {

    }

    [Serializable]
    public class NodeClickEvent
    {
        
    }

    [Serializable]
    public class SpatialSceneNode
    {
        public string jsonLdId;
        public string jsonLdType;
        public string entityId;

        public string[] ccf_annotations;
        public string representation_of;
        public string reference_organ;
        public bool unpickable;
        public bool wireframe;
        public bool _lighting;
        public string scenegraph;
        public string scenegraphNode;
        public bool zoomBasedOpacity;
        public bool zoomToOnLoad;
        public int[] color;
        public float opacity;
        public float[] transformMatrix;
        public string name;
        public string tooltip;
        public float priority;

        public int rui_rank;
        //public GLBObject glbObject; //for reference organs
        public string sex; //for reference organs
    }
}
