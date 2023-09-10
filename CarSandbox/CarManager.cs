using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using UnityEngine;


// This file handles everything related to the player i.e. the car
public class CarManager : MonoBehaviour
{
    // Reference menu script for car selection
    private RacingMenuManager.carData selectedCarData;

    [System.Serializable]
    public class CarSelection {
        public float speed;
        public float steeringSpeed;
        public float maxSpeed;
        public float maxSteeringSpeed;
        public float dampenThreshold;

        public CarSelection(float spd, float steerSpd, float maxSpd, float maxSteerSpd, float dmpnThrshld) {
            speed = spd;
            steeringSpeed = steerSpd;
            maxSpeed = maxSpd;
            maxSteeringSpeed = maxSteerSpd;
            dampenThreshold = dmpnThrshld;
        }
    }

    public static Transform CamTarget_fwd;
    public static Transform CamTarget_back;

    // Dictionary of stats for each car prefab
    private Dictionary<string, CarSelection> modelStatistics = new Dictionary<string, CarSelection>() {
        {"Car_Purple", new CarSelection(20, 25, 30, 1, 1)},
        {"Truck_Red", new CarSelection(20, 45, 30, 1, 1)},
        {"Bus_Blue", new CarSelection(20, 45, 30, 1, 1)}
    };

    private AudioSource engineAudio;
    [Tooltip("How fast the engine volume will increase")]
    [SerializeField] private float audioIncreaseSpeed_engine;

    private GameObject playerModel;
    private Vector3 modelRotationOffset;
    public static Rigidbody _rbCar;

    public CarSelection chosenCar;

    // So that input can be called in Update but physics handled in FixedUpdate
    public enum MoveKey {
        W,
        A,
        S,
        D,
        F
    }
    // Get amount of keys
    public static int MoveKey_amount = Enum.GetValues(typeof(MoveKey)).Length;
    // Array for which keys are being held
    bool[] heldKey = new bool[MoveKey_amount];

    // If car has flipped (share with other scripts --> public)
    public static bool upsideDown;
    public static bool experimentalMode;

    private void Awake() {
        experimentalMode = false;
        _rbCar = GetComponent<Rigidbody>();
        engineAudio = GetComponent<AudioSource>();

        // Spawn camera targets
        CamTarget_fwd = CreateEmptyGameObject("CamTarget_fwd", new Vector3(0,7,-15));
        CamTarget_back = CreateEmptyGameObject("CamTarget_back", new Vector3(0,7,15));

        modelRotationOffset = new Vector3(0, -90, 0);

        selectedCarData = RacingMenuManager.currentSelection.Value;
        // Set up player model
        playerModel = selectedCarData.prefab;
        GameObject obj = Instantiate(playerModel, Vector3.zero, transform.rotation * Vec3toQuat(modelRotationOffset));
        obj.transform.parent = transform;
        obj.transform.localPosition = Vector3.zero;

        // Select car that was chosen
        string chosenName = selectedCarData.name;
        chosenCar = modelStatistics[chosenName];
    }

    private void FixedUpdate() {
        // Do not exceed max speeds if not in experimental mode
        if(!experimentalMode) {
            _rbCar.velocity = Vector3.ClampMagnitude(_rbCar.velocity, chosenCar.maxSpeed);
            _rbCar.angularVelocity = Vector3.ClampMagnitude(_rbCar.angularVelocity, chosenCar.maxSteeringSpeed);
        }
        // Call appropriate functions
        CheckUpsideDown();
        HandleMovement();
    }

    private void Update() {
        HandleMovementInput();
    }

    /* Creates an empty GameObject as a child of the object this script is attached to
        @param name Name that will be given to the GameObject
        @param postion Position of the GameObject relative to the parent
        @return Transform variable of which to assign the empty GameObject
    */
    private Transform CreateEmptyGameObject(string name, Vector3 position) {
        Transform var;
        var = new GameObject($"{name}").transform;
        var.parent = transform;
        var.localPosition = position;

        return var;
    }

    // Handle movement physics (in FixedUpdate)
    private void HandleMovement() {
        if(heldKey[(int)MoveKey.W] && !upsideDown) {
            _rbCar.AddRelativeForce(Vector3.forward * chosenCar.speed);
            ChangeVolume(engineAudio, 1.0f, audioIncreaseSpeed_engine);
            if(heldKey[(int)MoveKey.D]) {
                // clockwise relative to object's Y axis
                _rbCar.AddRelativeTorque(Vector3.up * chosenCar.steeringSpeed);
            } else if (heldKey[(int)MoveKey.A]) {
                // counter clockwise relative to object's Y axis
                _rbCar.AddRelativeTorque(Vector3.down * chosenCar.steeringSpeed);
            }
            DampenHorizontalVelocity();
        }
        else if (heldKey[(int)MoveKey.S] && !upsideDown) {
            _rbCar.AddRelativeForce(Vector3.back * chosenCar.speed);
            ChangeVolume(engineAudio, 1.0f, audioIncreaseSpeed_engine);
            if(heldKey[(int)MoveKey.D]) {
                // clockwise relative to object's Y axis
                _rbCar.AddRelativeTorque(Vector3.down * chosenCar.steeringSpeed);
            } else if (heldKey[(int)MoveKey.A]) {
                // counter clockwise relative to object's Y axis
                _rbCar.AddRelativeTorque(Vector3.up * chosenCar.steeringSpeed);
            }
            DampenHorizontalVelocity();
        }
        else {
            ChangeVolume(engineAudio, 0.0f, audioIncreaseSpeed_engine);
        }

        // Flip rightside up if upside down
        if(upsideDown && heldKey[(int)MoveKey.F]) {
            transform.position += new Vector3(0,2,0);
            // Handle rotation
            transform.Rotate(new Vector3(0,0,-transform.rotation.eulerAngles.z));
        }
    }

    // Handle movement input (in Update)
    private void HandleMovementInput() {
        foreach(var k in Enum.GetNames(typeof(MoveKey))) {
            // Ensure that movement key is defined in Unity
            if(Enum.IsDefined(typeof(KeyCode), k)) {
                // https://www.loginradius.com/blog/engineering/enum-csharp/
                KeyCode kc = (KeyCode)Enum.Parse(typeof(KeyCode), k);
                if (Input.GetKeyDown(kc)) {
                    heldKey[(int)Enum.Parse(typeof(MoveKey), k)] = true;
                } else if (Input.GetKeyUp(kc)) {
                    heldKey[(int)Enum.Parse(typeof(MoveKey), k)] = false;
                }
            } else {
                Debug.Log($"Key {k} does not exist in KeyCode enum. Remove it!");
            }
        }

        
        if (Input.GetKeyDown(KeyCode.A)) {
            heldKey[(int)MoveKey.A] = true;
        } else if (Input.GetKeyUp(KeyCode.A)) {
            heldKey[(int)MoveKey.A] = false;
        }
        if (Input.GetKeyDown(KeyCode.A)) {
            heldKey[(int)MoveKey.A] = true;
        } else if (Input.GetKeyUp(KeyCode.A)) {
            heldKey[(int)MoveKey.A] = false;
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            heldKey[(int)MoveKey.S] = true;
        } else if (Input.GetKeyUp(KeyCode.S)) {
            heldKey[(int)MoveKey.S] = false;
        }
        if (Input.GetKeyDown(KeyCode.D)) {
            heldKey[(int)MoveKey.D] = true;
        } else if (Input.GetKeyUp(KeyCode.D)) {
            heldKey[(int)MoveKey.D] = false;
        }
        if (Input.GetKeyDown(KeyCode.F)) {
            heldKey[(int)MoveKey.F] = true;
        } else if (Input.GetKeyUp(KeyCode.F)) {
            heldKey[(int)MoveKey.F] = false;
        }
    }

    // Stops car from skidding too far; drag can only do so much...
    private void DampenHorizontalVelocity() {
        // https://discussions.unity.com/t/velocity-relative-to-local-axis/32800
        // Get velocity vector that is relative to the car's body
        Vector3 localVelocity = transform.InverseTransformDirection(_rbCar.velocity);
        // Slow down horizontally by counter
        if(localVelocity.z > chosenCar.dampenThreshold) {
            _rbCar.AddRelativeForce(new Vector3(0,0,chosenCar.dampenThreshold));
        }
    }

    // Changes "src" volume to "targetVolume" over "lerpTime" seconds
    private void ChangeVolume(AudioSource src, float targetVolume, float lerpTime = 0.0f) {
        float volumeLevel = src.volume;
        src.volume = Mathf.Lerp(volumeLevel, targetVolume, lerpTime * Time.deltaTime);
    }

    private void CheckUpsideDown() {
        //https://discussions.unity.com/t/how-to-check-if-an-object-is-upside-down/143397
        // If vehicle's up-vector has a down facing component
        // as "transform.up" points down, dot product with "Vector3.down" will be greater negative number
        // Debug.Log((transform.up, ' ', Vector3.down));
        if (Vector3.Dot(transform.up, Vector3.down) > -0.5) {
            upsideDown = true;
        } else {
            upsideDown = false;
        }
    }

    /* Converts a Vector3 of euler angles to Quaternion */
    private Quaternion Vec3toQuat(Vector3 v) {
        return Quaternion.Euler(v.x, v.y, v.z);
    }
}
