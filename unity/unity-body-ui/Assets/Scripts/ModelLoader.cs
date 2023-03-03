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

        LoadModel(url);
        await Task.Yield();
        return wrapper;
    }

    public async void DownloadFile(string url)
    {
        Debug.Log(url);
        string path = GetFilePath(url);
        //if (File.Exists(path))
        //{
        //    Debug.Log("Found file locally, loading...");
        //    LoadModel(path);
        //    return;
        //}

        await GetFileRequest(url, (UnityWebRequest req) =>
        {
            if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.Log($"{req.error} : {req.downloadHandler.text}");
            }
            else
            {
                LoadModel(path);
            }
        });
    }

    string GetFilePath(string url)
    {
        string[] pieces = url.Split('/');
        string filename = pieces[pieces.Length - 1];

        return $"{filePath}{filename}";
    }

    async void LoadModel(string url)
    {
        //WebGLPluginJS.SendConsoleLog("model loading");

        ResetWrapper();

        //WebGLPluginJS.SendConsoleLog("start");

        var gltf = new GLTFast.GltfImport();

        //WebGLPluginJS.SendConsoleLog(url);

        var success = await gltf.Load(url);

        //WebGLPluginJS.SendConsoleLog("before success");

        if (success)
        {
            //WebGLPluginJS.SendConsoleLog("start of if");

            GameObject model = new GameObject("glTF");

            //WebGLPluginJS.SendConsoleLog(gameObject.transform.ToString());

            try
            {
                await gltf.InstantiateMainSceneAsync(gameObject.transform);
            }
            catch(Exception e)
            {
                //WebGLPluginJS.SendConsoleLog(e.Message);
                //WebGLPluginJS.SendConsoleLog(e.StackTrace.ToString());
            }


            //WebGLPluginJS.SendConsoleLog("glTF component after the await");

            model.transform.SetParent(wrapper.transform);

            //WebGLPluginJS.SendConsoleLog("end of if");
        }

        //WebGLPluginJS.SendConsoleLog("after if");
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
