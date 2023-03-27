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

    string GetFilePath(string url)
    {
        string[] pieces = url.Split('/');
        string filename = pieces[pieces.Length - 1];

        return $"{filePath}{filename}";
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

    async Task GetFileRequest(string url, Action<UnityWebRequest> callback)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            req.downloadHandler = new DownloadHandlerFile(GetFilePath(url));

            var operation = req.SendWebRequest();

            while (!operation.isDone)
            {
                //Use to display process to user if needed
                //Debug.Log(operation.progress);
            }
            //move await Task.Yield() into while loop if testing after errors               
            await Task.Yield();

            callback(req);
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
