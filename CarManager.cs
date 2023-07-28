using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _steeringSpeed;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private float _maxSteeringSpeed;

    private AudioSource engineAudio;
    // How fast it would take for volume to go from beginning to target
    [SerializeField] private float audioIncreaseSpeed;

    // Model's Vector3.up is not actually up...
    private Vector3 modelRotationOffset;

    // How long Lerp will take
    // private float _LerpTime = 10f;

    private Rigidbody _rbCar;

    // If car has flipped (share with other scripts --> public)
    public bool upsideDown;
    // Upwards rotation (for if the car is flipped)
    // private Quaternion initialRotation = Quaternion.identity;

    private void Start() {
        _rbCar = GetComponent<Rigidbody>();
        engineAudio = GetComponent<AudioSource>();

        modelRotationOffset = new Vector3(0, 90, 0);
    }

    private void FixedUpdate() {
        // Do not exceed max speeds
        _rbCar.velocity = Vector3.ClampMagnitude(_rbCar.velocity, _maxSpeed);
        _rbCar.angularVelocity = Vector3.ClampMagnitude(_rbCar.angularVelocity, _maxSteeringSpeed);
        // call appropriate functions
        CheckUpsideDown();
        HandleMovement();
    }

    private void HandleMovement() {
        // Will add force relative to the X direction of the rigidbody
        // _speed determines magnitude of the force
        if(Input.GetKey(KeyCode.W) && !upsideDown) {
            _rbCar.AddRelativeForce(Vector3.forward * _speed);
            HandleSound(1.0f);
        }
        else if (Input.GetKey(KeyCode.S) && !upsideDown) {
            _rbCar.AddRelativeForce(Vector3.back * _speed);
            HandleSound(1.0f);
        }
        else {
            HandleSound(0.0f);
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
        }
        else if(Input.GetKey(KeyCode.A) && !upsideDown) {
            if(Input.GetKey(KeyCode.W)) {
                _rbCar.AddTorque(Vector3.down * _steeringSpeed);
            }
            else if (Input.GetKey(KeyCode.S)) {
                _rbCar.AddTorque(Vector3.up * _steeringSpeed);
            }
        }

        // Flip rightside up if upside down and if not too high
        if(upsideDown && Input.GetKey(KeyCode.F)) {
            MakeVehicleUpright();
        }
    }

    private void HandleSound(float targetVolume) {
        float volumeLevel = engineAudio.volume;
        engineAudio.volume = Mathf.Lerp(volumeLevel, targetVolume, audioIncreaseSpeed * Time.deltaTime);
    }

    private void CheckUpsideDown() {
        //https://discussions.unity.com/t/how-to-check-if-an-object-is-upside-down/143397
        // If vehicle's up-vector has a down facing component
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

        // Handle rotation -- TO DO
        // Quaternion smoothedRotation = Quaternion.Lerp(Quaternion.identity, initialRotation, _LerpTime * Time.deltaTime);
        // Vector3 initialForward = transform.forward;
        // Quaternion targetRotation = Quaternion.FromToRotation(transform.up, Vector3.up) * transform.rotation;
        // transform.rotation = targetRotation;
        transform.Rotate(new Vector3(0,0,180));
    }
}
