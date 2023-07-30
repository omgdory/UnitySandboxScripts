using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RacingMenuManager : MonoBehaviour
{
    [SerializeField] private GameObject carPrefab_Purple;
    [SerializeField] private Transform display;
    private GameObject displayCar;
    private Vector3 displayCarOffset_pos;
    private Vector3 displayCarOffset_rot;

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject settingsButton;
    [SerializeField] private GameObject creditsButton;
    [SerializeField] private GameObject quitButton;

    [SerializeField] private float rotateDisplaySpeed;
    [SerializeField] private bool rotateDisplayClockwise;

    void Start()
    {
        displayCarOffset_pos = display.transform.position;
        displayCarOffset_rot = new Vector3(0,90,0);
        CreateDisplayCar(displayCarOffset_pos, displayCarOffset_rot);
    }

    void FixedUpdate()
    {
        RotateTransform(displayCar.transform, rotateDisplaySpeed, rotateDisplayClockwise);
    }

    /* Function to rotate a transform
        @param obj Transform to rotate
        @param speed Speed at which to rotate
        @param clockwise Whether or not it will rotate clockwise relative to the global y axis.
        Set to true by default. */
    private void RotateTransform(Transform obj, float speed, bool clockwise = true) {
        float cw = clockwise ? 1.0f : -1.0f;
        Quaternion targetRotation = obj.rotation * Quaternion.Euler(0, 1.0f * cw , 0);
        obj.rotation = Quaternion.Lerp(obj.rotation, targetRotation, speed * Time.deltaTime);
    }

    private void CreateDisplayCar(Vector3 positionOffset, Vector3 rotationOffset) {
        if(displayCar != null) {
            Destroy(displayCar);
        }
        // https://discussions.unity.com/t/how-can-i-instantiate-a-gameobject-directly-into-another-gameobject-as-child/86294
        displayCar = Instantiate(carPrefab_Purple);
        displayCar.transform.parent = display;

        displayCar.transform.position += positionOffset;
        displayCar.transform.rotation *= Quaternion.Euler(rotationOffset);
    }

    public void onPlayButton() {
        // Start game -- go to next scene in build index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void onSettingsButton() {
        // TO DO
    }

    public void onCreditsButton() {
        // TO DO
    }

    public void onQuitButton() {
        Application.Quit();
    }
}
