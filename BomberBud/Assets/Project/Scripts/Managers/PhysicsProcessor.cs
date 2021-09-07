using System;
using System.Collections.Generic;
using Project.Scripts.Characters;
using UnityEngine;
using xOrfe.Utilities;
using Utils = Project.Scripts.Utilities.Utilities;

namespace Project.Scripts.Managers
{
    public class PhysicsProcessor : SingletonUtility<PhysicsProcessor>, IPhysicsProcessor
    {
        public float Gravity { get; set; }
        
        [SerializeField] private PlayerCharacterBase _playerCharacterBase;
        public PlayerCharacterBase PlayerCharacterBase
        {
            get => _playerCharacterBase;
            set => _playerCharacterBase = value;
        }

        public LevelManager LevelManager { get; set; }
        
        [SerializeField]private List<Content> _processQueue;
        public List<Content> ProcessQueue
        {
            get => _processQueue;
            set => _processQueue = value;
        }
        
        [SerializeField]private List<Content> _dummieQueue;

        public List<Content> DummieQueue
        {
            get => _dummieQueue;
            set => _dummieQueue = value;
        }

        private void Awake()
        {
            ProcessQueue = new List<Content>();
            DummieQueue = new List<Content>();
        }

        private void OnEnable()
        {
            GameManager.Instance.OnLevelStart += LevelStart;
            GameManager.Instance.OnLevelEnd += LevelEnd;
        }

        private void Update()
        {
            if (!GameManager.Instance.IsGameplayRunning) return;
            
            float deltaTime = Time.deltaTime;
            CalculateQueue(deltaTime);
            CalculateDummies(deltaTime);
            CalculatePlayer(deltaTime);
        }

        private void CalculatePlayer(float deltaTime)
        {
            if(_playerCharacterBase.Velocity != Vector2.zero)
                CalculatePhysicsAndTryToMove(_playerCharacterBase,deltaTime);
            
            Vector2Int matrixScale = Managers.LevelManager.Instance.LevelDefinitionScriptable.MapDefinition.MatrixScale;
                
            int chunkIndex = Utils.GetIndexFromCoord(_playerCharacterBase.CurrentChunk,matrixScale);

            CheckCollisionInChunkNonRigid(_playerCharacterBase, chunkIndex);
            CheckCollisionInChunkNonRigid(_playerCharacterBase, chunkIndex + 1);
            CheckCollisionInChunkNonRigid(_playerCharacterBase, chunkIndex - 1);
            CheckCollisionInChunkNonRigid(_playerCharacterBase, chunkIndex + matrixScale.x);
            CheckCollisionInChunkNonRigid(_playerCharacterBase, chunkIndex - matrixScale.x);
                
        }
        private void CalculateQueue(float deltaTime)
        {
            for(int i = 0 ; i < ProcessQueue.Count ; i++)
            {
                Content content = ProcessQueue[i];
                CalculatePhysicsAndTryToMove(content,deltaTime);
            }
        }

        private void CalculatePhysicsAndTryToMove(Content content,float deltaTime)
        {
            //f =  g * m * fc
            float frictionPower = (content.Friction * content.Mass) * deltaTime;
            bool xMove = Math.Abs(content.Velocity.x) > frictionPower;
            bool yMove = Math.Abs(content.Velocity.y) > frictionPower;
                
            //v = v - f
            content.Velocity = new Vector2(
                (xMove ? content.Velocity.x - (frictionPower * (content.Velocity.x > 0 ? 1 : -1)) : 0),
                (yMove ? content.Velocity.y - (frictionPower * (content.Velocity.y > 0 ? 1 : -1)) : 0));
                
            //p = p + v
            if (xMove || yMove)
                CanIMovePlease(content.Velocity * deltaTime , content);
            else
                Remove(content);
        }

        private void CalculateDummies(float deltaTime)
        {
            foreach (var dummie in _dummieQueue)
            {
                if(!dummie.InPhysicsProcessorQueue)CanIMovePlease(dummie.Velocity * deltaTime,dummie);
            }
        }
        public static void CanIMovePlease(Vector2 move,Content content)
        {
            Vector2 currentPos = content.gameObject.transform.position;
            Vector2Int moveChunk = content.CurrentChunk;
                
            Vector2Int matrixScale = Managers.LevelManager.Instance.LevelDefinitionScriptable.MapDefinition.MatrixScale;
            if (move.x != 0)
            {
                content.Velocity = new Vector2(content.Velocity.x, 0);
                if ((currentPos.y % 1) != 0) currentPos.y = Mathf.Round(currentPos.y);
                moveChunk = new Vector2Int(0,(int)currentPos.y);
                moveChunk.x = (int)Mathf.Round(currentPos.x) + (move.x > 0 ?  +1 : -1 );
                moveChunk += matrixScale / 2;
                currentPos += new Vector2(move.x,0);
            }
            else
            {
                content.Velocity = new Vector2(0, content.Velocity.y);
                if ((currentPos.x % 1) != 0) currentPos.x = Mathf.Round(currentPos.x);
                moveChunk = new Vector2Int((int)currentPos.x,0);
                moveChunk.y = (int)Mathf.Round(currentPos.y) + (move.y > 0 ?  +1 : -1 );
                moveChunk += matrixScale / 2;
                currentPos += new Vector2(0,move.y);
            }
            
            int moveChunkIndex = Utils.GetIndexFromCoord(moveChunk, matrixScale);


            if (moveChunkIndex < 0 || moveChunkIndex >
                Managers.LevelManager.Instance.LevelDefinitionScriptable.MapDefinition.MatrixLength - 1)
            {
                return;
            }

            if (CheckCollisionInChunk(content,moveChunkIndex)) return;
            
            Vector2 moveChunkPos = Utils.GetWorldFromCoordinate(moveChunk,matrixScale);
            
            if ((moveChunk != content.CurrentChunk)
                && Vector2.Distance(currentPos, moveChunkPos) < 0.5f)
            {
                int currentChunkIndex = Utils.GetIndexFromCoord(content.CurrentChunk, matrixScale);
                Managers.LevelManager.Instance.MapChunkMatrix[currentChunkIndex].Remove(content);
                Managers.LevelManager.Instance.MapChunkMatrix[moveChunkIndex].Add(content);
                content.CurrentChunk = moveChunk;
            }
                
            content.gameObject.transform.position = currentPos;
        }

        private static bool CheckCollisionInChunk(Content content,int chunkIndex)
        {
            if (Managers.LevelManager.Instance.MapChunkMatrix[chunkIndex].isRigid 
                && Managers.LevelManager.Instance.MapChunkMatrix[chunkIndex].LayerRigidContentCounts[content.PhysicsLayers[0]] > 0)
            {
                foreach (var rigidContent in Managers.LevelManager.Instance.MapChunkMatrix[chunkIndex].ContentsRigid)
                {
                    if (rigidContent == content) continue;
                    if (Vector2.Distance(content.transform.position, rigidContent.transform.position) < 1)
                    {
                        rigidContent.OnCollision(content);
                        content.OnCollision(rigidContent);
                        if (!rigidContent.IsStatic) rigidContent.AddForce(content.Velocity,1);
                        if(!content.IsDummie)content.Velocity = Vector2.zero;
                        return true;
                    }
                }
            }
            return false;
        }
        
        private static bool CheckCollisionInChunkNonRigid(Content content,int chunkIndex)
        {
            if (Managers.LevelManager.Instance.MapChunkMatrix[chunkIndex].LayerNonRigidContentCounts[content.PhysicsLayers[0]] > 0)
            {
                foreach (var nonRigidContent in Managers.LevelManager.Instance.MapChunkMatrix[chunkIndex].ContentsNonRigid)
                {
                    if (nonRigidContent == content) continue;
                    if (Vector2.Distance(content.transform.position, nonRigidContent.transform.position) < 1)
                    {
                        nonRigidContent.OnCollision(content);
                        content.OnCollision(nonRigidContent);
                        if (!nonRigidContent.IsStatic) nonRigidContent.AddForce(content.Velocity,1);
                        return true;
                    }
                }
            }
            return false;
        }
        
        public void LevelStart(LevelManager levelManager)
        {
            LevelManager = levelManager;
        }
        public void LevelEnd(bool succeed)
        {
            Reset();
        }
        public void Add(Content content)
        {
            content.InPhysicsProcessorQueue = true;
            ProcessQueue.Add(content);
        }
        public void Remove(Content content)
        {
            content.InPhysicsProcessorQueue = false;
            ProcessQueue.Remove(content);
        }

        public void AddDummie(Content content)
        {
            DummieQueue.Add(content);
        }
        public void RemoveDummie(Content content)
        {
            DummieQueue.Remove(content);
        }
        
        public void Reset()
        {
            ProcessQueue = new List<Content>();
            DummieQueue = new List<Content>();
            PlayerCharacterBase = null;
        }

    }
}