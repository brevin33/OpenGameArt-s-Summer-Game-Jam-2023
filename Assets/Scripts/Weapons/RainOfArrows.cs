using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RainOfArrows : Weapon
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
        transform.rotation = Quaternion.identity * Quaternion.Euler(180,0,0);
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

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Enemy")
        {
            hit = true;
            other.GetComponent<Enemy>().Hit(Vector3.zero, damage * Time.deltaTime * Mathf.Pow(comboMultiplyer, Player.combo));
        }
    }
}
