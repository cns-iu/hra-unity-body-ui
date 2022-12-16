using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganControlScript : MonoBehaviour
{
    [Header("JSBridge")]
    public JSBridge jsBridge;

    [Header("Scene Manager")]
    public SceneManager sceneManager;

    [Header("Camera Reference")]
    public Camera myMainCamera;

    [Header("Rotation Radius")]
    public float radius = 1;

    //private vars
    private Vector3 myObjectStartPosition, myMouseStartWorldPosition;

    private bool hovering = false;

    private bool isRotating = false;
    private bool isTranslating = false;
    private Transform _transform;

    new public Transform transform
    {
        get
        {
            return _transform ?? (_transform = GetComponent<Transform>());
        }
    }


        //Mouse Enter and Exit Triggers\\
    private void OnMouseEnter()
    {
        //Trigger the mouse down output in the JS Bridge
        jsBridge.GetNodeHoverStart();

        hovering = true;
    }

    private void OnMouseExit()
    {
        //Trigger the mouse down output in the JS Bridge
        jsBridge.GetNodeHoverStop();

        hovering = false;
    }

        //Mouse Hover and Move Triggers\\
    private void Update()
    {
        //only trigger when active scene node
        if (hovering)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Trigger the mouse down output in the JS Bridge
                jsBridge.GetNodeClick();
            }
            else if (Input.GetMouseButtonDown(1))
            {
                //Trigger the mouse down output in the JS Bridge
                jsBridge.GetNodeClick();
            }
            else if (Input.GetMouseButton(0))
            {
                //Trigger the mouse down output in the JS Bridge
                jsBridge.GetNodeDrag();
            }
            else if (Input.GetMouseButton(1))
            {
                //Trigger the mouse down output in the JS Bridge
                jsBridge.GetNodeDrag();
            }
        }
    }
}
