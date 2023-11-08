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
            newBullet.GetComponent<Bullet>().damage = damage;
            newBullet.GetComponent<Rigidbody>().AddForce(dir.normalized * bulletSpeed, ForceMode.Impulse);
            newBullet.transform.forward = dir.normalized;
            newBullet.transform.Rotate(new Vector3(90, 0, 0));
            //Destroy(newBullet, 5f);

            nextTimeToFire = Time.time + 1f / fireRate;
        }
    }
}
