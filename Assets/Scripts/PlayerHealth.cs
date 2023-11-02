using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    int health;

    // Start is called before the first frame update
    void Start()
    {
        Spawner.player = transform;
        health = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(int dmg)
    {
        health -= dmg;
        print(health);

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
