using UnityEngine;

namespace Project.Scripts.Scriptables
{
    [CreateAssetMenu(fileName = "LevelDefinitionScriptable", menuName = "ScriptableObjects/LevelDefinitionScriptable", order = 0)]
    public class LevelDefinitionScriptable : ScriptableObject
    {
        public MapDefinition MapDefinition;
        
    }
    
    [System.Serializable]
    public class MapDefinition
    {
        [SerializeField,Range(0f, 10000f)] public int Seed;
        [SerializeField,Range(0f, 20f)] public int AICharacterCount;
        [SerializeField,Range(0f, 85f)] public int DestroyableSpawnProbability;

        public GameObject PlayerPrefab;
        
        public GameObject GroundPrefab;
        public GameObject WallPrefab;
        public GameObject EmptyWallPrefab;
        public GameObject ObstaclePrefab;
        
        public Sprite[] DestroyableTextures;
        public GameObject AICharacterPrefab;

        public Vector2Int MatrixScale;
        public int MatrixLength => MatrixScale.x * MatrixScale.y;
    }
}