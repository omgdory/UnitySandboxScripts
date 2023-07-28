using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class interact_script : MonoBehaviour
{
    // Animation script
    // [SerializeField] private interact_animation myInteractAnimationScript;

    // Text prefab
    [SerializeField] private GameObject _text;
    private TextMeshProUGUI objectTextMesh;

    // If text is already visible
    private bool textShowing = false; // test

    private Coroutine currentTextDisplayRoutine = null;
    private string textClicked;
    private string currentText;
    
    private void Start()
    {
        objectTextMesh = _text.GetComponent<TextMeshProUGUI>();
    }
    
    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector3.zero);
            if(hit.collider != null)
            {
                textClicked = null;
                // If something is clicked (hit)
                if(hit.collider.gameObject.GetComponent<interact_animation>() != null)
                {
                    textClicked = hit.collider.gameObject.GetComponent<interact_animation>().interactableName;
                    hit.collider.gameObject.GetComponent<interact_animation>().callAnimation();
                }

                // display text
                if(!textShowing)
                {
                    currentTextDisplayRoutine = StartCoroutine(displayText(textClicked));
                } else
                {
                    endDisplayText();
                    currentTextDisplayRoutine = StartCoroutine(displayText(textClicked));
                }
            }
        }
    }

    // display text for 2 seconds, then make it disappear
    private IEnumerator displayText(string data)
    {
        objectTextMesh.text = data;
        textShowing = true;

        currentText = data;

        yield return new WaitForSeconds(2.0f); // wait before unshowing
        endDisplayText();
    }

    // End mid
    private void endDisplayText()
    {
        // Ensure that coroutine is running before stopping
        if(currentTextDisplayRoutine != null) StopCoroutine(currentTextDisplayRoutine);
        
        objectTextMesh.text = "";
        textShowing = false;
    }
}
