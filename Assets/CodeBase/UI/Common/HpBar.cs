using System;
using CodeBase.Enemy;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.Common
{
    public class HpBar : MonoBehaviour
    {
        public EnemyHealth Health;
        public Image ImageCurrent;

        private void Awake() => 
            Health.HealthChanged += OnHealthChanged;

        private void OnDestroy() => 
            Health.HealthChanged -= OnHealthChanged;

        private void OnHealthChanged() => 
            ImageCurrent.fillAmount = Health.Current / Health.Max;
    }
}