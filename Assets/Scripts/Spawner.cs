using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    public float incursionRamp;
    bool incursion = false;
    float incursionDelay = 3;
    float nextInc = 0;

    public GameObject portal;
    public GameObject endCam;
    public GameObject miniMap;
    public GameObject mainUI;
    public GameObject victUI;

    public TextMeshProUGUI timerTxt;

    public float spdPerc;
    public float healthPerc;
    public float dmgPerc;

    public int spdCap;
    public int healthCap;
    public int dmgCap;

    float spd;
    int flyHp;
    int GroHp;
    int dmg;

    public float spawnPointsPerc;
    public float waveUpDelPerc;
    public float waveDownDelPerc;
    public float pointIncrPerc;

    public int spawnPointsCap;
    public int waveUpCap;
    public int waveDownCap;
    public int pointIncrCap;

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

        if (Player.Level <= spdCap)
        {
            spd = enemies[0].GetComponent<NavMeshAgent>().speed;
            spd += (spd * spdPerc * (Player.Level - 1));
        }
        else
        {
            spd = enemies[0].GetComponent<NavMeshAgent>().speed;
            spd += (spd * spdPerc * spdCap);
        }
        if (Player.Level <= healthCap)
        {
            flyHp = enemies[0].GetComponent<FlyingEnemy>().health;
            flyHp += (int)(flyHp * healthPerc * (Player.Level - 1));
            GroHp = enemies[1].GetComponent<GroundEnemy>().health;
            GroHp += (int)(GroHp * healthPerc * (Player.Level - 1));
        }
        else
        {
            flyHp = enemies[0].GetComponent<FlyingEnemy>().health;
            flyHp += (int)(flyHp * healthPerc * healthCap);
            GroHp = enemies[1].GetComponent<GroundEnemy>().health;
            GroHp += (int)(GroHp * healthPerc * healthCap);
        }
        if (Player.Level <= dmgCap)
        {
            dmg = enemies[0].GetComponent<FlyingEnemy>().attkDmg;
            dmg += (int)(dmg * dmgPerc * (Player.Level - 1));
        }
        else
        {
            dmg = enemies[0].GetComponent<FlyingEnemy>().attkDmg;
            dmg += (int)(dmg * dmgPerc * dmgCap);
        }

        if (Player.Level <= spawnPointsCap)
        {
            spawnPoints += (int)(spawnPoints * spawnPointsPerc * (Player.Level - 1));
        }
        else
        {
            spawnPoints += (int)(spawnPoints * spawnPointsPerc * spawnPointsCap);
        }
        if (Player.Level <= waveUpCap)
        {
            upperWave -= (upperWave * waveDownDelPerc * (Player.Level - 1));
        }
        else
        {
            upperWave -= (upperWave * waveDownDelPerc * waveUpCap);
        }
        if (Player.Level <= waveDownCap)
        {
            lowerWave -= (lowerWave * waveDownDelPerc * (Player.Level - 1));
        }
        else
        {
            lowerWave -= (lowerWave * waveDownDelPerc * waveDownCap);
        }
        if (Player.Level <= pointIncrCap)
        {
            pointIncr += (int)(pointIncr * pointIncrPerc * (Player.Level - 1));
        }
        else
        {
            pointIncr += (int)(pointIncr * pointIncrPerc * pointIncrCap);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!incursion)
        {
            incursionTime -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(incursionTime / 60);
            float seconds = Mathf.FloorToInt(incursionTime % 60);
            if (seconds < 10)
            {
                timerTxt.text = minutes + ":0" + seconds;
            }
            else
            {
                timerTxt.text = minutes + ":" + seconds;
            }

            if (incursionTime < 0)
            {
                incursion = true;
                nextWave = 0;
                GameObject port = Instantiate(portal, new Vector3(0, 2, 0), Quaternion.identity);
                port.transform.Rotate(new Vector3(-90, 0, 0));
                Portal portScr = port.GetComponent<Portal>();
                portScr.endCam = endCam;
                portScr.miniMap = miniMap;
                portScr.mainUI = mainUI;
                portScr.victUI = victUI;
                timerTxt.text = "Portal Opened. Incursion Inbound!";
            }
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
            if (nextWave > incursionRamp && incursionDelay > 1)
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

        FlyingEnemy fly = newEn.GetComponent<FlyingEnemy>();
        if (fly)
        {
            fly.health = flyHp;
            fly.attkDmg = dmg;
        }
        else
        {
            GroundEnemy ground = newEn.GetComponent<GroundEnemy>();
            ground.health = GroHp;
            ground.damage = dmg;
        }
        newEn.GetComponent<NavMeshAgent>().speed = spd;
    }
}
