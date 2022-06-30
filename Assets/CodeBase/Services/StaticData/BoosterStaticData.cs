using CodeBase.Boosters;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Services.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Booster", fileName = "BoosterData")]
    public class BoosterStaticData : ScriptableObject
    {
        public BoosterType BoosterType;
        
        public AssetReferenceGameObject prefab;
    }
}