using UnityEngine;
namespace Project.Scripts.Characters
{
    public abstract class Character : Content, IAttacker
    {
            
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
        
        [SerializeField]private float _moveSpeed;
        public float MoveSpeed
        {
            set => _moveSpeed = value;
            get => _moveSpeed;
        }

        public virtual bool Attack()
        {
            throw new System.NotImplementedException();
        }

        protected virtual void ExecuteCharacterState()
        {
            switch (_characterState)
            {
                
            }
        }
    }
    
    public enum Direction
    {
        Horizontal,
        Vertical
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
