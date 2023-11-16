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

    public enum shooter
    {
        player,
        enemy
    }

    public shooter shotBy;

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

        if (shotBy == shooter.player)
        {
            HitEnemy(other, damage, 0);
            if (AoE)
            {
                Collider[] cols = Physics.OverlapSphere(transform.position, AoErange);
                foreach (Collider col in cols)
                {
                    HitEnemy(col, (int)(damage * AoEmod), 1);
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
                GrEn.poisoned = poison;
                GrEn.poisonTick = poisonTick;
                if (knockUp && GrEn.currentState != 0)
                {
                    GrEn.knockBack(25, GrEn.transform.forward + GrEn.transform.position, stunDur);
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
                    if (knockUp && !FlyEn.knocked)
                    {
                        FlyEn.knockBack(25, FlyEn.transform.forward + FlyEn.transform.position, stunDur);
                    }
                    FlyEn.poisoned = poison;
                    FlyEn.poisonTick = poisonTick;
                }

                FlyEn.TakeDamage(dmg);
            }
        }
    }
    void HitPlayer(Collider other)
    {
        PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
        if (player)
        {
            if (Player.reflectBonus != 0)
            {
                if (Random.value < other.transform.parent.GetComponent<Player>().reflectPerc * Player.reflectBonus)
                {
                    GetComponent<Rigidbody>().AddForce(GetComponent<Rigidbody>().velocity * -2, ForceMode.Impulse);
                    shotBy = shooter.player;
                    return;
                }
            }
            player.TakeDamage(damage);
        }
        if (other.gameObject.layer != 6 && other.gameObject.layer != 7)
        {
            Destroy(this.gameObject);
        }
    }
}
