using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health), typeof(SpriteRenderer), typeof(UnitMovement))]
public class Unit : MonoBehaviour {
    public Lane lane;
    public Unit target;

    public bool active;

    public float speed;
    public float health;
    public int cost;
    public List<string> tags;

    public UnityEvent<Lane> OnPlace;
    public UnityEvent OnAttack;
    public UnityEvent OnDie;
    public UnityEvent<float> OnTakeDamage;

    private bool _isEnemy;
    public bool IsEnemy {
        get => _isEnemy;
        set
        {
            _isEnemy = value;
            GetComponent<SpriteRenderer>().flipX = value;
        } 
    }

    public virtual void Place(Lane lane) 
    {
        this.lane = lane;
        active = true;
        OnPlace?.Invoke(lane);
    }

    #region STATES
    public enum State
    {
        Idle,
        Attack,
        Move,
        Affect,
        Die,
    }

    public State state { get; protected set; }

    public virtual void IdleStart()
    {

    }

    protected virtual void IdleUpdate()
    {

    }

    public virtual void AttackStart() 
    {
        state = State.Attack;

        print(gameObject.name + " is attacking!");
        OnAttack?.Invoke();
    }

    protected virtual void AttackUpdate()
    {

    }

    public virtual void MoveStart()
    {
        state = State.Move;
    }

    protected virtual void MoveUpdate()
    {

    }

    public virtual void AffectStart()
    {
        state = State.Affect;

    }

    protected virtual void AffectUpdate()
    {

    }

    public virtual void DieStart()
    {
        state = State.Die;

        GetComponent<Health>().Die();
        OnDie?.Invoke();
    }

    protected virtual void DieUpdate()
    {

    }
    #endregion

    #region HELPER FUNCTIONS
    public virtual void TakeDamage(float amount)
    {
        OnTakeDamage?.Invoke(amount);
        health -= amount;

        if (health <= 0f)
        {
            health = 0f;
            DieStart();
        }
    }

    public virtual Unit SelectTarget()
    {
        return null;
    }
    #endregion

    #region MESSAGES

    protected virtual void Update()
    {
        switch (state)
        {
            case State.Attack:
                AttackUpdate();
                break;
            case State.Move:
                MoveUpdate();
                break;
            case State.Affect:
                AffectUpdate();
                break;
            case State.Die:
                DieUpdate();
                break;
            default:
                break;
        }
    }

    protected virtual void Start() 
    {
        active = false;
        GetComponent<Health>().maxHealth = health;
        GetComponent<Health>().SetHealth(health);
    }
    #endregion
}
