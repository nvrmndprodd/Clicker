using System.Threading.Tasks;
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

        public async Task<GameObject> CreateEnemy(EnemyType enemyType, Vector3 at)
        {
            var enemyData = _staticData.ForEnemy(enemyType);

            var enemyPrefab = await _assets.Load<GameObject>(enemyData.Prefab);
            var enemyObject = Object.Instantiate(enemyPrefab, at, Quaternion.identity);

            var enemyHealth = enemyObject.GetComponent<EnemyHealth>();
            enemyHealth.Max = enemyData.Hp;
            enemyHealth.Current = enemyData.Hp;

            return enemyObject;
        }

        public GameObject CreateBooster()
        {
            return new GameObject();
        }
    }
}