using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{

    public GameObject weapon;
    public Player player;

    SpriteRenderer spriteRenderer;

    public void setSprite(Sprite sprite)
    {
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
