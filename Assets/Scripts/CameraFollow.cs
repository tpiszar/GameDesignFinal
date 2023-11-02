using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;
    Vector3 distance;
    Vector3 location;


    // Start is called before the first frame update
    void Start()
    {
        distance = this.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        location = player.position + distance;
        transform.position = location;
    }
}
