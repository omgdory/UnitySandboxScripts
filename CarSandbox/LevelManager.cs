using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static GameObject player;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector3 spawnPostion;
    [SerializeField] private Vector3 spawnRotation;

    [SerializeField] private GameObject levelPrefab;
    public static Transform level;

    void Awake() {
        // Set up level
        level = Instantiate(levelPrefab, Vector3.zero, Quaternion.identity).transform;
        // Spawn player
        player = Instantiate(playerPrefab, spawnPostion, Quaternion.Euler(spawnRotation));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
