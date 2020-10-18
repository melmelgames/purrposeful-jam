using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 moveDir;
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb2D;
    private Animator animator;

    [SerializeField] private GameObject foodBowl;
    [SerializeField] private GameObject waterBowl;
    [SerializeField] private GameObject litterTray;

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

        // Place food bowl when I key is pressed
        if (Input.GetKeyUp(KeyCode.I))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleFront"))
            {
                Vector2 position = rb2D.position + new Vector2(0, -1);
                PlaceFoodBowl(position);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleBack"))
            {
                Vector2 position = rb2D.position + new Vector2(0, 1);
                PlaceFoodBowl(position);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleRight"))
            {
                Vector2 position = rb2D.position + new Vector2(1, 0);
                PlaceFoodBowl(position);
            }
            if (animator.GetCurrentAnimatorStateInfo(0).IsTag("idleLeft"))
            {
                Vector2 position = rb2D.position + new Vector2(-1, 0);
                PlaceFoodBowl(position);
            }
        }
    }

    private void PlaceFoodBowl(Vector2 pos)
    {
        Instantiate(foodBowl, pos, Quaternion.identity);
    }

    private void PlaceWaterBowl(Vector2 pos)
    {
        Instantiate(waterBowl, pos, Quaternion.identity);
    }

    private void PlaceLitterTray(Vector2 pos)
    {
        Instantiate(litterTray, pos, Quaternion.identity);
    }
}
