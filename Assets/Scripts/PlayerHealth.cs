using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static int LifeStealCount = 1;
    public float lifeStealPerc = 0.1f;

    public int maxHealth;
    int health;
    public float dmgCooldown;
    float dmgTime;

    MeshRenderer mr;
    Material mat;
    public Material dmgMat;
    public float dmgFlashTime;

    public Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        Spawner.player = transform;
        health = maxHealth;

        mr = GetComponent<MeshRenderer>();
        mat = mr.material;

        healthBar.maxValue = health;
        healthBar.value = health;
    }

    // Update is called once per frame
    void Update()
    {
        dmgTime += Time.deltaTime;
    }

    public void TakeDamage(int dmg)
    {
        if (dmgTime > dmgCooldown)
        {
            health -= dmg;
            healthBar.value = health;
            if (health <= 0)
            {
                GetComponentInParent<Player>().Dead();
                Destroy(gameObject);
            }
            StartCoroutine(dmgFlash());
        }
    }

    public void GainHealth()
    {
        int hp = (int)(maxHealth * lifeStealPerc * LifeStealCount);
        if (health < maxHealth)
        {
            if (health + hp > maxHealth)
            {
                health = maxHealth;
            }
            else
            {
                health += hp;
                healthBar.value = health;
            }
        }
    }

    IEnumerator dmgFlash()
    {
        mr.material = dmgMat;
        yield return new WaitForSeconds(dmgFlashTime);
        mr.material = mat;
    }
}
