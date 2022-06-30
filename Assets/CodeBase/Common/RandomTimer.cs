using System;
using Random = UnityEngine.Random;

namespace CodeBase.Common
{
    public class RandomTimer
    {
        public float Time;
        public event Action OnTimerUp;
        
        private readonly int _lowerBound;
        private readonly int _upperBound;

        public RandomTimer(int lowerBound, int upperBound)
        {
            _lowerBound = lowerBound;
            _upperBound = upperBound;

            Time = Random.Range(_lowerBound, _upperBound);
        }

        public void UpdateTimer(float deltaTime)
        {
            Time -= deltaTime;
            if (Time > 0) return;

            Time = Random.Range(_lowerBound, _upperBound);
            OnTimerUp?.Invoke();
        }
    }
}