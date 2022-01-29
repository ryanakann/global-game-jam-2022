using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer))]
public class Unit : MonoBehaviour {
    public Lane lane { get; protected set; }
    //public Unit target { get; protected set; }
    public Unit target;

    public bool active;

    [Range(0f, 100f)]
    public float health = 50f;
    [SerializeField]
    private float currentHealth;

    [Range(0f, 10f)]
    public float speed = 1f;
    
    [Range(0f, 16f)]
    [SerializeField]
    protected float _attackRange = 0.5f;
    public float attackRange 
    {   
        get
        {
            return _attackRange / lane.cellWidth;
        } 
        protected set
        {
            _attackRange = value;
        } 
    }
    [Range(0.01f, 10f)]
    public float attackSpeed = 0.5f;
    [Range(0f, 100f)]
    public float attackDamage = 5f;
    private float timeSinceLastAttack;

    public enum AttackType
    {
        Melee,
        Ranged
    }
    public AttackType attackType;
    public GameObject projectile;

    [Range(0f, 1000f)]
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
        IdleStart();
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

    //public State state { get; protected set; }
    public State state;

    public virtual void IdleStart()
    {
        state = State.Idle;
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
        if (timeSinceLastAttack > 1 / attackSpeed)
        {
            timeSinceLastAttack = 0f;
            AttackExecute();
        } 
        else
        {
            timeSinceLastAttack += Time.deltaTime;
        }

    }

    protected virtual void AttackExecute()
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
        OnDie?.Invoke();
        Destroy(gameObject);
    }

    protected virtual void DieUpdate()
    {

    }
    #endregion

    #region HELPER FUNCTIONS
    public virtual void TakeDamage(float amount)
    {
        OnTakeDamage?.Invoke(amount);
        currentHealth -= amount;

        if (currentHealth <= 0f)
        {
            currentHealth = 0f;
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
            case State.Idle:
                IdleUpdate();
                break;
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
        timeSinceLastAttack = 0f;
        currentHealth = health;
        IdleStart();
    }

    private void OnGUI()
    {
#if UNITY_EDITOR
        GUIContent content = new GUIContent(state.ToString());
        GUIStyle style = new GUIStyle();
        style.alignment = TextAnchor.MiddleCenter;
        UnityEditor.Handles.color = Color.red;
        UnityEditor.Handles.Label(transform.position, content, style);
#endif
    }
    #endregion
}
