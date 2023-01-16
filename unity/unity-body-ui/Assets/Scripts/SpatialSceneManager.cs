using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System;
using TMPro;

public class SpatialSceneManager : MonoBehaviour
{

    [SerializeField] private NodeArray nodeArray;


    [SerializeField] private GameObject loaderParent;

    public TextMeshProUGUI textbox;

    public async void Start()
    {
        nodeArray = await Get("https://ccf-api.hubmapconsortium.org/v1/scene");

        textbox.text = nodeArray.nodes.Length.ToString();

        await GetOrgans();
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

            NodeArray _nodeArray = JsonUtility.FromJson<NodeArray>(
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

    public async Task GetOrgans()
    {
        List<Task<GameObject>> tasks = new List<Task<GameObject>>();
        List<GameObject> loaders = new List<GameObject>();
        Dictionary<GameObject, SpatialSceneNode> dict = new Dictionary<GameObject, SpatialSceneNode>();

        foreach (var node in nodeArray.nodes)
        {
            if (node.scenegraph == null) break;
            GameObject g = new GameObject()
            {
                name = "Loader"
            };
            g.AddComponent<ModelLoader>();
            loaders.Add(g);
            g.transform.parent = loaderParent.transform;
            Task<GameObject> t = g.GetComponent<ModelLoader>().GetModel(node.scenegraph);
            tasks.Add(t);
        }

        //await Task.WhenAll(tasks);

        //for (int i = 0; i < tasks.Count; i++)
        //{
        //    Organs.Add(tasks[i].Result);
        //    SetOrganData(tasks[i].Result, nodeArray.nodes[i]);
        //}

        //for (int i = 0; i < Organs.Count; i++)
        //{
        //    //add teleportation anchors and set label
        //    GameObject anchor = Instantiate(preTeleportationAnchor);
        //    anchor.SetActive(true);
        //    anchor.transform.parent = Organs[i].transform;

        //    //add tooltip to teleportation anchor label
        //    TMP_Text label = anchor.GetComponentInChildren<TMP_Text>();
        //    label.text = Organs[i].GetComponent<OrganData>().tooltip;

        //    //place organ
        //    PlaceOrgan(Organs[i], nodeArray.nodes[i]);
        //    SetOrganOpacity(Organs[i], nodeArray.nodes[i].opacity);
        //    SetOrganCollider(Organs[i]);
        //}
    }

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
}
