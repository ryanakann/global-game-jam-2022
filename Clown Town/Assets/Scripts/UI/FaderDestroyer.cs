using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaderDestroyer : MonoBehaviour
{
    SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        StartCoroutine(FadeDestroy());
    }

    IEnumerator FadeDestroy()
    {
        float t = 0;
        float duration = 1f;
        Color startColor = rend.color;
        Color targetColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        while (t < duration)
        {
            rend.color = Color.Lerp(startColor, targetColor, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
