using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextHandler : MonoBehaviour
{
    // Reference to CarManager script (for upsideDown variable)
    private CarManager carManager;

    // Reference to player
    GameObject player;

    // Vars for handling UI
    [SerializeField] private GameObject canvas;
    private GameObject targetObject;
    private TextMeshProUGUI UI_Speed;
    private GameObject FlipPromptTextMeshObject;
    private double currSpeed;
    private double speedConvert = 1.5;

    // Gotten from CarManager script (external reference)
    private bool upsideDown;

    void Start() {
        targetObject = GameObject.Find("PLAYER");
        if(!canvas.activeSelf)
            canvas.SetActive(true);
        UI_Speed = GameObject.Find("Canvas/UI_Speed").GetComponent<TextMeshProUGUI>();
        FlipPromptTextMeshObject = GameObject.Find("Canvas/UI_FlipPrompt");
        FlipPromptTextMeshObject.SetActive(false);
        canvas = GameObject.Find("Canvas");

        player = GameObject.Find("PLAYER");
        carManager = player.GetComponent<CarManager>();
    }

    void FixedUpdate() {
        upsideDown = carManager.upsideDown;

        currSpeed = GetSpeed() * speedConvert;
        UI_Speed.text = $"Speed: {currSpeed.ToString("F1")} mph";

        HandleUpsideDown();
    }

    private double GetSpeed() {
        // Object reference not set to an instance of an object
        double result = targetObject.GetComponent<Rigidbody>().velocity.magnitude;
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
}
