using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_player : MonoBehaviour
{

    [SerializeField] private GameObject _playerPrefab;
    private bool playerExists = false;
    private int numChars = 0;

    private GameObject player;

    private void Update() {
        if(playerExists == false && Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // Set to proper render layer
            mousePos.z = -1;
            
            player = Instantiate(_playerPrefab, mousePos, Quaternion.identity);
            player.transform.parent = GameObject.FindGameObjectWithTag("gameSwag").transform;
            numChars++;
            player.name = "Skellytron " + numChars;
            playerExists = true;
        }
    }
}
