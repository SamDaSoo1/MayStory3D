using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataReset : MonoBehaviour
{
    void Start()
    {
        if (PlayerPrefs.HasKey("Hammer"))
            PlayerPrefs.SetInt("Hammer", 0);

        if (PlayerPrefs.HasKey("SpearFish"))
            PlayerPrefs.SetInt("SpearFish", 0);

        if (PlayerPrefs.HasKey("DaggerSwap"))
            PlayerPrefs.SetInt("DaggerSwap", 0);

        if (PlayerPrefs.HasKey("WarriorSwap"))
            PlayerPrefs.SetInt("WarriorSwap", 0);
    }
}
