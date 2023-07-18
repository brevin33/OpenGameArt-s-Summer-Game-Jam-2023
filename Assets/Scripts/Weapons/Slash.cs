using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Slash : Weapon
{

    [SerializeField]
    int damage = 3;

    [SerializeField]
    float comboMultiplyer = 1.1f;

    [SerializeField]
    float knockBackDist;

    [SerializeField]
    float HitboxDuration = 0.2f;

    Player p;

    bool hit = false;

    private void Start()
    {
        p = player.GetComponent<Player>();
    }

    private void Update()
    {
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
            Vector3 knockBack = transform.rotation * Vector3.one;
            knockBack.y = 0;
            knockBack = knockBack.normalized * knockBackDist;
            other.GetComponent<Enemy>().Hit(knockBack, damage * Mathf.Pow(comboMultiplyer, Player.combo));
        }
    }
}
