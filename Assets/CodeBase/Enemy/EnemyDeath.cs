using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class EnemyDeath : MonoBehaviour
    {
        public EnemyAnimator Animator;
        public EnemyHealth Health;

        public GameObject DeathFX;

        public event Action<GameObject> Happened;

        private void Start() => 
            Health.HealthChanged += OnHealthChanged;

        private void OnDestroy() => 
            Health.HealthChanged -= OnHealthChanged;

        private void OnHealthChanged()
        {
            if (Health.Current <= 0)
                Die();
        }

        private void Die()
        {
            Health.HealthChanged -= OnHealthChanged;
            
            Animator.PlayDeath();

            StartCoroutine(DestroyTimer());
            
            Debug.Log(gameObject == null);
            Happened?.Invoke(gameObject);
        }

        private IEnumerator DestroyTimer()
        {
            var fx = SpawnDeathFX();
            
            yield return new WaitForSeconds(2);
            
            Destroy(fx);
            Destroy(gameObject);
        }

        private GameObject SpawnDeathFX() => 
            Instantiate(DeathFX, transform.position, Quaternion.identity);
    }
}