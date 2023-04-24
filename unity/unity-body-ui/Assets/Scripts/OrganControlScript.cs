using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganControlScript : MonoBehaviour
{
    [Header("JSBridge")]
    public JSBridge jsBridge;

    [Header("Scene Manager")]
    public SceneSetter sceneSetter;

    [Header("Camera Reference")]
    public Camera myMainCamera;

    [Header("Rotation Speed")]
    public float rotationSpeed = 0.3f;

    [Header("Rotation smoothness")]
    public float smoothFactor = 1;

    [Header("Translate speed")]
    public float translateSpeed = 0.05f;

    [Header("CamOffset")]
    public Vector3 _camOffset;

    [Header("Rotation Radius")]
    public float radius = 1;

    [Header("Alpha value")]
    public float alpha = 0.5f;

    //private vars
    private Vector3 myObjectStartPosition, myMouseStartWorldPosition;

    [SerializeField] List<Transform> leafChildren;

    private bool hovering = false;
    private bool rotating = false;
    private bool translating = false;

    private bool isRotating = false;
    private bool isTranslating = false;

    private Quaternion _initRotation;
    private Vector3 _initPosition;

    private GameObject topLevelOrgan;

    private void Start()
    {
        _initRotation = transform.rotation;
        _initPosition = transform.position;

        //since the test loader parent is now under an empty get the first child and keep in position 1
        topLevelOrgan = transform.root.gameObject.transform.GetChild(0).gameObject;
    }

    public void Reset()
    {
        this.transform.rotation = _initRotation;
        this.transform.position = _initPosition;
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

                Vector3 lMousePosition = Input.mousePosition;
                myMouseStartWorldPosition = lMousePosition;
                myObjectStartPosition = transform.position;

                rotating = true;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                //Trigger the mouse down output in the JS Bridge
                jsBridge.GetNodeClick();

                translating = true;
            }
        }

        if (Input.GetMouseButton(0) && rotating)
        {
            //Trigger the mouse down output in the JS Bridge
            jsBridge.GetNodeDrag();

            //rotate the cam around the obj
            Vector3 lMousePosition = Input.mousePosition;

            Vector3 dr = lMousePosition - myMouseStartWorldPosition;

            Vector3 _n = new Vector3(-dr.y, dr.x, 0);

            _n = _n.normalized;

            topLevelOrgan.transform.RotateAround(this.transform.position, _n, -dr.magnitude / radius);

            myMouseStartWorldPosition = lMousePosition;

            topLevelOrgan.transform.position = Vector3.zero;
        }
        else if (Input.GetMouseButton(1) && translating)
        {
            //Trigger the mouse down output in the JS Bridge
            jsBridge.GetNodeDrag();

            Vector3 newPos = topLevelOrgan.transform.position + (Vector3.right * Input.GetAxis("Mouse X"));

            topLevelOrgan.transform.position = Vector3.Lerp(topLevelOrgan.transform.position, newPos, translateSpeed);
        }

        //reset transition
        if (Input.GetMouseButtonUp(0))
        {
            rotating = false;
        }
        if (Input.GetMouseButtonUp(1))
        {
            translating = false;
        }
    }
}
