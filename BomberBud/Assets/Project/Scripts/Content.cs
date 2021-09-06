using Project.Scripts.Managers;
using UnityEngine;
using Utils = Project.Scripts.Utilities.Utilities;

namespace Project.Scripts
{
    [System.Serializable]
    public class Content : MonoBehaviour, IContent
    {
        public ContentType ContentType { get; set; }
        
        [SerializeField]private volatile int _currentChunkX;
        [SerializeField]private volatile int _currentChunkY;
        public Vector2Int CurrentChunk
        {
            get
            {
                Vector2Int pos = new Vector2Int(_currentChunkX, _currentChunkY);
                return pos;
            }
            set
            {
                Debug.Log("Set");
                _currentChunkX = value.x;
                _currentChunkY = value.y;
            }
        }

        [SerializeField]private bool _isDummie;
        public bool IsDummie
        {
            get => _isDummie;
            set => _isDummie = value;
        }

        public Vector2 Position
        {
            get => this.transform.position;
        }
        
        public bool InPhysicsProcessorQueue { get; set; }
        
        [SerializeField]private bool _isRigid;
        public bool IsRigid
        {
            get => _isRigid;
            set => _isRigid = value;
        }

        [SerializeField]private bool _isStatic;
        public bool IsStatic
        {
            get => _isStatic;
            set => _isStatic = value;
        }
        
        [SerializeField]private int[] _physicsLayers;
        public int[] PhysicsLayers
        {
            get => _physicsLayers;
            set => _physicsLayers = value;
        }
        
        [SerializeField]private Vector2 _velocity;
        public Vector2 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        [SerializeField]private float _mass;
        public float Mass
        {
            get => _mass;
            set => _mass = value;
        }

        [SerializeField]private float _friction;
        public float Friction
        {
            get => _friction;
            set => _friction = value;
        }

        [SerializeField]private string _collisionTag;
        public string CollisionTag
        {
            get => _collisionTag;
            set => _collisionTag = value;
        }
        
        [SerializeField] private bool _isDestroyable;
        public bool isDestroyable
        {
            get => _isDestroyable;
            set => _isDestroyable = value;
        }

        public virtual bool Destroy()
        {
            if(InPhysicsProcessorQueue)PhysicsProcessor.Instance.Remove(this);
            Vector2Int matrixScale = LevelManager.Instance.LevelDefinitionScriptable.MapDefinition.MatrixScale;
            
            int chunk = Utils.GetIndexFromCoord(CurrentChunk,matrixScale);
            LevelManager.Instance.MapChunkMatrix[chunk].Remove(this);
            return true;
        }

        public virtual void OnCollision(Content content)
        {
            //Debug.Log("Coll" + content.gameObject.name);
        }
        
        
        public SkinnedMeshRenderer Renderer { get; set; }
        
        private MaterialPropertyBlock _materialPropertyBlock;

        public virtual MaterialPropertyBlock MaterialPropertyBlock
        {
            get { return _materialPropertyBlock; }
            set
            {
                _materialPropertyBlock = value;
                ExecuteMaterialPropertyBlock();
            }
        }

        public void SetPosition(Vector3 position)
        {
            throw new System.NotImplementedException();
        }

        public void SetLayerOrder(int layerOrder)
        {
            throw new System.NotImplementedException();
        }
        
        public virtual void Move(Vector2 dir)
        {
            throw new System.NotImplementedException();
        }

        public virtual void AddForce(Vector2 dir, float force)
        {
            Velocity += dir * force;
            if (!InPhysicsProcessorQueue) PhysicsProcessor.Instance.Add(this);
        }
        
        protected virtual void ExecuteMaterialPropertyBlock()
        {
            Renderer.SetPropertyBlock(_materialPropertyBlock);
        }
        
    }

    public enum ContentType
    {
        
    }
}