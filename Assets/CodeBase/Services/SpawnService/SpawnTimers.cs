using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace CodeBase.Services.SpawnService
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

        private readonly Dictionary<int, float> SpeedTimers = new Dictionary<int, float>()
        {
            {20, 1.2f}, 
            {30, 1.5f}, 
            {45, 2f}, 
            {60, 3f}
        };

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
        
        private int _speedTimer;
        private float _speedCurrent;
        private bool _speedIsMax = false;

        #region EVENTS

        public event Action OnEnemyTimerUp;
        public event Action OnFreezeTimerUp;
        public event Action OnBombTimerUp;
        public event Action OnDoubleTimerUp;
        public event Action<float> OnSpeedTimerUp;

        #endregion

        public int SpeedTimerIndex { get; set; }

        public SpawnTimers(SpawnService spawnService)
        {
            _spawnService = spawnService;
            InitializeTimers();
        }

        public void Update(float deltaTime)
        {
            UpdateEnemySpawnTimer(deltaTime);
            UpdateFreezeSpawnTimer(deltaTime);
            UpdateBombSpawnTimer(deltaTime);
            UpdateDoubleTimer(deltaTime);

            if (!_speedIsMax)
                UpdateSpeedTimer(deltaTime);
        }

        private void InitializeTimers()
        {
            _enemySpawnTimer = Random.Range(EnemySpawnLowerBound, EnemySpawnUpperBound);
            _enemySpawnCurrent = 0f;
            
            _freezeSpawnTimer = Random.Range(FreezeSpawnLowerBound, FreezeSpawnUpperBound);
            _freezeSpawnCurrent = 0f;
            
            _bombSpawnTimer = Random.Range(BombSpawnLowerBound, BombSpawnUpperBound);
            _bombSpawnCurrent = 0f;
            
            _doubleSpawnTimer = Random.Range(DoubleSpawnLowerBound, DoubleSpawnUpperBound);
            _doubleSpawnCurrent = 0f;
            
            _speedTimer = 20;
            _speedCurrent = 0f;
            SpeedTimerIndex = 0;
        }

        private void UpdateEnemySpawnTimer(float deltaTime)
        {
            _enemySpawnCurrent += deltaTime;
            
            if (_enemySpawnCurrent < _enemySpawnTimer) return;
            
            OnEnemyTimerUp?.Invoke();
            _enemySpawnTimer = Random.Range(EnemySpawnLowerBound, EnemySpawnUpperBound);
            _enemySpawnCurrent = 0f;
        }
        private void UpdateFreezeSpawnTimer(float deltaTime)
        {
            _freezeSpawnCurrent += deltaTime;

            if (_freezeSpawnCurrent < _freezeSpawnTimer) return;
            
            OnFreezeTimerUp?.Invoke();
            _freezeSpawnTimer = Random.Range(FreezeSpawnLowerBound, FreezeSpawnUpperBound);
            _freezeSpawnCurrent = 0f;
        }
        private void UpdateBombSpawnTimer(float deltaTime)
        {
            _bombSpawnCurrent += deltaTime;

            if (_bombSpawnCurrent < _bombSpawnTimer) return;
            
            OnBombTimerUp?.Invoke();
            _bombSpawnTimer = Random.Range(BombSpawnLowerBound, BombSpawnUpperBound);
            _bombSpawnCurrent = 0f;
        }
        private void UpdateDoubleTimer(float deltaTime)
        {
            _doubleSpawnCurrent += deltaTime;

            if (_doubleSpawnCurrent < _doubleSpawnTimer) return;
            
            OnDoubleTimerUp?.Invoke();
            _doubleSpawnTimer = Random.Range(DoubleSpawnLowerBound, DoubleSpawnUpperBound);
            _doubleSpawnCurrent = 0f;
        }
        private void UpdateSpeedTimer(float deltaTime)
        {
            _speedCurrent += deltaTime / _spawnService.Speed;

            if (_speedCurrent < _speedTimer) return;
            
            OnSpeedTimerUp?.Invoke(SpeedTimers[_speedTimer]);

            if (SpeedTimerIndex >= SpeedTimers.Count - 1)
            {
                _speedIsMax = true;
                return;
            }

            var timers = SpeedTimers.Keys.ToList();
            _speedTimer = timers[SpeedTimerIndex + 1];
            SpeedTimerIndex++;
            _speedCurrent = 0f;
        }
    }
}