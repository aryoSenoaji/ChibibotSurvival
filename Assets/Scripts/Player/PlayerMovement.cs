using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 700f;

    Quaternion targetRotation;

    private void Awake()
    {
        
    }

    private void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        float moveAmount = Mathf.Abs(horizontal) + Mathf.Abs(vertical);

        var moveInput = (new Vector3(horizontal, 0, vertical)).normalized;

        if (moveAmount > 0)
        {
            transform.position += moveInput * moveSpeed * Time.deltaTime;
            targetRotation = Quaternion.LookRotation(moveInput);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 
            rotationSpeed * Time.deltaTime);
    }
}
