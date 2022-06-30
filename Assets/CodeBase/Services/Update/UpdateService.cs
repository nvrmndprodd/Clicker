using System;
using UnityEngine;

namespace CodeBase.Services.Update
{
    public class UpdateService : MonoBehaviour, IUpdateService
    {
        public event Action<float> OnUpdate;

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update() => 
            OnUpdate?.Invoke(Time.deltaTime);
    }
}