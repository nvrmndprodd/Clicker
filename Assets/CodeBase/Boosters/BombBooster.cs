using System;
using CodeBase.Enemy;
using UnityEngine;

namespace CodeBase.Boosters
{
    public class BombBooster : MonoBehaviour
    {
        private const float Radius = 100;
        private int _layerMask;


        private void Awake()
        {
            GetComponent<EnemyDeath>().Happened += OnDeath;

            _layerMask = 1 << LayerMask.NameToLayer("Enemy");
        }

        private void OnDeath(GameObject obj)
        {
            var hits = Physics.OverlapSphere(transform.position, Radius, _layerMask);

            foreach (var hit in hits) 
                hit.GetComponent<EnemyHealth>().TakeDamage(999);
        }
    }
}