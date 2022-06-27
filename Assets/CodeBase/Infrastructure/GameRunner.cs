using UnityEngine;

namespace CodeBase.Infrastructure
{
    public class GameRunner : MonoBehaviour
    {
        public GameBootstrapper BootstrapperPrefab;
        
        private void Awake()
        {
            Application.targetFrameRate = 60;
            
            var bootstrapper = FindObjectOfType<GameBootstrapper>();

            if(bootstrapper != null) return;

            Instantiate(BootstrapperPrefab);
        }
    }
}