using CodeBase.Boosters;
using CodeBase.Enemy;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        EnemyStaticData ForEnemy(EnemyType enemyType);
        BoosterStaticData ForBooster(BoosterType boosterType);
    }
}