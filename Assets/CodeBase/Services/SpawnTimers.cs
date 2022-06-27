using System;
using Random = UnityEngine.Random;

namespace CodeBase.Services
{
    public class SpawnTimers
    {
        #region CONST

        private const int EnemySpawnLowerBound = 2;
        private const int EnemySpawnUpperBound = 6;
        
        private const int FreezeSpawnLowerBound = 10;
        private const int FreezeSpawnUpperBound = 25;
        
        private const int BombSpawnLowerBound = 30;
        private const int BombSpawnUpperBound = 60;
        
        private const int DoubleSpawnLowerBound = 20;
        private const int DoubleSpawnUpperBound = 35;

        private readonly int[] DifficultyTimers = {30, 30, 60, 60};

        #endregion

        private readonly SpawnService _spawnService;
        
        private int _enemySpawnTimer;
        private float _enemySpawnCurrent;
        
        private int _freezeSpawnTimer;
        private float _freezeSpawnCurrent;
        
        private int _bombSpawnTimer;
        private float _bombSpawnCurrent;
        
        private int _doubleSpawnTimer;
        private float _doubleSpawnCurrent;
        
        private int _difficultyTimer;
        private float _difficultyCurrent;

        #region EVENTS

        public event Action OnEnemyTimerUp;
        public event Action OnFreezeTimerUp;
        public event Action OnBombTimerUp;
        public event Action OnDoubleTimerUp;
        public event Action OnDifficultyTimerUp;

        #endregion

        public int DifficultyTimerIndex { get; set; }

        public SpawnTimers(SpawnService spawnService) => 
            _spawnService = spawnService;

        public void Update(float deltaTime)
        {
            UpdateEnemySpawnTimer(deltaTime);
        }

        private void UpdateEnemySpawnTimer(float deltaTime)
        {
            _enemySpawnCurrent -= deltaTime;
            
            if (_enemySpawnCurrent >= _enemySpawnTimer) 
                OnEnemyTimerUp?.Invoke();
        }

        public void InitializeTimers()
        {
            _enemySpawnTimer = Random.Range(EnemySpawnLowerBound, EnemySpawnUpperBound);
            _enemySpawnCurrent = 0f;
            
            _freezeSpawnTimer = Random.Range(FreezeSpawnLowerBound, FreezeSpawnUpperBound);
            _freezeSpawnCurrent = 0f;
            
            _bombSpawnTimer = Random.Range(BombSpawnLowerBound, BombSpawnUpperBound);
            _bombSpawnCurrent = 0f;
            
            _doubleSpawnTimer = Random.Range(DoubleSpawnLowerBound, DoubleSpawnUpperBound);
            _doubleSpawnCurrent = 0f;
            
            _difficultyTimer = DifficultyTimers[0];
            _difficultyCurrent = 0f;
            DifficultyTimerIndex = 0;
        }
    }
}