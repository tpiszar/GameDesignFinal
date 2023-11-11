using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwap : MonoBehaviour
{
    public Shoot shoot;
    public Melee melee;
    public GameObject gun;
    public GameObject pick;
    public bool start;

    // Start is called before the first frame update
    void Start()
    {
        if (start)
        {
            shoot.enabled = false;
            gun.SetActive(false);
            melee.enabled = true;
            pick.SetActive(true);
        }
        else
        {
            shoot.enabled = true;
            gun.SetActive(true);
            melee.enabled = false;
            pick.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(Input.GetAxis("Mouse ScrollWheel")) > 0f)
        {
            start = !start;
            if (start)
            {
                shoot.enabled = false;
                gun.SetActive(false);
                melee.enabled = true;
                pick.SetActive(true);
            }
            else
            {
                shoot.enabled = true;
                gun.SetActive(true);
                melee.enabled = false;
                pick.SetActive(false);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            start = true;
            shoot.enabled = false;
            gun.SetActive(false);
            melee.enabled = true;
            pick.SetActive(true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            start = false;
            shoot.enabled = true;
            gun.SetActive(true);
            melee.enabled = false;
            pick.SetActive(false);
        }
    }
}
