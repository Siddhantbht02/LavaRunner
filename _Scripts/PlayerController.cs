using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController controller;
    private Animator animator;
    private Vector3 move;
    private int desiredLane = 1; // 0=Left, 1=Middle, 2=Right

    [Header("Movement Settings")]
    public float laneDistance = 3f;
    public float forwardSpeed = 10.0f;
    public float laneSwitchSpeed = 10f;

    [Header("Jumping & Gravity")]
    public float jumpForce = 8.0f;
    public float gravity = -20.0f;
    public float fallThreshold = -5f;

    private float verticalVelocity;
    private GameManager gameManager;
    private bool isDead = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if (isDead) return;

        // --- Keyboard Input ---
        if (Input.GetKeyDown(KeyCode.A)) MoveLeft();
        if (Input.GetKeyDown(KeyCode.D)) MoveRight();

        // --- Calculate Target X Position ---
        float targetX = (desiredLane - 1) * laneDistance;
        move = Vector3.zero;

        // Horizontal movement
        float horizontalSpeed = (targetX - transform.position.x) * laneSwitchSpeed;
        move.x = horizontalSpeed;

        // Vertical Movement
        if (controller.isGrounded)
        {
            verticalVelocity = -0.1f;

            // THE FIX IS HERE: The jump input must be checked *after* resetting verticalVelocity.
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        move.y = verticalVelocity;

        // Forward Movement
        move.z = forwardSpeed;

        controller.Move(move * Time.deltaTime);

        if (transform.position.y < fallThreshold) Die();
    }

    // --- Public methods for movement control ---
    public void MoveLeft()
    {
        if (isDead) return;
        desiredLane--;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    public void MoveRight()
    {
        if (isDead) return;
        desiredLane++;
        desiredLane = Mathf.Clamp(desiredLane, 0, 2);
    }

    public void SetLane(int laneIndex)
    {
        if (isDead) return;
        desiredLane = Mathf.Clamp(laneIndex, 0, 2);
    }

    public void Jump()
    {
        // The check for isGrounded is handled in Update before this is called
        if (isDead) return;
        verticalVelocity = jumpForce;
    }

    private void Die()
    {
        if (!isDead)
        {
            isDead = true;
            this.enabled = false;
            if (animator != null) animator.enabled = false;
            gameManager.GameOver();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "Obstacle") Die();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Coin" && !isDead)
        {
            gameManager.AddCoin();
            Destroy(other.gameObject);
        }
    }
}

