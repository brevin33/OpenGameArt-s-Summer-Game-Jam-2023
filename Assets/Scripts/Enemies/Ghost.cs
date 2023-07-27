using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : Enemy
{

    [SerializeField]
    float changeDirTimerTime;

    [SerializeField]
    float visibiilityChangeSpeed;

    float changeDirTimer = 0;

    float circleDist;

    float circleDir = 90;

    SpriteRenderer ghost;

    Material mat2;

    private void Awake()
    {
        circleDist = Random.Range(2.6f, 3.2f);
        ghost = GetComponent<SpriteRenderer>();
        mat2 = ghost.material;
    }

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
            StartCoroutine(flashWhite2());
        }
        if (knockBack.magnitude > 0)
        {
            velocity = knockBack;
        }
    }

    public IEnumerator flashWhite2()
    {
        soundManager.enemyHurt();
        mat2.SetInt("_white", 1);
        yield return new WaitForSeconds(0.3f);
        mat2.SetInt("_white", 0);
    }

    public override Vector3 path()
    {
        changeDirTimer += Time.deltaTime;
        if (changeDirTimer > changeDirTimerTime)
        {

            switch (Random.Range(0, 3))
            {
                case 0:
                    circleDir = 0; break;
                case 1:
                    circleDir = 90; break;
                case 2:
                    circleDir = 270; break;
            }
            changeDirTimer = 0;
        }

        Vector3 moveDir;

        float alpha = mat2.GetFloat("_Alpha");

        if (Vector3.Distance(transform.position, playerPos.position) < circleDist - 0.7f)
        {
            circleDir = 0;
        }

        if (Vector3.Distance(transform.position, playerPos.position) < circleDist + 1.1f && circleDir == 0)
        {
            mat2.SetFloat("_Alpha", Mathf.Lerp(0, 1, alpha + visibiilityChangeSpeed * Time.deltaTime));
        }
        else if(Vector3.Distance(transform.position, playerPos.position) < circleDist - .4f)
        {
            mat2.SetFloat("_Alpha", Mathf.Lerp(0, 1, alpha + visibiilityChangeSpeed * Time.deltaTime));
        }
        else {
            mat2.SetFloat("_Alpha", Mathf.Lerp(0, 1, alpha - visibiilityChangeSpeed* Time.deltaTime));
        }
if (Vector3.Distance(transform.position, playerPos.position) < circleDist)
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
}
