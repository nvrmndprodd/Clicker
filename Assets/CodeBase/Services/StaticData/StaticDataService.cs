using System.Collections.Generic;
using System.Linq;
using CodeBase.Boosters;
using CodeBase.Enemy;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
    class StaticDataService : IStaticDataService
    {
        private const string EnemyDataPath = "StaticData/Enemies";
        private const string BoosterDataPath = "StaticData/Boosters";

        private Dictionary<EnemyType, EnemyStaticData> _enemies;
        private Dictionary<BoosterType, BoosterStaticData> _boosters;

        public void Load()
        {
            _enemies = Resources
                .LoadAll<EnemyStaticData>(EnemyDataPath)
                .ToDictionary(x => x.EnemyType, x => x);

            _boosters = Resources
                .LoadAll<BoosterStaticData>(BoosterDataPath)
                .ToDictionary(x => x.BoosterType, x => x);
        }

        public EnemyStaticData ForEnemy(EnemyType enemyType) => 
            _enemies.TryGetValue(enemyType, out var enemyData) 
                ? enemyData 
                : null;
        
        public BoosterStaticData ForBooster(BoosterType boosterType) => 
            _boosters.TryGetValue(boosterType, out var boosterData) 
                ? boosterData 
                : null;
    }
}