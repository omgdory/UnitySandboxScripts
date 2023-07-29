using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextHandler : MonoBehaviour
{

    // Reference to player
    [SerializeField] private GameObject player;
    private Rigidbody rbPlayer;
    // Reference to CarManager script (for upsideDown variable)
    private CarManager carManager;

    // Vars for handling UI
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject SpeedTextMeshObject;
    [SerializeField] private GameObject FlipPromptTextMeshObject;
    [SerializeField] private GameObject InfoTextMeshObject;
    [SerializeField] private GameObject ToggleInfoButtonObject;
    private double currSpeed;
    private double speedConversionFactor = 1.5;

    // Gotten from CarManager script (external reference)
    private bool upsideDown;

    void Start() {
        if(!canvas.activeSelf) {
            canvas.SetActive(true);
        }

        // Disable flip prompt
        FlipPromptTextMeshObject.SetActive(false);

        // Set up player variables
        carManager = player.GetComponent<CarManager>();
        rbPlayer = player.GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        upsideDown = carManager.upsideDown;

        currSpeed = GetSpeedActualSpeed() * speedConversionFactor;
        SpeedTextMeshObject.GetComponent<TextMeshProUGUI>().text = $"Speed: {currSpeed.ToString("F1")} mph";

        HandleUpsideDown();
    }
 
    private double GetSpeedActualSpeed() {
        // Object reference not set to an instance of an object
        return rbPlayer.velocity.magnitude;
    }

    private void HandleUpsideDown() {
        // If upside down, prompt to flip car
        if(upsideDown) {
            FlipPromptTextMeshObject.SetActive(true);
            return;
        }
        FlipPromptTextMeshObject.SetActive(false);
    }

    // Toggle info on and off
    public void onToggleInfoButton() {
        TextMeshProUGUI buttonText = ToggleInfoButtonObject.GetComponentInChildren<TextMeshProUGUI>();

        if(InfoTextMeshObject.activeSelf) {
            InfoTextMeshObject.SetActive(false);
            buttonText.text = "Show Info";
            return;
        }
        InfoTextMeshObject.SetActive(true);
        buttonText.text = "Hide Info";
    }
}
