using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public Transform orientation;
    public Transform playerCam;

    Rigidbody rb;

    public float gravity = 10f;
    public float jumpHeight = 2f;
    public float moveSpeed = 3;

    public float sensitivity = 100f;

    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundDistance;

    bool isGrounded;
    Vector3 velocity;

    // input
    float xInput, yInput;

    float xRotation = 0f;

    void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    void Start() {
        velocity = Vector3.zero;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void FixedUpdate() {
        Movement();
    }

    void Update() {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundLayer);
        GetInput();
        Look();
        if (isGrounded && Input.GetButtonDown("Jump")) {
            velocity.y = Mathf.Sqrt(2 * jumpHeight * gravity);
        }
    }

    void GetInput() {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
    }

    private float desiredX;
    void Look() {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime;

        //Find current look rotation
        Vector3 rot = playerCam.transform.localRotation.eulerAngles;
        desiredX = rot.y + mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        //Perform the rotations
        playerCam.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    void Movement() {
        // gravity
        velocity.y -= gravity * Time.deltaTime;

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }

        Vector3 move = (orientation.transform.forward * yInput + orientation.transform.right * xInput).normalized;
        rb.velocity = move * moveSpeed + velocity;
    }
}
