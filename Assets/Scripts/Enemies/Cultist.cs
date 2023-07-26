using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cultist : Enemy
{

    [SerializeField]
    float fireBallcooldownTime;

    float fireBallcooldown = 0;

    [SerializeField]
    GameObject fireball;

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
        else if ( hitfalshTimer > 0.4f)
        {
            hitfalshTimer = 0;
            StartCoroutine(flashWhite());
        }
    }

    public override Vector3 path()
    {
        return Vector3.zero;
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
