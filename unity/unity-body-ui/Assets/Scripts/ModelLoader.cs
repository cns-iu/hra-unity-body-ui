using System;
using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System.Threading.Tasks;

public class ModelLoader : MonoBehaviour
{
    [SerializeField] private GameObject _wrapper;

    /// <summary>
    /// Gets the models for the organs based off the url
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public async Task<GameObject> GetModel(string url)
    {
        _wrapper = new GameObject
        {
            name = "Model"
        };

        //wait for the organ models to load and then return the gameobject with all the models attached
        await LoadModel(url);
        return this.gameObject;
    }

    /// <summary>
    /// Load a model using the GLTFast import
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    async Task LoadModel(string path)
    {
        //reset the wrapper to prevent problems
        ResetWrapper();

        //create a new GLTF variable using the package
        var gltf = new GLTFast.GltfImport();

        //create a sucess condition
        var success = await gltf.Load(path);

        if (success)
        {
            GameObject model = new GameObject("glTF");

            await gltf.InstantiateMainSceneAsync(gameObject.transform);

            model.transform.SetParent(_wrapper.transform);
        }
    }

    /// <summary>
    /// Remove the wrapper for an object
    /// </summary>
    void ResetWrapper()
    {
        if (_wrapper != null)
        {
            foreach (Transform trans in _wrapper.transform)
            {
                Destroy(trans.gameObject);
            }
        }
    }
}
