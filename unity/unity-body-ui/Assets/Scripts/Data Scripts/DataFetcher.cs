using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class DataFetcher : MonoBehaviour
{
    private NodeArray _nodeArray;

    /// <summary>
    /// Gets the node array for the given url
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<NodeArray> GetNodeArray(string url)
    {
        try
        {
            //do a unity web request
            using var www = UnityWebRequest.Get(url);
            var operation = www.SendWebRequest();

            //wait while the task isnt finished
            while (!operation.isDone)
                await Task.Yield();

            //if failed throw an error
            if (www.result != UnityWebRequest.Result.Success)
                Debug.LogError($"Failed: {www.error}");

            //get the text from the internet
            var result = www.downloadHandler.text;

            //clean up the text and change some labels
            var text = www.downloadHandler.text
           .Replace("@id", "jsonLdId")
           .Replace("@type", "jsonLdType")
           .Replace("\"object\":", "\"glbObject\":");

            //turn into a node array
            _nodeArray = JsonUtility.FromJson<NodeArray>(
                "{ \"nodes\":" +
                text
                + "}"
                );
            return _nodeArray;
        }
        catch (Exception ex)
        {
            //catch an error
            Debug.LogError($"{nameof(GetNodeArray)} failed: {ex.Message}");
            return default;
        }
    }
}


//Classes to catpruee JSON responses from CCF API, which we receive and send
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

//to enable receiving different data types (to parse and cast as string or boolean)
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

//organ rotation! Z-axis is locked for rotation
[Serializable]
public class Rotation
{
    public float rotationX;
    public float rotationY;
}

//not implemented yet, Bruce and Riley never decided how this data would be received
[Serializable]
public class NodeDragEvent
{

}

[Serializable]
public class NodeClickEvent
{

}