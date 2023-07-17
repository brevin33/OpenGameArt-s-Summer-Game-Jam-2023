using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]
    Sprite open;
    [SerializeField]
    Sprite close;

    [SerializeField]
    Game game;

    public bool isOpen = false;

    SpriteRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<SpriteRenderer>();
    }

    public void openDoor()
    {
        renderer.sprite = open;
        isOpen = true;
    }

    public void closeDoor()
    {
        renderer.sprite = close;
        isOpen = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("here");
        if (other.tag == "Player")
        {
            if (isOpen)
            {
                game.goNextLevel();
                closeDoor();
            }
        }
    }
}
