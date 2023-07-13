using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    [SerializeField]
    GameObject player;

    [SerializeField]
    Rect allowedArea = new Rect(-2.5f, -2.93f, 5f, 5.9f);

    public ref GameObject getPlayer()
    {
        return ref player;
    }


}
