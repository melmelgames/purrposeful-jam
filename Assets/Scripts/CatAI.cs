using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAI : MonoBehaviour
{
    public enum CatGender
    {
        Male, 
        Female
    }

    [SerializeField] private string catName;                // Cat's name
    [SerializeField] private CatGender gender;              // cat's gender
    [SerializeField] private int happiness;                 // cat's hapiness level
    [SerializeField] private int age;                       // cat's age
    [SerializeField] private int food;                      // cat's food meter: food=foodMax => not Hungry; food<foodMin => hungry
    [SerializeField] private float depleteConsumableTime;     // time it takes to deplete consumable by 1 unit
    [SerializeField] private float feedingTimeStep;         // time it takes to increase food by 1 unit
    [SerializeField] private int foodMax;                   // max amount of food
    [SerializeField] private int foodMin;                   // min amount of food
    [SerializeField] private int water;                     // cat's water meter: water=waterMax => not thirsty; water<waterMin => thirsty
    [SerializeField] private float drinkingTimeStep;         // time it takes to increase water by 1 unit
    [SerializeField] private int waterMax;                   // max amount of water
    [SerializeField] private int waterMin;                   // min amount of water
    [SerializeField] private bool needsLitterTray;          // does the cat need to poop?
    [SerializeField] private float timeBetweenPoops;        
    [SerializeField] private GameObject poop;               // poop GameObject for when the cats poops outside the litter tray
    [SerializeField] private bool canMate;
    [SerializeField] private bool isPregnant;
    [SerializeField] private float ageTimeStep;             // amount of time (in sec) it takes for a cat to age 1 month

    private RoamRandom roamRandomScript;
    private GoToConsumable goToConsumableScript;
    private GoToLitterTray goToLitterTrayScript;
    private Consumable consumable;
    private bool otherCatsNearby;                           // are there other cats nearby? check when feeding, drinking, and pooping
    private bool nearFood;                                  // is the cat near the food bowl        // if the food bowl
    private bool nearWater;                                  // is the cat near the water bowl      // or water bowl
    private bool nearLitter;                                  // is the cat near the litter tray    // or litter tray are too close to eachother
                                                              // the cat won't use either

    private CatGender otherCatGender;                       // gender of cat this cat bumps into
    private int otherCatAge;                                // age of cat this cat bumps into
    [SerializeField] private int pregancyAge;
    [SerializeField] private int lastPregnancyEngAge;
    private SpawnNewCats spawnNewCatsScript;

    private void Awake()
    {
        // init new cat
        age = 0;
        happiness = 100;
        food = foodMax;
        water = waterMax;
        // first 2 cats have set gender, Snowball is a female cat and Meow meow is a male cat
        if (FindObjectsOfType<CatAI>().Length > 2) 
        {
            int randomSex = Random.Range(0, 2);
            switch (randomSex)
            {
                case 0:
                    gender = CatGender.Male;
                    catName = "Meow meow " + FindObjectsOfType<CatAI>().Length.ToString();
                    break;
                case 1:
                    gender = CatGender.Female;
                    catName = "Snowball " + FindObjectsOfType<CatAI>().Length.ToString();
                    break;
            }
        }

        // other inits
        spawnNewCatsScript = FindObjectOfType<SpawnNewCats>();
        roamRandomScript = GetComponent<RoamRandom>();
        goToConsumableScript = GetComponent<GoToConsumable>();
        goToConsumableScript.enabled = false;
        goToLitterTrayScript = GetComponent<GoToLitterTray>();
        goToConsumableScript.enabled = false;
        otherCatsNearby = false;
        isPregnant = false;

    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DepleteFoodAndWater());
        StartCoroutine(Feed());
        StartCoroutine(Drink());
        StartCoroutine(Age());
        StartCoroutine(PeriodicBowlMovement());
    }

    // Update is called once per frame
    void Update()
    {
        // Go eat and drink
        if(goToConsumableScript.enabled == true)
        {
            GoToConsumable();
        }
        //Go poop
        if (needsLitterTray)
        {
            Poop();
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Cat")
        {
            otherCatsNearby = true;
            otherCatGender = collision.gameObject.GetComponent<CatAI>().GetCatGender();
            otherCatAge = collision.gameObject.GetComponent<CatAI>().GetCatAge();
            // mate
            if (CanMate())
            {
                Mate();
            }
        }

        if (collision.gameObject.tag == "Food")
        {
            nearFood = true;
            consumable = collision.gameObject.GetComponent<Consumable>();
        }

        if (collision.gameObject.tag == "Water")
        {
            nearWater = true;
            consumable = collision.gameObject.GetComponent<Consumable>();
        }

        if (collision.gameObject.tag == "Litter")
        {
            nearLitter = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Cat")
        {
            otherCatsNearby = false;
        }

        if (collision.gameObject.tag == "Food")
        {
            nearFood = false;
        }

        if (collision.gameObject.tag == "Water")
        {
            nearWater = false;
        }

        if (collision.gameObject.tag == "Litter")
        {
            nearLitter = false;
        }
    }

    // FEED when FOOD < foodMin
    private IEnumerator Feed()
    {
        while (true)
        {
            // if food < food min => go towards food bowl
            if (food <= foodMin)
            {
                roamRandomScript.enabled = false;
                goToConsumableScript.enabled = true;
                if (!otherCatsNearby && nearFood && !nearWater && !nearLitter)
                {
                    while (food < foodMax && consumable.GetConsumableAmount() > 0)
                    {
                        consumable.ConsumeConsumable();
                        food++;
                        yield return new WaitForSeconds(feedingTimeStep);
                    }
                }
            }
            // if food > foodMax => go back to randomly roaming
            if (food >= foodMax)
            {
                food = foodMax;
                goToConsumableScript.enabled = false;
                roamRandomScript.enabled = true;
            }
            // if consumableAmount < 0 => go back to randomly roaming
            if (consumable != null)
            {
                if (consumable.GetConsumableAmount() <= 0)
                {
                    goToConsumableScript.enabled = false;
                    roamRandomScript.enabled = true;
                }
            }
            yield return new WaitForSeconds(feedingTimeStep);
        }
            
    }

    // Cat gets hungry over time
     private IEnumerator DepleteFoodAndWater()
    {
        while (true)
        {
            yield return new WaitForSeconds(depleteConsumableTime);
            SpendEnergy();
        }
        

    }
    // Use food and water
    private void SpendEnergy()
    {
        if (food > 0)
        {
            food--;
        }
        if (water > 0)
        {
            water--;
        }
    }

    // DRINK when Water = 0
    private IEnumerator Drink()
    {
        while (true)
        {
            // if water < water min => go towards water bowl
            if (water <= waterMin)
            {
                roamRandomScript.enabled = false;
                goToConsumableScript.enabled = true;
                if (!otherCatsNearby && !nearFood && nearWater && !nearLitter)
                {
                    while (water < waterMax && consumable.GetConsumableAmount() > 0)
                    {
                        consumable.ConsumeConsumable();
                        water++;
                        yield return new WaitForSeconds(drinkingTimeStep);
                    }
                }
            }
            // if water > waterMax => go back to randomly roaming
            if (water >= waterMax)
            {
                water = waterMax;
                goToConsumableScript.enabled = false;
                roamRandomScript.enabled = true;
            }
            // if consumableAmount < 0 => go back to randomly roaming
            if (consumable != null)
            {
                if (consumable.GetConsumableAmount() <= 0)
                {
                    goToConsumableScript.enabled = false;
                    roamRandomScript.enabled = true;
                }
            }
            yield return new WaitForSeconds(drinkingTimeStep);
        }
    }
    private IEnumerator PeriodicBowlMovement()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenPoops);
            needsLitterTray = true;
        }
    }
    // POOP when needsLitterTray = true
    private void Poop()
    {
        // if needsLitterTray  ==> if CATS near LitterTray == 0 && LitterTray is CLEAN => POOP
        
        if (needsLitterTray)
        {
            roamRandomScript.enabled = false;
            goToLitterTrayScript.enabled = true;
            GoToLitter();
            if (!otherCatsNearby && !nearFood && !nearWater && nearLitter)
            {
                if (goToLitterTrayScript.GetLitterTray().IsClean())
                {
                    goToLitterTrayScript.GetLitterTray().UseTray();
                    needsLitterTray = false;
                    roamRandomScript.enabled = true;
                    goToLitterTrayScript.enabled = false;
                }
                else
                {
                    // poop outside litter box
                    Instantiate(poop, gameObject.transform.position, Quaternion.identity);
                    needsLitterTray = false;
                    roamRandomScript.enabled = true;
                    goToLitterTrayScript.enabled = false;
                }
            }
        }       
    }

    // MATE IF OLDER THN 4 MONTHS
    private void Mate()
    {
        // if AGE > 4 => MATE => if sucess => if female => PREGNANCY
        if (otherCatsNearby)
        {
            if((gender == CatGender.Male && otherCatGender == CatGender.Female) || (gender == CatGender.Female && otherCatGender == CatGender.Male))
            {
                if(age >= 4 && otherCatAge >= 4 && CanMate())
                {
                    //Mate
                    if(gender == CatGender.Female && !isPregnant)
                    {
                        Debug.Log("Pregant!");
                        canMate = false;
                        isPregnant = true;
                        pregancyAge = age;
                        StartCoroutine(Pregnancy());
                    }
                }
            }
        }
    }

    // PREGNANCY
    private IEnumerator Pregnancy()
    {
        // AFTER 9 weeks => 5 or 6 new cats
        // !canMate for 6 weeks
        while (isPregnant)
        {
            if(age == pregancyAge + 9)
            {
                //New Cats!
                Vector2 spawnPos = gameObject.transform.position;
                spawnNewCatsScript.SpawnCats(spawnPos);
                Debug.Log("New CATS!");
                lastPregnancyEngAge = age;
                isPregnant = false;
                StopCoroutine(Pregnancy());
            }
            Debug.Log("wait for it");
            yield return null;
        }
    }
    private bool CanMate()
    {
        if (gender == CatGender.Male && age >= 4)
        {
            canMate = true;
            return canMate;
        }
        if (gender == CatGender.Female && age >= 4 && !isPregnant && age >= lastPregnancyEngAge + 6)
        {
            canMate = true;
            return canMate;
        }
        return false;
    }

    // FIGHT
    private void Fight()
    {
        // if male => if AGE > 6 => if near other male => Prob. Fight 
    }
    // GO TO LITTER TRAY
    private void GoToLitter()
    {
        goToLitterTrayScript.GoToLitter();
    }

    // GO TO FOOD BOWL
    private void GoToFoodBowl()
    {
        goToConsumableScript.GoToFood();
    }

    // GO TO WATER BOWL
    private void GoToWaterBowl()
    {
        goToConsumableScript.GoToWater();
    }

    // GO TO CONSUMABLE
    private void GoToConsumable()
    {
        if (food < foodMin)
        {
            GoToFoodBowl();
        }

        if (water < waterMin)
        {
            GoToWaterBowl();
        }
    }

    // Age the cat over time
    private IEnumerator Age()
    {
        while (true)
        {
            yield return new WaitForSeconds(ageTimeStep);
            age++;
        }
    }

    public CatGender GetCatGender()
    {
        return gender;
    }

    public int GetCatAge()
    {
        return age;
    }
}
