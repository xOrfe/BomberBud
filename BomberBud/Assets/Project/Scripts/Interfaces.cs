using System.Collections.Generic;
using Project.Scripts.Characters;
using Project.Scripts.Managers;
using Project.Scripts.Scriptables;
using UnityEngine;

namespace Project.Scripts
{
    #region Content
    public interface IContent : IPhysics, IMaterialController
    {
        ContentType ContentType { set; get; }
        Vector2Int CurrentChunk { set; get; }
        void SetPosition(Vector3 position);
    }
    public interface IPhysics: IMovable
    {
        Vector2 Position { get; }
        int[] PhysicsLayers { set; get; }
        bool InPhysicsProcessorQueue { set; get; }
        bool IsRigid { set; get; }
        bool IsStatic { set; get; }
        Vector2 Velocity { set; get; }
        float Mass { set; get; }
        float Friction { set; get; }
        string CollisionTag { set; get; }
        bool isDestroyable { set; get; }
        bool Destroy();
        void OnCollision(Content content);
    }
    public interface IMovable
    {
        void Move(Vector2 dir);
        void AddForce(Vector2 dir,float force);
    }
    public interface IMaterialController
    {
        SkinnedMeshRenderer Renderer{ set; get; }
        void SetLayerOrder(int layerOrder);
        MaterialPropertyBlock MaterialPropertyBlock { set; get; }
    }
    public interface IAttacker
    {
        bool Attack();
    }
    
    
    #endregion

    #region Management

    public interface IManagement
    {
        void Reset();
    }
    
    public delegate void OnGameStartDelegate();
    public delegate void OnLevelStartDelegate(LevelManager levelManager);
    public delegate void OnLevelEndDelegate(bool succeed);
    public delegate void OnGameExitDelegate();
    public delegate void OnProgressDelegate(int val);
    public interface IGameManagement : IManagement
    {
        int CurrentLevel { get; set; }
        LevelDefinitionScriptable[] LevelDefinitions { get; set; }
        
        event OnGameStartDelegate OnGameStart;
        event OnLevelStartDelegate OnLevelStart;
        event OnLevelEndDelegate OnLevelEnd;
        event OnGameExitDelegate OnGameEnd;
        event OnProgressDelegate OnProgress;

        bool IsGameplayRunning { get; set; }
        void Progress(int progressAmount);
        void GameStart();
        void LevelStart(LevelManager levelManager);
        void LevelEnd(bool succeed);
        void GameEnd();

    }

    public interface IMapManagement
    {
        PlayerCharacterBase PlayerCharacterBase { get; set; }
        AICharacterBase[] AICharacterBases { get; set; }
        MapChunk[] MapChunkMatrix { get; set; }
        void PopulateMap();
    }
    
    public interface ILevelManagement : IManagement, IMapManagement
    {
        LevelDefinitionScriptable LevelDefinitionScriptable { set; get; }
        void PopulateWorld();
        
    }


    public interface ISoundManagement : IManagement
    {
        
    }

    public interface IVFXManagement : IManagement
    {
        
    }
    public interface IPhysicsProcessor : IManagement
    {
        float Gravity { set; get; }
        LevelManager LevelManager { set; get; }
        List<Content> ProcessQueue { set; get; }
        void LevelStart(LevelManager levelManager);
        void Add(Content physics);
        void Remove(Content physics);
    }

    #endregion
    
}
