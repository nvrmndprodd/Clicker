using System.Collections.Generic;
using System.Linq;
using CodeBase.Services.Progress;

namespace CodeBase.Services.LevelServices.SpeedService
{
    public class SpeedService : ISpeedService
    {
        private readonly IPersistentProgressService _progressService;

        private readonly Dictionary<int, int> _speedTimers = new Dictionary<int, int>()
        {
            {20, 2}, 
            {30, 3}, 
            {45, 5}, 
            {60, 10}
        };

        private int _speedTimerIndex;
        private float _timer;
        private bool _speedIsMax = false;

        public SpeedService(IPersistentProgressService progressService)
        {
            _progressService = progressService;
            _timer = _speedTimers.Keys.ToList()[0];
        }

        public void OnUpdate(float deltaTime)
        {
            if (_speedIsMax) return;

            _timer -= deltaTime;
            
            if (_timer <= 0) 
                OnTimerUp();
        }

        public void Clear()
        {
            _speedTimerIndex = 0;
            _timer = _speedTimers.Keys.ToList()[0];
            _speedIsMax = false;
            _progressService.Progress.LevelProgress.Speed = 1;
        }

        private void OnTimerUp()
        {
            _progressService.Progress.LevelProgress.Speed = _speedTimers.Values.ToList()[_speedTimerIndex];
            
            if (_speedTimerIndex >= _speedTimers.Count - 1)
            {
                _speedIsMax = true;
                return;
            }
            
            var timers = _speedTimers.Keys.ToList();
            _timer = timers[_speedTimerIndex + 1];
            _speedTimerIndex++;
        }
    }
}