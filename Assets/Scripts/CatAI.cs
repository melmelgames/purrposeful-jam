using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAI : MonoBehaviour
{

    [SerializeField] private string catName;
    [SerializeField] private string gender;
    [SerializeField] private float happiness;
    [SerializeField] private int age;
    [SerializeField] private float food;
    [SerializeField] private float water;
    [SerializeField] private bool needsLitterTray;
    [SerializeField] private bool canMate;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // FEED when FOOD = 0
    private void Feed()
    {
        // if FOOD == 0 ==> if CATS near FOOD == 0 => FEED
    }

    // DRINK when Water = 0
    private void Drink()
    {
        // if WATER == 0 ==> if CATS near WATER == 0 => DRINK
    }

    // POOP when needsLitterTray = true
    private void Poop()
    {
        // if needsLitterTray  ==> if CATS near LitterTray == 0 && LitterTray is CLEAN => POOP
    }

    // MATE IF OLDER THN 4 MONTHS
    private void Mate()
    {
        // if AGE > 4 => MATE => if sucess => if female => PREGNANCY
    }

    // PREGNANCY
    private void Pregnancy()
    {
        // AFTER 9 weeks => 5 or 6 new cats
        // !canMate for 6 weeks
    }

    // FIGHT
    private void Fight()
    {
        // if male => if AGE > 6 => if near other male => Prob. Fight 
    }
}
