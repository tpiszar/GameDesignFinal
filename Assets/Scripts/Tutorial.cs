using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI directTxt;
    public TextMeshProUGUI timerTxt;

    int stage = 0;

    public GameObject wall;

    public float timer;

    public GameObject portal;
    public GameObject endCam;
    public GameObject mainUI;
    public GameObject victUI;

    public Transform[] spawns;

    public GameObject[] enemies;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (stage == 0 && Input.GetMouseButtonDown(1))
        {
            directTxt.text = "Left Click to Attack in Mouse Direction. Break Through the Resources to Proceed.";
            stage++;
        }
        else if (stage == 2 && transform.childCount == 3)
        {
            directTxt.text = "Melee or Shoot the Enemies to Defeat Them.";
            Destroy(wall);
            stage++;
        }
        else if (stage == 3 && transform.childCount == 0)
        {
            directTxt.text = "Each World Has an Incusion Timer. When it Reaches 0 a Portal Opens and Enemies Begin Rapidly Spawning.";
            stage++;
        }
        else if (stage == 4)
        {
            timer -= Time.deltaTime;
            float minutes = Mathf.FloorToInt(timer / 60);
            float seconds = Mathf.FloorToInt(timer % 60);
            if (seconds < 10)
            {
                timerTxt.text = minutes + ":0" + seconds;
            }
            else
            {
                timerTxt.text = minutes + ":" + seconds;
            }

            if (timer <= 0)
            {
                timerTxt.text = "Reach the Portal and Escape!";
                GameObject port = Instantiate(portal, new Vector3(0, 2, 0), Quaternion.identity);
                port.transform.Rotate(new Vector3(-90, 0, 0));
                Portal portScr = port.GetComponent<Portal>();
                portScr.endCam = endCam;
                portScr.mainUI = mainUI;
                portScr.victUI = victUI;
                stage++;
            }
        }
        else
        {
            if (transform.childCount == 0)
            {
                spawnWave();
            }
        }
    }

    public void wallTrigger()
    {
        if (stage == 1)
        {
            directTxt.text = "Scroll the Mouse Wheel or Press 2 to switch to your gun. Hold Left Click and Shoot the Enemy.";
        }
        stage++;
    }

    void spawnWave()
    {
        GameObject newEn = Instantiate(enemies[0], spawns[0].position, Quaternion.identity);
        newEn.transform.parent = transform;
        for (int i = 1; i < 11; i++)
        {
            StartCoroutine(Spawn(i % 2, i % 2, i));
        }
    }

    IEnumerator Spawn(int type, int pos, int num)
    {
        yield return new WaitForSeconds(num);

        GameObject newEn = Instantiate(enemies[type], spawns[pos].position, Quaternion.identity);

        newEn.transform.parent = transform;
    }
}
