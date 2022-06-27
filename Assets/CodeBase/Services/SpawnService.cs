using System;
using CodeBase.Infrastructure;
using UnityEngine;

namespace CodeBase.Services
{
    public class SpawnService : MonoBehaviour, IService
    {
        private readonly GameFabric _fabric;
        private readonly SpawnTimers _timers;
        
        public float CurrentSpeed { get; }

        public SpawnService(GameFabric fabric)
        {
            _fabric = fabric;
            _timers = new SpawnTimers(this);
        }

        private void Update()
        {
            _timers.Update(Time.deltaTime);
        }
    }
}