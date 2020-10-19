using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : MonoBehaviour
{
    public enum ConsumableType 
    { 
        Food, 
        Water 
    }

    [SerializeField] private int consumableAmount;
    [SerializeField] private ConsumableType consType;

    private void Update()
    {
        DestroyWhenEmpty();
    }

    public ConsumableType GetConsumableType()
    {
        return consType;
    }

    public void ConsumeConsumable()
    {
        consumableAmount--;
    }

    public int GetConsumableAmount()
    {
        return consumableAmount;
    }
    
    // Destroys consumable when consumableAmount reaches zero
    private void DestroyWhenEmpty()
    {
        if(consumableAmount <= 0)
        {
            if(consType == ConsumableType.Food)
            {
                PlayerController.instance.MinusOneFoodBowl();
            }
            if (consType == ConsumableType.Water)
            {
                PlayerController.instance.MinusOneWaterBowl();
            }
            Destroy(this.gameObject);
        }
    }
}
