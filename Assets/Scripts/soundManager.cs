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
    [SerializeField]
    private AudioSource pickupSound;
    [SerializeField]
    private AudioSource walkSound;



    static AudioSource winGameSoundS;
    static AudioSource loseGameSoundS;
    static AudioSource attckSoundS;
    static AudioSource playerHurtSoundS;
    static AudioSource enemyHurtSoundS;
    static AudioSource enimesBeatSoundS;
    static AudioSource pickupSoundS;
    static AudioSource walkSoundS;






    private void Awake()
    {
        winGameSoundS = winGameSound;
        loseGameSoundS = loseGameSound;
        attckSoundS = attckSound;
        playerHurtSoundS = playerHurtSound;
        enemyHurtSoundS=enemyHurtSound;
        enimesBeatSoundS = enimesBeatSound;
        pickupSoundS = pickupSound;
        walkSoundS = walkSound;
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

    public static void pickup()
    {
        pickupSoundS.Play();
    }
    public static void walk()
    {
        walkSoundS.Play();
    }



}
