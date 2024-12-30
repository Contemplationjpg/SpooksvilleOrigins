using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Player", menuName = "Entity/Player")]
public class PlayerType : EntityType
{
    public int currentSugar = 0;
    public int maxSugar = 100;
    public int baseActionCount = 3;
}
