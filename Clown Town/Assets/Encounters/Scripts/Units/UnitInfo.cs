using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Encounters
{
    public class UnitInfo : Info<UnitInfo>
    {
        [Range(0f, 200f)]
        [SerializeField]
        private float _maxHealth = 100f;
        public float MaxHealth { get => _maxHealth; }

        [SerializeField]
        [Range(0f, 10f)]
        [Tooltip("Attacks per second.")]
        private float _attackSpeed = 1f;
        public float AttackSpeed { get => _attackSpeed; }

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

        public Vector2 GetGridPosition(EncounterInfo encounterInfo)
        {
            return encounterInfo.WorldToGridPosition(transform.position);
        }

        public void SetGridPosition(EncounterInfo encounterInfo, Vector2Int gridPosition)
        {
            transform.position = encounterInfo.GridToWorldPosition(gridPosition);
        }
    }
}