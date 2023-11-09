using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

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
            GroundEnemy GrEn = other.gameObject.GetComponentInParent<GroundEnemy>();
            if (GrEn)
            {
                GrEn.TakeDamage(damage);
            }
            else
            {
                FlyingEnemy FlyEn = other.gameObject.GetComponentInParent<FlyingEnemy>();
                if (FlyEn)
                {
                    FlyEn.TakeDamage(damage);
                }
            }
            if (other.gameObject.layer != 3 && other.gameObject.layer != 7)
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            PlayerHealth player = other.gameObject.GetComponent<PlayerHealth>();
            if (player)
            {
                player.TakeDamage(damage);
            }
            if (other.gameObject.layer != 6 && other.gameObject.layer != 7)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
