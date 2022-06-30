namespace CodeBase.Services.Progress
{
    public class PlayerProgress
    {
        public LevelProgress LevelProgress;
        public Stats PlayerStats;

        public PlayerProgress()
        {
            LevelProgress = new LevelProgress();
            PlayerStats = new Stats();
        }
    }
}