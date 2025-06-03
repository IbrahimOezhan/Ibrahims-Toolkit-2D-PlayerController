using UnityEngine;
using UnityEngine.InputSystem;

namespace IbrahKit
{
    public class Player_Movement_Top : MonoBehaviour
    {
        private Vector2 input;
        private Rigidbody2D rigidbody2d;
        private Input2D input2D;

        [SerializeField] private float moveSpeed;

        private void Awake()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            input2D = new();
        }

        private void OnEnable()
        {
            input2D.Enable();
            input2D.PlayerTopDown.Move.started += Move;
            input2D.PlayerTopDown.Move.performed += Move;
            input2D.PlayerTopDown.Move.canceled += Move;
        }

        private void OnDisable()
        {
            input2D.PlayerTopDown.Move.started -= Move;
            input2D.PlayerTopDown.Move.performed -= Move;
            input2D.PlayerTopDown.Move.canceled -= Move;
            input2D.Dispose();
        }

        private void Update()
        {
            Movement();
        }

        private void Move(InputAction.CallbackContext _context)
        {
            input = _context.ReadValue<Vector2>();
        }

        void Movement()
        {
            if (rigidbody2d.linearVelocity.x > 0f) GetComponent<SpriteRenderer>().flipX = false;
            else if (rigidbody2d.linearVelocity.x < 0f) GetComponent<SpriteRenderer>().flipX = true;
            rigidbody2d.linearVelocity = input * moveSpeed;
        }
    }
}