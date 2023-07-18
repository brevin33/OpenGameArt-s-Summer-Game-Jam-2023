using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Dragon : Enemy
{

    [SerializeField]
    float fireBallcooldownTime;

    float fireBallcooldown = 0;

    [SerializeField]
    GameObject fireball;

    [SerializeField]
    float changeDirTimerTime;

    float changeDirTimer = 0;

    float circleDist;

    float circleDir = 90;

    private void Awake()
    {
        circleDist = Random.Range(2.4f, 3.3f);
    }

    public override Vector3 path()
    {
        changeDirTimer += Time.deltaTime;
        if (changeDirTimer > changeDirTimerTime)
        {
            
            switch(Random.Range(0, 3)){
                case 0:
                    circleDir = 0; break;
                case 1:
                    circleDir = 90; break;
                case 2:
                    circleDir = 270; break;
            }
            changeDirTimer = 0;
        }
        float randomMovePotential = 0.7f;
        Vector3 moveDir;
        if (Vector3.Distance(transform.position,playerPos.position) < circleDist)
        {
            moveDir = Quaternion.Euler(0, circleDir, 0) * (player.transform.position - transform.position);
        }
        else
        {
            moveDir = (player.transform.position - transform.position);
        }
        moveDir.y = 0;
        return moveDir.normalized;
    }

    public override void specialAblility()
    {
        fireBallcooldown += Time.deltaTime;
        if (fireBallcooldown > fireBallcooldownTime)
        {
            fireBallcooldown = 0;
            shootFireball();
        }
    }

    private void shootFireball()
    {
        Vector3 fireballDir = (playerPos.position - transform.position).normalized;
        fireballDir.y = 0f;
        fireballDir = fireballDir.normalized;
        GameObject f = Instantiate(fireball, transform.position + (fireballDir * 0.3f),  Quaternion.Euler(90, 270, 0));
        f.GetComponent<Fireball>().setUp(fireballDir,player);
    }
}
