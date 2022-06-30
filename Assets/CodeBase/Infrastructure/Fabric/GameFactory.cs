using System.Threading.Tasks;
using CodeBase.Boosters;
using CodeBase.Enemy;
using CodeBase.Infrastructure.AssetManagement;
using CodeBase.Services.StaticData;
using UnityEngine;

namespace CodeBase.Infrastructure.Fabric
{
    public class GameFactory : IGameFactory
    {
        private readonly IStaticDataService _staticData;
        private readonly IAssetProvider _assets;
        
        public GameFactory(IStaticDataService staticData, IAssetProvider assets)
        {
            _staticData = staticData;
            _assets = assets;
        }

        public async Task<GameObject> CreateEnemy(EnemyType enemyType, Vector3 at, float multiplier)
        {
            var enemyData = _staticData.ForEnemy(enemyType);

            var enemyPrefab = await _assets.Load<GameObject>(enemyData.Prefab);
            var enemyObject = Object.Instantiate(enemyPrefab, at, Quaternion.identity);

            var enemyHealth = enemyObject.GetComponent<EnemyHealth>();
            enemyHealth.Max = enemyData.Hp * multiplier;
            enemyHealth.Current = enemyData.Hp * multiplier;

            return enemyObject;
        }

        public async Task<GameObject> CreateBooster(BoosterType boosterType, Vector3 at)
        {
            var boosterData = _staticData.ForBooster(boosterType);

            var boosterPrefab = await _assets.Load<GameObject>(boosterData.prefab);

            return Object.Instantiate(boosterPrefab, at, Quaternion.identity);
        }
    }
}