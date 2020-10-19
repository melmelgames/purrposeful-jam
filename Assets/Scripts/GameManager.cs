using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private int averageCatHappiness;
    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartCoroutine(AverageCatsHappiness());
    }

    private IEnumerator AverageCatsHappiness()
    {
        while (true)
        {
            CatAI[] cats = FindObjectsOfType<CatAI>();
            int happinessSum = 0;
            int numberOfCats = 0;
            foreach (CatAI cat in cats)
            {
                happinessSum += cat.GetCatHappinessLevel();
                numberOfCats++;
            }
            averageCatHappiness = happinessSum / numberOfCats;
            yield return new WaitForSeconds(1f);
        }
    }

    public int GetCatAverageHappiness()
    {
        return averageCatHappiness;
    }
}
