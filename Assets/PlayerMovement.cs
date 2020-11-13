﻿using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;
    public Vector3 offset;
    public Transform player;
    public GameObject FirstPersonCamera;
    public GameObject ThirdPersonCamera;

    public float speed = 12f;
    public float gravity = -9.81f;
    public float turnSmoothTime = 0.1f;
    public float turnSmoothVelocity;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    public float jumpHeight = 3f;

    Vector3 velocity;
    bool isGrounded;

    private void Start()
    {
        // Sets camera positions
        //CameraPos = player.position;
        offset = new Vector3(0, 1, -5);
        transform.position = player.position;

        FirstPersonCamera.SetActive(true);
        ThirdPersonCamera.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (FirstPersonCamera.activeSelf)
        {
            Debug.Log("First Person!");
            FirstPerson();
        }
        else if (ThirdPersonCamera.activeSelf)
        {
            Debug.Log("Third Person!");
            ThirdPerson();
        }
    }

    void FirstPerson()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void ThirdPerson()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            controller.Move(direction * speed * Time.deltaTime);
        }
    }

}
