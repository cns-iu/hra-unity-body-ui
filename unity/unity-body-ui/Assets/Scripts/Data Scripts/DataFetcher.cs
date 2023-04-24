using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DataFetcher : MonoBehaviour
{
    [SerializeField] private NodeArray _nodeArray;

    public NodeArray NodeArray
    {
        get { return _nodeArray; }
    }

    public async Task<NodeArray> Get(string url)
    {
        try
        {
            using var www = UnityWebRequest.Get(url);
            var operation = www.SendWebRequest();

            while (!operation.isDone)
                await Task.Yield();

            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {www.error}");

            var result = www.downloadHandler.text;

            var text = www.downloadHandler.text
           .Replace("@id", "jsonLdId")
           .Replace("@type", "jsonLdType")
           .Replace("\"object\":", "\"glbObject\":");

            _nodeArray = JsonUtility.FromJson<NodeArray>(
                "{ \"nodes\":" +
                text
                + "}"
                );
            return _nodeArray;
        }
        catch (Exception ex)
        {
            Debug.LogError($"{nameof(Get)} failed: {ex.Message}");
            return default;
        }
    }
}


//Json Funcs\\
[Serializable]
public class NodeArray
{
    [SerializeField] public SpatialSceneNode[] nodes;
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
    public GLBObject glbObject; //for reference organs
    public string sex; //for reference organs
}

[Serializable]
public class GLBObject
{
    public string id;
    public string file;
}

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