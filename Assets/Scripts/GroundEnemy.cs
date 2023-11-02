using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GroundEnemy : MonoBehaviour
{
    public Transform body;
    public Transform head;
    public Transform gun;
    public float rotationSpeed;
    public float gunRotMag;

    public NavMeshAgent agent;
    public Transform target;
    public float pathRate;
    float nextPath = 0;
    public float startDetect;
    public float detectDistance;
    public float minShoot;
    public float shootDistance;
    public enum state
    {
        idle,
        following,
        shooting
    }
    state currentState = state.idle;
    public float idleTime;
    float nextMove;
    public float patrolRange;

    public Transform shootPoint;
    public GameObject bullet;

    public int damage = 10;
    public float fireRate = 15f;
    public float bulletSpeed = 20f;

    private float nextTimeToFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currentState = state.following;
        agent.updateRotation = false;
        target = Spawner.player;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawLine(transform.position, transform.forward * detectDistance, Color.red);
        //Debug.DrawLine(transform.position, -transform.forward * detectDistance, Color.red);
        //Debug.DrawLine(transform.position, transform.right * detectDistance, Color.red);
        //Debug.DrawLine(transform.position, -transform.right * detectDistance, Color.red);

        //Debug.DrawLine(transform.position + new Vector3(0, 1, 0), transform.forward * startDetect + new Vector3(0, 1, 0), Color.blue);
        //Debug.DrawLine(transform.position + new Vector3(0, 1, 0), -transform.forward * startDetect + new Vector3(0, 1, 0), Color.blue);
        //Debug.DrawLine(transform.position + new Vector3(0, 1, 0), transform.right * startDetect + new Vector3(0, 1, 0), Color.blue);
        //Debug.DrawLine(transform.position + new Vector3(0, 1, 0), -transform.right * startDetect + new Vector3(0, 1, 0), Color.blue);

        //Debug.DrawLine(transform.position + new Vector3(0, -1, 0), transform.forward * minShoot + new Vector3(0, -1, 0), Color.green);
        //Debug.DrawLine(transform.position + new Vector3(0, -1, 0), -transform.forward * minShoot + new Vector3(0, -1, 0), Color.green);
        //Debug.DrawLine(transform.position + new Vector3(0, -1, 0), transform.right * minShoot + new Vector3(0, -1, 0), Color.green);
        //Debug.DrawLine(transform.position + new Vector3(0, -1, 0), -transform.right * minShoot + new Vector3(0, -1, 0), Color.green);

        //Debug.DrawLine(transform.position + new Vector3(0, -.5f, 0), transform.forward * shootDistance + new Vector3(0, -.5f, 0), Color.cyan);
        //Debug.DrawLine(transform.position + new Vector3(0, -.5f, 0), -transform.forward * shootDistance + new Vector3(0, -.5f, 0), Color.cyan);
        //Debug.DrawLine(transform.position + new Vector3(0, -.5f, 0), transform.right * shootDistance + new Vector3(0, -.5f, 0), Color.cyan);
        //Debug.DrawLine(transform.position + new Vector3(0, -.5f, 0), -transform.right * shootDistance + new Vector3(0, -.5f, 0), Color.cyan);

        switch (currentState)
        {
            case state.idle:
                Idle();
                //print("Idle " + Vector3.Distance(transform.position, target.position) + " " + agent.hasPath);
                break;
            case state.following:
                Following();
                //print("Following " + Vector3.Distance(transform.position, target.position) + " " + agent.hasPath);
                break;
            case state.shooting:
                Shooting();
                //print("Shooting " + Vector3.Distance(transform.position, target.position) + " " + agent.hasPath);
                break;
        }
    }

    void Idle()
    {
        if (Vector3.Distance(transform.position, target.position) < startDetect)
        {
            currentState = state.shooting;
        }
        else
        {
            if (!agent.hasPath || Vector3.Distance(agent.destination, transform.position) < 1)
            {
                if (nextMove > idleTime)
                {
                    agent.SetDestination(this.transform.position + 
                        new Vector3(Random.Range(-patrolRange, patrolRange), 0, Random.Range(-patrolRange, patrolRange)));
                    nextMove = 0;
                }
                else
                {
                    nextMove += Time.deltaTime;
                }
            }
        }
    }

    void Following()
    {
        if (Vector3.Distance(transform.position, target.position) < shootDistance)
        {
            currentState = state.shooting;
        }
        else if (nextPath > pathRate)
        {
            agent.SetDestination(target.position);
            nextPath = 0;
        }
        else
        {
            nextPath += Time.deltaTime;
        }

        Vector3 bodyLookPoint = target.position;
        bodyLookPoint.y = body.position.y;
        //head.LookAt(target.position);
        //body.LookAt(bodyLookPoint);
        //gun.LookAt(target.position);

        Vector3 bodyDir = bodyLookPoint - body.position;
        //Vector3 headDir = target.position - head.position;
        Vector3 gunDir = target.position - gun.position;

        Quaternion bodyRot = Quaternion.LookRotation(bodyDir.normalized);
        body.rotation = Quaternion.Slerp(body.rotation, bodyRot, rotationSpeed * Time.deltaTime);
        //head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(headDir.normalized), rotationSpeed * Time.deltaTime);

        if (Mathf.Abs(body.rotation.eulerAngles.magnitude - bodyRot.eulerAngles.magnitude) < gunRotMag)
        {
            gun.rotation = Quaternion.Slerp(gun.rotation, Quaternion.LookRotation(gunDir.normalized), rotationSpeed * Time.deltaTime);

            if (Time.time >= nextTimeToFire)
            {
                nextTimeToFire = Time.time + 1f / fireRate;
            }
        }

        Vector3 gunRot = gun.localEulerAngles;
        gunRot.z = 0;
        gun.localEulerAngles = gunRot;
        //Vector3 headRot = head.localEulerAngles;
        //headRot.y = 0;
        //headRot.z = 0;
        //head.localEulerAngles = headRot;
    }

    void Shooting()
    {
        if (Vector3.Distance(transform.position, target.position) > shootDistance)
        {
            currentState = state.following;
            agent.SetDestination(target.position);
            //agent.updateRotation = true;
        }
        else if (!agent.hasPath && Vector3.Distance(transform.position, target.position) > minShoot)
        {
            agent.SetDestination(target.position);
            //agent.updateRotation = false;
        }
        else if (Vector3.Distance(agent.destination, transform.position) < 1 || Vector3.Distance(transform.position, target.position) < minShoot)
        {
            agent.ResetPath();
            //agent.updateRotation = true;
        }

        Vector3 bodyLookPoint = target.position;
        bodyLookPoint.y = body.position.y;
        //head.LookAt(target.position);
        //body.LookAt(bodyLookPoint);
        //gun.LookAt(target.position);

        Vector3 bodyDir = bodyLookPoint - body.position;
        //Vector3 headDir = target.position - head.position;
        Vector3 gunDir = target.position - gun.position;

        Quaternion bodyRot = Quaternion.LookRotation(bodyDir.normalized);
        body.rotation = Quaternion.Slerp(body.rotation, bodyRot, rotationSpeed * Time.deltaTime);
        //head.rotation = Quaternion.Slerp(head.rotation, Quaternion.LookRotation(headDir.normalized), rotationSpeed * Time.deltaTime);

        if (Mathf.Abs(body.rotation.eulerAngles.magnitude - bodyRot.eulerAngles.magnitude) < gunRotMag)
        {
            gun.rotation = Quaternion.Slerp(gun.rotation, Quaternion.LookRotation(gunDir.normalized), rotationSpeed * Time.deltaTime);

            if (Time.time >= nextTimeToFire)
            {
                Shoot();
                nextTimeToFire = Time.time + 1f / fireRate;
            }
        }

        Vector3 gunRot = gun.localEulerAngles;
        gunRot.z = 0;
        gun.localEulerAngles = gunRot;
        //Vector3 headRot = head.localEulerAngles;
        //headRot.y = 0;
        //headRot.z = 0;
        //head.localEulerAngles = headRot;
    }

    void Shoot()
    {
        Vector3 dir = shootPoint.up;//target.position - shootPoint.position;

        GameObject newBullet = Instantiate(bullet, shootPoint.position, Quaternion.identity);
        newBullet.GetComponent<Bullet>().damage = damage;
        newBullet.GetComponent<Rigidbody>().AddForce(dir.normalized * bulletSpeed, ForceMode.Impulse);
        newBullet.transform.forward = dir.normalized;
        newBullet.transform.Rotate(new Vector3(90, 0, 0));
        Destroy(newBullet, 5f);
    }

    public int health = 100;

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (currentState == state.idle)
        {
            currentState = state.shooting;
        }
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
