using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    bool throwing;
    public float speed = 5f;

    private void Start()
    {
        throwing = false;
    }

    public void Throw(Vector3 start, bool throwRight, Transform target, float damage)
    {
        if (throwing) return;
        throwing = true;
        transform.position = start;
        StartCoroutine(ThrowCR(throwRight, target, damage));
    }

    IEnumerator ThrowCR(bool throwRight, Transform target, float damage)
    {
        while (transform.position.x < target.position.x)
        {
            Vector3 direction = throwRight ? Vector3.right : Vector3.left;
            transform.position += direction * speed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        if (target.GetComponent<Unit>())
        {
            target.GetComponent<Unit>().TakeDamage(damage);
        }
        Destroy(gameObject);
    }
}
