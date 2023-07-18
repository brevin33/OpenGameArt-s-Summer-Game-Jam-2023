using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : Weapon
{
    [SerializeField]
    int damage = 3;

    [SerializeField]
    float comboMultiplyer = 1.1f;

    [SerializeField]
    float knockBackDist;

    [SerializeField]
    float HitboxDuration = 0.2f;

    [SerializeField]
    float speed;

    Player p;

    Rect allowedArea = new Rect(-5.47f, -2.93f, 10.94f, 5.9f);


    bool hit = false;

    Vector3 moveDir;

    private void Start()
    {
        moveDir = transform.position - player.transform.position;
        transform.rotation *= Quaternion.Euler(0, 0, -225);
        p = player.GetComponent<Player>();
    }

    private void Update()
    {
        Vector3 newPosition = transform.position + moveDir * speed;
        if (newPosition.x < allowedArea.xMin)
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
        else if (newPosition.x > allowedArea.xMax)
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
        if (newPosition.z < allowedArea.yMin)
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
        else if (newPosition.z > allowedArea.yMax)
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
        transform.position += moveDir * speed;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            hit = true;
            Vector3 knockBack =  transform.rotation * Quaternion.Euler(0, 0, 180) * Vector3.one;
            knockBack.y = 0;
            knockBack = knockBack.normalized * knockBackDist;
            other.GetComponent<Enemy>().Hit(knockBack, damage * Mathf.Pow(comboMultiplyer, Player.combo));
            p.hitCombo();
            Destroy(gameObject);
        }
    }
}
