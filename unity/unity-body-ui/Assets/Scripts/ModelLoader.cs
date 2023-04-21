using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class ModelLoader : MonoBehaviour
{
    [SerializeField] private string filePath;
    [SerializeField] private GameObject wrapper;

    public async Task<GameObject> GetModel(string url)
    {
        filePath = $"{Application.persistentDataPath}/Models/";
        wrapper = new GameObject
        {
            name = "Model"
        };

        await LoadModel(url);
        return this.gameObject;
    }

    async Task LoadModel(string path)
    {
        ResetWrapper();

        var gltf = new GLTFast.GltfImport();

        var success = await gltf.Load(path);

        if (success)
        {
            GameObject model = new GameObject("glTF");

            await gltf.InstantiateMainSceneAsync(gameObject.transform);

            model.transform.SetParent(wrapper.transform);
        }
    }

    void ResetWrapper()
    {
        if (wrapper != null)
        {
            foreach (Transform trans in wrapper.transform)
            {
                Destroy(trans.gameObject);
            }
        }
    }
}
