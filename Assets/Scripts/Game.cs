using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    [SerializeField]
    GameObject player;

    [SerializeField]
    Rect allowedArea = new Rect(-5.47f, -2.93f, 10.94f, 5.9f);

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

}
