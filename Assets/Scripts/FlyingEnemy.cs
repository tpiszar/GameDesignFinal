using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class FlyingEnemy : MonoBehaviour
{
    public Transform player;
    NavMeshAgent agent;
    public float attkDist;
    bool attack = false;
    public float attkRate;
    float nextAttk = 0;
    public float attkSpeed;
    float baseSpeed;
    float baseAccel;
    float baseAng;
    Vector3 returnPos;
    bool returning = false;
    public float closeEnough;

    bool knocked;
    Rigidbody rig;

    public int attkDmg;

    public Slider healthBar;
    public Canvas canvas;

    // Start is called before the first frame update

    void Start()
    {
        player = Spawner.player;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.transform.position);
        baseSpeed = agent.speed;
        baseAccel = agent.acceleration;
        baseAng = agent.angularSpeed;
        rig = GetComponent<Rigidbody>();

        healthBar.maxValue = health;
        healthBar.value = health;
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (knocked)
        {
            if (rig.velocity.magnitude < 0.001f)
            {
                agent.enabled = true;
                agent.acceleration = baseAng;
                agent.angularSpeed = baseAng;
                agent.speed = baseSpeed;
                agent.updateRotation = true;
                knocked = false;
                nextAttk = 0;
            }
        }
        else if (attack)
        {
            agent.SetDestination(player.transform.position);
        }
        else if (returning) 
        {
            if ((transform.position - agent.destination).magnitude < closeEnough)
            {
                nextAttk = 0;
                agent.SetDestination(player.transform.position);
                agent.angularSpeed = 1000;
                agent.updateRotation = true;
                agent.acceleration = baseAng;
                agent.angularSpeed = baseAng;
                returning = false;
            }
        }
        else
        {
            nextAttk += Time.deltaTime;
            if ((transform.position - player.transform.position).magnitude < attkDist)
            {
                if (nextAttk >= attkRate)
                {
                    agent.isStopped = false;
                    attack = true;
                    agent.speed = attkSpeed;
                    agent.acceleration = 100000;
                    agent.SetDestination(player.transform.position);
                    returnPos = transform.position;
                }
                else
                {
                    agent.isStopped = true;
                }
            }
            else
            {
                agent.isStopped = false;
                agent.SetDestination(player.transform.position);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            attack = false;
            returning = true;
            agent.updateRotation = false;
            agent.speed = baseSpeed;
            agent.SetDestination(returnPos);

            player.GetComponent<PlayerHealth>().TakeDamage(attkDmg);
        }
    }

    public int health = 50;

    public void TakeDamage(int amount)
    {
        health -= amount;
        canvas.enabled = true;
        healthBar.value = health;
        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void knockBack(float impact, Transform attacker)
    {
        agent.enabled = false;
        Vector3 dir = transform.position - attacker.transform.position;
        dir.y = 0;
        dir.Normalize();
        dir *= impact;
        rig.AddForce(dir, ForceMode.Impulse);
        rig.angularVelocity = Vector3.zero;
        attack = false;
        returning = false;
        knocked = true;
    }
}
