using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Credits : MonoBehaviour
{
    TextMeshProUGUI text;

    bool showing;

    string[] freakingCredits = new string[]
        {
            "clownhead in command - Wyclown Clowner",
            "megaclown - Ryclown Clownann",
            "event clownmaster - Siclown Clownle",
            "junior clownman - Edclown Clownsi-Clownson",
            "baby clownster - Clownerv Phoclown",
            "the big clown on top - Clownby Dyclown",
            "clownlet - Gilclown Clownke",
            "background clown - Ticlown Clownyan",
            "invisiclown - Jackclown Eclown",
        };
    int currentCredit = 0;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (!showing)
        {
            text.text = freakingCredits[currentCredit];
            StartCoroutine(Show());
            currentCredit++;
            if (currentCredit >= freakingCredits.Length)
                currentCredit = 0;
        }
    }

    IEnumerator Show()
    {
        showing = true;
        Color startColor = text.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 1f);
        float t = 0;
        float duration = 1f;
        while (t < duration)
        {
            text.color = Color.Lerp(startColor, endColor, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        t = 0;
        while (t < duration)
        {
            text.color = Color.Lerp(endColor, startColor, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        showing = false;
    }


}
