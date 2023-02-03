using System;
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

    [Header("Rotation Speed")]
    public float rotationSpeed = 1;

    [Header("Rotation smoothness")]
    public float smoothFactor = 1;

    [Header("Translate speed")]
    public float translateSpeed = 1;

    [Header("CamOffset")]
    public Vector3 _camOffset;

    [Header("Rotation Radius")]
    public float radius = 1;

    [Header("Alpha value")]
    public float alpha = 0.5f;

    [Header("Organ List")]
    public List<GameObject> organList;

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

    private void Start()
    {
        _initRotation = transform.rotation;
        _initPosition = transform.position;

        FindLeafChildren(this.transform);

        //search through the object and set all opacity
        foreach (var item in leafChildren)
        {
            Renderer renderer = item.GetComponent<MeshRenderer>();

            if (renderer == null) continue;
            Color updatedColor = renderer.material.color;
            updatedColor.a = alpha;
            renderer.material.color = updatedColor;

            Shader standard;
            standard = Shader.Find("Standard");
            renderer.material.shader = standard;
            MaterialExtensions.ToFadeMode(renderer.material);
        }
    }

    public void IsolateOrgan(int t)
    {
        foreach(GameObject organ in organList)
        {
            organ.SetActive(false);
        }

        organList[t].SetActive(true);
    }

    public void Reset()
    {
        this.transform.rotation = _initRotation;
        this.transform.position = _initPosition;
    }

    public void FindLeafChildren(Transform trans)
    {
        Transform[] allChildren = trans.GetComponentsInChildren<Transform>();
        foreach (var child in allChildren) 
        {
            if (child.childCount == 0)
            {
                Debug.Log(child.gameObject);
                leafChildren.Add(child);
            }
        }
    }

    public void TurnOff()
    {
        SetOrgans(false);
    }

    public void TurnOn()
    {
        SetOrgans(true);
    }

    public void SetOrgans(bool _bool)
    {
        foreach (Transform leaf in leafChildren)
        {
            leaf.gameObject.SetActive(_bool);
        }
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

            this.transform.RotateAround(this.transform.position, _n, -dr.magnitude / radius);

            myMouseStartWorldPosition = lMousePosition;
        }
        else if (Input.GetMouseButton(1) && translating)
        {
            //Trigger the mouse down output in the JS Bridge
            //jsBridge.GetNodeDrag();

            Vector3 newPos = transform.position + (Vector3.right * Input.GetAxis("Mouse X"));

            this.transform.position = Vector3.Lerp(transform.position, newPos, translateSpeed);
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
