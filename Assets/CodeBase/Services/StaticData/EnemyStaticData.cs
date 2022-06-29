using CodeBase.Enemy;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace CodeBase.Services.StaticData
{
    [CreateAssetMenu(menuName = "StaticData/Enemy", fileName = "EnemyData")]
    public class EnemyStaticData : ScriptableObject
    {
        public EnemyType EnemyType;
        
        public AssetReferenceGameObject Prefab;

        [Range(1, 5)] 
        public int Hp = 1;
    }
}