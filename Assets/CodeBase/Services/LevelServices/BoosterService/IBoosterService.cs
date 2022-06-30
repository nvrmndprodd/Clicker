namespace CodeBase.Services.LevelServices.BoosterService
{
    public interface IBoosterService : IService
    {
        public void OnUpdate(float deltaTime);
        public void Clear();
    }
}