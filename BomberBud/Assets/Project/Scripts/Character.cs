using UnityEngine;

namespace Project.Scripts
{
    public abstract class Character : MonoBehaviour,IContent,IMovable,IDestroyable, IAttacker
    {
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
        
        private CharacterState _characterState;
        public virtual CharacterState CharacterState
        {
            get { return _characterState; }
            set
            {
                _characterState = value;
                ExecuteCharacterState();
            }
        }
        
        public virtual void SetPosition(Vector3 position)
        {
            throw new System.NotImplementedException();
        }

        public virtual void SetLayerOrder(int layerOrder)
        {
            throw new System.NotImplementedException();
        }

        public virtual bool Move(Vector3 dir)
        {
            throw new System.NotImplementedException();
        }
        
        public virtual bool Attack(Vector3 dir)
        {
            throw new System.NotImplementedException();
        }
        
        public virtual bool Destroy()
        {
            throw new System.NotImplementedException();
        }

        protected virtual void ExecuteCharacterState()
        {
            switch (_characterState)
            {
                
            }
        }
        protected virtual void ExecuteMaterialPropertyBlock()
        {
            Renderer.SetPropertyBlock(_materialPropertyBlock);
        }

        
    }

    public enum CharacterState
    {
        Idle,
        Walking,
        Attacking,
        GettingDamage,
        Death
    }
}
