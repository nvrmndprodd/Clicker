using System.Threading.Tasks;
using CodeBase.Services;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Infrastructure.AssetManagement
{
    public interface IAssetProvider : IService
    {
        void Initialize();
        Task<T> Load<T>(AssetReference monsterDataPrefabReference) where T : class;
        Task<T> Load<T>(string address) where T : class;
        void Cleanup();
    }
}