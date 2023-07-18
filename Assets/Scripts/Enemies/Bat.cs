using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Bat : Enemy
{
    [SerializeField]
    float changeDirTimerTime;

    float changeDirTimer;

    Vector3 moveDir;
    public override Vector3 path()
    {
        float randomMovePotential = 1f;
        if (changeDirTimer >= changeDirTimerTime)
        {
            moveDir = new Vector3(Random.Range(-randomMovePotential, randomMovePotential), 0, Random.Range(-randomMovePotential, randomMovePotential));
            changeDirTimer = 0f;
        }
        changeDirTimer += Time.deltaTime;
        return moveDir.normalized;
    }
}
