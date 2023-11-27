using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public static int healthCanCount = 3;
    public float healthCanPerc = 0.25f;
    public TextMeshProUGUI canCountTxt;
    public Button healtCanBtn;

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

    public float explodeRange;
    public float explodeRate;
    float nextExplode = 0;

    public float reflectChance = 0.15f;

    public GameObject explosion;

    public Animator animator;
    public Transform mesh;

    // Start is called before the first frame update
    void Start()
    {
        Spawner.player = transform;
        health = maxHealth;

        //mr = GetComponent<MeshRenderer>();
        //mat = mr.material;

        healthBar.maxValue = health;
        healthBar.value = health;

        canCountTxt.text = healthCanCount.ToString();
        if (healthCanCount == 0)
        {
            healtCanBtn.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        dmgTime += Time.deltaTime;
        nextExplode += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (healthCanCount > 0)
            {
                useHealthCan();
            }
        }

    }

    public void TakeDamage(int dmg)
    {
        if (dmgTime > dmgCooldown)
        {
            dmgTime = 0;
            health -= dmg;

            if (Player.gasLeakBonus > 0)
            {
                if (nextExplode > explodeRate)
                {
                    nextExplode = 0;

                    GameObject explode = Instantiate(explosion, transform.position, Quaternion.identity);
                    explode.transform.localScale = Vector3.one * 2.5f;

                    Collider[] cols = Physics.OverlapSphere(transform.position, explodeRange + Player.gasLeakBonus);
                    foreach (Collider col in cols)
                    {
                        GroundEnemy GrEn = col.gameObject.GetComponentInParent<GroundEnemy>();
                        if (GrEn)
                        {
                            GrEn.knockBack(100, transform.position, 1);
                            GrEn.TakeDamage((int)(dmg * Player.gasLeakBonus + 1));
                            
                        }
                        else
                        {
                            FlyingEnemy FlyEn = col.gameObject.GetComponentInParent<FlyingEnemy>();
                            if (FlyEn)
                            {
                                FlyEn.knockBack(100, transform.position, 1);
                                FlyEn.TakeDamage((int)(dmg * Player.gasLeakBonus + 1));
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
                    
                    //animator.SetBool("Dead", true);
                    //mesh.parent = null;
                    //mesh.GetComponent<Movement>().enabled = false;
                    //mesh.GetComponent<Melee>().enabled = false;
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
                }
                if (type != 0)
                {
                    Player.ResourceCount--;
                }
            }
        }
        healthBar.value = health;
    }

    public void useHealthCan()
    {
        healthCanCount--;
        canCountTxt.text = healthCanCount.ToString();
        int hp = (int)(healthCanPerc * maxHealth);
        if (health + hp > maxHealth)
        {
            health = maxHealth;
        }
        else
        {
            health += hp;
        }
        if (healthCanCount == 0)
        {
            healtCanBtn.interactable = false;
        }
        healthBar.value = health;
    }

    IEnumerator dmgFlash()
    {
        //mr.material = dmgMat;
        yield return new WaitForSeconds(dmgCooldown);// dmgFlashTime);
        //mr.material = mat;
    }
}
