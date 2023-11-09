using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rock : MonoBehaviour
{
    public Slider healthBar;
    public Canvas canvas;

    public int healthMin;
    public int healthMax;
    public int resourceMin;
    public int resourceMax;

    int health;
    int resource;

    // Start is called before the first frame update
    void Start()
    {
        health = Random.Range(healthMin, healthMax + 1);
        resource = Random.Range(resourceMin, resourceMax + 1);

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
            Player.ResourceCount += resource;
            Destroy(gameObject);
        }
    }
}
