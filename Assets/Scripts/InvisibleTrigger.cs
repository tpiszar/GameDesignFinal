using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleTrigger : MonoBehaviour
{
    public Tutorial tut;
    public int stage;

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
        if (tut.stage != stage)
        {
            return;
        }

        if (other.gameObject.GetComponentInParent<PlayerHealth>())
        {

            tut.wallTrigger();
            Destroy(gameObject);
        }
    }
}
