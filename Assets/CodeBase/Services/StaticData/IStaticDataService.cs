using CodeBase.Enemy;

namespace CodeBase.Services.StaticData
{
    public interface IStaticDataService : IService
    {
        public EnemyStaticData ForEnemy(EnemyType enemyType);
    }
}