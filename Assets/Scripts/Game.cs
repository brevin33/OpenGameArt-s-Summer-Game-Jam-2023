using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.Mathematics;
using UnityEditor.SceneManagement;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
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

    [SerializeField]
    GameObject rareSpawn;

    List<List<int>> stageEnimes = new List<List<int>> { new List<int> { 0 },
        new List<int> { 0 },
        new List<int> { 0,1 },
        new List<int> { 0,1 },
        new List<int> { 0, 1, 2 },
        new List<int> { 0, 1, 2, 3 },
        new List<int> { 2,3 },
        new List<int> { 2,3,4},
        new List<int> { 3,4,5 },
        new List<int> { 3,4,5 },
        new List<int> { 6 }};

    List<int> numEnimes = new List<int> { 3,3,4,5,4,4,4,4,4,5,1};

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

    [SerializeField]
    GameObject godRays;

    [SerializeField]
    GameObject[] controls;

    [SerializeField]
    GameObject winScreen;

    [SerializeField]
    GameObject loseScreen;

    public int place = 0;

    int aliveEnemies;

    Player playerScript;

    private void Awake()
    {
        playerScript = player.GetComponent<Player>();
        playerScript.weapons.Add(weapons[Random.Range(0,weapons.Length)]);
    }


    public void EnemyDied()
    {
        Debug.Log(aliveEnemies);
        aliveEnemies--;
        if (aliveEnemies <= 0)
        {
            for (int i = 0;i < doors.Length; i++)
            {
                doors[i].openDoor();
            }
            godRays.SetActive(true);
            spawnWeaponPickup();
        }
    }

    GameObject weapon1;

    GameObject weapon2;

    GameObject selectedWeapon;

    public void goNextLevel(bool door)
    {
        controls[0].SetActive(false);
        controls[1].SetActive(false);
        selectedWeapon = door ? weapon1 : weapon2;
        godRays.SetActive(false);
        if (selectedWeapon == null)
        {
            selectedWeapon = weapons[Random.Range(0,weapons.Length)];
        }
        place++;
        if (place >= stageEnimes.Count)
        {
            winScreen.SetActive(true);
            return;
        }
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
        spawnEnemies(numEnimes[place]);
    }

    public void gameOver()
    {
        loseScreen.SetActive(true);
    }


    public void closeDoors()
    {
        for (int i = 0; i < doors.Length; i++)
        {
            doors[i].closeDoor();
        }
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
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
            int rareEnemy = Random.Range(0,1000);
            if (rareEnemy == 0)
            {
                GameObject e = Instantiate(rareSpawn, spawnPos, Quaternion.identity);
            }
            else
            {
                int r = Random.Range(0, stageEnimes[place].Count);
                if (r == 6)
                {
                    spawnPos.y += 0.5f;
                }
                GameObject e = Instantiate(enimes[stageEnimes[place][r]], spawnPos, Quaternion.identity);
                e.GetComponent<Enemy>().setup(player, this);
            }
        }
        aliveEnemies = amount;
    }
}
