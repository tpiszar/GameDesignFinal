using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rock : MonoBehaviour
{
    public int health = 100;
    public Slider healthBar;
    public Canvas canvas;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = health;
        healthBar.value = health;
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(int dmg)
    {
        health -= dmg;
        canvas.enabled = true;
        healthBar.value = health;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
