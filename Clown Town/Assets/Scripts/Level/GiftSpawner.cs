using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounters;

public class GiftSpawner : Singleton<GiftSpawner>
{

    float timer = 0;
    Vector2 timerRange = new Vector2(3f, 20f);

    public GameObject giftPrefab;

    EncounterInfo info;

    // Start is called before the first frame update
    void Start()
    {
        info = GetComponentInParent<EncounterInfo>();
        timer = Random.Range(timerRange[0], timerRange[1]);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= 0)
        {
            var gift = Instantiate(giftPrefab).transform;
            gift.position = new Vector2(Random.Range(info.Bounds.min.x, info.Bounds.max.x), Random.Range(info.Bounds.min.y, info.Bounds.max.y));
            timer = Random.Range(timerRange[0], timerRange[1]);
        }
        timer -= Time.deltaTime;
    }
}
