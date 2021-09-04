using Project.Scripts.Scriptables;
using UnityEditor.Rendering.LookDev;
using UnityEngine;

namespace Project.Scripts
{
    #region Content
    public interface IContent : IMaterialController
    {
        void SetPosition(Vector3 position);
        
        void SetLayerOrder(int layerOrder);
    }

    public interface IMaterialController
    {
        SkinnedMeshRenderer Renderer{ set; get; }
        MaterialPropertyBlock MaterialPropertyBlock{ set; get; }
    }
    
    public interface IDestroyable
    {
        bool Destroy();
    }
    
    public interface IAttacker
    {
        bool Attack(Vector3 dir);
    }
    
    public interface IMovable
    {
        bool Move(Vector3 dir);
    }
    #endregion

    #region Management

    public interface IManagement
    {
        void Reset();
    }
    
    public delegate void OnGameStartDelegate();
    public delegate void OnLevelStartDelegate();
    public delegate void OnLevelEndDelegate(bool succeed);
    public delegate void OnGameExitDelegate();
    public delegate void OnProgressDelegate(int val);
    public interface IGameManagement : IManagement
    {
        event OnGameStartDelegate OnGameStart;
        event OnLevelStartDelegate OnLevelStart;
        event OnLevelEndDelegate OnLevelEnd;
        event OnGameExitDelegate OnGameEnd;
        event OnProgressDelegate OnProgress;

        void GameStart();
        void GameEnd();
        
    }

    public interface IMapManagement
    {
        void PopulateMap();
    }
    
    public interface ILevelManagement : IManagement, IMapManagement
    {
        LevelDefinitionScriptable LevelDefinitionScriptable { set; get; }
        void PopulateWorld();

        void Progress(int progressAmount);
        void LevelStart();
        void LevelEnd();
    }


    public interface ISoundManagement : IManagement
    {
        
    }

    public interface IVFXManagement : IManagement
    {
        
    }
    

    #endregion
    
}
