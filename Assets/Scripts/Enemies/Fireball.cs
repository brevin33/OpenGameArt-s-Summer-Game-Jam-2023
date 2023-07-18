using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField]
    float speed;

    [SerializeField]
    float knockBackStrength;

    [SerializeField]
    int damage;

    [SerializeField]
    float HitboxDuration;


    Vector3 moveDir;

    public Player player;

    public void setUp(Vector3 dir, Player p)
    {
        moveDir = dir;
        player = p;
    }

    private void Update()
    {
        transform.position += moveDir * speed;
        HitboxDuration -= Time.deltaTime;
        if (HitboxDuration <= 0)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Vector3 knockBack = other.transform.position - transform.position;
            knockBack.y = 0;
            knockBack = knockBack.normalized * knockBackStrength;
            player.takeDamage(damage, knockBack);
        }
    }
}
