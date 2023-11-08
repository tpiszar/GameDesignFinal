using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class MouseSelect : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask rayMask;
    public Transform player;

    bool looking = false;
    Vector3 lookDir;
    public float rotSpeed;

    public Shoot shootScr;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, rayMask))
            {
                if (looking)
                {
                    agent.enabled = true;
                    agent.updateRotation = true;
                    agent.ResetPath();
                    looking = false;
                }
                agent.updatePosition = true;
                agent.SetDestination(hit.point);
            }
        }
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, rayMask))
            {
                //agent.enabled = false;
                agent.updateRotation = false;
                lookDir = hit.point - player.position;
                lookDir.y = 0;
                looking = true;
                if (shootScr.enabled)
                {
                    shootScr.shootPos = hit.point;
                }
            }
        }
        if (looking)
        {
            Quaternion lookRotation = Quaternion.LookRotation(lookDir);
            player.rotation = Quaternion.Slerp(player.rotation, lookRotation, Time.deltaTime * rotSpeed);
        }
    }
}
