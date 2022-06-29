using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Fabric;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Services.SpawnService
{
    public class SpawnService : MonoBehaviour, IService
    {
        private const string StartPoint = "StartPoint";
        
        private IGameFactory _factory;
        private SpawnTimers _timers;
        private SpawnPoint[,] _spawnPoints = new SpawnPoint[8, 8];
        
        private TextMeshProUGUI _speedText;

        public float Speed { get; private set; } = 1f;
        
        private void Awake()
        {
            _timers = new SpawnTimers(this);

            _timers.OnEnemyTimerUp += SpawnEnemy;
            _timers.OnSpeedTimerUp += IncreaseSpeed;

            _speedText = GameObject.FindWithTag("SpeedText").GetComponent<TextMeshProUGUI>();
        }

        private void Start() => 
            CreateSpawnPoints();

        public void Construct(IGameFactory factory) => 
            _factory = factory;

        private void Update() => 
            _timers.Update(Time.deltaTime * Speed);

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
            var enemy = await _factory.CreateEnemy(enemyType, spawnPoint.transform.position, Speed);
            
             spawnPoint.Unit = enemy;
             spawnPoint.IsFree = false;
            
             enemy.GetComponent<EnemyDeath>().Happened += OnEnemyDeath;
        }

        private void IncreaseSpeed(float speed)
        {
            print(speed);
            Speed = speed;
            _speedText.text = $"x{speed}";
        }

        private void OnEnemyDeath(GameObject enemy)
        {
            foreach (var point in _spawnPoints)
            {
                if (point.Unit != enemy) return;
        
                point.Unit = null;
                point.IsFree = true;
            }
        }

        private SpawnPoint FindFreePoint()
        {
            var (i, j) = (Random.Range(0, 8), Random.Range(0, 8));
            while (!_spawnPoints[i, j].IsFree)
            {
                (i, j) = (Random.Range(0, 8), Random.Range(0, 8));
            }

            var spawnPoint = _spawnPoints[i, j];
            return spawnPoint;
        }
    }
}