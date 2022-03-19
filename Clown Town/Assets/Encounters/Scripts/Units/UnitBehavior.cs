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
        [SerializeField]
        private bool _alive;

        [SerializeField]
        private GameObject _attackEffect;
        
        [HideInInspector]
        public UnityEvent<float> OnAttack;
        [HideInInspector]
        public UnityEvent<float> OnHealthChanged;
        [HideInInspector]
        public UnityEvent OnDie;

        private float _attackCooldown;

        public virtual void Init(UnitInfo unitInfo)
        {
            _unitInfo = unitInfo;

            _currentHealth = _unitInfo.MaxHealth;
            _alive = _currentHealth > 0f;
            _attackCooldown = 0f;
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TakeDamage(Random.Range(10f, 20f));
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Attack();
            }
        }

        protected virtual void Attack()
        {
            if (!_alive) return;
            if (_attackCooldown > 0f) return;
            StartCoroutine(AttackCR());
        }

        protected virtual IEnumerator AttackCR()
        {
            var maxCoolddown = 1 / _unitInfo.AttackSpeed;
            _attackCooldown = maxCoolddown;
            
            OnAttack?.Invoke(_unitInfo.AttackSpeed);

            while (_attackCooldown >  Mathf.Max(maxCoolddown - _unitInfo.AttackSpeed / 2f, 0f))
            {
                _attackCooldown -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            var effect = Instantiate(_attackEffect, transform);
            effect.transform.localPosition = Vector3.right;
            effect.transform.localScale = new Vector3(effect.transform.localScale.x, _unitInfo.AttackWidth, effect.transform.localScale.z);

            Destroy(effect, 1f);

            while (_attackCooldown > 0f)
            {
                _attackCooldown -= Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }

            _attackCooldown = 0f;
        }

        public virtual void TakeDamage(float damage)
        {
            if (!_alive) return;
            _currentHealth = Mathf.Max(0f, _currentHealth - damage);
            if (Mathf.Approximately(_currentHealth, 0f))
            {
                Die();
            }

            OnHealthChanged?.Invoke(_currentHealth);
        }

        protected virtual void Die()
        {
            if (!_alive) return;
            _alive = false;
            Debug.Log($"{name} is DEAD");
            OnDie?.Invoke();
            Destroy(gameObject, 1f);
        }
    }
}