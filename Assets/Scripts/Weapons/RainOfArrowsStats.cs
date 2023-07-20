using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainOfArrowsStats : Stats
{
    public override float getDistFromPlayer(Vector3 mousePos, Vector3 playerPos)
    {

        return Vector3.Distance(mousePos,playerPos) ;
    }
}
