using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrganControlScript : MonoBehaviour
{
    [Header("JSBridge")]
    [SerializeField] public JSBridge jsBridge;

    [Header("Rotation Speed")]
    [SerializeField] private float _rotationSpeed = 0.3f;

    [Header("Rotation smoothness")]
    [SerializeField] private float _smoothFactor = 1;

    [Header("Translate speed")]
    [SerializeField] private float _translateSpeed = 0.05f;

    [Header("CamOffset")]
    [SerializeField] private Vector3 _camOffset;

    [Header("Rotation Radius")]
    [SerializeField] private float _radius = 1;

    [Header("Alpha value")]
    [SerializeField] private float _alpha = 0.5f;

    [Header("Leaf children")]
    [SerializeField] private List<Transform> _leafChildren;

    //dont need to expose these
    private Vector3 myObjectStartPosition, myMouseStartWorldPosition;

    private bool hovering = false;
    private bool rotating = false;
    private bool translating = false;

    private bool isRotating = false;
    private bool isTranslating = false;

    [SerializeField]  private GameObject _topLevelOrgan;

    private void Start()
    {
        //since the test loader parent is now under an empty get the first child and keep in position 1
        _topLevelOrgan = transform.root.GetChild(0).gameObject;
    }

    //Mouse Enter and Exit Triggers\\
    private void OnMouseEnter()
    {
        //Trigger the mouse down output in the JS Bridge
        //jsBridge.GetNodeHoverStart();

        hovering = true;
    }

    private void OnMouseExit()
    {
        //Trigger the mouse down output in the JS Bridge
        //jsBridge.GetNodeHoverStop();

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
                //jsBridge.GetNodeClick();

                Vector3 lMousePosition = Input.mousePosition;
                myMouseStartWorldPosition = lMousePosition;
                myObjectStartPosition = transform.position;

                rotating = true;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                //Trigger the mouse down output in the JS Bridge
                //jsBridge.GetNodeClick();

                translating = true;
            }
        }

        if (Input.GetMouseButton(0) && rotating)
        {
            //Trigger the mouse down output in the JS Bridge
            //jsBridge.GetNodeDrag();

            //rotate the cam around the obj
            Vector3 lMousePosition = Input.mousePosition;

            Vector3 dr = lMousePosition - myMouseStartWorldPosition;

            Vector3 _n = new Vector3(-dr.y, dr.x, 0);

            _n = _n.normalized;

            _topLevelOrgan.transform.RotateAround(this.transform.position, _n, -dr.magnitude / _radius);

            myMouseStartWorldPosition = lMousePosition;

            _topLevelOrgan.transform.position = Vector3.zero;
        }
        else if (Input.GetMouseButton(1) && translating)
        {
            //Trigger the mouse down output in the JS Bridge
            //jsBridge.GetNodeDrag();

            Vector3 newPos = _topLevelOrgan.transform.position + (Vector3.right * Input.GetAxis("Mouse X")) + (Vector3.up * Input.GetAxis("Mouse Y"));

            _topLevelOrgan.transform.position = Vector3.Lerp(_topLevelOrgan.transform.position, newPos, _translateSpeed);

            Debug.Log(this.name);
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
