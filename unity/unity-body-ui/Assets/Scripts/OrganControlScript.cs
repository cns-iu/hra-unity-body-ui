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

    [Header("CamOffset")]
    public Vector3 _camOffset;

    [Header("Rotation Radius")]
    public float radius = 1;

    //private vars
    private Vector3 myObjectStartPosition, myMouseStartWorldPosition;

    private bool hovering = false;
    private bool rotating = false;
    private bool translating = false;

    private bool isRotating = false;
    private bool isTranslating = false;


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

                _camOffset = myMainCamera.transform.position - transform.position;

                rotating= true;
            }
            else if (Input.GetMouseButtonDown(1))
            {
                //Trigger the mouse down output in the JS Bridge
                //jsBridge.GetNodeClick();

                translating = true;
            }

            if (Input.GetMouseButton(0) && rotating)
            {
                //Trigger the mouse down output in the JS Bridge
                //jsBridge.GetNodeDrag();

                //rotate the cam around the obj


                Quaternion camTurnAngle =
                    Quaternion.AngleAxis((Input.GetAxis("Mouse X") + Input.GetAxis("Mouse Y")) * rotationSpeed, Vector3.up);

                _camOffset = camTurnAngle * _camOffset;

                Vector3 newPos = this.transform.position + _camOffset;

                myMainCamera.transform.position = Vector3.Slerp(myMainCamera.transform.position, newPos, smoothFactor);

                myMainCamera.transform.LookAt(this.transform.position);
            }
            else if (Input.GetMouseButton(1) && translating)
            {
                //Trigger the mouse down output in the JS Bridge
                //jsBridge.GetNodeDrag();

                Debug.Log("Here");

                Vector3 newPos = myMainCamera.transform.position + Vector3.right * Input.GetAxis("Mouse X");  

                myMainCamera.transform.position = Vector3.Slerp(myMainCamera.transform.position, newPos, smoothFactor);
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
}
