using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Transform player;
    int wave = 0;
    int maxWaves;
    float nextWave;
    float nextSpawn;

    //int curType;
    //Vector3 selSpawn;

    //[System.Serializable]
    //public class subWave
    //{
    //    [SerializeField]
    //    public int[] EnemyCount;
    //}

    [System.Serializable]
    public class Wave
    {
        [SerializeField]
        //public subWave[] subs;
        //public float[] subDelays;
        public float startDelay;
        public float spawnDelay;
        public GameObject[] enemies;
        public int[] enemyCount;
    }

    [SerializeField]
    public Wave[] waves;


    // Start is called before the first frame update
    void Start()
    {
        maxWaves = waves.Length;
    }

    // Update is called once per frame
    void Update()
    {
        nextWave += Time.deltaTime;
        if (wave < maxWaves && nextWave > waves[wave].startDelay)
        {
            nextWave = 0;
            int count = 0;
            for (int i = 0; i < waves[wave].enemies.Length; i++)
            {
                //curType = i;
                //int lastPick = -1;
                for (int j = 0; j < waves[wave].enemyCount[i]; j++)
                {
                    //int pick;
                    //do
                    //{
                    //    pick = Random.Range(0, transform.childCount);
                    //} while (pick == lastPick);
                    //selSpawn = transform.GetChild(Random.Range(0, transform.childCount)).position;
                    //Invoke("Spawn", waves[i].spawnDelay * j);
                    //GameObject newEn = Instantiate(waves[wave].enemies[i], transform.GetChild(pick).position, Quaternion.identity);
                    //lastPick = pick;
                    StartCoroutine(Spawn(i, Random.Range(0, transform.childCount), count));
                    count++;
                }

            }
            wave++;
        }
    }

    //void Spawn()
    //{
    //    GameObject newEn = Instantiate(waves[wave].enemies[curType], selSpawn, Quaternion.identity);
    //}

    IEnumerator Spawn(int type, int pos, int num)
    {
        yield return new WaitForSeconds(waves[wave].spawnDelay * num);

        GameObject newEn = Instantiate(waves[wave - 1].enemies[type], transform.GetChild(pos).position, Quaternion.identity);
    }
}
