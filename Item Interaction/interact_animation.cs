using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interact_animation : MonoBehaviour
{
    // private static string swag = "I love~swag_and~skinny jeans_on~my pants";
    // string[] swagArr = swag.Split(' ', '_', '~');

    public string interactableName;
    [SerializeField] private float duration = 0.1f;
    [SerializeField] private Vector2 maxScale;
    [SerializeField] private Vector2 minScale;

    private Color32 maxColor = new Color32(255,255,255,255); // No Tint
    private Color32 minColor = new Color32(155,155,155,255); // Tint

    private SpriteRenderer meshRenderer;
    private float tTime = 0.0f;
    private Coroutine animationRoutine = null;

    private void Start()
    {
        meshRenderer = this.gameObject.GetComponent<SpriteRenderer>();
    }

    public void callAnimation()
    {
        if(animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
        }
        animationRoutine = StartCoroutine(timeAnimation());
    }

    private IEnumerator timeAnimation()
    {
        // bounce in
        var temp = this.transform.localScale;
        var colorTemp = meshRenderer.color;
        tTime = 0.0f;
        while(tTime < 1.0f)
        {
            tTime += Time.deltaTime / duration;
            this.transform.localScale = Vector2.Lerp(temp, minScale, tTime);
            meshRenderer.color = Color32.Lerp(colorTemp, minColor, tTime);
            
            yield return null; // goes to next frame
        }

        // bounce out
        temp = this.transform.localScale;
        tTime = 0.0f;
        while(tTime < 1.0f)
        {
            tTime += Time.deltaTime / duration;
            this.transform.localScale = Vector2.Lerp(temp, maxScale, tTime);
            meshRenderer.color = Color32.Lerp(colorTemp, maxColor, tTime);
            yield return null; // goes to next frame
        }
    }
}
