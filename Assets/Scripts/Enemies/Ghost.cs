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

    Material mat;

    private void Awake()
    {
        circleDist = Random.Range(2.6f, 3.2f);
        ghost = GetComponent<SpriteRenderer>();
        mat = ghost.material;
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

        float alpha = mat.GetFloat("_Alpha");

        if (Vector3.Distance(transform.position, playerPos.position) < circleDist - 0.7f)
        {
            circleDir = 0;
        }

        if (Vector3.Distance(transform.position, playerPos.position) < circleDist + 1.1f && circleDir == 0)
        {
            mat.SetFloat("_Alpha", Mathf.Lerp(0, 1, alpha + visibiilityChangeSpeed * Time.deltaTime));
        }
        else if(Vector3.Distance(transform.position, playerPos.position) < circleDist - .4f)
        {
            mat.SetFloat("_Alpha", Mathf.Lerp(0, 1, alpha + visibiilityChangeSpeed * Time.deltaTime));
        }
        else {
            mat.SetFloat("_Alpha", Mathf.Lerp(0, 1, alpha - visibiilityChangeSpeed* Time.deltaTime));
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
