using System.Collections;
using System.Collections.Generic;
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
    private Rigidbody _rbCar;

    public CarSelection chosenCar;

    private bool carActive;
    // If car has flipped (share with other scripts --> public)
    public static bool upsideDown;
    public static bool experimentalMode;

    private void Start() {
        experimentalMode = false;
        _rbCar = GetComponent<Rigidbody>();
        engineAudio = GetComponent<AudioSource>();

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

    private void HandleMovement() {
        // Will add force relative to the X direction of the rigidbody
        // _speed determines magnitude of the force
        if(Input.GetKey(KeyCode.W) && !upsideDown) {
            carActive = true;
            _rbCar.AddRelativeForce(Vector3.forward * chosenCar.speed);
            ChangeVolume(engineAudio, 1.0f, audioIncreaseSpeed_engine);
        }
        else if (Input.GetKey(KeyCode.S) && !upsideDown) {
            carActive = true;
            _rbCar.AddRelativeForce(Vector3.back * chosenCar.speed);
            ChangeVolume(engineAudio, 1.0f, audioIncreaseSpeed_engine);
        }
        else {
            carActive = false;
            ChangeVolume(engineAudio, 0.0f, audioIncreaseSpeed_engine);
        }

        // Will add a torque to rotate the rb in the clockwise direction
        // _steeringSpeed determines magnitude of the torque
        if(Input.GetKey(KeyCode.D) && !upsideDown && carActive) {
            if(Input.GetKey(KeyCode.W)) {
                _rbCar.AddTorque(Vector3.up * chosenCar.steeringSpeed);
            }
            else if (Input.GetKey(KeyCode.S)) {
                _rbCar.AddTorque(Vector3.down * chosenCar.steeringSpeed);
            }
            DampenVelocity();
        }
        else if(Input.GetKey(KeyCode.A) && !upsideDown && carActive) {
            if(Input.GetKey(KeyCode.W)) {
                _rbCar.AddTorque(Vector3.down * chosenCar.steeringSpeed);
            }
            else if (Input.GetKey(KeyCode.S)) {
                _rbCar.AddTorque(Vector3.up * chosenCar.steeringSpeed);
            }
            DampenVelocity();
        }

        // Flip rightside up if upside down and if not too high
        if(upsideDown && Input.GetKey(KeyCode.F)) {
            MakeVehicleUpright();
        }
    }

    // Stops car from skidding too far; drag can only do so much...
    private void DampenVelocity() {
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

    private void MakeVehicleUpright() {
        // Handle movement; Lerp between current and targetted position
        // Vector3 smoothedPosition = Vector3.Lerp(transform.position, transform.position + new Vector3(0,5,0), _LerpTime * Time.deltaTime);
        transform.position += new Vector3(0,2,0);

        // Handle rotation
        transform.Rotate(new Vector3(0,0,-transform.rotation.eulerAngles.z));
    }

    /* Converts a Vector3 of euler angles to Quaternion */
    private Quaternion Vec3toQuat(Vector3 v) {
        return Quaternion.Euler(v.x, v.y, v.z);
    }
}
