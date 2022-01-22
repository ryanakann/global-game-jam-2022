using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health), typeof(SpriteRenderer), typeof(UnitMovement))]
public class Unit : MonoBehaviour {
    public float speed;
    public float health;
    public int cost;
    public Sprite sprite;
    public List<string> tags;

    protected virtual void OnPlace(Lane lane) {

    }
    protected virtual void OnAttack() {
        print(gameObject.name + " is attacking!");
    }
    protected virtual void OnTakeDamage(float amount) {

    }
    protected virtual void OnDie() {
        GetComponent<Health>().Die();
    }
    private void Start() {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
}
