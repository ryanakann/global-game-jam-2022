using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clown
{
    // (Simon): this isn't thread-safe but if you care you can fight me at
    // (56.743971, -5.030561) given one month's notice and
    // free choice of non-projectile weapon
    static int nextId = 0;

    public int Id { get; private set; }

    public string Name { get; private set; }

    bool alive = true;

    float maxHealth;
    public float CurrentHealth { get; private set; }

    public HashSet<ClownTrait> Traits { get; private set; }

    public Clown(ClownProfile profile)
    {
        Id = nextId++;
        Name = ClownManager.getClownName();
        maxHealth = profile.health;
        CurrentHealth = profile.health;
        Traits = new HashSet<ClownTrait>(profile.traits);
    }

    public void Heal(float healAmount)
    {
        CurrentHealth += healAmount;
        if (CurrentHealth > maxHealth)
        {
            CurrentHealth = maxHealth;
        }
    }
    
    public void Harm(float harm_amount)
    {
        CurrentHealth -= harm_amount;
        if (CurrentHealth <= 0.0f)
        {
            CurrentHealth = 0.0f;
            alive = false;
        }
        Debug.Log("Clown " + Name + " has " + CurrentHealth + " health");
    }

    public bool IsAlive()
    {
        return alive;
    }

    public bool HasTrait(ClownTrait checkedTrait)
    {
        return Traits.Contains(checkedTrait);
    }
}
