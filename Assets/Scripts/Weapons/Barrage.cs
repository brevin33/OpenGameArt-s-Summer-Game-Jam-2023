using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Barrage : Weapon
{
    [SerializeField]
    float fireRate;

    float fireRateCooldown = 0;

    [SerializeField]
    GameObject Arrow;

    Arrow lastarrow;

    [SerializeField]
    float barrageScatter;

    Vector3 moveDir;

    [SerializeField]
    float HitboxDuration;


    public bool hit;

    Player p;


    private void Start()
    {
        moveDir = transform.position - player.transform.position;
        p = player.GetComponent<Player>();
        transform.rotation *= Quaternion.Euler(0, 0, 130);
    }
    private void Update()
    {
        fireRateCooldown += Time.deltaTime;
        if (fireRateCooldown > fireRate) {
            fireRateCooldown = 0;
            createArrow();
        }

        HitboxDuration -= Time.deltaTime;
        if (HitboxDuration <= 0)
        {
            if (hit)
            {
                p.hitCombo();
            }
            else
            {
                p.dropCombo();
            }
            Destroy(gameObject);
        }
    }


    void createArrow()
    {
        GameObject hitBox = Instantiate(Arrow, transform.position, transform.rotation);
        lastarrow = hitBox.GetComponent<Arrow>();
        lastarrow.player = player;
        StartCoroutine(changeMoveDir());
        lastarrow.updateCombo = false;
        lastarrow.barrage = this;

    }
    IEnumerator changeMoveDir()
    {
        yield return new WaitForEndOfFrame();
        lastarrow.moveDir = moveDir + new Vector3(Random.Range(-barrageScatter, barrageScatter), Random.Range(-barrageScatter, barrageScatter), Random.Range(-barrageScatter, barrageScatter));
        lastarrow.moveDir.y = 0;
        lastarrow.moveDir = lastarrow.moveDir.normalized;
        lastarrow.transform.rotation = Quaternion.LookRotation(lastarrow.moveDir) * Quaternion.Euler(90,140,0);

    }
}
    