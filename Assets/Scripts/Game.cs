using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEditor.SceneManagement;
using UnityEditor.UI;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{

    [SerializeField]
    GameObject player;

    [SerializeField]
    Rect allowedArea = new Rect(-5.47f, -2.93f, 10.94f, 5.9f);

    [SerializeField]
    GameObject[] enimes;

    [SerializeField]
    GameObject[] walls;

    [SerializeField]
    GameObject[] floors;

    [SerializeField]
    Door[] doors;

    List<List<int>> stageEnimes = new List<List<int>> { new List<int> { 0 }, new List<int> { 0 } };

    [SerializeField]
    Transform newRoomPlayerSpawnPos;

    [SerializeField]
    GameObject[] weapons;

    [SerializeField]
    GameObject weaponsPickup;

    [SerializeField]
    GameObject[] nextRoomWeaponBanners;

    [SerializeField]
    SpriteRenderer[] nextRoomWeaponBannersSpriteRender;


    public int place = 0;

    int aliveEnemies;

    Player playerScript;

    private void Awake()
    {
        playerScript = player.GetComponent<Player>();
    }


    public void EnemyDied()
    {
        aliveEnemies--;
        if (aliveEnemies <= 0)
        {
            for (int i = 0;i < doors.Length; i++)
            {
                doors[i].openDoor();
            }
            spawnWeaponPickup();
        }
    }

    GameObject weapon1;

    GameObject weapon2;

    GameObject selectedWeapon;

    public void goNextLevel(bool door)
    {
        selectedWeapon = door ? weapon1 : weapon2;
        if (selectedWeapon == null)
        {
            selectedWeapon = weapons[Random.Range(0,weapons.Length)];
        }
        place++;
        player.transform.position = newRoomPlayerSpawnPos.position;
        playerScript.velocity = Vector3.zero;
        int r = Random.Range(0, weapons.Length);
        int r2 = Random.Range(0, weapons.Length);
        while (r2 == r)
        {
            r2 = Random.Range(0, weapons.Length);
        }
        weapon1 = weapons[r];
        weapon2 = weapons[r2];
        nextRoomWeaponBannersSpriteRender[0].sprite = weapon1.GetComponent<SpriteRenderer>().sprite;
        nextRoomWeaponBanners[0].transform.localScale = weapon1.transform.localScale;
        nextRoomWeaponBannersSpriteRender[1].sprite = weapon2.GetComponent<SpriteRenderer>().sprite;
        nextRoomWeaponBanners[1].transform.localScale = weapon2.transform.localScale;
        spawnEnemies(4);
    }

    public void gameOver()
    {

    }


    public void closeDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].closeDoor();
        }
    }



    void spawnWeaponPickup()
    {
        Vector3 spawnPos = new Vector3(0f,.1f,0f);
        Quaternion rotation = Quaternion.Euler(90f,0f,0f);
        GameObject e = Instantiate(weaponsPickup, spawnPos, rotation);
        e.GetComponent<WeaponPickup>().setup(selectedWeapon, playerScript, selectedWeapon.GetComponent<SpriteRenderer>().sprite);
        e.transform.localScale = selectedWeapon.transform.localScale;
    }

    void spawnEnemies(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            Vector3 spawnPos = player.transform.position;
            while (Vector2.Distance(player.transform.position, spawnPos) < 1.2f)
            {
                float xPos = Random.Range(-5.4f, 5.4f);
                float zPos = Random.Range(-2.8f, 2.8f);
                spawnPos = new Vector3(xPos, 0.4f, zPos);
            };
            int r = Random.Range(0, stageEnimes[place].Count);
            GameObject e = Instantiate(enimes[stageEnimes[place][r]], spawnPos, Quaternion.identity);
            e.GetComponent<Enemy>().setup(player, this);
        }
        aliveEnemies = amount;
    }
}
