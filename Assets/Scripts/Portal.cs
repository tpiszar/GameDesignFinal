using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using UnityEngine;
using UnityEngine.AI;

public class Portal : MonoBehaviour
{
    public GameObject endCam;
    public GameObject miniMap;
    public GameObject mainUI;
    public GameObject victUI;

    public float fadeIn;
    float time;
    public MeshRenderer mesh;
    public Light lit;
    public float intensity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (time < fadeIn)
        {
            time += Time.deltaTime;
            lit.intensity = (time / fadeIn) * intensity;
            mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, time / fadeIn);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponentInParent<Player>())
        {
            other.gameObject.GetComponent<NavMeshAgent>().enabled = false;
            other.transform.position = new Vector3(0, -100, 0);
            Destroy(other.transform.parent.GetComponentInChildren<Camera>().gameObject);
            endCam.SetActive(true);
            miniMap.SetActive(false);
            mainUI.SetActive(false);
            victUI.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
