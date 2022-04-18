using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSpriteFiller : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        foreach (SpriteRenderer s in GetComponentsInChildren<SpriteRenderer>())
        {
            s.transform.parent.localScale *= Random.Range(-5f, 5f);
            s.transform.parent.Rotate(0, 0, Random.Range(0f, 360f));
            s.transform.position += Random.insideUnitSphere * Random.Range(-0.25f, 0.25f);
        }
    }
}
