using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ColliderTest : MonoBehaviour
{
    [SerializeField] private SpatialSceneManager _sceneManager;

    [SerializeField] private JSBridge _jsBridge;

    private void OnEnable()
    {
        //wait till all the organs are loaded
        SpatialSceneManager.OnOrgansLoaded += () =>
        {
            //add a collider for each organ in the scene manager
            foreach (var o in _sceneManager.Organs)
            {
                AddColliderAroundChildren(o);
            }
        };
    }

    /// <summary>
    /// Adds a box colider around an organ and then attaches an organ control script
    /// </summary>
    /// <param name="wrapper"></param>
    private void AddColliderAroundChildren(GameObject wrapper)
    {
        var pos = wrapper.transform.localPosition;
        var rot = wrapper.transform.localRotation;
        var scale = wrapper.transform.localScale;

        // need to clear out transforms while encapsulating bounds
        wrapper.transform.localPosition = Vector3.zero;
        wrapper.transform.localRotation = Quaternion.identity;
        wrapper.transform.localScale = Vector3.one;

        // start with root object's bounds
        var bounds = new Bounds(Vector3.zero, Vector3.zero);
        if (wrapper.transform.TryGetComponent<Renderer>(out var mainRenderer))
        {
            // as mentioned here https://forum.unity.com/threads/what-are-bounds.480975/
            // new Bounds() will include 0,0,0 which you may not want to Encapsulate
            // because the vertices of the mesh may be way off the model's origin
            // so instead start with the first renderer bounds and Encapsulate from there
            bounds = mainRenderer.bounds;
        }

        var descendants = wrapper.GetComponentsInChildren<Transform>();
        foreach (Transform desc in descendants)
        {
            if (desc.gameObject.tag == "Untagged" && desc.gameObject.name != "Model")
            {
                if (desc.TryGetComponent<Renderer>(out var childRenderer))
                {
                    // use this trick to see if initialized to renderer bounds yet
                    // https://answers.unity.com/questions/724635/how-does-boundsencapsulate-work.html
                    if (bounds.size == Vector3.zero)
                        bounds = childRenderer.bounds;
                    bounds.Encapsulate(childRenderer.bounds);
                }
            }

        }

        //add the box collider to the child
        var boxCol = wrapper.transform.GetChild(0).AddComponent<BoxCollider>();

        boxCol.center = bounds.center - wrapper.transform.position;
        boxCol.size = bounds.size;

        // restore transforms
        wrapper.transform.localPosition = pos;
        wrapper.transform.localRotation = rot;
        wrapper.transform.localScale = scale;


        //attach the organ control script
        boxCol.AddComponent<OrganControlScript>();

        //pass the reference to the js bridge
        boxCol.GetComponent<OrganControlScript>().jsBridge = _jsBridge;
    }
}
