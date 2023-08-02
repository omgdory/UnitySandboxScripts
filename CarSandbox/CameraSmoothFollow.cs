using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSmoothFollow : MonoBehaviour
{
    // https://www.youtube.com/watch?v=MFQhpwc6cKE&t=59s
    private Vector3 TargetFwd_Offset = new Vector3(15,0,0);
    private Vector3 TargetBack_Offset = new Vector3(15,180,0);
    
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
            HandleCameraTargetForward();
        }
        else if(cameraSelection == 1) {
            HandleCameraTargetBackward();
        }
    }

    void HandleCameraTargetForward() {
        // Handle movement; Lerp between current and targetted position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, CarManager.CamTarget_fwd.position, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        // Handle rotation; multiply offset by target rotation
        transform.rotation = CarManager.CamTarget_fwd.rotation * Quaternion.Euler(TargetFwd_Offset);
    }

    void HandleCameraTargetBackward() {
        // Handle movement; Lerp between current and targetted position
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, CarManager.CamTarget_back.position, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
        // Handle rotation; multiply offset by target rotation
        transform.rotation = CarManager.CamTarget_back.rotation * Quaternion.Euler(TargetBack_Offset);
    }
}
