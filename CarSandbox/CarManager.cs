using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// This file handles everything related to the player i.e. the car
public class CarManager : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _steeringSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxSteeringSpeed;

    [Tooltip("How much velocity is needed until dampening begins to occur")]
    [SerializeField] private float dampenThreshold;

    private AudioSource engineAudio;
    [Tooltip("How fast the engine volume will increase")]
    [SerializeField] private float audioIncreaseSpeed_engine;

    private Vector3 modelRotationOffset;
    private Rigidbody _rbCar;

    // If car has flipped (share with other scripts --> public)
    public bool upsideDown;

    private void Start() {
        _rbCar = GetComponent<Rigidbody>();
        engineAudio = GetComponent<AudioSource>();

        modelRotationOffset = new Vector3(0, 90, 0);
    }

    private void FixedUpdate() {
        // Do not exceed max speeds
        _rbCar.velocity = Vector3.ClampMagnitude(_rbCar.velocity, _maxSpeed);
        _rbCar.angularVelocity = Vector3.ClampMagnitude(_rbCar.angularVelocity, _maxSteeringSpeed);
        // Call appropriate functions
        CheckUpsideDown();
        HandleMovement();
    }

    private void HandleMovement() {
        // Will add force relative to the X direction of the rigidbody
        // _speed determines magnitude of the force
        if(Input.GetKey(KeyCode.W) && !upsideDown) {
            _rbCar.AddRelativeForce(Vector3.forward * _speed);
            ChangeVolume(engineAudio, 1.0f, audioIncreaseSpeed_engine);
        }
        else if (Input.GetKey(KeyCode.S) && !upsideDown) {
            _rbCar.AddRelativeForce(Vector3.back * _speed);
            ChangeVolume(engineAudio, 1.0f, audioIncreaseSpeed_engine);
        }
        else {
            ChangeVolume(engineAudio, 0.0f, audioIncreaseSpeed_engine);
        }

        // Will add a torque to rotate the rb in the clockwise direction
        // _steeringSpeed determines magnitude of the torque
        if(Input.GetKey(KeyCode.D) && !upsideDown) {
            if(Input.GetKey(KeyCode.W)) {
                _rbCar.AddTorque(Vector3.up * _steeringSpeed);
            }
            else if (Input.GetKey(KeyCode.S)) {
                _rbCar.AddTorque(Vector3.down * _steeringSpeed);
            }
            DampenVelocity();
        }
        else if(Input.GetKey(KeyCode.A) && !upsideDown) {
            if(Input.GetKey(KeyCode.W)) {
                _rbCar.AddTorque(Vector3.down * _steeringSpeed);
            }
            else if (Input.GetKey(KeyCode.S)) {
                _rbCar.AddTorque(Vector3.up * _steeringSpeed);
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
        if(localVelocity.z > dampenThreshold) {
            _rbCar.AddRelativeForce(new Vector3(0,0,dampenThreshold));
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
        transform.Rotate(new Vector3(0,0,180));
    }
}
