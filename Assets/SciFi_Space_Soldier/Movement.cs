using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public Animator animator;
    public Transform player;

    public NavMeshAgent agent;

    public Shoot shoot;

    float nextTimeToFire = 0;
    public float fireRate = 4f;
    float nextMelee = 0;

    bool m = true;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            m = true;
            animator.SetBool("Pick", true);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            m = false;
            animator.SetBool("Pick", false);
        }
        if (m)
        {
            if (Input.GetMouseButtonDown(0) && Time.time >= nextMelee)
            {
                animator.SetTrigger("Melee");
                nextMelee = Time.time + 1f;
            }
        }
        else
        {
            if (Input.GetMouseButton(0) && Time.time >= nextTimeToFire)
            {
                animator.SetTrigger("Shoot");
                nextTimeToFire = Time.time + 1f / fireRate;
                animator.SetFloat("FireRate", fireRate / 5f);
            }
        }

        Vector3 pos = player.position;
        pos.y = 0;
        transform.position = pos;

        float x = 0;
        float y = 0;

        if (agent.path.corners.Length > 1)
        {
            Vector3 agentDir = agent.path.corners[1] - transform.position;
            agentDir.Normalize();
            //print(Vector3.Dot(agentDir, transform.forward) + " " + Vector3.Dot(agentDir, transform.right));

            float forward = Vector3.Dot(agentDir, transform.forward);
            float right = Vector3.Dot(agentDir, transform.right);

            if (forward > 0.707f)
            {
                y = 1;
                
            }
            else if (forward < -0.707f)
            {
                y = -1;
            }
            else if (right < -.0707f)
            {
                x = 1;
            }
            else if (right > .0707f)
            {
                x = -1;
            }
        }

        animator.SetFloat("X", x);
        animator.SetFloat("Y", y);
        animator.speed = agent.speed / 6;
    }
}
