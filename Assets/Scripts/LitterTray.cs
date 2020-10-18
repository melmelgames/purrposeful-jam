using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LitterTray : MonoBehaviour
{
    [SerializeField] private bool isClean;

    public void CleanTray()
    {
        isClean = true;
    }

    public bool IsClean()
    {
        return isClean;
    }

    public void UseTray()
    {
        isClean = false;
    }
  
}
