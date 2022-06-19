using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounters;

public class GiftSpawner : Singleton<GiftSpawner>
{

    float timer = 0, padding = 1.5f;
    Vector2 timerRange = new Vector2(3f, 6f);

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
            gift.position = new Vector2(Random.Range(info.Bounds.min.x+padding, info.Bounds.max.x-padding), Random.Range(info.Bounds.min.y+padding, info.Bounds.max.y-padding));
            timer = Random.Range(timerRange[0], timerRange[1]);
        }
        timer -= Time.deltaTime;
    }
}
