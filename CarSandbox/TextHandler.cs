using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Handles player UI
public class TextHandler : MonoBehaviour
{
    // https://forum.unity.com/threads/using-gameobject-find.523066/#:~:text=Find%20is%20just%20bad%20is,again%20break%20a%20Find%20call.
    // https://starmanta.gitbooks.io/unitytipsredux/content/first-question.html
    // Vars for handling UI
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject SpeedTMPObject;
    [SerializeField] private GameObject FlipPromptTMPObject;
    [SerializeField] private GameObject InfoTMPObject;
    [SerializeField] private GameObject ToggleInfoButtonObject;
    [SerializeField] private GameObject ToggleExpModeButtonObject;
    private double currSpeed;
    private double speedConversionFactor = 1.5;

    void Awake() {
        if(!canvas.activeSelf) {
            canvas.SetActive(true);
        }
    }

    void FixedUpdate() {
        currSpeed = GetSpeedActualSpeed() * speedConversionFactor;
        SpeedTMPObject.GetComponent<TextMeshProUGUI>().text = $"Speed: {currSpeed.ToString("F1")} mph";

        HandleUpsideDown();
    }
 
    private double GetSpeedActualSpeed() {
        // Object reference not set to an instance of an object
        return CarManager._rbCar.velocity.magnitude;
    }

    private void HandleUpsideDown() {
        // If upside down, prompt to flip car
        if(CarManager.upsideDown) {
            FlipPromptTMPObject.SetActive(true);
            return;
        }
        FlipPromptTMPObject.SetActive(false);
    }

    // Toggle info on and off
    public void onToggleInfoButton() {
        TextMeshProUGUI buttonText = ToggleInfoButtonObject.GetComponentInChildren<TextMeshProUGUI>();

        if(InfoTMPObject.activeSelf) {
            InfoTMPObject.SetActive(false);
            buttonText.text = "Show Info";
            return;
        }
        InfoTMPObject.SetActive(true);
        buttonText.text = "Hide Info";
    }

    // Toggle experimental mode
    public void onToggleExperimentalModeButton() {
        TextMeshProUGUI buttonText = ToggleExpModeButtonObject.GetComponentInChildren<TextMeshProUGUI>();

        if(CarManager.experimentalMode) {
            CarManager.experimentalMode = false;
            buttonText.text = "Experimental Mode is off";
            return;
        }
        CarManager.experimentalMode = true;
        buttonText.text = "Experimental Mode is on";
        return;
    }
}
