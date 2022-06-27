using System;

namespace CodeBase.Services.SceneManagement
{
    public interface ISceneLoader
    {
        void Load(string name, Action onLoaded = null);
    }
}