using UnityEngine;

public interface Weapon
{
    int getDamage();

    float getCooldown();
    bool attack(Vector3 mousePos, Transform player);
}


public class Slash: Weapon
{
    public GameObject slash;

    public int getDamage()
    {
        return 10;
    }

    public float getCooldown()
    {
        return 0.2f;
    }
    public bool attack(Vector3 mousePos, Transform player)
    {
        Vector3 facingDir = mousePos - player.position;
        facingDir.Normalize();
        Vector3 spawnPos =  player.position + facingDir * 0.75f;
        Instantiate(slash, spawnPos, player.rotation);
        return true;
    }
}