using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float groundDrag = 5f;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask groundLayer;
    private bool isGrounded;

    private float horizontalInput;
    private float verticalInput;
    private Vector3 moveDirection;
    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // รับค่า input จาก WASD
        horizontalInput = Input.GetAxis("Horizontal");  // A/D
        verticalInput = Input.GetAxis("Vertical");      // W/S

        // เช็คว่าตัวละครยังอยู่บนพื้นดิน
        isGrounded = Physics.Raycast(transform.position, Vector3.down,
                                     playerHeight * 0.5f + 0.2f, groundLayer);

        // ปรับ drag
        rb.drag = isGrounded ? groundDrag : 0;

        // เคลื่อนที่
        MovePlayer();
    }

    void MovePlayer()
    {
        // คำนวณทิศทางการเคลื่อนที่
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;

        // ใช้ Rigidbody ในการเคลื่อนที่
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        // จำกัดความเร็วสูงสุด
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }
}