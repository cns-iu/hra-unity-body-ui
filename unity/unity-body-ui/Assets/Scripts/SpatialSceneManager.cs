using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.Networking;
using System;
using TMPro;
//using UnityEditor.Experimental.GraphView;

public class SpatialSceneManager : MonoBehaviour
{

    [SerializeField] private NodeArray nodeArray;

    [SerializeField] private GameObject loaderParent;

    //Variables for tissue blocks
    public List<GameObject> TissueBlocks;
    [SerializeField] private GameObject preTissueBlock;


    public List<GameObject> Organs;

    public TextMeshProUGUI textbox;

    public async void Start()
    {
        nodeArray = await Get("https://ccf-api.hubmapconsortium.org/v1/scene");

        textbox.text = nodeArray.nodes.Length.ToString();

        await GetOrgans();

        CreateAndPlaceTissueBlocks();
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

        textbox.text = "Before models get loaded";

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

        await Task.WhenAll(tasks);

        for (int i = 0; i < tasks.Count; i++)
        {
            Organs.Add(tasks[i].Result);
            SetOrganData(tasks[i].Result, nodeArray.nodes[i]);
        }

        for (int i = 0; i < Organs.Count; i++)
        {
            //place organ
            PlaceOrgan(Organs[i], nodeArray.nodes[i]);
            //SetOrganOpacity(Organs[i], nodeArray.nodes[i].opacity);
            //SetOrganCollider(Organs[i]);
        }
    }


    void CreateAndPlaceTissueBlocks()
    {
        for (int i = 1; i < nodeArray.nodes.Length; i++)
        {
            if (nodeArray.nodes[i].scenegraph != null) continue;
            Matrix4x4 reflected = ReflectZ() * MatrixExtensions.BuildMatrix(nodeArray.nodes[i].transformMatrix);
            GameObject block = Instantiate(
                preTissueBlock,
                reflected.GetPosition(),
                reflected.rotation
            );
            block.transform.localScale = reflected.lossyScale * 2f;
            SetTissueBlockData(block, nodeArray.nodes[i]);
            SetCellTypeData(block);
            TissueBlocks.Add(block);
        }
    }

    void PlaceOrgan(GameObject organ, SpatialSceneNode node) //-1, 1, -1 -> for scale
    {
        Matrix4x4 reflected = ReflectZ() * MatrixExtensions.BuildMatrix(node.transformMatrix);
        organ.transform.position = reflected.GetPosition();
        organ.transform.rotation = new Quaternion(0f, 0f, 0f, 1f); //hard-coded to avoid bug when running natively on Quest 2
        organ.transform.localScale = new Vector3(
            reflected.lossyScale.x,
            reflected.lossyScale.y,
            -reflected.lossyScale.z
        );

    }

    void SetOrganData(GameObject obj, SpatialSceneNode node)
    {
        OrganData dataComponent = obj.AddComponent<OrganData>();
        dataComponent.SceneGraph = node.scenegraph;
        dataComponent.RepresentationOf = node.representation_of;
        dataComponent.tooltip = node.tooltip;
    }

    public static void SetOrganOpacity(GameObject organWrapper, float alpha)
    {
        List<Transform> list = new List<Transform>();

        Debug.Log(organWrapper.name);
        Debug.Log(organWrapper.transform.GetChild(0).name);

        list = LeavesFinder.FindLeaves(organWrapper.transform.GetChild(0), list);

        foreach (var item in list)
        {
            Renderer renderer = item.GetComponent<MeshRenderer>();

            if (renderer == null) continue;
            Color updatedColor = renderer.material.color;
            updatedColor.a = alpha;
            renderer.material.color = updatedColor;

            Shader standard;
            standard = Shader.Find("Standard");
            renderer.material.shader = standard;
            MaterialExtensions.ToFadeMode(renderer.material);
        }


    }

    Matrix4x4 ReflectZ()
    {
        var result = new Matrix4x4(
            new Vector4(1, 0, 0, 0),
            new Vector4(0, 1, 0, 0),
            new Vector4(0, 0, -1, 0),
            new Vector4(0, 0, 0, 1)
        );
        return result;
    }

    void SetTissueBlockData(GameObject obj, SpatialSceneNode node)
    {
        TissueBlockData dataComponent = obj.AddComponent<TissueBlockData>();
        dataComponent.EntityId = node.entityId;
        dataComponent.Name = node.name;
        dataComponent.Tooltip = node.tooltip;
        dataComponent.CcfAnnotations = node.ccf_annotations;
    }

    void SetCellTypeData(GameObject obj)
    {
        obj.AddComponent<CellTypeData>();
        obj.AddComponent<CellTypeDataFetcher>();
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
