using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomizationManager : MonoBehaviour
{
    public static RandomizationManager instance;
    public AreaType area;
    void Awake()
    {
        instance = this;       
    }
    
    
}
