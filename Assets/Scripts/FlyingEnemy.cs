using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

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

    public bool knocked;
    Rigidbody rig;

    public int attkDmg;

    public Slider healthBar;
    public Canvas canvas;

    public bool poisoned = false;
    public float poisonTick = 1;
    float nextPoison = 0;

    float stunned = 0;

    public SpawnEffect[] poisonEffs;

    public ParticleSystem knockBackEff;

    public GameObject deathEff;

    public float rotationSpeed;

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

        poisonEffs = GetComponentsInChildren<SpawnEffect>();

        maxHealth = health;

        agent.updateRotation = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool moving = false;
        bool looking = true;

        if (poisoned)
        {
            nextPoison += Time.deltaTime;
            if (nextPoison > poisonTick)
            {
                nextPoison = 0;
                TakeDamage(1);
            }
        }

        if (knocked)
        {
            looking = false;
            if (rig.velocity.magnitude < 0.001f)
            {
                stunned -= Time.deltaTime;
                if (stunned < 0)
                {
                    agent.enabled = true;
                    agent.acceleration = baseAng;
                    agent.angularSpeed = baseAng;
                    agent.speed = baseSpeed;
                    //agent.updateRotation = true;
                    knocked = false;
                    nextAttk = 0;
                }
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
                //agent.updateRotation = true;
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

                if (agent.path.corners.Length > 1)
                {
                    moving = true;


                }
            }
        }

        if (moving)
        {
            Vector3 agentDir = agent.path.corners[1] - transform.position;

            Quaternion bodyRot = Quaternion.LookRotation(agentDir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, bodyRot, rotationSpeed * Time.deltaTime);
        }
        else if (looking)
        {
            Vector3 lookDir = player.transform.position - transform.position;

            Quaternion bodyRot = Quaternion.LookRotation(lookDir.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, bodyRot, rotationSpeed * Time.deltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
        {
            attack = false;
            returning = true;
            //agent.updateRotation = false;
            agent.speed = baseSpeed;
            agent.SetDestination(returnPos);

            player.GetComponentInParent<PlayerHealth>().TakeDamage(attkDmg);
        }
    }

    public int health = 50;
    public int maxHealth;

    public void TakeDamage(int amount)
    {
        if (health == maxHealth)
        {
            canvas.enabled = true;
        }

        health -= amount;
        healthBar.value = health;
        if (health <= 0)
        {
            if (!poisoned)
            {
                Instantiate(deathEff, transform.position, Quaternion.identity);
            }

            if (Player.lifeStealBonus != 0)
            {
                FindObjectOfType<PlayerHealth>().GainHealth(0);
            }
            Destroy(this.gameObject);
        }
    }

    public void knockBack(float impact, Vector3 attacker)
    {
        agent.enabled = false;
        Vector3 dir = transform.position - attacker;
        dir.y = 0;
        dir.Normalize();
        dir *= impact;
        rig.AddForce(dir, ForceMode.Impulse);
        rig.angularVelocity = Vector3.zero;
        attack = false;
        returning = false;
        knocked = true;

        knockBackEff.Play();
    }

    public void knockBack(float impact, Vector3 attacker, float stunTime)
    {
        agent.enabled = false;
        Vector3 dir = transform.position - attacker;
        dir.y = 0;
        dir.Normalize();
        dir *= impact;
        rig.AddForce(dir, ForceMode.Impulse);
        rig.angularVelocity = Vector3.zero;
        attack = false;
        returning = false;
        knocked = true;

        stunned = stunTime;

        knockBackEff.Play();
    }
}
