using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CurrencyDropper))]
public class Gift : MonoBehaviour
{
    [HideInInspector]
    public bool clicked;

    float timer;
    Vector2 timerRange = new Vector2(3f, 10f);

    [HideInInspector]
    public CurrencyDropper dropper;
    // Start is called before the first frame update
    void Start()
    {
        dropper = GetComponent<CurrencyDropper>();
        timer = Random.Range(timerRange[0], timerRange[1]);
    }

    private void Update()
    {
        if (timer <= 0)
        {
            Click(false);
        }
        timer -= Time.deltaTime;
    }

    public void Click(bool drop=true)
    {
        if (clicked)
            return;
        if (drop)
            dropper.Drop();
        clicked = true;
        StartCoroutine(ClownZone.instance.FadeEnemy(gameObject));
    }
}
