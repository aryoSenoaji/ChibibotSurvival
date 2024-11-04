using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("-----> Pergerakan Player <-----")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sprintSpeed = 8f;
    [SerializeField] float rotationSpeed = 700f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] Animator animator;
    [SerializeField] Transform cameraTransform;

    [SerializeField] Rigidbody rb;
    private bool isGrounded;
    private Quaternion targetRotation;
    private Vector3 moveInput;

    private void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
        rb.freezeRotation = true;
    }

    private void Update()
    {
        // Cek apakah karakter berada di tanah
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Cek apakah tombol shift ditekan untuk sprint
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        // Ambil orientasi kamera untuk pergerakan relatif kamera
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        Vector3 cameraRight = cameraTransform.right;

        // Tentukan nilai moveInput di sini agar bisa diakses di FixedUpdate
        moveInput = (cameraRight * horizontal + cameraForward * vertical).normalized * currentSpeed;

        float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));
        animator.SetFloat("moveAmount", moveAmount * (currentSpeed / moveSpeed), 0.1f, Time.deltaTime);

        // Tentukan target rotasi jika karakter bergerak
        if (moveAmount > 0)
        {
            targetRotation = Quaternion.LookRotation(moveInput);
        }

        // Lompatan
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        }
    }

    private void FixedUpdate()
    {
        // Pergerakan karakter menggunakan Rigidbody.MovePosition
        Vector3 targetPosition = rb.position + moveInput * Time.fixedDeltaTime;
        rb.MovePosition(targetPosition);

        // Rotasi karakter menggunakan Rigidbody.MoveRotation
        rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
    }
}
