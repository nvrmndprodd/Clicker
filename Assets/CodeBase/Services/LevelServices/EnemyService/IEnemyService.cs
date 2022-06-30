using UnityEngine;

namespace CodeBase.Services.LevelServices.EnemyService
{
    public interface IEnemyService : IService
    {
        public void OnUpdate(float deltaTime);
        SpawnPoint FindFreePoint();
        public void OnEnemyDeath(GameObject enemy);
        public void Clear();
    }
}