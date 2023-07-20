using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    [SerializeField]
    public float cooldown;
    [SerializeField]
    public float distFromPlayer;


    public virtual float getDistFromPlayer(Vector3 mousePos, Vector3 playerPos)
    {
        return distFromPlayer;
    }
}
