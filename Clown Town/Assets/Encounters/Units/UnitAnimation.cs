using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Encounters
{
    public class UnitAnimation : MonoBehaviour
    {
        private UnitInfo _unitInfo;

        private Animator _animator;
        private SpriteRenderer _spriteRenderer;

        protected virtual void Init(UnitInfo unitInfo)
        {
            _unitInfo = unitInfo;

            _animator = GetComponentInChildren<Animator>();
            _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

            _unitInfo.unitBehavior.OnHealthChanged?.AddListener(HitEffect);
        }

        #region HIT HANDLING
        private bool _hitInProgress = false;
        private float _hitT = 0f;
        private readonly float _hitDuration = 1f;
        private readonly float _hitPeriods = 3f;
        private float _hitFrequency => _hitPeriods / _hitDuration;
        private readonly Color _normalColor = Color.white;
        private readonly Color _hitColor = Color.red;
        private float colorFn(float x) => 0.5f * (Mathf.Sin(2f * Mathf.PI * _hitFrequency * x - Mathf.PI / 2f) + 1);

        private void HitEffect(float health)
        {
            _hitT = 0f;
            if (!_hitInProgress)
            {
                _hitInProgress = true;
                StartCoroutine(HitEffectCR());
            }
        }

        private IEnumerator HitEffectCR()
        {
            while (_hitT < _hitDuration)
            {
                _spriteRenderer.color = Color.Lerp(_normalColor, _hitColor, colorFn(_hitT));
                _hitT += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            _hitInProgress = false;
        }
        #endregion
    }
}