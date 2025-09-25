using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float runSpeed = 6.7f;
    public float dragForce = 5f;
    public float jumpForce = 4f;
    public float airMultiplier = 0.1f;

    [Header("key Bindings")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode runKey = KeyCode.LeftShift;

    [Header("Look Direction")]
    public Transform lookDirection;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask groundLayers;

    bool isRunning;
    bool jumpReady;
    bool isGrounded;
    float hInput;
    float vInput;
    Vector3 moveDirection;
    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        isGrounded = true;
        isRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if player on the ground
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, groundLayers);

        if (isGrounded)
        {
            rb.linearDamping = dragForce;
        }
        else
        {
            rb.linearDamping = 0f;
        }

        HandleInput();
        CheckAndLimitMoveSpeed();
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (jumpReady)
        {
            Jump();
            jumpReady = false;
            isGrounded = false;
        }
    }

    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = lookDirection.forward * vInput + lookDirection.right * hInput;

        // Apply move force
        if (isGrounded)
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * (isRunning ? runSpeed : 1f), ForceMode.Force);
        else
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
    }

    private void Jump()
    {
        // Reset y velocity
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void HandleInput()
    {
        // Gets the input
        hInput = Input.GetAxisRaw("Horizontal");
        vInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKey(jumpKey) && isGrounded)
        {
            jumpReady = true;
        }

        isRunning = false;
        if (Input.GetKey(runKey))
        {
            isRunning = true;
        }
    }

    private void CheckAndLimitMoveSpeed()
    {
        // Get flat velocity
        Vector3 flatVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if (flatVelocity.magnitude > moveSpeed)
        {
            Vector3 maxVelocity = flatVelocity.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(maxVelocity.x, rb.linearVelocity.y, maxVelocity.z);
        }
    }
}
