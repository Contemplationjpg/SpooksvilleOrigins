using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SeedManager : MonoBehaviour
{
    public static SeedManager instance;
    public int currentSeed = 1000000;
    void Awake()
    {
        instance = this;
    }

    public void SetCurrentSeed(int newSeed)
    {
        currentSeed = Math.Abs(newSeed);
    }

    public int GenerateRandomSeed()
    {
        int seed = UnityEngine.Random.Range(1000000, 9999999);
        print(seed);
        return seed;   
    }
}
