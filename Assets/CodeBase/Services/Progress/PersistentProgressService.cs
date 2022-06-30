namespace CodeBase.Services.Progress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        public PlayerProgress Progress { get; }

        public PersistentProgressService() => 
            Progress = new PlayerProgress();
    }
}