using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tutorial : MonoBehaviour
{
    public TextMeshProUGUI directTxt;
    public TextMeshProUGUI directTxt2;
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

    public PlayerHealth health;

    public GameObject[] instructions;

    bool healthCan = true;
    public TextMeshProUGUI canCount;
    public TextMeshProUGUI dynCount;
    public Button canBtn;
    public Button dynBtn;

    public GameObject miniMap;

    float startTime = 0;
    float dynTime = 0;
    bool dynDone = false;

    // Start is called before the first frame update
    void Start()
    {
        PlayerHealth.healthCanCount = 0;
        DynamiteDropper.dynamiteCount = 0;
        canCount.text = "0";
        dynCount.text = "0";
        canBtn.interactable = false;
        dynBtn.interactable = false;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (health.health <= 10)
        {
            health.health = health.maxHealth;
        }
        if (healthCan && health.maxHealth - health.health >= 30)
        {
            //Health canister
            instructions[3].SetActive(true);
            timerTxt.enabled = false;
            Time.timeScale = 0f;
            canCount.text = "3";
            canBtn.interactable = true;
            PlayerHealth.healthCanCount = 3;
            healthCan = false;
        }

        if (stage == 0)
        {
            startTime += Time.deltaTime;
            if (startTime > 3)
            {
                instructions[0].SetActive(true);
                timerTxt.enabled = false;
                timerTxt.text = "Continue Forward.";
                Time.timeScale = 0f;
                stage++;
            }
        }
        else if (stage == 3 && transform.childCount == 3)
        {
            timerTxt.text = "Defeat the Enemies to Continue.";
            Destroy(wall);
            stage++;
        }
        else if (stage == 4)
        {
            //Dynamite
            dynTime += Time.deltaTime;
            if (!dynDone && dynTime > 3)
            {
                dynDone = true;
                instructions[4].SetActive(true);
                timerTxt.enabled = false;
                Time.timeScale = 0f;
                dynCount.text = "3";
                dynBtn.interactable = true;
                DynamiteDropper.dynamiteCount = 3;
            }
            //Timer and Incursion
            if (transform.childCount == 0)
            {
                stage++;
                instructions[5].SetActive(true);
                timerTxt.enabled = false;
                Time.timeScale = 0f;
            }
        }
        else if (stage == 5)
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
                portScr.miniMap = miniMap;
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



        //if (stage == 0 && Input.GetMouseButtonDown(1))
        //{
        //    directTxt.text = "Left Click to Attack in Mouse Direction. Breaking Materials Gives You Resources That Can Be Used to Buy Upgardes After Each World. Break Through the Materials to Proceed.";
        //    stage++;
        //}
        //else if (stage == 2 && transform.childCount == 4)
        //{
        //    directTxt.text = "Scroll the Mouse Wheel or Press 2 to switch to your gun. Hold Left Click and Shoot the Enemy.";
        //    stage++;
        //}
        //else if (stage == 3 && transform.childCount == 3)
        //{
        //    directTxt.text = "Melee or Shoot the Enemies to Defeat Them.";
        //    Destroy(wall);
        //    stage++;
        //}
        //else if (stage == 4 && transform.childCount == 0)
        //{
        //    directTxt.text = "Each World Has an Incursion Timer. When it Reaches 0 a Portal Opens and Enemies Begin Rapidly Spawning.";
        //    stage++;
        //}
        //else if (stage == 5)
        //{
        //    timer -= Time.deltaTime;
        //    float minutes = Mathf.FloorToInt(timer / 60);
        //    float seconds = Mathf.FloorToInt(timer % 60);
        //    if (seconds < 10)
        //    {
        //        timerTxt.text = minutes + ":0" + seconds;
        //    }
        //    else
        //    {
        //        timerTxt.text = minutes + ":" + seconds;
        //    }

        //    if (timer <= 0)
        //    {
        //        timerTxt.text = "Reach the Portal and Escape!";
        //        GameObject port = Instantiate(portal, new Vector3(0, 2, 0), Quaternion.identity);
        //        port.transform.Rotate(new Vector3(-90, 0, 0));
        //        Portal portScr = port.GetComponent<Portal>();
        //        portScr.endCam = endCam;
        //        portScr.mainUI = mainUI;
        //        portScr.victUI = victUI;
        //        stage++;
        //    }
        //}
        //else
        //{
        //    if (transform.childCount == 0)
        //    {
        //        spawnWave();
        //    }
        //}
    }

    public void wallTrigger()
    {
        //if (stage == 1)
        //{
        //    directTxt.text = "Melee the enemy to kill it. Melee attacks knock back and stun enemies prventing attacks for a short time.";
        //}
        //stage++;

        if (stage == 1)
        {
            //Materials
            instructions[1].SetActive(true);
            timerTxt.enabled = false;
            Time.timeScale = 0f;
            timerTxt.text = "Mine Through the Materials to Continue.";
            stage++;
        }
        else if (stage == 2)
        {
            //Weapons
            instructions[2].SetActive(true);
            timerTxt.enabled = false;
            Time.timeScale = 0f;
            timerTxt.text = "Melee the Close Enemy and Shoot the Far One.";
            stage++;
        }
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

    public void Next(int instr)
    {
        mainUI.SetActive(true);
        instructions[instr].SetActive(false);
        timerTxt.enabled = true;
        Time.timeScale = 1f;
    }
}
