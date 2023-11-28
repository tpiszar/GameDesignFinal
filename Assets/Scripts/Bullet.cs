using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool knockUp = false;
    public float impact = 25;
    public bool AoE = false;
    public float AoEmod;
    public float AoErange;
    public bool poison = false;
    public float poisonTick;

    public float stunDur = 0;

    public int armorPiercing = 0;

    public GameObject explosion;

    public GameObject particleHit;

    public float poisonEffBalance = 1f;

    public enum shooter
    {
        player,
        enemy
    }

    public shooter shotBy;

    public GameObject particleReflect;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter(Collision collision)
    {
        //if (shotBy == shooter.player)
        //{
        //    GroundEnemy enemy = collision.gameObject.GetComponentInParent<GroundEnemy>();
        //    if (enemy)
        //    {
        //        enemy.TakeDamage(damage);
        //    }
        //}
        //else
        //{
        //    PlayerHealth player = collision.gameObject.GetComponent<PlayerHealth>();
        //    if (player)
        //    {
        //        player.TakeDamage(damage);
        //    }
        //}
        //Destroy(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 2)
        {
            return;
        }

        if (shotBy == shooter.player)
        {
            HitEnemy(other, damage, 0);
            if (AoE)
            {
                if (other.gameObject.layer != 7 && other.gameObject.layer != 3)
                {
                    GameObject explode = Instantiate(explosion, transform.position, Quaternion.identity);
                    explode.transform.localScale = Vector3.one * AoErange * 0.6f;
                    Destroy(explode, 2f);
                    Collider[] cols = Physics.OverlapSphere(transform.position, AoErange);
                    foreach (Collider col in cols)
                    {
                        HitEnemy(col, (int)(damage * AoEmod), 1);
                    }
                }
            }
            if (other.gameObject.layer != 3 && other.gameObject.layer != 7)
            {
                if (armorPiercing == 0)
                {
                    Destroy(this.gameObject);
                }
                else
                {
                    armorPiercing--;
                }
                SpawnHit(other.gameObject);
            }
        }
        else
        {
            HitPlayer(other);
        }
    }

    void HitEnemy(Collider other, int dmg, int hit)
    {
        GroundEnemy GrEn = other.gameObject.GetComponentInParent<GroundEnemy>();
        if (GrEn)
        {
            if (hit == 0)
            {
                if (poison && !GrEn.poisoned)
                {
                    GrEn.poisoned = poison;
                    GrEn.poisonTick = poisonTick;
                    foreach (SpawnEffect effs in GrEn.poisonEffs)
                    {
                        effs.enabled = true;
                    }
                }

                if (knockUp && GrEn.currentState != 0)
                {
                    GrEn.knockBack(25, FindObjectOfType<PlayerHealth>().transform.position, stunDur);
                }
            }
            if (GrEn.poisoned)
            {
                foreach (SpawnEffect effs in GrEn.poisonEffs)
                {
                    effs.spawnEffectTime = GrEn.poisonTick * GrEn.maxHealth;
                    effs.timer = (effs.spawnEffectTime * (1f - (float)GrEn.health / (float)GrEn.maxHealth)) - poisonEffBalance;
                }
            }

            GrEn.TakeDamage(dmg);
        }
        else
        {
            FlyingEnemy FlyEn = other.gameObject.GetComponentInParent<FlyingEnemy>();
            if (FlyEn)
            {
                if (hit == 0)
                {
                    if (poison && !FlyEn.poisoned)
                    {
                        FlyEn.poisoned = poison;
                        FlyEn.poisonTick = poisonTick;
                        foreach (SpawnEffect effs in FlyEn.poisonEffs)
                        {
                            effs.enabled = true;
                        }
                    }
                    if (knockUp && !FlyEn.knocked)
                    {
                        FlyEn.knockBack(25, FlyEn.transform.forward + FlyEn.transform.position, stunDur);
                    }
                }
                if (FlyEn.poisoned)
                {
                    foreach (SpawnEffect effs in FlyEn.poisonEffs)
                    {
                        effs.spawnEffectTime = FlyEn.poisonTick * FlyEn.maxHealth;
                        effs.timer = effs.spawnEffectTime * (1f - (float)FlyEn.health / (float)FlyEn.maxHealth);
                    }
                }

                FlyEn.TakeDamage(dmg);
            }
        }
    }
    void HitPlayer(Collider other)
    {
        if (other.gameObject.layer != 6)
        {
            PlayerHealth player = other.gameObject.GetComponentInParent<PlayerHealth>();
            if (player)
            {
                if (Player.reflectBonus != 0)
                {
                    float baseChance = other.transform.GetComponentInParent<Player>().reflectPerc;
                    float chance = baseChance;
                    for (int i = 1; i < Player.reflectBonus; i++)
                    {
                        chance += baseChance / Mathf.Pow(2, Player.reflectBonus - 1);
                    }
                    if (Random.value < chance)
                    {
                        GameObject reflectEffect = Instantiate(particleReflect, transform.position - transform.up * 0.15f, Quaternion.identity);
                        reflectEffect.transform.LookAt(transform.position);
                        GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity * -2, ForceMode.Impulse);
                        shotBy = shooter.player;
                        return;
                    }
                }
                else
                {
                    SpawnHit(other.gameObject);
                    Destroy(this.gameObject);
                }
                player.TakeDamage(damage);
            }
        }
        if (other.gameObject.layer != 6 && other.gameObject.layer != 7)
        {
            SpawnHit(other.gameObject);
            Destroy(this.gameObject);
        }
    }

    void SpawnHit(GameObject hit)
    {
        GameObject hitEffect = Instantiate(particleHit, transform.position - transform.up * 0.15f, Quaternion.identity);
        //hitEffect.transform.parent = hit.transform;

        //MeshRenderer mesh = hit.GetComponent<MeshRenderer>();
        //if (!mesh)
        //{
        //    mesh = hit.GetComponentInChildren<MeshRenderer>();
        //}
        //if (!mesh)
        //{
        //    mesh = hit.GetComponentInParent<MeshRenderer>();
        //}
        //if (mesh)
        //{
        //    GameObject hitEffect = Instantiate(particleHit, transform.position - transform.up * 0.15f, Quaternion.identity);
        //    hitEffect.transform.parent = hit.transform;
        //    var main = hitEffect.GetComponent<ParticleSystem>().main;
        //    ParticleSystem[] systems = hitEffect.GetComponentsInChildren<ParticleSystem>();
        //    main.startColor = mesh.material.color;
        //    foreach (ParticleSystem s in systems)
        //    {
        //        var sMain = s.main;
        //        sMain.startColor = mesh.material.color;
        //    }
        //}
    }
}
