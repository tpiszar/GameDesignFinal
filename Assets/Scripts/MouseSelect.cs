using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class MouseSelect : MonoBehaviour
{
    public NavMeshAgent agent;
    public LayerMask rayMask;
    public LayerMask rayMaskAttack;
    public Transform player;
    private LineRenderer lineRenderer;

    bool looking = false;
    Vector3 lookDir;
    public float rotSpeed;

    public Shoot shootScr;

    [SerializeField] private GameObject clickMarkerPrefab;

    public Camera miniCam;

    float distance;

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = 0.15f;
        lineRenderer.endWidth = 0.15f;
        lineRenderer.positionCount = 0;

        distance = player.transform.localScale.x + 0.05f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            Ray miniRay = miniCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit miniHit;
            if (Physics.Raycast(miniRay, out miniHit, 100, rayMask))
            {
                //mini.doMiniMap(miniHit);
                agent.updatePosition = true;
                agent.SetDestination(miniHit.point);
                Vector3 pos = agent.path.corners[agent.path.corners.Length - 1];

                clickMarkerPrefab.SetActive(true);
                clickMarkerPrefab.transform.position = pos;
            }
            else
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100, rayMask))
                {
                    if (looking)
                    {
                        agent.enabled = true;
                        agent.updateRotation = true;

                        //MAY BE NECESSARY
                        //agent.ResetPath();
                        looking = false;
                    }
                    agent.updatePosition = true;
                    agent.SetDestination(hit.point);
                    Vector3 pos = agent.path.corners[agent.path.corners.Length - 1];

                    clickMarkerPrefab.SetActive(true);
                    clickMarkerPrefab.transform.position = pos;

                }
            }

        }

        if (Vector3.Distance(agent.destination, player.position) <= distance)
        {
            clickMarkerPrefab.SetActive(false);
            lineRenderer.positionCount = 0;
        }
        else if (agent.hasPath)
        {
            DrawPath();
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100, rayMaskAttack))
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

    void DrawPath()
    {
        lineRenderer.positionCount = agent.path.corners.Length;
        lineRenderer.SetPosition(0, player.position);

        if (agent.path.corners.Length < 2) 
        {
            return;
        }

        for (int i = 0; i < agent.path.corners.Length; i++)
        {
            Vector3 pointPos = new Vector3(agent.path.corners[i].x, agent.path.corners[i].y, agent.path.corners[i].z);

            lineRenderer.SetPosition(i, pointPos);
        }
    }
}
