using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Boss : Enemy
{

    [SerializeField]
    float fireBallcooldownTime;

    float fireBallcooldown = 0;

    [SerializeField]
    GameObject fireball;

    [SerializeField]
    float changeDirTimerTime;

    float changeDirTimer = 5;

    float timer2 = 0;

    float savedVelocityMagnitude;


    public override void Hit(Vector3 knockBack, float damage)
    {
        HP -= damage;
        healthBar.fillAmount = HP / maxHP;
        healthBarAlpha = 1;
        if (HP <= 0)
        {
            game.EnemyDied();
            Destroy(gameObject);
        }
        else if (hitfalshTimer > 0.4f)
        {
            hitfalshTimer = 0;
            StartCoroutine(flashWhite());
        }
    }

    public override Vector3 path()
    {
        changeDirTimer += Time.deltaTime;
        if (changeDirTimer > changeDirTimerTime)
        {
            timer2 += Time.deltaTime;
            savedVelocityMagnitude = savedVelocityMagnitude == 0 ? velocity.magnitude : savedVelocityMagnitude;
            Vector3 lerpValue = velocity.normalized * savedVelocityMagnitude;
            velocity = Vector3.Lerp(lerpValue, Vector3.zero, timer2/2);
            if (timer2 > 2)
            {
                timer2 = 0;
                changeDirTimer = 0;
                savedVelocityMagnitude = 0;
                Vector3 moveDir = (playerPos.position - transform.position);
                moveDir.y = 0;
                return moveDir.normalized;
            }
        }
        return velocity.normalized;
    }

    public override void specialAblility()
    {
        fireBallcooldown += Time.deltaTime;
        if (fireBallcooldown > fireBallcooldownTime)
        {
            fireBallcooldown = 0;
            shotGunFireball();
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

    private void shotGunFireball()
    {
        for (int i = -3; i < 4; i++)
        {
            Vector3 fireballDir = (playerPos.position - transform.position).normalized;
            fireballDir =  Quaternion.Euler(0,15*i,0) * fireballDir;
            fireballDir.y = 0f;
            fireballDir = fireballDir.normalized;
            GameObject f = Instantiate(fireball, transform.position + (fireballDir * 0.3f), Quaternion.Euler(90, 270, 0));
            f.GetComponent<Fireball>().setUp(fireballDir, player);
        }
    }
}
