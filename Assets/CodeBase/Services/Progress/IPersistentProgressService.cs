namespace CodeBase.Services.Progress
{
    public interface IPersistentProgressService : IService
    {
        PlayerProgress Progress { get; }
    }
}