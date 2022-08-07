using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuipBubble : MonoBehaviour
{

    [HideInInspector]
    public List<Transform> sequence = new List<Transform>();

    [HideInInspector]
    public List<float> alphaSequence = new List<float>();

    int currentPoint = -1;

    Coroutine lerpCoroutine;

    CanvasGroup canvasGroup;

    bool fading;

    private void Start()
    {
        Invoke("Fade", 4f);
    }

    void Fade()
    {
        StartCoroutine(CoFade());
    }

    public void Setup(Sprite face, string speakerName, string text)
    {
        transform.FindDeepChild("Face").GetComponent<Image>().sprite = face;
        transform.FindDeepChild("Name").GetComponent<TextMeshProUGUI>().text = speakerName;
        transform.FindDeepChild("Text").GetComponent<TextMeshProUGUI>().text = text;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void UpdateBubble()
    {
        // if out of sequence, delet.
        // if lerping, stop, and restart with new target
        // lerp position, lerp scale
        if (fading)
            return;
        currentPoint++;
        if (currentPoint >= sequence.Count)
        {
            if (--QuipManager.instance.quipCount == 0)
                FX_Spawner.instance.SpawnFX(FXType.LastQuip, Vector3.zero, Quaternion.identity);
            StartCoroutine(CoFade());
            return;
        }
        if (lerpCoroutine != null)
            StopCoroutine(lerpCoroutine);
        lerpCoroutine = StartCoroutine(LerpBubble());
    }

    IEnumerator CoFade()
    {
        fading = true;
        QuipManager.instance.QuipEvent -= UpdateBubble;
        // wait a bit then cancel the lerpCoroutine if it exists
        if (lerpCoroutine != null)
            StopCoroutine(lerpCoroutine);
        Vector3 startScale = transform.localScale;
        Vector3 endScale = Vector3.zero;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = 0;
        float t = 0f;
        float duration = 0.25f;
        while (t < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            transform.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }

    IEnumerator LerpBubble()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = sequence[currentPoint].position;
        Vector3 startScale = transform.localScale;
        Vector3 endScale = sequence[currentPoint].localScale;
        float startAlpha = canvasGroup.alpha;
        float endAlpha = alphaSequence[currentPoint];
        float t = 0f;
        float duration = 0.25f;
        while (t < duration)
        {
            transform.position = Vector3.Lerp(startPos, endPos, t/duration);
            transform.localScale = Vector3.Lerp(startScale, endScale, t / duration);
            canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
    }
}
