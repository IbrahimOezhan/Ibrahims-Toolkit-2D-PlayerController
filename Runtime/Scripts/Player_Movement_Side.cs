using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace IbrahKit
{
    public class Player_Movement_Side : MonoBehaviour
    {
        private Input2D input2d;

        private float input;
        private bool isFacingRight = true;
        private bool isJumping;

        private float coyoteTime = 0.2f;
        private float coyoteTimeTimer;

        private float jumpBufferTime = 0.2f;
        private float jumpBufferTimer;

        [SerializeField] private float speed = 8f;
        [SerializeField] private float jumpingPower = 16f;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private LayerMask groundLayer;

        private void Awake()
        {
            input2d = new();
        }

        private void OnEnable()
        {
            input2d.PlayerSide.Move.canceled += Move;
            input2d.PlayerSide.Move.performed += Move;
            input2d.PlayerSide.Move.started += Move;
            input2d.PlayerSide.Jump.canceled += JumpUp;
            input2d.PlayerSide.Jump.performed += JumpDown;
        }

        private void OnDisable()
        {
            input2d.PlayerSide.Move.canceled -= Move;
            input2d.PlayerSide.Move.performed -= Move;
            input2d.PlayerSide.Move.started -= Move;
            input2d.PlayerSide.Jump.canceled -= JumpUp;
            input2d.PlayerSide.Jump.performed -= JumpDown;
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime;

            rb.linearVelocity = new Vector2(input * speed, rb.linearVelocity.y);

            if (IsGrounded()) coyoteTimeTimer = coyoteTime;
            else coyoteTimeTimer -= deltaTime;

            jumpBufferTimer -= deltaTime;

            if (coyoteTimeTimer > 0f && jumpBufferTimer > 0f && !isJumping) Jump();

            if (!IsGrounded()) rb.gravityScale = 3;
            else rb.gravityScale = 1;

            Flip();
        }

        private void Move(InputAction.CallbackContext _context)
        {
            input = _context.ReadValue<float>();
        }

        private void Jump()
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpingPower);
            jumpBufferTimer = 0f;
            StartCoroutine(JumpCooldown());
        }

        private void JumpDown(InputAction.CallbackContext _context)
        {
            jumpBufferTimer = jumpBufferTime;
        }

        private void JumpUp(InputAction.CallbackContext _context)
        {
            if (rb.linearVelocity.y > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);
                coyoteTimeTimer = 0f;
            }
        }

        private bool IsGrounded()
        {
            return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        }

        private void Flip()
        {
            if (input != 0f)
            {
                Vector3 _localScale = transform.localScale;
                isFacingRight = !isFacingRight;
                _localScale.x = input;
                transform.localScale = _localScale;
            }
        }

        private IEnumerator JumpCooldown()
        {
            isJumping = true;
            yield return new WaitForSeconds(0.4f);
            isJumping = false;
        }
    }
}