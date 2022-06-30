using System;

namespace CodeBase.Services.Update
{
    public interface IUpdateService : IService
    {
        public event Action<float> OnUpdate;
    }
}