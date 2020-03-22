using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    private Animator animator;
    private Rigidbody playerRigidbody;

    private void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetFloat("Speed", Input.GetAxis("Vertical"));
    }
}
