using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GenerateLevel : MonoBehaviour
{
    //[SerializeField]
    //public GameObject[][] Instances;

    [System.Serializable]
    public class Spot
    {
        public GameObject[] instances;
    }

    [SerializeField]
    public Spot[] spots;

    public Material[] skyBoxes;

    public float spawnRadius;
    public GameObject player;

    public MeshRenderer[] ground;
    public Material[] groundMats;

    public MeshRenderer[] outer;
    public Material[] outerMats;

    public MeshRenderer[] smalls;
    public Material[] smallMats;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spots.Length; i++)
        {
            int randInst = Random.Range(0, spots[i].instances.Length);
            Instantiate(spots[i].instances[randInst], transform.GetChild(i).position, Quaternion.identity);
        }

        int randBox = Random.Range(0, skyBoxes.Length);
        RenderSettings.skybox = skyBoxes[randBox];

        Vector3 randomDirection = Random.insideUnitSphere * spawnRadius;

        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, spawnRadius, 1);
        Vector3 finalPosition = hit.position;

        player.transform.position = finalPosition;

        int randMat = Random.Range(0, groundMats.Length);

        Material mat = groundMats[randMat];
        for (int i = 0; i < ground.Length; i++)
        {
            ground[i].material = mat;
        }

        Material outerMat = outerMats[randMat];
        for (int i = 0; i < ground.Length; i++)
        {
            outer[i].material = outerMat;
        }

        Material smallMat = smallMats[randMat];
        for (int i = 0; i < smalls.Length; i++)
        {
            smalls[i].material = smallMat;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
