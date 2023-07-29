using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    // https://www.youtube.com/watch?v=MFQhpwc6cKE&t=59s

    [SerializeField] private Transform target1;
    [SerializeField] private Transform target2;
    private Vector3 Target1_Offset = new Vector3(15,0,0);
    private Vector3 Target2_Offset = new Vector3(15,180,0);
    
    // speed at which camera will return to position
    private float smoothSpeed = 10f;

    private int cameraSelection = 0;
    private int maxTargets = 2;

    void Update() {
        if(Input.GetKeyDown(KeyCode.Tab)) {
            SwapSelection();
        }
    }

    void FixedUpdate() {
        HandleCamera();
    }

    void SwapSelection() {
        if(cameraSelection >= maxTargets-1) {
            cameraSelection = 0;
            return;
        }
        cameraSelection++;
    }

    void HandleCamera() {
        if(cameraSelection == 0) {
            HandleCameraTarget1();
        }
        else if(cameraSelection == 1) {
            HandleCameraTarget2();
        }
    }

    void HandleCameraTarget1() {
        // Handle movement; Lerp between current and targetted position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target1.position, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        // Handle rotation; multiply offset by target rotation
        transform.rotation = target1.rotation * Quaternion.Euler(Target1_Offset);
    }

    void HandleCameraTarget2() {
        // Handle movement; Lerp between current and targetted position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, target2.position, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        // Handle rotation; multiply offset by target rotation
        transform.rotation = target2.rotation * Quaternion.Euler(Target2_Offset);
    }
}
