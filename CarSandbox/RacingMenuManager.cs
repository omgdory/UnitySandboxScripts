using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RacingMenuManager : MonoBehaviour
{
    public class carData {
        public string name;
        public string displayName;
        public GameObject prefab;

        public carData() {
            this.name = " ";
            this.displayName = " ";
            this.prefab = null;
        }

        public carData(string name, string displayName, GameObject prefab) {
            this.name = name;
            this.displayName = displayName;
            this.prefab = prefab;
        }
    }
    // Car prefabs
    [SerializeField] private GameObject Car_Purple;
    [SerializeField] private GameObject Truck_Red;
    [SerializeField] private GameObject Bus_Blue;

    private LinkedList<carData> carList = new LinkedList<carData>();
    // public -- will be accessed in CarManager.cs
    public static LinkedListNode<carData> currentSelection;

    [SerializeField] private Transform display;
    private GameObject displayCar;
    private Vector3 displayCarOffset_pos;
    private Vector3 displayCarOffset_rot;

    [SerializeField] private float rotateDisplaySpeed;
    [SerializeField] private bool rotateDisplayClockwise;

    [SerializeField] private Transform carNameObject;
    private TextMeshProUGUI carNameText;

    void Start()
    {
        displayCarOffset_pos = display.transform.position;
        displayCarOffset_pos += new Vector3(-2, 0, 2);
        displayCarOffset_rot = new Vector3(0,-150,0);

        InitializeSelectionList(carList);
        // Default selection is the purple car
        currentSelection = carList.First;
        CreateDisplayCar(currentSelection.Value.prefab, displayCarOffset_pos, displayCarOffset_rot);
        carNameText = carNameObject.GetComponent<TextMeshProUGUI>();
        carNameText.text = currentSelection.Value.displayName;
    }

    void FixedUpdate()
    {
        RotateTransform(displayCar.transform, rotateDisplaySpeed, rotateDisplayClockwise);
    }

    private void InitializeSelectionList(LinkedList<carData> myList) {
        while(myList.Count != 0) {
            myList.RemoveLast();
        }
        myList.AddLast(new carData("Car_Purple", "Urple", Car_Purple));
        myList.AddLast(new carData("Truck_Red", "Schmucklenuts-mobile", Truck_Red));
        myList.AddLast(new carData("Bus_Blue", "Ambata", Bus_Blue));
    }

    /* Call in FixedUpdate; rotates a transform (who would've guessed!)
        @param obj Transform to rotate
        @param speed Speed at which to rotate
        @param clockwise Whether or not it will rotate clockwise relative to the global y axis.
        True by default. */
    private void RotateTransform(Transform obj, float speed, bool clockwise = true) {
        float cw = clockwise ? 1 : -1;
        Quaternion targetRotation = obj.rotation * Quaternion.Euler(0, 1 * cw , 0);
        obj.rotation = Quaternion.Lerp(obj.rotation, targetRotation, speed * Time.deltaTime);
    }

    private void CreateDisplayCar(GameObject prefab, Vector3 positionOffset, Vector3 rotationOffset) {
        if(displayCar != null) {
            Destroy(displayCar);
        }
        // https://discussions.unity.com/t/how-can-i-instantiate-a-gameobject-directly-into-another-gameobject-as-child/86294
        displayCar = Instantiate(prefab);
        displayCar.transform.parent = display;

        displayCar.transform.position += positionOffset;
        displayCar.transform.rotation = Quaternion.Euler(Vector3.zero);
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

    public void onLeftSelectButton() {
        Quaternion currRotation = displayCar.transform.rotation;
        // see link in "onRightSelectButton"
        currentSelection = currentSelection.Previous ?? carList.Last;
        CreateDisplayCar(currentSelection.Value.prefab, displayCarOffset_pos, currRotation.eulerAngles);
        carNameText.text = currentSelection.Value.displayName;
    }

    public void onRightSelectButton() {
        Quaternion currRotation = displayCar.transform.rotation;
        // https://stackoverflow.com/questions/1028274/is-the-linkedlist-in-net-a-circular-linked-list
        currentSelection = currentSelection.Next ?? carList.First;
        CreateDisplayCar(currentSelection.Value.prefab, displayCarOffset_pos, currRotation.eulerAngles);
        carNameText.text = currentSelection.Value.displayName;
    }

    // public void onDebugButton() {
    //     Debug.Log("Debugggggggg");
    // }
}
