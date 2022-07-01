using CodeBase.Enemy;
using CodeBase.Services;
using CodeBase.Services.Progress;
using UnityEngine;

namespace CodeBase.Common
{
    public class InputService : MonoBehaviour, IService
    {
        private IPersistentProgressService _progressService;
        private int _layerMask;

        public void Construct(IPersistentProgressService progressService) => 
            _progressService = progressService;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            _layerMask = 1 << LayerMask.NameToLayer("Enemy");
        }

        void Update()
        {
            RaycastHit hit = new RaycastHit();
            for (int i = 0; i < Input.touchCount; ++i)
            {
                if (Input.GetTouch(i).phase.Equals(TouchPhase.Began))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(i).position);
                    if (Physics.Raycast(ray, out hit))
                        if (hit.transform.TryGetComponent<EnemyHealth>(out var enemyHealth))
                            enemyHealth.TakeDamage(_progressService.Progress.PlayerStats.Damage);
                }
            }
        }
    }
}