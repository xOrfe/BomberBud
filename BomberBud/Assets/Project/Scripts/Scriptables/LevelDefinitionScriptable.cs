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
        public Vector2Int MatrixScale{ set; get; }
        public string MatMatrixName { set; get; }

        public MapDefinition(Vector2Int matrixScale)
        {
            MatrixScale = matrixScale;
            
        }
        
    }
}