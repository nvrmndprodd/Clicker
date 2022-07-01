using System;
using System.Collections;
using CodeBase.Services.Progress;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyHealth : MonoBehaviour
    {
        public EnemyAnimator Animator;
        public GameObject HitFX;

        [SerializeField]
        private float _current;

        [SerializeField]
        private float _max;

        private Stats _playerStats;
        public event Action HealthChanged;

        public float Current
        {
            get => _current;
            set => _current = value;
        }

        public float Max
        {
            get => _max;
            set => _max = value; 
        }

        public void Construct(Stats playerStats) => 
            _playerStats = playerStats;

        public void TakeDamage(float damage)
        {
            Current -= damage;

            Animator.PlayHit();

            StartCoroutine(SpawnHitFX());
      
            HealthChanged?.Invoke();
        }

        private IEnumerator SpawnHitFX()
        {
            var fx = Instantiate(HitFX, transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
            Destroy(fx);
        }

        //private void OnMouseDown() => 
        //    TakeDamage(_playerStats.Damage);
    }
}