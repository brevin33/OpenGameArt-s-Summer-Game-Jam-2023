using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class AutoDepthOfFeild : MonoBehaviour
{

    [SerializeField]
    private GameObject Player;

    [SerializeField]
    Volume volume; 

    DepthOfField depthOfField;

    private void Start()
    {
        volume.profile.TryGet(out depthOfField);
    }


    private void Update()
    {
        Vector2 p = new Vector2( Player.transform.position.z, Player.transform.position.y);
        Vector2 c = new Vector2(transform.position.z, transform.position.y);
        float focalDistance = Vector2.Distance(p, c) * 0.8f;
        depthOfField.focusDistance.value = focalDistance;
    }
}
