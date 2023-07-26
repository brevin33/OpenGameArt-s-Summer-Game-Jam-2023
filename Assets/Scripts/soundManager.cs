using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource winGameSound;
    [SerializeField]
    private AudioSource loseGameSound;
    [SerializeField]
    private AudioSource attckSound;
    [SerializeField]
    private AudioSource playerHurtSound;
    [SerializeField]
    private AudioSource enemyHurtSound;
    [SerializeField]
    private AudioSource enimesBeatSound;



    static AudioSource winGameSoundS;
    static AudioSource loseGameSoundS;
    static AudioSource attckSoundS;
    static AudioSource playerHurtSoundS;
    static AudioSource enemyHurtSoundS;
    static AudioSource enimesBeatSoundS;






    private void Awake()
    {
        winGameSoundS = winGameSound;
        loseGameSoundS = loseGameSound;
        attckSoundS = attckSound;
        playerHurtSoundS = playerHurtSound;
        enemyHurtSoundS=enemyHurtSound;
        enimesBeatSoundS = enimesBeatSound;

    }

    public static void winGame()
    {
        winGameSoundS.Play();
    }

    public static void loseGame()
    {
        loseGameSoundS.Play();
    }

    public static void attack()
    {
        attckSoundS.Play();
    }

    public static void playerHurt()
    {
        playerHurtSoundS.Play();
    }

    public static void enemyHurt()
    {
        enemyHurtSoundS.Play();
    }

    public static void enimesBeat()
    {
        enimesBeatSoundS.Play();
    }
}
