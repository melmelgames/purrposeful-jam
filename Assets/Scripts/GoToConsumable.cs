using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToConsumable : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb2D;

    [SerializeField] private Vector2 targetPos;
    [SerializeField] private float delta;

    [SerializeField] private Consumable[] consumables;

    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        StartCoroutine(LookForCosumables());

    }

    public void GoToFood()
    {
        //consumables = FindObjectsOfType<Consumable>();
        if(consumables != null)
        {
            foreach (Consumable consumable in consumables)
            {
                if (consumable.GetConsumableType() == Consumable.ConsumableType.Food)
                {
                    targetPos = consumable.transform.position;
                    Vector2 moveDir = targetPos - rb2D.position;
                    rb2D.MovePosition(rb2D.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
                    if (ReachedTarget())
                    {
                        rb2D.MovePosition(rb2D.position);
                    }
                }
            }
        }
        
    }

    public void GoToWater()
    {
        //consumables = FindObjectsOfType<Consumable>();
        if (consumables != null)
        {
            foreach (Consumable consumable in consumables)
            {
                if (consumable.GetConsumableType() == Consumable.ConsumableType.Water)
                {
                    targetPos = consumable.transform.position;
                    Vector2 moveDir = targetPos - rb2D.position;
                    rb2D.MovePosition(rb2D.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
                    if (ReachedTarget())
                    {
                        rb2D.MovePosition(rb2D.position);
                    }
                }
            }
        }

    }

    private bool ReachedTarget()
    {
        if (Vector2.Distance(targetPos, rb2D.position) < delta)
        {
            return true;
        }
        return false;
    }

    private IEnumerator LookForCosumables()
    {
        while (true)
        {
            consumables = FindObjectsOfType<Consumable>();
            yield return new WaitForSeconds(1f);
        }
    }
}
