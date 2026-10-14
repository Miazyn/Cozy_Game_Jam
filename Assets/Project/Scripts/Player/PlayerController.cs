using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour
{
    InputControls controls;

    [Header("Player Movement")]
    [SerializeField] float speed = 2f;
    [SerializeField] float sprintSpeed = 4f;
    [SerializeField] float characterRotationSmoothing = 0.03f;
    float playerHeight;

    [SerializeField] CharacterController charController;
    Rigidbody rb;

    bool isSprinting = false;

    public Vector2 MoveDirection;
    bool isGrounded;

    public float gravity = -9.8f;

    Vector3 velocity;
    public Transform GroundCheck;
    public float groundDistance = 0.4f;
    public LayerMask GroundMask;

    public bool CanMove;

    private void Awake()
    {
        CanMove = true;
    }

    private void Start()
    {
        controls = gameObject.GetComponent<Player>().controls;
        playerHeight = transform.position.y;
        rb = GetComponent<Rigidbody>();

        controls.Player.Sprint.performed += sprinting => Sprinting();
    }

    private void Update()
    {
        if (CanMove)
        {
            isGrounded = Physics.CheckSphere(GroundCheck.position, groundDistance, GroundMask);


            if (isGrounded && velocity.y < 0)
            {
                velocity.y = -2;
            }
            MoveDirection = controls.Player.Move.ReadValue<Vector2>();

            charController.Move(new Vector3(MoveDirection.x, 0, MoveDirection.y) * speed * Time.deltaTime);
            Rotation();


            velocity.y += gravity * Time.deltaTime;
            charController.Move(velocity * speed);
        }
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    void Rotation()
    {
        float horizontalMovement = MoveDirection.x;
        float verticalMovement = MoveDirection.y;

        Vector3 rotationOfCharacter = new Vector3(horizontalMovement, 0.0f, verticalMovement);
        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(rotationOfCharacter), characterRotationSmoothing);
        }
    }

    void Sprinting()
    {
        if (speed != speed + sprintSpeed)
        {
            speed += sprintSpeed;
            isSprinting = !isSprinting;
        }
    }
}
