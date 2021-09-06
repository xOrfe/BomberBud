using System.Collections.Generic;
using Project.Scripts.Characters;
using Project.Scripts.Scriptables;
using UnityEngine;
using xOrfe.Utilities;
using Random = UnityEngine.Random;
using Utils = Project.Scripts.Utilities.Utilities;

namespace Project.Scripts.Managers
{
    public class LevelManager : SingletonUtility<LevelManager>,ILevelManagement 
    {
        [SerializeField]private LevelDefinitionScriptable _levelDefinitionScriptable;
        public LevelDefinitionScriptable LevelDefinitionScriptable
        {
            get => _levelDefinitionScriptable;
            set => _levelDefinitionScriptable = value;
        }
        
        [SerializeField]private MapChunk[] _mapChunkMatrix;
        public MapChunk[] MapChunkMatrix
        {
            get => _mapChunkMatrix;
            set => _mapChunkMatrix = value;
        }
        
        public PlayerCharacterBase PlayerCharacterBase { get; set; }
        public AICharacterBase[] AICharacterBases { get; set; }
        private void Start()
        {
            PopulateWorld();
            //GameManager.Instance.LevelStart(this);
            Random.InitState(LevelDefinitionScriptable.MapDefinition.Seed);
            GameManager.Instance.IsGameplayRunning = true;
        }
        public void PopulateWorld()
        {
            PopulateMap();
            GameObject player = Instantiate(LevelDefinitionScriptable.MapDefinition.PlayerPrefab);
            player.GetComponent<Content>().CurrentChunk = new Vector2Int(1, 2);
            player.transform.position = Utils.GetWorldFromIndex(LevelDefinitionScriptable.MapDefinition.MatrixScale.x * 2 + 1,LevelDefinitionScriptable.MapDefinition.MatrixScale);
        }
        
        public void PopulateMap()
        {
            SetupMapChunkMatrix();
            PopulateOuterWalls();
            for (int y = 1; y < LevelDefinitionScriptable.MapDefinition.MatrixScale.y -1; y++)
            {
                for (int x = 1; x < LevelDefinitionScriptable.MapDefinition.MatrixScale.x - 1; x++)
                {
                    bool isWall = ((x % 2) == 0) && ((y % 2) == 0);
                    GameObject go = isWall
                        ? LevelDefinitionScriptable.MapDefinition.WallPrefab
                        : LevelDefinitionScriptable.MapDefinition.GroundPrefab;
                    Vector2Int coord = new Vector2Int(x, y);
                    CreateContent(coord, go);
                }
            }
            int createdAI = 0;
            foreach (var chunk in MapChunkMatrix)
            {
                if (chunk.isRigid || chunk.Coord.x < 3 || chunk.Coord.y < 3) continue;
                
                if (Random.Range(0, 100) > LevelDefinitionScriptable.MapDefinition.DestroyableSpawnProbability)
                {
                    if ((Random.Range(0, 100) > 70) && createdAI < LevelDefinitionScriptable.MapDefinition.AICharacterCount)
                    {
                        createdAI++;
                        CreateContent(chunk.Coord, LevelDefinitionScriptable.MapDefinition.AICharacterPrefab);
                    }
                    continue;
                }
                CreateContent(chunk.Coord, LevelDefinitionScriptable.MapDefinition.ObstaclePrefab);
            }
        }
        private void SetupMapChunkMatrix()
        {
            MapChunkMatrix = new MapChunk[LevelDefinitionScriptable.MapDefinition.MatrixLength];
            for (int i = 0; i < LevelDefinitionScriptable.MapDefinition.MatrixLength; i++)
            {
                MapChunkMatrix[i] = new MapChunk(Utils.GetCoordFromIndex(i, LevelDefinitionScriptable.MapDefinition.MatrixScale));
            }
        }
        private void PopulateOuterWalls()
        {
            GameObject go = LevelDefinitionScriptable.MapDefinition.WallPrefab;
            GameObject EmptyWallGo = LevelDefinitionScriptable.MapDefinition.EmptyWallPrefab;
            
            CreateContent(new Vector2Int(1, 1), go);
            CreateContent(new Vector2Int(LevelDefinitionScriptable.MapDefinition.MatrixScale.x - 2, 1), go);

            
            for (int x = 1; x < LevelDefinitionScriptable.MapDefinition.MatrixScale.y - 1; x++)
            {
                CreateContent(new Vector2Int(0, x), go);
                CreateContent(new Vector2Int(LevelDefinitionScriptable.MapDefinition.MatrixScale.x - 1, x), go);
            }
            
            for (int y = 0; y < LevelDefinitionScriptable.MapDefinition.MatrixScale.x; y++)
            {
                CreateContent(new Vector2Int(y, 0), go);
                CreateContent(new Vector2Int(y,LevelDefinitionScriptable.MapDefinition.MatrixScale.y - 1), go);
            }
            
            for (int x = 0; x < LevelDefinitionScriptable.MapDefinition.MatrixScale.y ; x++)
            {
                CreateContent(new Vector2Int(-1, x), EmptyWallGo,true);
                CreateContent(new Vector2Int(LevelDefinitionScriptable.MapDefinition.MatrixScale.x , x), EmptyWallGo,true);
            }
            for (int y = -1; y < LevelDefinitionScriptable.MapDefinition.MatrixScale.x + 1; y++)
            {
                CreateContent(new Vector2Int(y, -1), EmptyWallGo,true);
                CreateContent(new Vector2Int(y,LevelDefinitionScriptable.MapDefinition.MatrixScale.y), EmptyWallGo,true);
            }
            
        }
        public void CreateContent(Vector2Int pos,GameObject prefab, bool isEmpty = false)
        {
            GameObject go = Instantiate(prefab);
            go.transform.position = Utils.GetWorldFromCoordinate(pos,LevelDefinitionScriptable.MapDefinition.MatrixScale);
            if (isEmpty) return;
            int index = Utils.GetIndexFromCoord(pos,LevelDefinitionScriptable.MapDefinition.MatrixScale);
            Content content = go.GetComponent<Content>();
            MapChunkMatrix[index].Add(content);
            content.CurrentChunk = pos;

        }
        public void Reset()
        {
            throw new System.NotImplementedException();
        }
        
    }

    [System.Serializable]
    public struct MapChunk
    {
        //
        //TODO might be rigid sync Problem at runtime
        //

        public int[] LayerRigidContentCounts;
        public int[] LayerNonRigidContentCounts;
        
        public readonly Vector2Int Coord;
        public List<Content> ContentsRigid;
        public List<Content> ContentsNonRigid;
        
        public bool isRigid => ContentsRigid.Count > 0;

        public MapChunk(Vector2Int Coord)
        {
            LayerRigidContentCounts = new int[3];
            LayerNonRigidContentCounts = new int[3];
            ContentsRigid = new List<Content>();
            ContentsNonRigid = new List<Content>();
            this.Coord = Coord;
        }
        
        public void Add(Content content)
        {
            if (content.IsRigid)
            {
                ContentsRigid.Add(content);
                foreach (var layer in content.PhysicsLayers) LayerRigidContentCounts[layer]++;
            }
            else
            {
                ContentsNonRigid.Add(content);
                foreach (var layer in content.PhysicsLayers) LayerNonRigidContentCounts[layer]++;
            }
        }
        public void Remove(Content content)
        {
            if (content.IsRigid)
            {
                ContentsRigid.Remove(content);
                foreach (var layer in content.PhysicsLayers) LayerRigidContentCounts[layer]--;
            }
            else
            {
                ContentsNonRigid.Remove(content);
                foreach (var layer in content.PhysicsLayers) LayerNonRigidContentCounts[layer]--;
            }
            
        }
    }
}