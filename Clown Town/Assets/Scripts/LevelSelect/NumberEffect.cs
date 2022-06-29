using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NumberEffect : MonoBehaviour
{
    SpriteRenderer num1, num2;

    public Sprite[] numbers;

    bool launched;

    Rigidbody2D rb;

    public static Vector2 minAngle = Helper.DegreeToVector2(45);
    public static Vector2 maxAngle = Helper.DegreeToVector2(135);

    float torque = 50f;

    public void Launch(float force)
    {
        if (launched)
            return;

        launched = true;

        rb.bodyType = RigidbodyType2D.Dynamic;

        Vector2 dir = Quaternion.Euler(0, Random.Range(-45, 45), 0) * Vector2.up;
        rb.AddForce(dir * force);
        rb.AddTorque(Random.Range(-torque, torque));

        StartCoroutine(Fade());
    }

    public void SetNumber(int number, float force = 160f)
    {
        rb = GetComponent<Rigidbody2D>();
        num1 = transform.FindDeepChild("Num1").GetComponent<SpriteRenderer>();
        num2 = transform.FindDeepChild("Num2").GetComponent<SpriteRenderer>();

        string numString = number.ToString();

        num1.sprite = numbers[(int)char.GetNumericValue(numString[0])];
        if (numString.Length == 2) 
        {
            num2.sprite = numbers[(int)char.GetNumericValue(numString[1])];
        }
        Launch(force);
    }

    IEnumerator Fade()
    {
        float t = 0;
        float duration = 1f;
        Color startColor = num1.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        while (t <= duration)
        {
            num1.color = Color.Lerp(startColor, endColor, t / duration);
            num2.color = Color.Lerp(startColor, endColor, t / duration);
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
