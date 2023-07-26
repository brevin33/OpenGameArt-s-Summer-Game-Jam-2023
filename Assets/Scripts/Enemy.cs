using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Image = UnityEngine.UI.Image;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    public float HP = 6;

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;

    [SerializeField, Range(0f, 300f)]
    float maxAcceleration = 3f;

    [SerializeField, Range(0f, 1f)]
    float bounciness = 0.5f;

    [SerializeField]
    int damage;

    [SerializeField]
    public Transform playerPos;

    [SerializeField]
    public Player player;

    [SerializeField]
    Rect allowedArea = new Rect(-5.47f, -2.93f, 10.94f, 5.9f);

    [SerializeField]
    float knockBackStrength;

    [SerializeField]
    public Image healthBar;

    public Game game;

    public float maxHP;

    public Vector3 velocity;

    bool facingLeft = true;

    public float healthBarAlpha = 0;

    float startWait = 0.55f;

    Material mat;

    public float hitfalshTimer = 0;

    private void Start()
    {
        mat = GetComponent<SpriteRenderer>().material;
        maxHP = HP;
    }

    public void setup(GameObject p, Game g)
    {
        player = p.GetComponent<Player>();
        playerPos = p.transform;
        game = g;
    }


    public virtual void specialAblility (){
    }
    public virtual void Hit(Vector3 knockBack, float damage)
    {
        HP -= damage;
        healthBar.fillAmount = HP/maxHP;
        healthBarAlpha = 1;
        if (HP <= 0)
        {
            game.EnemyDied();
            Destroy(gameObject);
        }
        else if (hitfalshTimer > 0.4f)
        {
            hitfalshTimer = 0;
            StartCoroutine(flashWhite());
        }
        if (knockBack.magnitude > 0)
        {
            velocity = knockBack;
        }
    }

    public virtual Vector3 path()
    {
        float randomMovePotential = 0.7f;
        Vector3 movedir = ((playerPos.position - transform.position).normalized + new Vector3(Random.Range(-randomMovePotential, randomMovePotential),0, Random.Range(-randomMovePotential, randomMovePotential)));
        movedir.y = 0f;
        return movedir.normalized;
    }

    private void Update()
    {
        hitfalshTimer += Time.deltaTime;
        startWait -= Time.deltaTime;
        healthBar.color = new Color(1, 0, 0, healthBarAlpha);
        if (startWait > 0)
        {
            return;
        }
        Vector3 desiredVelocity = path() * maxSpeed;

        healthBarAlpha = Mathf.Max(0, healthBarAlpha - 0.8f * Time.deltaTime);

        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        velocity.x =
            Mathf.MoveTowards(velocity.x, desiredVelocity.x, maxSpeedChange);
        velocity.z =
            Mathf.MoveTowards(velocity.z, desiredVelocity.z, maxSpeedChange);
        velocity.y =
            Mathf.MoveTowards(velocity.y, desiredVelocity.y, maxSpeedChange);
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
        specialAblility();
    }

    private void flipCharacter()
    {
        if (facingLeft && velocity.x > 0.5f)
        {
            facingLeft = false;
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1,1,1));
        }
        else if (!facingLeft && velocity.x < -0.5f)
        {
            facingLeft = true;
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
        }
    }

    public IEnumerator flashWhite()
    {
        mat.SetInt("_white",1);
        yield return new WaitForSeconds(0.3f);
        mat.SetInt("_white", 0);
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
