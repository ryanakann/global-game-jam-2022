using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Encounters;

public class Projectile : MonoBehaviour
{
    bool throwing;
    float damage;
    public float speed = 5f;

    private void Start()
    {
        throwing = true;
    }

    public void Throw(Vector3 start, bool throwRight, float damage)
    {
        if (throwing) return;
        throwing = true;
        transform.position = start;
        this.damage = damage;
        StartCoroutine(ThrowCR(throwRight));
    }

    IEnumerator ThrowCR(bool throwRight)
    {
        while (throwing)
        {
            IteratePosition(throwRight);
            StopThrowingIfOutOfScreen();

            yield return new WaitForEndOfFrame();
        }
        RemovePoopie();
    }

    private void IteratePosition(bool throwRight)
    {
        Vector3 direction = throwRight ? Vector3.right : Vector3.left;
        transform.position += direction * speed * Time.deltaTime;
    }

    private void StopThrowingIfOutOfScreen()
    {
        Vector3 cameraPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (cameraPosition.x < 0f || cameraPosition.x > 1f)
        {
            throwing = false;
        }
    }

    private UnitBehavior_Enemy GetEnemyOrNull(Collider2D collision)
    {
        return collision.gameObject.GetComponent<UnitBehavior_Enemy>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var maybeEnemy = GetEnemyOrNull(collision);
        if (maybeEnemy)
        {
            maybeEnemy.TakeDamage(damage);
            RemovePoopie();
        }
    }

    private void RemovePoopie()
    {
        Destroy(gameObject);
    }
}
