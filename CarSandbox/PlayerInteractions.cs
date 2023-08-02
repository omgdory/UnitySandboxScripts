using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    const int LEFTCLICK = 0;
    const int RIGHTCLICK = 0;

    [Tooltip("Prefab that will be spawned")]
    [SerializeField] private GameObject objectToSpawn;
    // [Tooltip("Where to spawn objects")]
    // [SerializeField] private Transform level;

    private Stack<GameObject> spawnedProps;
    private GameObject newestProp; // most recently spawned prop

    private Camera relativeCamera;
    private Transform cameraTransform;

    // https://forum.unity.com/threads/raycast-coming-from-center-of-camera.321510/
    private Ray rayOrigin;
    private RaycastHit hitInfo;

    void Awake() {
        spawnedProps = new Stack<GameObject>();

        relativeCamera = Camera.main; // in case we want to use a different camera later along the line
        cameraTransform = relativeCamera.transform;
    }

    void Update() {
        HandleInput();
    }

    private void HandleInput() {
        // Spawn prop
        if(Input.GetKeyDown(KeyCode.E)) {
            if(Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hitInfo, 100.0f)) {
                newestProp = CreateRelativeObject(transform.gameObject, objectToSpawn, hitInfo.point);
                // newestProp.transform.parent = level;
                spawnedProps.Push(newestProp);
            }
        }
        // Undo
        if(Input.GetKeyDown(KeyCode.Z)) {
            if(spawnedProps.Count != 0) {
                Destroy(newestProp);
                spawnedProps.Pop();
                newestProp = spawnedProps.Count != 0 ? spawnedProps.Peek() : null;
            }
        }
    }

    /* Function to instantiate object relative to another
        @param src Source object
        @param obj Object to spawn
        @param position Position of which to spawn */
    private GameObject CreateRelativeObject(GameObject src, GameObject obj, Vector3 position) {
        Vector3 positionOffset = new Vector3(0, 1, 0);
        GameObject result = Instantiate(obj, position + positionOffset, Quaternion.identity);
        // Match rotation of spawned object to that of src object
        result.transform.Rotate(src.transform.rotation.eulerAngles);
        return result;
    }
}
