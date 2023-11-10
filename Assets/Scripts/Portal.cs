using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameObject endCam;
    public GameObject mainUI;
    public GameObject victUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Player>())
        {
            Destroy(other.transform.parent.gameObject);
            endCam.SetActive(true);
            mainUI.SetActive(false);
            victUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
