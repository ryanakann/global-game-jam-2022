using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Encounters
{
    public class UnitBehavior : MonoBehaviour, IInitializable<UnitInfo>
    {
        private UnitInfo _unitInfo;

        [SerializeField]
        private float _currentHealth;
        public UnityEvent<float> OnHealthChanged;
        public UnityEvent OnAttack;

        private float _attackCooldown;

        public virtual void Init(UnitInfo unitInfo)
        {
            _unitInfo = unitInfo;

            _currentHealth = _unitInfo.MaxHealth;
            _attackCooldown = 0f;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(Random.Range(10f, 20f));
            }
        }

        protected virtual void Attack()
        {
            if (_attackCooldown > 0) return;
            _attackCooldown = 1 / _unitInfo.AttackSpeed;
        }

        public virtual void TakeDamage(float damage)
        {
            _currentHealth = Mathf.Max(0f, _currentHealth - damage);
            if (Mathf.Approximately(_currentHealth, 0f))
            {
                Die();
            }

            OnHealthChanged?.Invoke(_currentHealth);
        }

        protected virtual void Die()
        {
            Debug.Log($"{name} is DEAD");
            Destroy(gameObject);
        }
    }
}