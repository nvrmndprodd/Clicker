using System.Threading.Tasks;
using CodeBase.Boosters;
using CodeBase.Common;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Fabric;
using CodeBase.Services.LevelServices.EnemyService;
using CodeBase.Services.Progress;
using UnityEngine;

namespace CodeBase.Services.LevelServices.BoosterService
{
    public class BoosterService : IBoosterService
    {
        private readonly IGameFactory _factory;
        private readonly IPersistentProgressService _progressService;
        private readonly IEnemyService _enemyService;
        
        private RandomTimer _bombTimer;
        private RandomTimer _freezeTimer;
        private RandomTimer _doubleTimer;
        private RandomTimer _freezeActiveTimer;
        private RandomTimer _doubleDamageActiveTimer;

        public BoosterService(IGameFactory factory, IPersistentProgressService progressService, IEnemyService enemyService)
        {
            _factory = factory;
            _progressService = progressService;
            _enemyService = enemyService;

            CreateTimers();
        }

        public void OnUpdate(float deltaTime)
        {
            _bombTimer.UpdateTimer(deltaTime);
            _freezeTimer.UpdateTimer(deltaTime);
            _doubleTimer.UpdateTimer(deltaTime);

            _freezeActiveTimer?.UpdateTimer(deltaTime);
            _doubleDamageActiveTimer?.UpdateTimer(deltaTime);
        }

        public void Clear()
        {
            if (_bombTimer != null) _bombTimer.OnTimerUp -= OnBombTimerUp;
            _bombTimer = null;
            
            if (_freezeTimer != null) _freezeTimer.OnTimerUp -= OnFreezeTimerUp;
            _freezeTimer = null;
            
            if (_doubleTimer != null) _doubleTimer.OnTimerUp -= OnDoubleTimerUp;
            _doubleTimer = null;

            if (_freezeActiveTimer != null) _freezeActiveTimer.OnTimerUp -= Unfreeze;
            _freezeActiveTimer = null;
            
            if (_doubleDamageActiveTimer != null) _doubleDamageActiveTimer.OnTimerUp -= DisableDoubleDamage;
            _doubleDamageActiveTimer = null;
        }

        private void CreateTimers()
        {
            if (_bombTimer != null) _bombTimer.OnTimerUp -= OnBombTimerUp;
            _bombTimer = new RandomTimer(30, 61);
            _bombTimer.OnTimerUp += OnBombTimerUp;
            
            if (_freezeTimer != null) _freezeTimer.OnTimerUp -= OnFreezeTimerUp;
            _freezeTimer = new RandomTimer(10, 26);
            _freezeTimer.OnTimerUp += OnFreezeTimerUp;

            if (_doubleTimer != null) _doubleTimer.OnTimerUp -= OnDoubleTimerUp;
            _doubleTimer = new RandomTimer(20, 36);
            _doubleTimer.OnTimerUp += OnDoubleTimerUp;
        }

        private async void OnBombTimerUp() => 
            await CreateBooster(BoosterType.Bomb);

        private async void OnFreezeTimerUp()
        {
            var booster = await CreateBooster(BoosterType.Freeze);
            booster.GetComponent<EnemyDeath>().Happened += Freeze;
        }

        private void Freeze(GameObject obj)
        {
            _freezeActiveTimer = new RandomTimer(5, 6);
            _freezeActiveTimer.OnTimerUp += Unfreeze;
            _progressService.Progress.LevelProgress.Freezed = true;
        }

        private void Unfreeze()
        {
            _progressService.Progress.LevelProgress.Freezed = false;
            _freezeActiveTimer = null;
        }

        private async void OnDoubleTimerUp()
        {
            var booster = await CreateBooster(BoosterType.DoubleDamage);
            booster.GetComponent<EnemyDeath>().Happened += DoubleDamage;
        }

        private void DoubleDamage(GameObject obj)
        {
            _doubleDamageActiveTimer = new RandomTimer(5, 6);
            _doubleDamageActiveTimer.OnTimerUp += DisableDoubleDamage;
            _progressService.Progress.PlayerStats.Damage = 2;
        }

        private void DisableDoubleDamage()
        {
            _progressService.Progress.PlayerStats.Damage = 1;
            _doubleDamageActiveTimer = null;
        }

        private async Task<GameObject> CreateBooster(BoosterType type)
        {
            var spawnPoint = _enemyService.FindFreePoint();
            var bomb = await _factory.CreateBooster(type, spawnPoint.transform.position);

            spawnPoint.Unit = bomb;
            spawnPoint.IsFree = false;

            bomb.GetComponent<EnemyDeath>().Happened += _enemyService.OnEnemyDeath;
            return bomb;
        }
    }
}