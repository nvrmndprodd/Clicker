using System.Collections;
using UnityEngine;

namespace CodeBase.Boosters
{
    public class BoosterEffect : MonoBehaviour
    {
        public CanvasGroup Canvas;
    
        private float _timer = 0;
        private bool _timerIsActive;

        private void Update()
        {
            if (!_timerIsActive) return;
        
            _timer -= Time.deltaTime;
        
            if (_timer <= 0)
            {
                StartCoroutine(FadeIn());
                _timerIsActive = false;
            }
        }

        public void Activate(float time)
        {
            Canvas.alpha = 1f;
        
            _timerIsActive = true;
            _timer = time;
        }

        private IEnumerator FadeIn()
        {
            while (Canvas.alpha > 0)
            {
                Canvas.alpha -= 0.03f;
                yield return new WaitForSeconds(0.03f);
            }
        }
    }
}
