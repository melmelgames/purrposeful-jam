using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToConsumable : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    private Rigidbody2D rb2D;

    [SerializeField] private Vector2 foodTargetPos;
    [SerializeField] private Vector2 waterTargetPos;
    [SerializeField] private float delta;

    [SerializeField] private bool goToFood;
    [SerializeField] private bool goToWater;

    [SerializeField] private Consumable[] consumables;

    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        waterTargetPos = GetRandomWaterPosition();
        foodTargetPos = GetRandomFoodPosition();
    }
    private void Update()
    {
        if (ReachedFoodTarget())
        {
            foodTargetPos = GetRandomFoodPosition();
        }
        if (ReachedWaterTarget())
        {
            waterTargetPos = GetRandomWaterPosition();
        }
    }
    private void FixedUpdate()
    {
        if (goToFood)
        {
            GoToFood(foodTargetPos);
        }
        if (goToWater)
        {
            GoToWater(waterTargetPos);
        }
    }

    public void SetGoToFood()
    {
        goToFood = true;
    }
    private void GoToFood(Vector2 foodTargetPos)
    {
        Vector2 moveDir = foodTargetPos - rb2D.position;
                    
        rb2D.MovePosition(rb2D.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
                                       
        if (ReachedFoodTarget())
        {
            rb2D.MovePosition(rb2D.position);
            goToFood = false;
        }
    }

    public void SetGoToWater()
    {
        goToWater = true;
    }

    private void GoToWater(Vector2 waterTargetPos)
    {
        Vector2 moveDir = waterTargetPos - rb2D.position;
                    
        rb2D.MovePosition(rb2D.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
                    
        if (ReachedWaterTarget())
        {
            rb2D.MovePosition(rb2D.position);
            goToWater = false;
        }
    }
    
    private bool ReachedFoodTarget()
    {
        if (Vector2.Distance(foodTargetPos, rb2D.position) < delta)
        {
            return true;
        }
        return false;
    }

    private bool ReachedWaterTarget()
    {
        if (Vector2.Distance(waterTargetPos, rb2D.position) < delta)
        {
            return true;
        }
        return false;
    }

    private void LookForConsumables()
    {
        consumables = FindObjectsOfType<Consumable>();
        
    }

    private Vector2 GetRandomFoodPosition()
    {
        LookForConsumables();
        int randomIndex = Random.Range(0, consumables.Length);
        if (consumables[randomIndex] != null && consumables[randomIndex].GetComponent<Consumable>().GetConsumableType() == Consumable.ConsumableType.Food)
        {
            return consumables[randomIndex].transform.position;
        }
        return Vector2.zero;
    }

    private Vector2 GetRandomWaterPosition()
    {
        LookForConsumables();
        int randomIndex = Random.Range(0, consumables.Length);
        if (consumables[randomIndex] != null && consumables[randomIndex].GetComponent<Consumable>().GetConsumableType() == Consumable.ConsumableType.Water)
        {
            return consumables[randomIndex].transform.position;
        }
        return Vector2.zero;
    }
}
