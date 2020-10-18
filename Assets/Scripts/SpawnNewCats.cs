using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnNewCats : MonoBehaviour
{
    [SerializeField] private GameObject catPrefab;
    public void SpawnCats(Vector2 position)
    {
        int numberOfCats = Random.Range(4, 7);
        for(int i = 0; i <= numberOfCats; i++)
        {
            Instantiate(catPrefab, position, Quaternion.identity);
        }
    }
}
