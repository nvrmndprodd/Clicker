using System;
using CodeBase.Enemy;
using UnityEngine;

namespace CodeBase.Boosters
{
    public class FreezeBooster : MonoBehaviour
    {
        private void Awake() => 
            GetComponent<EnemyDeath>().Happened += OnFreeze;

        private void OnFreeze(GameObject obj)
        {
            GameObject.FindWithTag("Ice").GetComponent<BoosterEffect>().Activate(5f);
            Destroy(gameObject);
        }
    }
}