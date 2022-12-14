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
    }

    private void OnMouseExit()
    {
        //Trigger the mouse down output in the JS Bridge
        jsBridge.GetNodeHoverStop();
    }

        //Mouse Hover and Move Triggers\\
    private void Update()
    {
        if (sceneManager.interactivity)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //Trigger the mouse down output in the JS Bridge
                jsBridge.GetNodeClick();

                //set the bool to enable rotation during drag
                isRotating = true;

                //start rolling ball
                Vector3 lMousePosition = Input.mousePosition;
                myMouseStartWorldPosition = lMousePosition;
                myObjectStartPosition = transform.position;
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

                //rolling ball alg
                Vector3 lMousePosition = Input.mousePosition;

                Vector3 dr = lMousePosition - myMouseStartWorldPosition;

                Vector3 _n = new Vector3(-dr.y, dr.x, 0);

                _n = _n.normalized;

                this.transform.RotateAround(this.transform.position, _n, -dr.magnitude / radius);

                myMouseStartWorldPosition = lMousePosition;
            }
            else if (Input.GetMouseButton(1))
            {
                //Trigger the mouse down output in the JS Bridge
                jsBridge.GetNodeDrag();

                var mousePos = Input.mousePosition;
                mousePos.z = 10;

                Vector3 lMousePosition = myMainCamera.ScreenToWorldPoint(mousePos);

                Debug.Log(lMousePosition);

                this.transform.position = new Vector3(lMousePosition.x, lMousePosition.y, this.transform.position.z);
            }
        }
    }
}
