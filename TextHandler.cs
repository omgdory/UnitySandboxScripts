using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextHandler : MonoBehaviour
{
    // Reference to CarManager script (for upsideDown variable)
    private CarManager carManager;

    // Reference to player
    private GameObject player;
    private Rigidbody rbPlayer;

    // Vars for handling UI
    [SerializeField] private GameObject canvas;
    private GameObject SpeedTextMeshObject;
    private GameObject FlipPromptTextMeshObject;
    private GameObject InfoTextMeshObject;
    private GameObject ToggleInfoButtonObject;
    private double currSpeed;
    private double speedConvert = 1.5;

    // Gotten from CarManager script (external reference)
    private bool upsideDown;

    void Start() {
        if(!canvas.activeSelf) {
            canvas.SetActive(true);
        }
        
        // Find appropriate objects
        canvas = GameObject.Find("Canvas");
        SpeedTextMeshObject = GameObject.Find("Canvas/UI_Speed");
        FlipPromptTextMeshObject = GameObject.Find("Canvas/UI_FlipPrompt");
        InfoTextMeshObject = GameObject.Find("Canvas/UI_Info");
        ToggleInfoButtonObject = GameObject.Find("Canvas/ToggleInfoButton");

        // Disable flip prompt
        FlipPromptTextMeshObject.SetActive(false);

        // Set up player variables
        player = GameObject.Find("PLAYER");
        carManager = player.GetComponent<CarManager>();
        rbPlayer = player.GetComponent<Rigidbody>();
    }

    void FixedUpdate() {
        upsideDown = carManager.upsideDown;

        currSpeed = GetSpeed() * speedConvert;
        SpeedTextMeshObject.GetComponent<TextMeshProUGUI>().text = $"Speed: {currSpeed.ToString("F1")} mph";

        HandleUpsideDown();
    }

    private double GetSpeed() {
        // Object reference not set to an instance of an object
        double result = rbPlayer.velocity.magnitude;
        if(result <= 0.1)
            return 0.0;
        return result;
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
