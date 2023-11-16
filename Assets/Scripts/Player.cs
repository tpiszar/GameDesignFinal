using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
//using Unity.VisualScripting;
//using JetBrains.Annotations;
using UnityEngine.AI;
using JetBrains.Annotations;
using System;

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

    public static int poisonBonus = 0;
    public static int AoEBonus = 0;
    public static int lifeStealBonus = 0;
    public static int knockBackBonus = 0;
    public static int jugBonus = 0;
    public static int glassBonus = 0;
    public static int antiGravBonus = 0;
    public static int lifeSupportBonus = 0;
    public static int richMatsBonus = 0;
    public static int armorPierceBonus = 0;
    public static int gasLeakBonus = 0;
    public static int reflectBonus = 0;
    public static int resourceSpdBonus = 0;
    public static int rockStealBonus = 0;
    public static int rockShatterBonus = 0;

    public float spdPerc;
    public float healthPerc;
    public float meleeDmgPerc;
    public float bulletDmgPerc;
    public float fireRatePerc;

    public float poisonPerc;
    public float AoEperc;
    public float knockBackTime;
    public float jugHpPerc, jugSpdPerc;
    public float glassDmgPerc, glassHpPerc;
    public float antiGravSpdPerc, antiGravDmgPerc;
    public float reflectPerc;
    public float resourceSpdPerc;

    public int spdCap;
    public int healthCap;
    public int meleeDmgCap;
    public int bulletDmgCap;
    public int fireRateCap;

    NavMeshAgent agent;
    PlayerHealth healthScr;
    Melee meleeScr;
    Shoot shootScr;

    float baseSpd;
    float resourceSpd;
    public float resourceSpdTime;
    float resourceEndTime;

    // Start is called before the first frame update
    void Start()
    {
        meleeScr = GetComponentInChildren<Melee>();
        shootScr = GetComponentInChildren<Shoot>();
        agent = GetComponentInChildren<NavMeshAgent>();
        healthScr = GetComponentInChildren<PlayerHealth>();

        Level++;
        float speed = (agent.speed * spdPerc * speedBonus);

        int hp = (int)(healthScr.maxHealth * healthPerc * healthBonus);

        int mDmg = (int)(meleeScr.meleeDmg * meleeDmgPerc * meleeDmgBonus);

        int sDmg = (int)(shootScr.damage * bulletDmgPerc * bulletDmgBonus);

        float sFR = (int)(shootScr.fireRate * fireRatePerc * fireRateBonus);

        if (poisonBonus > 0)
        {
            shootScr.poisonPerc = poisonPerc;
            shootScr.poisonTick = shootScr.poisonTick / Mathf.Pow(2, poisonBonus);
        }
        if (AoEBonus > 0)
        {
            shootScr.AoE = true;
            shootScr.AoEmod += (shootScr.AoEmod * AoEperc * (AoEBonus - 1));
            shootScr.AoErange += AoEBonus;
        }
        if (knockBackBonus > 0)
        {
            shootScr.knockBack = true;
            shootScr.stunDur = knockBackTime * (knockBackBonus - 1);
        }
        
        if (jugBonus > 0)
        {
            healthScr.maxHealth += (int)(healthScr.maxHealth * jugHpPerc * jugBonus);
        }
        if (glassBonus > 0)
        {
            meleeScr.meleeDmg += (int)(meleeScr.meleeDmg * glassDmgPerc * glassBonus);
            shootScr.damage += (int)(shootScr.damage * glassDmgPerc * glassBonus);
        }
        if (antiGravBonus > 0)
        {
            agent.speed += (agent.speed * antiGravSpdPerc * antiGravBonus);
        }

        shootScr.armorPiercing = armorPierceBonus;

        float reflectTotal = reflectPerc;
        for (int i = 1; i < reflectBonus; i++)
        {
            reflectPerc *= 0.5f;
            reflectTotal += reflectPerc;
        }
        reflectPerc = reflectTotal;

        agent.speed += speed;
        healthScr.maxHealth += hp;
        meleeScr.meleeDmg += mDmg;
        shootScr.damage += sDmg;
        shootScr.fireRate += sFR;
        if (jugBonus > 0)
        {
            agent.speed = (agent.speed / (jugSpdPerc * jugBonus));
        }
        if (glassBonus > 0)
        {
            healthScr.maxHealth = (int)(healthScr.maxHealth / (glassHpPerc * glassBonus));
        }
        if (antiGravBonus > 0)
        {
            meleeScr.meleeDmg = (int)(meleeScr.meleeDmg / (antiGravDmgPerc * antiGravBonus));
            shootScr.damage = (int)(shootScr.damage / (antiGravDmgPerc * antiGravBonus));
        }
        healthScr.health = healthScr.maxHealth;
        healthScr.healthBar.maxValue = healthScr.health;
        healthScr.healthBar.value = healthScr.health;

        baseSpd = agent.speed;
        resourceSpd = agent.speed + agent.speed * resourceSpdPerc * resourceSpdBonus;
    }

    // Update is called once per frame
    void Update()
    {
        resourceTxt.text = ResourceCount.ToString();

        if (resourceEndTime < resourceSpdTime)
        {
            resourceEndTime += Time.deltaTime;
            if (resourceEndTime > resourceSpdTime)
            {
                agent.speed = baseSpd;
            }
        }
    }

    public void Dead()
    {
        Time.timeScale = 0;
        mainUI.SetActive(false);
        deathScreen.SetActive(true);
        deathScreen.GetComponentInChildren<TextMeshProUGUI>().text = "You Died at Dig Cite " + Level.ToString();
    }

    public void RockSpeedUp()
    {
        agent.speed = resourceSpd;
        resourceEndTime = 0;
    }
}
