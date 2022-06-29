using System.Collections.Generic;
using System.Linq;
using CodeBase.Enemy;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    class StaticDataService : IStaticDataService
    {
        private const string EnemyDataPath = "StaticData/Enemies";

        private Dictionary<EnemyType, EnemyStaticData> _enemies;

        public void Load() =>
            _enemies = Resources
                .LoadAll<EnemyStaticData>(EnemyDataPath)
                .ToDictionary(x => x.EnemyType, x => x);

        public EnemyStaticData ForEnemy(EnemyType enemyType) => 
            _enemies.TryGetValue(enemyType, out var enemyData) 
                ? enemyData 
                : null;
    }
}