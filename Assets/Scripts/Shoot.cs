using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public Vector3 shootPos;

    public Transform shootPoint;
    public GameObject bullet;

    public int damage = 10;
    public float fireRate = 15f;
    public float bulletSpeed = 20f;

    private float nextTimeToFire = 0f;

    public bool knockBack = false;
    public float impact = 400;

    public bool AoE = false;
    public float AoEmod = 2;
    public float AoErange = 20;

    public float poisonTick = 1;
    public float poisonPerc = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
        {
            shootPos.y = shootPoint.position.y;
            Vector3 dir = shootPos - shootPoint.position;//target.position - shootPoint.position;

            GameObject newBullet = Instantiate(bullet, shootPoint.position, Quaternion.identity);
            Bullet bull = newBullet.GetComponent<Bullet>();
            bull.damage = damage;
            bull.knockUp = knockBack;
            bull.impact = impact / (fireRate * fireRate);
            bull.AoE = AoE;
            bull.AoEmod = AoEmod;
            bull.AoErange = AoErange;
            if (Random.value <= poisonPerc * (4 / fireRate))
            {
                bull.poison = true;
                bull.poisonTick = poisonTick;
            }
            newBullet.GetComponent<Rigidbody>().AddForce(dir.normalized * bulletSpeed, ForceMode.Impulse);
            newBullet.transform.forward = dir.normalized;
            newBullet.transform.Rotate(new Vector3(90, 0, 0));
            //Destroy(newBullet, 5f);

            nextTimeToFire = Time.time + 1f / fireRate;
        }
    }
}
