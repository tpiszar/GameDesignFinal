using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public int attkDmg;

    // Start is called before the first frame update

    void Start()
    {
        player = Spawner.player;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(player.transform.position);
        baseSpeed = agent.speed;
        baseAccel = agent.acceleration;
        baseAng = agent.angularSpeed;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            print(attack + " " + returning + " " + agent.isStopped);
        }
        if (attack)
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
}
