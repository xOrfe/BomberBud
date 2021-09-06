using Project.Scripts.Managers;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Project.Scripts.Characters
{
    public class ArtificalDummieMotor : MonoBehaviour
    {
        public AICharacterBase characterBase;

        private void Start()
        {
            PhysicsProcessor.Instance.AddDummie(characterBase);
            characterBase.Velocity = GetRandomDir();
        }

        public void OnCollision()
        {
            //Debug.Log("I collided with something but idk what bcz im dummie");
            characterBase.Velocity = GetRandomDir();
        }

        public static Vector2 GetRandomDir()
        {
            int rand = Random.Range(0, 4);
            Vector2 dir = new Vector2();
            switch (rand)
            {
                case 0:
                    dir = new Vector2(0,1);
                    break;
                case 1:
                    dir = new Vector2(1,0);
                    break;
                case 2:
                    dir = new Vector2(0,-1);
                    break;
                case 3:
                    dir = new Vector2(-1,0);
                    break;
            }
            return dir;
        }
    }
}