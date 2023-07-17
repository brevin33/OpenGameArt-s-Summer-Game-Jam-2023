using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
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


    public int place = 0;

    int aliveEnemies;


    public void EnemyDied()
    {
        aliveEnemies--;
        if (aliveEnemies <= 0)
        {
            for (int i = 0;i < doors.Length; i++)
            {
                doors[i].openDoor();
            }
        }
    }

    public void goNextLevel()
    {
        place++;
        player.transform.position = newRoomPlayerSpawnPos.position;
        spawnEnemies(4);
    }

    public void gameOver()
    {

    }

    private void Start()
    {
        spawnEnemies(4);
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
