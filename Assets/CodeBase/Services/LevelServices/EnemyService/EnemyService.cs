using CodeBase.Common;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Fabric;
using CodeBase.Services.Progress;
using UnityEngine;

namespace CodeBase.Services.LevelServices.EnemyService
{
    public class EnemyService : IEnemyService
    {
        private const string StartPoint = "StartPoint";
        
        private readonly IPersistentProgressService _progressService;
        private readonly IGameFactory _factory;
        private RandomTimer _timer;
        private SpawnPoint[,] _spawnPoints = new SpawnPoint[8, 8];

        public EnemyService(IPersistentProgressService progressService, IGameFactory gameFactory)
        {
            _progressService = progressService;
            _factory = gameFactory;

            _timer = new RandomTimer(2, 7);
            _timer.OnTimerUp += SpawnEnemy;
            
            CreateSpawnPoints();
        }

        public void OnUpdate(float deltaTime)
        {
            if (_progressService.Progress.LevelProgress.Freezed)
            {
                Debug.Log("Freezed");
                return;
            }
            _timer.UpdateTimer(deltaTime * _progressService.Progress.LevelProgress.Speed);
            Debug.Log(_timer.Time);
        }

        public SpawnPoint FindFreePoint()
        {
            var (i, j) = (Random.Range(0, 8), Random.Range(0, 8));
            while (!_spawnPoints[i, j].IsFree)
            {
                (i, j) = (Random.Range(0, 8), Random.Range(0, 8));
            }

            var spawnPoint = _spawnPoints[i, j];
            return spawnPoint;
        }

        public void OnEnemyDeath(GameObject enemy)
        {
            foreach (var point in _spawnPoints)
            {
                if (point.Unit != enemy) return;
        
                point.Unit = null;
                point.IsFree = true;
            }
        }

        public void Clear()
        {
            foreach (var spawnPoint in _spawnPoints)
                if (spawnPoint)
                    Object.Destroy(spawnPoint);

            _spawnPoints = new SpawnPoint[8, 8];
            _timer.OnTimerUp -= SpawnEnemy;
            _timer = new RandomTimer(2, 7);
            _timer.OnTimerUp += SpawnEnemy;
            
            CreateSpawnPoints();
        }

        private void CreateSpawnPoints()
        {
            var startPoint = GameObject.FindWithTag(StartPoint);

            for (var i = 0; i < 8; ++i)
            {
                for (var j = 0; j < 8; ++j)
                {
                    var offset = new Vector3(-2 * j, 0, 2 * i);
                    var go = new GameObject($"SpawnPoint{i}-{j}")
                    {
                        transform =
                        {
                            position = startPoint.transform.position + offset
                        }
                    };
                    var spawnPoint = go.AddComponent<SpawnPoint>();
                    _spawnPoints[i, j] = spawnPoint;
                }
            }
        }

        private async void SpawnEnemy()
        {
            var enemyType = (EnemyType) Random.Range(0, 3);
            
            var spawnPoint = FindFreePoint();
            var enemy = await _factory
                .CreateEnemy(enemyType, spawnPoint.transform.position, _progressService.Progress.LevelProgress.Speed);
            
            spawnPoint.Unit = enemy;
            spawnPoint.IsFree = false;
            
            enemy.GetComponent<EnemyDeath>().Happened += OnEnemyDeath;
        }
    }
}