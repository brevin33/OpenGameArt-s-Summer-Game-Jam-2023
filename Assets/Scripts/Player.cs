using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.IMGUI.Controls.CapsuleBoundsHandle;
using UnityEngine.UI;
using static UnityEngine.UI.Image;

public class Player : MonoBehaviour
{

    [SerializeField]
    Camera camera;

    [SerializeField, Range(0f, 100f)]
    float maxSpeed = 10f;

    [SerializeField, Range(0f, 300f)]
    float maxAcceleration = 10f;

    [SerializeField, Range(0f, 1f)]
    float bounciness = 0.5f;

    [SerializeField]
    Rect allowedArea = new Rect(-5f, -5f, 10f, 10f);

    Vector3 velocity;

    Vector3 mousePos;

    Weapon[] weapons;

    bool attacking;

    int attackingWeapon;

    float nextWeapon;

    Vector2 playerInput;

    bool tryAttacking;

    int combo = 1;

    public void hitCombo()
    {
        combo += 1;
    }

    public void dropCombo()
    {
        combo = 1;
    }

    void Update()
    {
        getInput();

        WASDMovment();

        attack();

        faceMouse();
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
        transform.position = Vector3.zero;
        playerInput.x = Input.GetAxis("Horizontal");
        playerInput.y = Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        if (Input.GetButtonDown("Fire1"))
        {
            tryAttacking = true;
        }

        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            mousePos = raycastHit.point;
        }
    }


    void faceMouse()
    {
        Vector3 facingDir = mousePos - transform.localPosition;
        // -----------------------------------------------------------    Not Implimented ---------------------------------
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

    IEnumerator doAttacks()
    {
        combo = 1;
        for (int i = 0; i < weapons.Length; i++)
        {
            Weapon weapon = weapons[i];
            weapon.attack(mousePos, transform);
            yield return new WaitForSeconds(weapon.getCooldown());
        }
        attacking = false;
    }

}
