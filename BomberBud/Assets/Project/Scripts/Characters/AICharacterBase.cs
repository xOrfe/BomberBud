using Project.Scripts.Managers;

namespace Project.Scripts.Characters
{
    public class AICharacterBase : Character
    {
        private ArtificalDummieMotor _artificalDummieMotor;
        private void Start()
        {
            _artificalDummieMotor = gameObject.AddComponent<ArtificalDummieMotor>();
            GetComponent<ArtificalDummieMotor>().characterBase = this;
        }
        
        public override void OnCollision(Content content)
        {
            _artificalDummieMotor.OnCollision();
        }
        
        private void OnDestroy()
        {
            PhysicsProcessor.Instance?.RemoveDummie(this);
        }
    }
}