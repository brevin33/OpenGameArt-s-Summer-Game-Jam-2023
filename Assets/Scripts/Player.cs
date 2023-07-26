using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.CapsuleBoundsHandle;
using UnityEngine.UI;
using static UnityEngine.UI.Image;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using Unity.Mathematics;
using Microsoft.Win32.SafeHandles;
using TMPro;
using Unity.VisualScripting;
using Random = UnityEngine.Random;

public class Player : MonoBehaviour
{

    [SerializeField]
    private Camera camera;

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;

    [SerializeField, Range(0f, 300f)]
    float maxAcceleration = 10f;

    [SerializeField, Range(0f, 1f)]
    float bounciness = 0.5f;

    [SerializeField]
    Rect allowedArea = new Rect(-2.5f, -2.93f, 5f, 5.9f);

    [SerializeField]
    int HP;

    [SerializeField]
    GameObject[] Hearts;

    [SerializeField]
    LayerMask mousePosHits;

    [SerializeField]
    float hitInvulneriblilityTime;

    [SerializeField]
    SpriteRenderer[] renders;

    [SerializeField]
    Game game;

    [SerializeField]
    float attackCooldownTime = 1f;

    public Vector3 velocity;

    public static Vector3 mousePos;

    public static Vector3 playerPos;

    [SerializeField]
    public List<GameObject> weapons;

    bool attacking;

    int attackingWeapon;

    float nextWeapon;

    Vector2 playerInput;

    bool tryAttacking;

    bool facingLeft;

    public static int combo = 0;

    bool hitInvulneriblility;

    [SerializeField]
    TextMeshProUGUI comboText;

    [SerializeField]
    TextMeshProUGUI damageMultText;

    float textAlpha = 0;

    [SerializeField]
    Image reloadBar;

    public int c;

    bool gameOver;
    public void hitCombo()
    {
        combo += 1;
        comboText.text = "combo " + combo.ToString();
        damageMultText.text = "damage " + (Mathf.Round((Mathf.Pow(1.1f,combo) * 100f))/100f).ToString();
        if (combo >= 3) {
            textAlpha = 1;
        }
    }

    public void dropCombo()
    {
        Debug.Log(combo);
        combo = 0;
    }

    public void pickupWeapon(GameObject weapon)
    {
        weapons.Add(weapon);
    }

    public void takeDamage(int amount, Vector3 knockBack)
    {
        c = combo;
        if (hitInvulneriblility)
        {
            return;
        }
        for (int i = Mathf.Max(HP-amount, 0); i < HP; i++)
        {
            Hearts[i].SetActive(false);
        }
        HP -= amount;
        if (HP <= 0)
        {
            game.gameOver();
            gameOver = true;
        }
        velocity = knockBack;
        hitInvulneriblility = true;
        StartCoroutine(Invulnerablility());
    }


    public void gainHP( int amount)
    {
        if (HP+amount > 9)
        {
            for (int i = HP; i < 9; i++)
            {
                Hearts[i].SetActive(true);
            }
            HP = 9;
            return;
        }
        for (int i = HP; i < amount + HP; i++)
        {
            Hearts[i].SetActive(true);
        }
        HP += amount;
    }

    private void Awake()
    {
        for (int i = 0; i < HP; i++) {
            Hearts[i].SetActive(true);
        }
        combo = 0;
    }

    void Update()
    {
        if (gameOver)
        {
            return;
        }
        getInput();

        WASDMovment();

        attack();

        flipCharacter();

        textAlpha -= 0.7f * Time.deltaTime;

        comboText.alpha = textAlpha;
        damageMultText.alpha = textAlpha;

        playerPos = transform.position;
    }

    void attack()
    {
        if (attacking)
        {
            return;
        }
        if (tryAttacking)
        {
            attacking = true;
            StartCoroutine(doAttacks());
        }
    }

    void getInput()
    {
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        if (Input.GetButtonDown("Fire1"))
        {
            tryAttacking = true;
        }
        else
        {
            tryAttacking= false;
        }
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, mousePosHits))
        {
            mousePos = new Vector3( raycastHit.point.x, 0.5f, raycastHit.point.z);
        }
    }

    private void flipCharacter()
    {
        if (facingLeft && playerInput.x < 0)
        {
            facingLeft = false;
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
        }
        else if (!facingLeft && playerInput.x > 0)
        {
            facingLeft = true;
            transform.localScale = Vector3.Scale(transform.localScale, new Vector3(-1, 1, 1));
        }
    }

    void WASDMovment()
    {

        Vector3 desiredVelocity =
            new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

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
    }

    public void createHitbox(float distFromPlayer, GameObject prefab)
    {
        Vector3 facingDir = mousePos - transform.position;
        facingDir.y = 0;
        facingDir.Normalize();
        Vector3 spawnPos = transform.position + facingDir * distFromPlayer;
        quaternion rotation = Quaternion.LookRotation(facingDir);
        rotation *= Quaternion.Euler(90, 270, 0);
        GameObject hitBox = Instantiate(prefab, spawnPos, rotation);
        hitBox.GetComponent<Weapon>().player = this;
    }

    IEnumerator doAttacks()
    {
        combo = 0;
        for (int i = weapons.Count-1; i >= 0; i--)
        {
            t = 0;
            yield return new WaitUntil(nextAttack);
            GameObject weapon = weapons[i];
            Stats stats = weapon.GetComponent<Stats>();
            createHitbox(stats.getDistFromPlayer(mousePos,playerPos), weapon);
            yield return new WaitForSeconds(stats.cooldown);
        }
        reloadTime = 0;
        reloadBar.enabled = true;
        yield return new WaitUntil(reload);
        attacking = false;
    }

    float reloadTime;
    private bool reload()
    {
        reloadTime += Time.deltaTime;
        reloadBar.fillAmount = reloadTime/attackCooldownTime;
        if (reloadTime > attackCooldownTime) {
            reloadBar.enabled=false;
            return true;
        }
        return false;
    }

    float t;
    bool nextAttack()
    {
        t += Time.deltaTime;
        if (t > 1.5f)
        {
            combo = 0;
        }
        return tryAttacking;
    }

    IEnumerator Invulnerablility()
    {
        bool turnOn = false;
        for (int j = 0; j < 6; j++)
        {
            for (int i = 0; i < renders.Length; i++)
            {
                renders[i].enabled = turnOn;
            }
            yield return new WaitForSeconds(hitInvulneriblilityTime/6);
            turnOn = !turnOn;
        }
        for (int i = 0; i < renders.Length; i++)
        {
            renders[i].enabled = true;
        }
        hitInvulneriblility = false;
    }

}
