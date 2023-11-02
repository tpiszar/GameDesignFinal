using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Transform player;
    int wave = 0;
    int maxWaves;
    float nextWave;

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
        public float delay;
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
        if (nextWave > waves[wave].delay)
        {
            nextWave = 0;
            for (int i = 0; i < waves[wave].enemies.Length; i++)
            {
                for (int j = 0; j < waves[wave].enemyCount[i]; j++)
                {
                    GameObject newEn = Instantiate(waves[wave].enemies[i], transform.GetChild(Random.Range(0, transform.childCount)).position, Quaternion.identity);
                }

            }
            wave++;
            if (wave == maxWaves) 
            {
                Destroy(this);
            }
        }
    }
}
