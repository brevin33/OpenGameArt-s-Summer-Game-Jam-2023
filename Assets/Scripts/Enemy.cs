using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    float HP = 6;

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;

    [SerializeField, Range(0f, 300f)]
    float maxAcceleration = 3f;

    [SerializeField, Range(0f, 1f)]
    float bounciness = 0.5f;

    [SerializeField]
    int damage;

    [SerializeField]
    private Transform playerPos;

    [SerializeField]
    private Player player;

    [SerializeField]
    Rect allowedArea = new Rect(-5.47f, -2.93f, 10.94f, 5.9f);

    Vector3 velocity;

    bool facingLeft = true;

    public virtual void Hit(Vector3 knockBack, float damage)
    {
        HP -= damage;
        if(HP <= 0)
        {
            Game.EnemyDied();
            Destroy(gameObject);
        }
        velocity = knockBack;
    }

    public virtual Vector3 path()
    {
        return (playerPos.position - transform.position).normalized;
    }

    private void Update()
    {
        Vector3 desiredVelocity = path() * maxSpeed;

        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        velocity.x =
            Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z =
            Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);

        Vector3 displacement = velocity * Time.deltaTime;
        Vector3 newPosition = transform.localPosition + displacement;
        if (newPosition.x < allowedArea.xMin)
        {
            newPosition.x = allowedArea.xMin;
            velocity.x = -velocity.x * bounciness;
        }
        else if (newPosition.x > allowedArea.xMax)
        {
            newPosition.x = allowedArea.xMax;
            velocity.x = -velocity.x * bounciness;
        }
        if (newPosition.z < allowedArea.yMin)
        {
            newPosition.z = allowedArea.yMin;
            velocity.z = -velocity.z * bounciness;
        }
        else if (newPosition.z > allowedArea.yMax)
        {
            newPosition.z = allowedArea.yMax;
            velocity.z = -velocity.z * bounciness;
        }
        transform.localPosition = newPosition;
        flipCharacter();
    }

    private void flipCharacter()
    {
        if (facingLeft && velocity.x > 0)
        {
            facingLeft = false;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        else if (!facingLeft && velocity.x < 0)
        {
            facingLeft = true;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            dealDamage();
        }
    }

    public virtual void dealDamage()
    {
        player.takeDamage(damage);
    }
}
