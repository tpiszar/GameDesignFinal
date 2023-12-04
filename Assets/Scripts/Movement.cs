using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Movement : MonoBehaviour
{
    public Animator animator;
    public Transform player;

    public NavMeshAgent agent;

    public float difference;

    public float baseSpeed;

    public bool fixRotation = false;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("delayedStart", 0.2f);
    }

    void delayedStart()
    {
        animator.speed = agent.speed / 6;
        animator.SetFloat("InverseSpeed", 1 / (agent.speed / baseSpeed));
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = player.position;
        pos.y -= difference;
        transform.position = pos;
        
        if (fixRotation)
        {
            transform.eulerAngles = transform.parent.eulerAngles;
        }

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
    }
}
