using System.Collections;
using UnityEngine;

namespace Parodystudios.GravityModificationPuzzle
{
    /// <summary>
    /// CharacterMovement:
    ///-Manages the movement and actions of the character in the game.
    ///- Utilizes Rigidbody and Animator components for physics and animation control.
    ///- Allows the character to move based on user input (WASD keys).
    ///-Implements jumping functionality when the character is grounded.
    ///- Handles hologram activation and rotation based on arrow key input.
    ///- Applies custom gravity to the character when not grounded.
    ///- Monitors the character's grounded state and checks for a game over condition if the character remains off the ground for a specified duration.
    ///- Provides animator parameters for the character's movement states (idle, running, falling).
    /// </summary>

    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement : MonoBehaviour
    {
        [Header("Character Settings")]
        [SerializeField] private float speed = 6f;
        [SerializeField] private float turnSmoothTime = 0.1f;
        [SerializeField] private float jumpForce = 8f;
        [SerializeField] private float gravity = -9.81f;
        [SerializeField] private float groundDistance = 0.4f;
        [SerializeField] private LayerMask groundMask;
        [SerializeField] private float decelerationFactor = 5f;
        [SerializeField] private Transform holoGram;
        [SerializeField] private Transform holoGramTransform;
        [SerializeField] private float offGroundThreshold = 5f;
        [SerializeField] private Quaternion rot;


        #region private variables
        private float turnSmoothVelocity = 0.1f;
        private Rigidbody rb;
        private Animator animator;
        private bool isGrounded;
        private bool hologramRotated;
        private bool isfalling = false;
        private float offGroundTimer = 0;
        #endregion

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            // Movement
            Movement();

            // Jump
            Jump();

            //Gravity Rotation
            ModifyGravity();

            //Apply Custom Gravity
            ApplyCustomGravity();


            animator.SetBool("Falling", !isGrounded);

        }

        // Check for the Jump input and perform a jump action if the character is grounded.
        private void Jump()
        {
            isGrounded = Physics.CheckSphere(transform.position, groundDistance, groundMask);

            if (isGrounded && Input.GetButtonDown("Jump"))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                //animator.SetBool("Falling", false);
            }


        }

        // Move the character based on the user's input.
        private void Movement()
        {
            Vector3 moveDirection = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
            {
                moveDirection += transform.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                moveDirection -= transform.forward;
            }
            if (Input.GetKey(KeyCode.D))
            {
                moveDirection += transform.right;
            }
            if (Input.GetKey(KeyCode.A))
            {
                moveDirection -= transform.right;
            }

            if (moveDirection.magnitude >= 0.1f)
            {
                Vector3 localMoveDir = transform.TransformDirection(moveDirection.normalized);

                rb.velocity = moveDirection.normalized * speed;
            }
            else
            {
                // Set animator parameters for idle state
                animator.SetBool("Running", false);
                animator.SetBool("Idle", true);
                animator.SetBool("Falling", false);

                rb.velocity -= rb.velocity * decelerationFactor * Time.deltaTime;

                if (rb.velocity.magnitude < 0.1f)
                {
                    rb.velocity = Vector3.zero;
                }
            }

            animator.SetFloat("Speed", moveDirection.magnitude);
        }

        // Activate the hologram and set its position and rotation.
        void ActivateHologram(float x, float z)
        {
            holoGram.transform.SetPositionAndRotation(holoGramTransform.position , transform.rotation * Quaternion.Euler(x, 0, z));
            holoGram.gameObject.SetActive(true);
            hologramRotated = true;
            rot = holoGram.rotation;
        }


        //Modify the gravity depending on the user input
        private void ModifyGravity()
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                ActivateHologram(0, 90);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                ActivateHologram(0, -90);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                ActivateHologram(90, 0);
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ActivateHologram(-90, 0);
            }
            else if (Input.GetKeyDown(KeyCode.Return) && hologramRotated)
            {
                hologramRotated = false;
                holoGram.gameObject.SetActive(false);

                StartCoroutine(SmoothTransform(holoGram));
                //transform.SetLocalPositionAndRotation(holoGram.position, holoGram.rotation);
            }
        }

        IEnumerator SmoothTransform(Transform targetTransform)
        {
            float timeElapsed = 0f;
            Vector3 startPosition = transform.localPosition;
            Quaternion startRotation = transform.localRotation;

            while (timeElapsed < .5f)
            {
                timeElapsed += Time.deltaTime;
                float t = Mathf.Clamp01(timeElapsed / .5f);

                transform.localPosition = Vector3.Lerp(startPosition, targetTransform.localPosition, t);
                transform.localRotation = Quaternion.Lerp(startRotation, rot, t);

                yield return null;
            }

            transform.localPosition = targetTransform.localPosition;
            transform.localRotation = rot;
        }

        // Apply custom gravity to the character.
        private void ApplyCustomGravity()
        {
            if (!isGrounded)
            {
                rb.AddForce(transform.up * gravity, ForceMode.Force);
            }
        }

        // Check for the continuous time the character is off the ground and return true if it exceeds a threshold.
        public bool CHeckForGameOver()
        {
            if (!isGrounded)
            {
                offGroundTimer += Time.deltaTime;
                if (offGroundTimer >= offGroundThreshold)
                {
                    return true;
                }
            }
            else
            {
                offGroundTimer = 0f;
                return false;
            }
            return false;
        }
    }
}
