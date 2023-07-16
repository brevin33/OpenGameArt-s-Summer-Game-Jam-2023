using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{

    [SerializeField]
    GameObject player;

    [SerializeField]
    Rect allowedArea = new Rect(-5.47f, -2.93f, 10.94f, 5.9f);

    [SerializeField]
    GameObject[][] Enimes;

    public int place;

    static int aliveEnemies;

    public static void EnemyDied()
    {
        aliveEnemies--;
    }

    public static void goNextLevel()
    {

    }

    public static void gameOver()
    {

    }



    void spawnEnemies()
    {
        Vector3 spawnPos = Vector3.one * 99;
        while (Vector2.Distance(player.transform.position, spawnPos) < 0.8)
        {
            float xPos = Random.Range(-5.4f, 5.4f);
            float zPos = Random.Range(-2.8f, 2.8f);
            spawnPos = new Vector3(xPos, 0.5f, zPos);
        };

        Instantiate(Enimes[place][Random.Range(0,Enimes.Length-1)], spawnPos, Quaternion.identity);

    }
}
