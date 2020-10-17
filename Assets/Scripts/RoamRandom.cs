using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoamRandom : MonoBehaviour
{
    
    [SerializeField] private float moveSpeed;
    [SerializeField] private float minRoamX;
    [SerializeField] private float maxRoamX;
    [SerializeField] private float minRoamY;
    [SerializeField] private float maxRoamY;
    [SerializeField] private Vector2 catVelocity;
    private Rigidbody2D rb2D;

    [SerializeField] private Vector2 targetPos;
    [SerializeField] private float delta;

    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        targetPos = GenerateRandomTargetPosition();
    }

    private void Update()
    {
        if (ReachedTarget())
        {
            targetPos = GenerateRandomTargetPosition();
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter(targetPos);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Boundary")
        {
            Debug.Log("Boundary!");
            targetPos = new Vector2(0f, 0f);
        }
    }

    private void MoveCharacter(Vector2 targetPos)
    {
        Vector2 moveDir = targetPos - rb2D.position;
        rb2D.MovePosition(rb2D.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private Vector2 GenerateRandomTargetPosition()
    {
        float randomX = Random.Range(minRoamX, maxRoamX);
        float randomY = Random.Range(minRoamY, maxRoamY);
        Vector2 randomTargetPos;
        randomTargetPos.x = rb2D.position.x + randomX;
        randomTargetPos.y = rb2D.position.y + randomY;

        return randomTargetPos;
    }

    private bool ReachedTarget()
    {
        
        if (Vector2.Distance(targetPos, rb2D.position) < delta)
        {
            return true;         
        }
        return false;
    }
}
