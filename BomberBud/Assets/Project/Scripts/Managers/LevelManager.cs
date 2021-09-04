using Project.Scripts.Scriptables;
using UnityEngine;
using xOrfe.Utilities;

namespace Project.Scripts.Managers
{
    public abstract class LevelManager : SingletonUtility<LevelManager> , ILevelManagement
    {
        public LevelDefinitionScriptable LevelDefinitionScriptable { get; set; }
        
        public void PopulateWorld()
        {
            throw new System.NotImplementedException();
        }
        
        public void PopulateMap()
        {
            throw new System.NotImplementedException();
        }
        
        public void Progress(int progressAmount)
        {
            throw new System.NotImplementedException();
        }

        public void LevelStart()
        {
            throw new System.NotImplementedException();
        }

        public void LevelEnd()
        {
            throw new System.NotImplementedException();
        }
        
        public void Reset()
        {
            throw new System.NotImplementedException();
        }
    }
}