using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFiller : Singleton<ScreenFiller>
{
    // total number
    // how long
    // curve dictates the distribution
    // how many timesteps
    // figure out how large each sample needs to be
    // figure out how many samples total?
    // determine how long to wait before next sample, so figure out how to scale the curve, if you know the max, that's all you need?

    public AnimationCurve sampleDelayCurve;
    float numSamples=10f, totalTime=1.5f, timeNormalizer=0;
    List<GameObject> fillers = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (var t in gameObject.FindComponentsInChildrenWithTag<Image>("UI_Filler", forceActive:true))
        {
            fillers.Add(t.gameObject);
        }

        for (int i = 0; i < numSamples; i++)
        {
            timeNormalizer += sampleDelayCurve.Evaluate(i / numSamples);
        }
        DontDestroyOnLoad(transform.parent);
    }

    public void Fill(int fill)
    {
        StartCoroutine(CoFill(fill != 0));
    }

    IEnumerator CoFill(bool fill)
    {
        int revealedIcons = 0;
        int j = 0;
        int sampleSize = Mathf.CeilToInt(fillers.Count / numSamples);
        float delay;

        while (revealedIcons < fillers.Count)
        {
            for (int i = revealedIcons; i < revealedIcons+Mathf.Max(sampleSize, 1f); i++)
            {
                if (i >= fillers.Count)
                    break;
                fillers[i].gameObject.SetActive(fill);
            }
            revealedIcons += Mathf.Max(sampleSize, 1);

            delay = totalTime * (sampleDelayCurve.Evaluate(j / numSamples) / timeNormalizer);
            yield return new WaitForSeconds(delay);
            j++;
        }
    }
}
