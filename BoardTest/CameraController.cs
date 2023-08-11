using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraRigTransform;
    [SerializeField] private Transform cameraTransform; // for camera (child) of the camera rig

    [SerializeField] private float closestY;
    [SerializeField] private float farthestY;

    [SerializeField] private float movementSpeed;
    [SerializeField] private float zoomSpeed;

    private Vector3 newRigPosition; // position for rig to be after new frame
    private Vector3 newCameraPosition; // position for camera after new frame


    // Instantiate newPosition vectors as start positions of each object
    void Start()
    {
        newRigPosition = cameraRigTransform.position;
        newCameraPosition = cameraTransform.localPosition;
    }

    void Update()
    {
        HandleCameraMovementInput();
    }

    void FixedUpdate()
    {
        HandleCameraPhysics();
    }



    private void HandleCameraMovementInput()
    {
        // Right click
        if (Input.GetMouseButton(1))
        {
            if(Input.GetAxis("Mouse X") != 0)
            {
                float xMovement = Input.GetAxisRaw("Mouse X") * movementSpeed;
                float zMovement = Input.GetAxisRaw("Mouse Y") * movementSpeed;
                newRigPosition += new Vector3(-xMovement, 0.0f, -zMovement); // Move opposite to mouse movement, so negate
            }
        }


        // Zoom in/out; moving camera relative to parent (camera rig)
        if(Input.mouseScrollDelta != Vector2.zero) // account for zoom limit (y=10)
        {
            float yMovement = Input.mouseScrollDelta.y * zoomSpeed;

            // Check limits before updating
            // Note: yMovement will be negated, so check the opposite
            if (!(cameraTransform.localPosition.y < closestY && yMovement > 0) &&
                !(cameraTransform.localPosition.y > farthestY && yMovement < 0))
            {
                float zMovement = Input.mouseScrollDelta.y * zoomSpeed;

                newCameraPosition += new Vector3(0.0f, -yMovement, zMovement); // y goes in opposite direction, so negate
                
                // A bit choppy, but will work for now...
            }
        }
    }

    private void HandleCameraPhysics() {
        // Lerp to move smoothly
        cameraTransform.localPosition = Vector3.Lerp(cameraTransform.localPosition, newCameraPosition, 1.0f);
        cameraRigTransform.position = Vector3.Lerp(cameraRigTransform.position, newRigPosition, 1.0f);
    }
}
