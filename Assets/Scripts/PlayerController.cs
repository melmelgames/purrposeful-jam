using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveDir;
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb2D;
    private Animator animator;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInput();
    }

    private void FixedUpdate()
    {
        MovePlayer(moveDir);
    }

    private void MovePlayer(Vector2 moveDir)
    {
        rb2D.MovePosition(rb2D.position + moveDir * moveSpeed * Time.fixedDeltaTime);
    }

    private void HandleInput()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Vertical");
        animator.SetFloat("horizontal_input", moveDir.x);
        animator.SetFloat("vertical_input", moveDir.y);
    }
}
