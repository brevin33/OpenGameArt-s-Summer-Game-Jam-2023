using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Arrow : Weapon
{
    [SerializeField]
    float damage = 3;

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

    public bool updateCombo = true;

    public Barrage barrage = null;

    public Vector3 moveDir;

    private void Start()
    {
        moveDir = (transform.position - player.transform.position).normalized;
        transform.rotation *= Quaternion.Euler(0, 0, -225);
        p = player.GetComponent<Player>();
    }

    private void Update()
    {
        Vector3 newPosition = transform.position + moveDir * speed * Time.deltaTime;
        if (newPosition.x < allowedArea.xMin)
        {
            if (updateCombo)
            {
                if (hit)
                {
                    p.hitCombo();
                }
                else
                {
                    p.dropCombo();
                }
            }
            else
            {
                barrage.hit = barrage.hit || hit;
            }
            Destroy(gameObject);
        }
        else if (newPosition.x > allowedArea.xMax)
        {
            if (updateCombo)
            {
                if (hit)
                {
                    p.hitCombo();
                }
                else
                {
                    p.dropCombo();
                }
            }
            else
            {
                barrage.hit = barrage.hit || hit;
            }
            Destroy(gameObject);
        }
        if (newPosition.z < allowedArea.yMin)
        {
            if (updateCombo)
            {
                if (hit)
                {
                    p.hitCombo();
                }
                else
                {
                    p.dropCombo();
                }
            }
            else
            {
                barrage.hit = barrage.hit || hit;
            }
            Destroy(gameObject);
        }
        else if (newPosition.z > allowedArea.yMax)
        {
            if (updateCombo)
            {
                if (hit)
                {
                    p.hitCombo();
                }
                else
                {
                    p.dropCombo();
                }
            }
            else
            {
                barrage.hit = barrage.hit || hit;
            }
            Destroy(gameObject);
        }
        transform.position = newPosition;
        HitboxDuration -= Time.deltaTime;
        if (HitboxDuration <= 0)
        {
            if (updateCombo)
            {
                if (hit)
                {
                    p.hitCombo();
                }
                else
                {
                    p.dropCombo();
                }
            }
            else
            {
                barrage.hit = barrage.hit || hit;
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
            if (updateCombo)
            {
                p.hitCombo();
            }
            else
            {
                barrage.hit = barrage.hit || hit;
            }
            Destroy(gameObject);
        }
    }
}
