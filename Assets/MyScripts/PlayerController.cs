using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class ThirdPersonController : MonoBehaviour
{
        public float walkSpeed = 0.2f;
        public float runSpeed = 0.5f;
        public float gravity = -9.81f;
        public Transform cameraTransform;
        public Animator animator;

    private CharacterController controller;
    private Vector3 velocity;
    private Vector3 lastValidMoveDir = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 input = new Vector3(h, 0, v);
        input = Vector3.ClampMagnitude(input, 1f);
        bool isMoving = input.magnitude > 0.1f;
        bool isRunning = Input.GetKey(KeyCode.LeftShift);

        Vector3 moveDir = Vector3.zero;

        if (isMoving)
        {
            Vector3 camForward = cameraTransform.forward;
            Vector3 camRight = cameraTransform.right;
            camForward.y = 0;
            camRight.y = 0;
            camForward.Normalize();
            camRight.Normalize();

            moveDir = (camForward * v + camRight * h).normalized;

            // Only update cached direction if input is valid
            if (moveDir.sqrMagnitude > 0.01f)
                lastValidMoveDir = moveDir;
        }

        // Gravity
        if (controller.isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;

        float speed = isRunning ? runSpeed : walkSpeed;
        Vector3 movement = lastValidMoveDir * (isMoving ? speed : 0) + new Vector3(0, velocity.y, 0);
        controller.Move(movement * Time.deltaTime);

        // Rotation only if grounded and valid input
        if (controller.isGrounded && isMoving && lastValidMoveDir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lastValidMoveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }

        // Animations
        if (animator != null)
        {
            animator.SetBool("isMoving", isMoving);
            animator.SetBool("isRunning", isRunning);
        }

        // Debug lines
        Debug.DrawLine(transform.position, transform.position + transform.forward * 2f, Color.red);
        Debug.DrawLine(transform.position, transform.position + lastValidMoveDir * 2f, Color.green);
    }
}
