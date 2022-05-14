using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounters;

public class ClownZone : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var info = collision.GetComponent<UnitInfo>();
        if (info != null && info.UnitType == UnitType.Foe)
        {
            ClownManager.DamageClowns(1);
            StartCoroutine(FadeEnemy(info.gameObject));
        }
    }

    IEnumerator FadeEnemy(GameObject enemy)
    {
        SpriteRenderer r = enemy.GetComponentInChildren<SpriteRenderer>();
        var startAlpha = new Color(r.color.r, r.color.g, r.color.b, r.color.a);
        var endColor = new Color(r.color.r, r.color.g, r.color.b, 0f);
        float maxTimer = 0.5f;
        float timer = maxTimer;
        while (timer > 0)
        {
            r.color = Color.Lerp(startAlpha, endColor, 1f - timer/maxTimer);
            timer -= Time.deltaTime;
            yield return null;
        }
        Destroy(enemy);
    }
}
