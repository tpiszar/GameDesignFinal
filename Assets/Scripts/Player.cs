using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;
using JetBrains.Annotations;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    public static int ResourceCount = 0;
    public static int Level = 0;
    public TextMeshProUGUI resourceTxt;
    public GameObject deathScreen;
    public GameObject mainUI;

    public static int speedBonus = 0;
    public static int healthBonus = 0;
    public static int meleeDmgBonus = 0;
    public static int bulletDmgBonus = 0;
    public static int fireRateBonus = 0;

    public float spdPerc;
    public float healthPerc;
    public float meleeDmgPerc;
    public float bulletDmgPerc;
    public float fireRatePerc;

    public int spdCap;
    public int healthCap;
    public int meleeDmgCap;
    public int bulletDmgCap;
    public int fireRateCap;

    NavMeshAgent agent;
    PlayerHealth healthScr;
    Melee meleeScr;
    Shoot shootScr;

    // Start is called before the first frame update
    void Start()
    {
        meleeScr = GetComponentInChildren<Melee>();
        shootScr = GetComponentInChildren<Shoot>();
        agent = GetComponentInChildren<NavMeshAgent>();
        healthScr = GetComponentInChildren<PlayerHealth>();

        Level++;
        agent.speed += (agent.speed * spdPerc * speedBonus);
        healthScr.maxHealth += (int)(healthScr.maxHealth * healthPerc * healthBonus);
        meleeScr.meleeDmg += (int)(meleeScr.meleeDmg * meleeDmgPerc * meleeDmgBonus);
        shootScr.damage += (int)(shootScr.damage * bulletDmgPerc * bulletDmgBonus);
        shootScr.fireRate += (int)(shootScr.fireRate * fireRatePerc * fireRateBonus);
        
    }

    // Update is called once per frame
    void Update()
    {
        resourceTxt.text = ResourceCount.ToString();
    }

    public void Dead()
    {
        Time.timeScale = 0;
        mainUI.SetActive(false);
        deathScreen.SetActive(true);
        deathScreen.GetComponentInChildren<TextMeshProUGUI>().text = "You Died at Dig Cite " + Level.ToString();
    }
}
