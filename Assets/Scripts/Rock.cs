using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static Bullet;

public class Rock : MonoBehaviour
{
    public Slider healthBar;
    public Canvas canvas;

    public int healthMin;
    public int healthMax;
    public int resourceMin;
    public int resourceMax;

    int health;
    int maxHealth;
    int resource;

    public int rockStealPerc;
    public float shatterPerc = 0.5f;
    public float shatterRange;

    public GameObject particleShatter;

    public ParticleSystem particleHit;

    public GameObject particleBreak;

    List<int> healths = new List<int>();
    List<int> values = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        health = Random.Range(healthMin, healthMax + 1);
        resource = Random.Range(resourceMin, resourceMax + 1);

        healthBar.maxValue = health;
        healthBar.value = health;
        canvas.enabled = false;

        maxHealth = health;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Hit(int dmg)
    {
        if (health == maxHealth)
        {
            canvas.enabled = true;
        }
        health -= dmg;
        healthBar.value = health;
        particleHit.Play();
        if (health <= 0)
        {
            GameObject brek = Instantiate(particleBreak, transform.position, Quaternion.identity);
            brek.transform.Rotate(new Vector3(-90, 0, 0));

            Player.ResourceCount += resource + (1 * Player.richMatsBonus);
            if (Player.resourceSpdBonus > 0 )
            {
                FindObjectOfType<Player>().RockSpeedUp();
            }
            if (Player.rockStealBonus > 0 )
            {
                FindObjectOfType<PlayerHealth>().GainHealth(1);
            }
            if (Player.rockShatterBonus > 0 )
            {
                GameObject shatter = Instantiate(particleShatter, transform.position, Quaternion.identity);
                shatter.transform.Rotate(new Vector3(-90, 0, 0));

                int baseDmg = FindObjectOfType<Melee>().meleeDmg;
                Collider[] cols = Physics.OverlapSphere(transform.position, shatterRange + Player.rockShatterBonus);
                foreach (Collider col in cols)
                {
                    GroundEnemy GrEn = col.gameObject.GetComponentInParent<GroundEnemy>();
                    if (GrEn)
                    {
                        GrEn.knockBack(50, transform.position);
                        GrEn.TakeDamage((int)(baseDmg * shatterPerc * Player.rockShatterBonus));

                    }
                    else
                    {
                        FlyingEnemy FlyEn = col.gameObject.GetComponentInParent<FlyingEnemy>();
                        if (FlyEn)
                        {
                            FlyEn.knockBack(50, transform.position);
                            FlyEn.TakeDamage((int)(baseDmg * shatterPerc * Player.rockShatterBonus));
                        }
                    }
                }
            }
            
            Destroy(gameObject);
        }
    }
}
