using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void GameEvent();

public class Clown
{
    // (Simon): this isn't thread-safe but if you care you can fight me at
    // (56.743971, -5.030561) given one month's notice and
    // free choice of non-projectile weapon
    static int nextId = 0;

    public int Id { get; private set; }

    public string Name { get; private set; }

    [HideInInspector]
    public bool alive = true;

    float maxHealth;
    public float CurrentHealth { get; private set; }

    public ClownPersonality Personality { get; private set; }
    public HashSet<ClownTrait> Traits { get; private set; }

    public GameEvent deathEvent;

    Sprite headSprite;
    ClownBody body;

    [HideInInspector]
    public ClownDisplay display;


    public Clown(ClownProfile profile)
    {
        //Debug.Log("Creating Clown" + nextId);
        Id = nextId++;
        Name = ClownManager.getClownName();
        maxHealth = profile.health;
        CurrentHealth = profile.health;
        Personality = profile.personality;
        Traits = new HashSet<ClownTrait>(profile.traits);
        headSprite = ClownManager.GetClownHead();
        body = ClownManager.GetClownBody();

        //Debug.Log("\t Name: " + Name);
        //Debug.Log("\t Personality: " + Personality);
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
        // if dead, clown quip
        Debug.Log("Clown " + Name + " has " + CurrentHealth + " health");

        if (display != null && CurrentHealth > 0)
        {
            // TODO: different clown voices
            FX_Spawner.instance.SpawnFX(FXType.ClownHarm, Vector3.zero, Quaternion.identity);
            ExplainerManager.Explain(Cue.ClownHarm);
            display.Harm((int)harm_amount, SelectionController.instance.fueling);
        }
        // if alive, clown or another clown quip
        if (alive)
        {
            ClownManager.SayQuipInFlowchartForClownForEvent(Id, EventTypes.ClownGetHurt);
            if (Random.value < 0.5f)
            {
                ClownManager.SayQuipInFlowchartForClownForEvent(ClownManager.getRandomClownIdExcludingAnother(Id), EventTypes.AnotherClownHurt);
            }
        }
        else
        {
            ClownManager.SayQuipInFlowchartForClownForEvent(Id, EventTypes.ClownGetKilled);
            if (Random.value < 0.5f)
            {
                ClownManager.SayQuipInFlowchartForClownForEvent(ClownManager.getRandomClownIdExcludingAnother(Id), EventTypes.AnotherClownKilled);
            }
            FX_Spawner.instance.SpawnFX(FXType.ClownDie, Vector3.zero, Quaternion.identity);
            ExplainerManager.Explain(Cue.ClownDie);
            ClownManager.KillClown(Id);
        }
    }

    public void Kill()
    {
        if (display != null)
            display.Kill(SelectionController.instance.fueling);
        CurrentHealth = 0;
        alive = false;
        deathEvent?.Invoke();
    }

    public bool IsAlive()
    {
        return alive;
    }

    public bool HasTrait(ClownTrait checkedTrait)
    {
        return Traits.Contains(checkedTrait);
    }

    public ClownDisplay SpawnDisplayAtPosition(Vector2 pos)
    {
        GameObject displayPrefab = ClownManager.instance.displayPrefab;
        GameObject displayInstance = GameObject.Instantiate(displayPrefab, pos, Quaternion.identity);

        ClownDisplay display = displayInstance.GetComponent<ClownDisplay>();
        display.SetHead(headSprite);
        display.SetBody(body);
        display.SetClown(this);
        return display;
    }
}
