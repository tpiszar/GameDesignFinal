using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float lifeStealPerc = 0.1f;
    public float rockStealPerc = 0.2f;

    public int maxHealth;
    public int health;
    public float dmgCooldown;
    float dmgTime;

    MeshRenderer mr;
    Material mat;
    public Material dmgMat;
    public float dmgFlashTime;

    public Slider healthBar;

    public bool gasLeak = false;
    public float explodeRange;
    public float explodeRate;
    float nextExplode = 0;

    public float reflectChance = 0.15f;

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
        nextExplode += Time.deltaTime;
    }

    public void TakeDamage(int dmg)
    {
        if (dmgTime > dmgCooldown)
        {
            health -= dmg;

            if (gasLeak)
            {
                if (nextExplode > explodeRate)
                {
                    nextExplode = 0;
                    Collider[] cols = Physics.OverlapSphere(transform.position, explodeRange + Player.gasLeakBonus);
                    foreach (Collider col in cols)
                    {
                        GroundEnemy GrEn = col.gameObject.GetComponentInParent<GroundEnemy>();
                        if (GrEn)
                        {
                            GrEn.knockBack(50, transform.position);
                            GrEn.TakeDamage((int)(dmg * Player.gasLeakBonus));
                            
                        }
                        else
                        {
                            FlyingEnemy FlyEn = col.gameObject.GetComponentInParent<FlyingEnemy>();
                            if (FlyEn)
                            {
                                FlyEn.knockBack(50, transform.position);
                                FlyEn.TakeDamage((int)(dmg * Player.gasLeakBonus));
                            }
                        }
                    }
                }
            }
            if (health <= 0)
            {
                if (Player.lifeSupportBonus > 0)
                {
                    health = 1;
                    Player.lifeSupportBonus--;
                }
                else
                {
                    GetComponentInParent<Player>().Dead();
                    Destroy(gameObject);
                }
            }
            healthBar.value = health;
            StartCoroutine(dmgFlash());
        }
    }

    public void GainHealth(int type)
    {
        int hp;
        if (type == 0)
        {
            hp = (int)(maxHealth * lifeStealPerc * Player.lifeStealBonus);
        }
        else
        {
            hp = (int)(maxHealth * rockStealPerc * Player.rockStealBonus);
        }
        if (health < maxHealth)
        {
            if (type == 0 || Player.ResourceCount > 0)
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
                if (type != 0)
                {
                    Player.ResourceCount--;
                }
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
