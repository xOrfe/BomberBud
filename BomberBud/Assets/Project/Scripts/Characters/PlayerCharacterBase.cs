using Project.Scripts.Managers;
using UnityEngine;

namespace Project.Scripts.Characters
{
    public abstract class PlayerCharacterBase : Character
    {
        [SerializeField] private GameObject attackPrefab;
        private void Start()
        {
            gameObject.AddComponent<PlayerMotor>();
            GetComponent<PlayerMotor>().characterBase = this;
            PhysicsProcessor.Instance.PlayerCharacterBase = this;
        }
        
        public override void AddForce(Vector2 dir, float force)
        {
            Velocity += dir * force;
        }
        public override void OnCollision(Content content)
        {
            if (content.gameObject.tag == "AI") GameManager.Instance.IsGameplayRunning = false;
        }
        
        public override bool Attack()
        {
            LevelManager.Instance.CreateContent(CurrentChunk,attackPrefab);
            return true;
        }
        private void OnDestroy()
        {
            if(InPhysicsProcessorQueue)PhysicsProcessor.Instance.Remove(this);
        }
        
        public override bool Destroy()
        {
            GameManager.Instance.IsGameplayRunning = false;
            Destroy(gameObject);
            return true;
        }
    }
}