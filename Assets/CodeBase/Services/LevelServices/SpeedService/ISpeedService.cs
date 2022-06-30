namespace CodeBase.Services.LevelServices.SpeedService
{
    public interface ISpeedService : IService
    {
        public void OnUpdate(float deltaTime);
        public void Clear();
    }
}