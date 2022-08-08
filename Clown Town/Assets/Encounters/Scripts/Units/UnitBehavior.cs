using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Encounters
{
    public class UnitBehavior : MonoBehaviour, IInitializable<UnitInfo>
    {
        [HideInInspector]
        public UnitInfo _info;

        [HideInInspector]
        public UnityEvent<float> OnAttack;
        [HideInInspector]
        public UnityEvent<float> OnHealthChanged;
        [HideInInspector]
        public UnityEvent OnDie;

        bool attacking;

        protected bool flip = false;

        public FXType spawnFX, attackFX, harmFX, dieFX;


        public virtual void Init(UnitInfo unitInfo)
        {
            _info = unitInfo;

            _info.CurrentHealth = _info.MaxHealth;
            FX_Spawner.instance.SpawnFX(spawnFX, Vector3.zero, Quaternion.identity);
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (_info == null) return;
            if (_info.UnitType == UnitType.Foe) return;
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
            if (!_info.Alive || attacking) return;
            StartCoroutine(AttackCR());
        }

        protected virtual IEnumerator AttackCR()
        {
            attacking = true;
            FX_Spawner.instance.SpawnFX(attackFX, Vector3.zero, Quaternion.identity);
            //print($"{gameObject.name} is attacking");
            var maxCoolddown = 1 / _info.AttackSpeed;
            
            OnAttack?.Invoke(_info.AttackSpeed);

            yield return new WaitForSeconds(1 / (2 * _info.AttackSpeed));

            if (_info.AttackEffect)
            {
                var effect = Instantiate(_info.AttackEffect, transform);
                var attack = effect.GetComponent<UnitAttack>();

                // Poop doesnt have animation ;)
                if (effect.GetComponent<Animator>())
                {
                    effect.GetComponent<Animator>().speed = _info.AttackSpeed;
                }

                effect.GetComponent<Projectile>()?.Throw(transform.position, true, _info.AttackDamage);

                // Non projectile effects. this is what you call jank city
                if (attack)
                {
                    effect.gameObject.SetActive(false);

                    attack?.Init(_info);
                    effect.transform.localPosition = ((flip) ? Vector3.left : Vector3.right) * _info.AttackRange / 2f;
                    effect.transform.localScale = new Vector3(_info.AttackRange, _info.AttackWidth, effect.transform.localScale.z);
                    Destroy(effect.gameObject, 1 / _info.AttackSpeed + 0.1f);

                    effect.gameObject.SetActive(true);
                }

            }

            yield return new WaitForSeconds(_info.AttackCooldown);
            attacking = false;
        }

        public virtual void TakeDamage(float damage)
        {
            if (!_info.Alive) return;

            _info.CurrentHealth = Mathf.Max(0f, _info.CurrentHealth - damage);
            
            if (!_info.Alive)
            {
                Die();
            }

            FX_Spawner.instance.SpawnFX(harmFX, Vector3.zero, Quaternion.identity);

            OnHealthChanged?.Invoke(_info.CurrentHealth);
        }

        protected virtual void Die()
        {
            //Debug.Log($"{name} is DEAD");
            FX_Spawner.instance.SpawnFX(dieFX, Vector3.zero, Quaternion.identity);
            OnDie?.Invoke();
            Destroy(gameObject, 1f);
        }
    }
}