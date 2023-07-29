using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// https://www.youtube.com/watch?v=-GWjA6dixV4
public class menuHandler : MonoBehaviour
{
    public void onPlayButton() {
        // Start game -- go to next scene in build index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void onQuitButton() {
        Application.Quit();
    }
}
