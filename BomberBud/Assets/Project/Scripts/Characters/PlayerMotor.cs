using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.Scripts.Characters
{
    public class PlayerMotor : MonoBehaviour
    {
        private PlayerInputActions _playerInputActions;
        private InputAction _movement;
        public PlayerCharacterBase characterBase;
        private void Awake()
        {
            _playerInputActions = new PlayerInputActions();
        }

        private void OnEnable()
        {
            _movement = _playerInputActions.Player.Movement;
            _movement.Enable();
            
            _playerInputActions.Player.Attack.performed += Attack;
            _playerInputActions.Player.Attack.Enable();
        }

        private void Update()
        {
            Vector2 val = _movement.ReadValue<Vector2>();
            if (val.x + val.y == 0) return;
            characterBase.AddForce(val,characterBase.MoveSpeed * Time.deltaTime);
        }

        private void Attack(InputAction.CallbackContext obj)
        {
            Debug.Log("asdas");
            characterBase.Attack();
        }

        private void OnDisable()
        {
            _movement.Disable();
            _playerInputActions.Player.Attack.Disable();
        }
    }
}