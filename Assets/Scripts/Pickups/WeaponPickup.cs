using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    public GameObject weapon;
    public Player player;

    [SerializeField]
    SpriteRenderer spriteRenderer;

    public void setup(GameObject w, Player p, Sprite sprite)
    {
        player = p;
        weapon = w;
        spriteRenderer.sprite = sprite;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.pickupWeapon(weapon);
            Destroy(gameObject);
        }
    }
}
