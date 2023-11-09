using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Transform player;
    int wave = 0;
    //int maxWaves;
    float nextWave;
    //float nextSpawn;

    public int spawnPoints = 5;
    public int flyingPoints = 1;
    public int groundPoints = 3;
    public int pointIncr = 1;
    public float waveDelay;
    public float upperWave;
    public float lowerWave;
    float spawnDelay = 1;

    public GameObject[] enemies;

    public float incursionTime;
    float timer = 0;
    bool incursion = false;
    float incursionDelay = 3;
    float nextInc = 0;

    //int curType;
    //Vector3 selSpawn;

    //[System.Serializable]
    //public class subWave
    //{
    //    [SerializeField]
    //    public int[] EnemyCount;
    //}

    //[System.Serializable]
    //public class Wave
    //{
    //    [SerializeField]
    //    //public subWave[] subs;
    //    //public float[] subDelays;
    //    public float startDelay;
    //    public float spawnDelay;
    //    public GameObject[] enemies;
    //    public int[] enemyCount;
    //}

    //[SerializeField]
    //public Wave[] waves;


    // Start is called before the first frame update
    void Start()
    {
        //maxWaves = waves.Length;
        incursionTime *= 60;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > incursionTime)
        {
            incursion = true;
        }

        nextWave += Time.deltaTime;
        //if (wave < maxWaves && nextWave > waves[wave].startDelay)
        //{
        //    nextWave = 0;
        //    int count = 0;
        //    for (int i = 0; i < waves[wave].enemies.Length; i++)
        //    {
        //        //curType = i;
        //        //int lastPick = -1;
        //        for (int j = 0; j < waves[wave].enemyCount[i]; j++)
        //        {
        //            //int pick;
        //            //do
        //            //{
        //            //    pick = Random.Range(0, transform.childCount);
        //            //} while (pick == lastPick);
        //            //selSpawn = transform.GetChild(Random.Range(0, transform.childCount)).position;
        //            //Invoke("Spawn", waves[i].spawnDelay * j);
        //            //GameObject newEn = Instantiate(waves[wave].enemies[i], transform.GetChild(pick).position, Quaternion.identity);
        //            //lastPick = pick;
        //            StartCoroutine(Spawn(i, Random.Range(0, transform.childCount), count));
        //            count++;
        //        }

        //    }
        //    wave++;
        //}
        if (incursion)
        {
            if (nextWave > waveDelay && incursionDelay > 1)
            {
                nextWave = 0;
                waveDelay = Random.Range(lowerWave, upperWave + 1);
                incursionDelay--;
            }
            nextInc += Time.deltaTime;
            if (nextInc > incursionDelay)
            {
                nextInc = 0;
                StartCoroutine(Spawn(Random.Range(0, 2), Random.Range(0, transform.childCount), 1));
            }
        }
        else if (nextWave > waveDelay)
        {
            nextWave = 0;
            waveDelay = Random.Range(lowerWave, upperWave + 1);
            int count = 0;
            int flying = 0;
            int ground = 0;
            int points = spawnPoints + pointIncr * wave;
            while (points >= groundPoints)
            {
                int chosen = Random.Range(0, 2);
                if (chosen == 0)
                {
                    flying++;
                    points -= flyingPoints;
                }
                else
                {
                    ground++;
                    points -= groundPoints;
                }
            }
            flying += points;

            for (int i = 0; i < flying; i++)
            {
                StartCoroutine(Spawn(0, Random.Range(0, transform.childCount), count));
                count++;
            }
            for (int i = 0; i < ground; i++)
            {
                StartCoroutine(Spawn(1, Random.Range(0, transform.childCount), count));
                count++;
            }

            wave++;
        }
    }

    //void Spawn()
    //{
    //    GameObject newEn = Instantiate(waves[wave].enemies[curType], selSpawn, Quaternion.identity);
    //}

    //IEnumerator Spawn(int type, int pos, int num)
    //{
    //    yield return new WaitForSeconds(waves[wave].spawnDelay * num);

    //    GameObject newEn = Instantiate(waves[wave - 1].enemies[type], transform.GetChild(pos).position, Quaternion.identity);
    //}

    IEnumerator Spawn(int type, int pos, int num)
    {
        yield return new WaitForSeconds(spawnDelay * num);

        GameObject newEn = Instantiate(enemies[type], transform.GetChild(pos).position, Quaternion.identity);
    }
}
