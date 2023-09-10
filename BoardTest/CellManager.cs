using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{
    [SerializeField]
    private Outline script;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // https://stackoverflow.com/questions/63890336/how-to-detect-if-the-mouse-is-over-the-object-unity-c-sharp
    private void OnMouseEnter()
    {
        script.enabled = true;
    }

    private void OnMouseExit()
    {
        script.enabled = false;
    }
}
