using UnityEngine;

namespace Encounters
{
    public enum UnitType
    {
        Friend,
        Foe,
    }

    public class UnitInfo : Info<UnitInfo>
    {
        [Header("General")]
        private UnitType _unitType;
        public UnitType UnitType
        {
            get => _unitType;
            set => _unitType = value;
        }

        [Range(0f, 200f)]
        [SerializeField]
        private float _maxHealth = 100f;
        public float MaxHealth { get => _maxHealth; }

        [SerializeField]
        private float _currentHealth;
        public float CurrentHealth { get => _currentHealth; set => _currentHealth = value; }

        [SerializeField]
        public bool Alive { get => CurrentHealth > 0f; }

        [Header("Attacks")]
        [SerializeField]
        [Range(0f, 10f)]
        [Tooltip("Attacks per second.")]
        private float _attackSpeed = 1f;
        public float AttackSpeed { get => _attackSpeed; }

        [SerializeField]
        [Range(0f, 10f)]
        [Tooltip("Wait time between attacks.")]
        private float _attackCooldown = 1f;
        public float AttackCooldown { get => _attackCooldown; }

        [SerializeField]
        [Range(0f, 100f)]
        [Tooltip("How much health is taken away from enemies per attack.")]
        private float _attackDamage = 10f;
        public float AttackDamage { get => _attackDamage; }

        [SerializeField]
        [Range(0f, 20f)]
        [Tooltip("How far this unit will start attacking from, measured in tiles.")]
        private float _attackRange = 1f;
        public float AttackRange { get => _attackRange; }

        [SerializeField]
        [Range(0f, 20f)]
        [Tooltip("Width of attack, measured in tiles.")]
        private float _attackWidth = 1f;
        public float AttackWidth { get => _attackWidth; }

        [SerializeField]
        private GameObject _attackEffect;
        public GameObject AttackEffect { get => _attackEffect; }


        public Vector2 GetGridPosition(EncounterInfo encounterInfo)
        {
            return encounterInfo.WorldToGridPosition(transform.position);
        }

        public void SetGridPosition(EncounterInfo encounterInfo, Vector2 gridPosition)
        {
            transform.position = encounterInfo.GridToWorldPosition(gridPosition);
        }
    }
}