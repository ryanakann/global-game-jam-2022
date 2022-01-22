using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {
    public float maxHealth;
    private float health { get; set; }
    void Start() {
        SetHealth(maxHealth);
    }
    public void SetHealth(float amount) {
        health = amount;
        if (health > maxHealth) {
            health = maxHealth;
        }
        if (health < 0) {
            Die();
        }
    }

    public void IncrementHealth(float amount) {
        SetHealth(health + amount);
    }
    public void Damage(float amount) {
        IncrementHealth(-amount);
    }

    public void Die() {
        print(gameObject.name + " is dead");
        Destroy(gameObject);
    }

}
