using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health), typeof(SpriteRenderer), typeof(UnitMovement))]
public class Unit : MonoBehaviour {
    public Lane lane;

    public float speed;
    public float health;
    public int cost;
    public List<string> tags;

    public UnityEvent<Lane> OnPlace;
    public UnityEvent OnAttack;
    public UnityEvent OnDie;
    public UnityEvent<float> OnTakeDamage;

    public virtual void Place(Lane lane) 
    {
        this.lane = lane;
        OnPlace?.Invoke(lane);
    }
    public virtual void Attack() 
    {
        print(gameObject.name + " is attacking!");
        OnAttack?.Invoke();
    }
    public virtual void TakeDamage(float amount)
    {
        OnTakeDamage?.Invoke(amount);
    }
    public virtual void Die() 
    {
        GetComponent<Health>().Die();
        OnDie?.Invoke();
    }
    private void Start() 
    {
        GetComponent<Health>().maxHealth = health;
        GetComponent<Health>().SetHealth(health);
    }
}
