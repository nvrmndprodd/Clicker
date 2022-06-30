using System.Threading.Tasks;
using CodeBase.Boosters;
using CodeBase.Enemy;
using CodeBase.Services;
using UnityEngine;

namespace CodeBase.Infrastructure.Fabric
{
    public interface IGameFactory : IService
    {
        Task<GameObject> CreateEnemy(EnemyType enemyType, Vector3 at, float multiplier);
        Task<GameObject> CreateBooster(BoosterType boosterType, Vector3 at);
    }
}