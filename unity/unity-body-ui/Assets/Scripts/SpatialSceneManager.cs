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
    //action
    public static event Action OnOrgansLoaded;

    //public var for organs
    public List<GameObject> Organs;

    //private vars
    [Header("Current node array loaded")]
    [SerializeField] private NodeArray _nodeArray;

    [Header("Parent object for the organs")]
    [SerializeField] private GameObject _loaderParent;

    [Header("Data fetcher script")]
    [SerializeField] private DataFetcher dataFetcher;

    [Header("Tissue block prefab and list")]
    [SerializeField] private GameObject _preTissueBlock;
    [SerializeField] private List<GameObject> _TissueBlocks;

    private List<string> _MaleEntityIds;
    private List<string> _FemaleEntityIds;

    private void Start()
    {
        //uncomment to load the inital scene when in unity editor runtime
        //SetScene("https://ccf-api.hubmapconsortium.org/v1/scene?sex=male");
    }

    /// <summary>
    /// Set the initial scene, only call one with the inital models
    /// </summary>
    /// <param name="url"></param>
    public async void SetScene(String url)
    {
        //make a call to the data fetcher and grab the node array
        DataFetcher httpClient = dataFetcher;
        _nodeArray = await httpClient.GetNodeArray(url);

        //wait till this function finishes
        await GetOrgans();

        //invoke the organs
        OnOrgansLoaded?.Invoke();

        //create and place said tissue blocks
        CreateAndPlaceTissueBlocks();

        //parent the tissue blocks so that they rotate with the models
        ParentTissueBlocksToOrgans(_TissueBlocks, Organs);
    }

    /// <summary>
    /// NOT IMPLEMENTED
    /// Load scene after it has been set initially
    /// </summary>
    /// <param name="_apiCall"></param>
    public async void ChangeScene(string url)
    {

    }

    /// <summary>
    /// Get the organs from the node array and load them
    /// </summary>
    /// <returns></returns>
    public async Task GetOrgans()
    {
        List<Task<GameObject>> tasks = new List<Task<GameObject>>();
        List<GameObject> loaders = new List<GameObject>();
        Dictionary<GameObject, SpatialSceneNode> dict = new Dictionary<GameObject, SpatialSceneNode>();

        foreach (var node in _nodeArray.nodes)
        {
            if (node.scenegraph == null) break;
            GameObject g = new GameObject()
            {
                name = "Loader"
            };
            g.AddComponent<ModelLoader>();
            loaders.Add(g);
            g.transform.parent = _loaderParent.transform;
            Task<GameObject> t = g.GetComponent<ModelLoader>().GetModel(node.scenegraph);
            tasks.Add(t);
        }

        await Task.WhenAll(tasks);
        //await Task.Delay(10000); //Task.Delay input is in milliseconds

        for (int i = 0; i < tasks.Count; i++)
        {
            Organs.Add(tasks[i].Result);
            SetOrganData(tasks[i].Result, _nodeArray.nodes[i]);
        }

        for (int i = 0; i < Organs.Count; i++)
        {
            //place organ
            PlaceOrgan(Organs[i], _nodeArray.nodes[i]);
            SetOrganOpacity(Organs[i], _nodeArray.nodes[i].opacity);
            //SetOrganCollider(Organs[i]);
        }
    }

    /// <summary>
    /// Instantiate the tissue blocks and then set their data
    /// </summary>
    void CreateAndPlaceTissueBlocks()
    {
        for (int i = 1; i < _nodeArray.nodes.Length; i++)
        {
            if (_nodeArray.nodes[i].scenegraph != null) continue;
            Matrix4x4 reflected = MatrixExtensions.ReflectZ() * MatrixExtensions.BuildMatrix(_nodeArray.nodes[i].transformMatrix);
            GameObject block = Instantiate(
                _preTissueBlock,
                reflected.GetPosition(),
                reflected.rotation
            );
            block.transform.localScale = reflected.lossyScale * 2f;
            SetTissueBlockData(block, _nodeArray.nodes[i]);
            _TissueBlocks.Add(block);
        }
    }

    /// <summary>
    /// Place the organs in their proper location using the transform matrix from the node array
    /// </summary>
    /// <param name="organ"></param>
    /// <param name="node"></param>
    void PlaceOrgan(GameObject organ, SpatialSceneNode node) //-1, 1, -1 -> for scale
    {
        Matrix4x4 reflected = MatrixExtensions.ReflectZ() * MatrixExtensions.BuildMatrix(node.transformMatrix);
        organ.transform.position = reflected.GetPosition();
        organ.transform.rotation = new Quaternion(0f, 0f, 0f, 1f); //hard-coded to avoid bug when running natively on Quest 2
        organ.transform.localScale = new Vector3(
            reflected.lossyScale.x,
            reflected.lossyScale.y,
            -reflected.lossyScale.z
        );

    }

    /// <summary>
    /// Set the organ data to get grabbed when the model gets hovered over
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="node"></param>
    void SetOrganData(GameObject obj, SpatialSceneNode node)
    {
        OrganData dataComponent = obj.AddComponent<OrganData>();
        dataComponent.SceneGraph = node.scenegraph;
        dataComponent.RepresentationOf = node.representation_of;
        dataComponent.Tooltip = node.tooltip;
    }

    /// <summary>
    /// NOT IMPLEMENTED
    /// Function that sets the opacity for the organs
    /// If you uncomment the code all the organs will turn white
    /// </summary>
    /// <param name="organWrapper"></param>
    /// <param name="alpha"></param>
    public static void SetOrganOpacity(GameObject organWrapper, float alpha)
    {
        List<Transform> list = new List<Transform>();

        list = LeavesFinder.FindLeaves(organWrapper.transform.GetChild(0), list);

        foreach (var item in list)
        {
            Renderer renderer = item.GetComponent<MeshRenderer>();

            if (renderer == null) continue;
            Color updatedColor = renderer.material.color;
            updatedColor.a = alpha;
            renderer.material.color = updatedColor;

            Shader standard;
            //standard = Shader.Find("Standard");
            //renderer.material.shader = standard;
            //MaterialExtensions.ToFadeMode(renderer.material);
        }
    }

    /// <summary>
    /// Parents the tissue blocks to the organs
    /// </summary>
    /// <param name="tissueBlocks"></param>
    /// <param name="organs"></param>
    async void ParentTissueBlocksToOrgans(List<GameObject> tissueBlocks, List<GameObject> organs)
    {
        // Add back to AssignEntityIdsToDonorSexLists if delay bug
        _MaleEntityIds = await GetEntityIdsBySex("https://ccf-api.hubmapconsortium.org/v1/tissue-blocks?sex=male");
        _FemaleEntityIds = await GetEntityIdsBySex("https://ccf-api.hubmapconsortium.org/v1/tissue-blocks?sex=female");

        // assign donor sex to organ
        await GetOrganSex();

        // assign donor sex to tissue block and parent to organ
        for (int i = 0; i < _TissueBlocks.Count; i++)
        {
            TissueBlockData tissueData = _TissueBlocks[i].GetComponent<TissueBlockData>();
            if (_MaleEntityIds.Contains(tissueData.EntityId))
            {
                tissueData.DonorSex = "Male";
            }
            else
            {
                tissueData.DonorSex = "Female";
            }

            for (int j = 0; j < Organs.Count; j++)
            {
                OrganData organData = Organs[j].GetComponent<OrganData>();

                foreach (var annotation in tissueData.CcfAnnotations)
                {
                    if (organData.RepresentationOf == annotation && organData.DonorSex == tissueData.DonorSex)
                    {
                        _TissueBlocks[i].transform.parent = Organs[j].transform.GetChild(0).transform;
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Get a list of ids based off the sex of the organs
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<List<string>> GetEntityIdsBySex(string url)
    {
        List<string> result = new List<string>();
        DataFetcher httpClient = dataFetcher;
        NodeArray nodeArray = await httpClient.GetNodeArray(url);
        foreach (var node in nodeArray.nodes)
        {
            result.Add(node.jsonLdId);
        }
        return result;
    }

    /// <summary>
    /// Set the organ data to have the sex of the model
    /// </summary>
    /// <returns></returns>
    public async Task GetOrganSex()
    {
        DataFetcher httpClient = dataFetcher;
        NodeArray nodeArray = await httpClient.GetNodeArray("https://ccf-api.hubmapconsortium.org/v1/reference-organs");
        
        foreach (var organ in Organs)
        {
            OrganData organData = organ.GetComponent<OrganData>();

            foreach (var node in nodeArray.nodes)
            {
                if (organData.SceneGraph == node.glbObject.file)
                {
                    organData.DonorSex = node.sex;
                }
            }
        }
    }

    /// <summary>
    /// Se the tissue block data
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="node"></param>
    void SetTissueBlockData(GameObject obj, SpatialSceneNode node)
    {
        TissueBlockData dataComponent = obj.AddComponent<TissueBlockData>();
        dataComponent.EntityId = node.entityId;
        dataComponent.Name = node.name;
        dataComponent.Tooltip = node.tooltip;
        dataComponent.CcfAnnotations = node.ccf_annotations;
    }
}
