using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 6f;
    public float sprintSpeed = 10f;
    public float acceleration = 12f;
    public float deceleration = 15f;

    [Header("Jump")]
    public float jumpHeight = 2f;
    public float gravity = -20f;

    [Header("Ground Detection")]
    public LayerMask groundMask = ~0;
    [Range(0.05f, 0.3f)]
    public float groundCheckRadius = 0.15f;
    public float groundCheckOffset = 0.05f;

    [Header("Camera")]
    public Transform playerCamera;

    [Header("Animation")]
    public Animator animator;

    private CharacterController controller;
    private Vector3 currentMovement;

    public Vector3 velocity;
    public bool isGrounded;

    private bool jumpStarted;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (animator == null)
        {
            animator = GetComponentInChildren<Animator>();
        }

        if (animator == null)
        {
            Debug.LogError(
                "PlayerMovement: No Animator found on the player or its children.",
                this
            );
        }

        if (animator != null && !HasParameter("Jump"))
        {
            Debug.LogWarning(
                "Add a Trigger parameter named Jump to your Animator Controller.",
                this
            );
        }

        // Establish the initial grounded state.
        isGrounded = CheckGrounded();
    }

    void Update()
    {
        // Check grounding before accepting jump input.
        isGrounded = CheckGrounded();

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
            jumpStarted = false;
        }

        HandleJump();
        CalculateMovement();

        // Apply gravity.
        velocity.y += gravity * Time.deltaTime;

        // Apply horizontal and vertical movement together.
        Vector3 finalMovement =
            currentMovement + Vector3.up * velocity.y;

        controller.Move(finalMovement * Time.deltaTime);

        // Check grounding again after movement.
        isGrounded = CheckGrounded();

        // Reset downward velocity after landing.
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
            jumpStarted = false;
        }

        UpdateAnimations();
    }

    bool CheckGrounded()
    {
        // Use the CharacterController's actual dimensions.
        Vector3 worldCenter = transform.TransformPoint(controller.center);

        float bottomY =
            worldCenter.y - controller.height * 0.5f;

        Vector3 spherePosition = new Vector3(
            worldCenter.x,
            bottomY + controller.radius * 0.5f + groundCheckOffset,
            worldCenter.z
        );

        bool sphereGrounded = Physics.CheckSphere(
            spherePosition,
            groundCheckRadius,
            groundMask,
            QueryTriggerInteraction.Ignore
        );

        // CharacterController provides a fallback check.
        return sphereGrounded || controller.isGrounded;
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            // Prevent a second jump before landing.
            jumpStarted = true;
            isGrounded = false;

            velocity.y = Mathf.Sqrt(
                jumpHeight * -2f * gravity
            );

            // Trigger Rifle jump through the Animator.
            if (animator != null && HasParameter("Jump"))
            {
                animator.ResetTrigger("Jump");
                animator.SetTrigger("Jump");
            }
        }
    }

    void CalculateMovement()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 cameraForward = playerCamera != null
            ? playerCamera.forward
            : transform.forward;

        Vector3 cameraRight = playerCamera != null
            ? playerCamera.right
            : transform.right;

        // Keep movement on the horizontal plane.
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 direction =
            cameraForward * vertical +
            cameraRight * horizontal;

        direction = Vector3.ClampMagnitude(direction, 1f);

        float targetSpeed = Input.GetKey(KeyCode.LeftShift)
            ? sprintSpeed
            : moveSpeed;

        Vector3 targetMovement = direction * targetSpeed;

        float speedChange = direction.sqrMagnitude > 0.01f
            ? acceleration
            : deceleration;

        currentMovement = Vector3.MoveTowards(
            currentMovement,
            targetMovement,
            speedChange * Time.deltaTime
        );
    }

    void UpdateAnimations()
    {
        if (animator == null)
            return;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (HasParameter("MoveX"))
        {
            animator.SetFloat("MoveX", horizontal);
        }

        if (HasParameter("MoveZ"))
        {
            animator.SetFloat("MoveZ", vertical);
        }

        if (HasParameter("IsGrounded"))
        {
            animator.SetBool("IsGrounded", isGrounded);
        }

        if (HasParameter("VerticalVelocity"))
        {
            animator.SetFloat("VerticalVelocity", velocity.y);
        }

        if (HasParameter("IsRunning"))
        {
            bool moving =
                Mathf.Abs(horizontal) > 0.1f ||
                Mathf.Abs(vertical) > 0.1f;

            bool running =
                Input.GetKey(KeyCode.LeftShift) && moving;

            animator.SetBool("IsRunning", running);
        }
    }

    bool HasParameter(string parameterName)
    {
        if (animator == null)
            return false;

        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name == parameterName)
                return true;
        }

        return false;
    }

    void OnDrawGizmosSelected()
    {
        CharacterController characterController =
            GetComponent<CharacterController>();

        if (characterController == null)
            return;

        Vector3 worldCenter =
            transform.TransformPoint(characterController.center);

        float bottomY =
            worldCenter.y - characterController.height * 0.5f;

        Vector3 spherePosition = new Vector3(
            worldCenter.x,
            bottomY + characterController.radius * 0.5f
                + groundCheckOffset,
            worldCenter.z
        );

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            spherePosition,
            groundCheckRadius
        );
    }
}