using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slime : Enemy
{
    [SerializeField]
    float changeDirTimerTime;

    [SerializeField]
    float jumpHeight;

    [SerializeField]
    float startY;

    float changeDirTimer;

    public Vector3 moveDir;

    bool first = true;
    public override Vector3 path()
    {
        float randomMovePotential = 1f;
        moveDir.y -= 2f*Time.deltaTime;
        if (moveDir.y <= -jumpHeight || first)
        {
            //transform.position =  new Vector3( transform.position.x, startY, transform.position.z);
            moveDir = new Vector3(Random.Range(-randomMovePotential, randomMovePotential), jumpHeight, Random.Range(-randomMovePotential, randomMovePotential));
            changeDirTimer = 0f;
            first = false;
        }
        changeDirTimer += Time.deltaTime;
        return moveDir.normalized;
    }
}
