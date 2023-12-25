using UnityEngine;

using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6.0f;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f; // Custom gravity value
    float turnSmoothVelocity;
    Vector3 velocity;

    public Transform cam;

    void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
        }

        // Apply custom gravity
        bool isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // To keep the character on the ground
        }

        velocity.y += gravity * Time.deltaTime;

        // Apply the gravity direction based on character's local down
        Vector3 gravDir = transform.up * gravity * Time.deltaTime;
        controller.Move(gravDir);

        controller.Move(velocity * Time.deltaTime);
    }



    //[Header("Character Settings")]
    //[SerializeField] private float speed = 6f;
    //[SerializeField] private float turnSmoothTime = 0.1f;
    //[SerializeField] private float jumpForce = 8f;
    //[SerializeField] private float gravity = -9.81f;
    //[SerializeField] private float groundDistance = 0.4f;
    //[SerializeField] private LayerMask groundMask;

    //#region private fields
    //private Vector3 velocity;
    //private bool isGrounded;
    //private float turnSmoothVelocity;
    //private CharacterController controller;
    //private Animator animator;
    //#endregion

    //private void Awake()
    //{
    //    controller = GetComponent<CharacterController>();
    //    animator = GetComponent<Animator>();
    //}

    //void Update()
    //{
    //    // Movement
    //    Movement();

    //    // Jump
    //    Jump();
    //}

    ////character movement logic
    //private void Movement()
    //{
    //    float horizontal = Input.GetAxis("Horizontal");
    //    float vertical = Input.GetAxis("Vertical");
    //    Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

    //    if (direction.magnitude >= 0.1f)
    //    {
    //        animator.SetBool("Running", true);
    //        animator.SetBool("Idle", false);
    //        animator.SetBool("Falling", false);

    //        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
    //        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
    //        transform.rotation = Quaternion.Euler(0f, angle, 0f);

    //        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
    //        controller.Move(moveDir.normalized * speed * Time.deltaTime);
    //    }
    //    else
    //    {
    //        animator.SetBool("Running", false);
    //        animator.SetBool("Idle", true);
    //        animator.SetBool("Falling", false);
    //    }
    //}

    ////character jump logic
    //private void Jump()
    //{
    //    isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

    //    if (isGrounded && velocity.y < 0)
    //    {
    //        velocity.y = -2f;
    //        animator.SetBool("Falling", false);
    //    }
    //    else if (!isGrounded)
    //    {
    //        animator.SetBool("Falling", true);
    //    }

    //    if (Input.GetButtonDown("Jump") && isGrounded)
    //    {
    //        velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
    //    }

    //    velocity.y += gravity * Time.deltaTime;
    //    controller.Move(velocity * Time.deltaTime);
    //}
}
