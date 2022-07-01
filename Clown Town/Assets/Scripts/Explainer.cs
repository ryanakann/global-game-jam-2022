using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(LineRenderer))]
public class Explainer : MonoBehaviour
{

    LineRenderer lr;
    TextMeshProUGUI text;


    public void SetExplanation(Explanation explanation)
    {
        lr = GetComponent<LineRenderer>();
        text = GetComponent<TextMeshProUGUI>();

        text.text = explanation.text;
        transform.position = explanation.position;
        lr.SetPosition(1, explanation.pointerPosition);
    }

    public void Clear()
    {
        ExplainerManager.instance.Nextplanation();
        Destroy(gameObject); // in REALITY, we should re-use the same object, but we don't actually want to design a GOOD game now, do we?
    }
}
