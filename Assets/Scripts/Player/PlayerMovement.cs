using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [Header("-----> Pergerakan Player <-----")]
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float sprintSpeed = 8f; // Kecepatan saat sprint
    [SerializeField] float rotationSpeed = 700f;

    [SerializeField] Animator animator;
    [SerializeField] CharacterController characterController;
    [SerializeField] Transform cameraTransform; // Tambahkan referensi untuk kamera

    Quaternion targetRotation;

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Cek apakah tombol shift ditekan untuk sprint
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : moveSpeed;

        // Ambil orientasi kamera untuk pergerakan relatif kamera
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0; // Mengabaikan pergerakan vertikal
        Vector3 cameraRight = cameraTransform.right;

        Vector3 moveInput = (cameraRight * horizontal + cameraForward * vertical).normalized;

        float moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal) + Mathf.Abs(vertical));

        if (moveAmount > 0)
        {
            characterController.Move(moveInput * currentSpeed * Time.deltaTime);
            targetRotation = Quaternion.LookRotation(moveInput);
        }

        // Rotasi karakter
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
            rotationSpeed * Time.deltaTime);

        // Update nilai moveAmount pada animator, sesuaikan dengan currentSpeed
        animator.SetFloat("moveAmount", moveAmount * (currentSpeed / moveSpeed), 0.1f, Time.deltaTime);
    }
}
