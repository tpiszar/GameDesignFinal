using System.Collections;
using System.Collections.Generic;
using TMPro;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeMenu : MonoBehaviour
{
    public static int lastChosen1 = -1;
    public static int lastChosen2 = -1;

    public TextMeshProUGUI titleTxt;
    public TextMeshProUGUI up1Txt;
    public TextMeshProUGUI up2Txt;
    public TextMeshProUGUI up1CountTxt;
    public TextMeshProUGUI up2CountTxt;
    public TextMeshProUGUI resourceTxt;
    public Button up1Decr;
    public Button up1Incr;
    public Button up2Decr;
    public Button up2Incr;

    public int speedPrice;
    public int healthPrice;
    public int meleePrice;
    public int bulletPrice;
    public int firePrice;

    public Player player;

    int value1;
    int value2;
    int price1;
    int price2;
    int cap1;
    int cap2;
    int floor1;
    int floor2;

    // Start is called before the first frame update
    void Start()
    {
        titleTxt.text = "Dig Cite " + Player.Level + " Cleared";
        resourceTxt.text = "Resource Count: " + Player.ResourceCount;

        int chosen1 = -1;
        do
        {
            chosen1 = Random.Range(0, 5);
        } while (chosen1 == lastChosen1 || chosen1 == lastChosen2);
        int chosen2 = -1;
        do
        {
            chosen2 = Random.Range(0, 5);
        } while (chosen2 == chosen1 || chosen2 == lastChosen2 || chosen2 == lastChosen1);
        lastChosen1 = chosen1;
        lastChosen2 = chosen2;

        switch (chosen1)
        {
            case 0:
                up1Txt.text = "Speed: (" + speedPrice + ")";
                up1CountTxt.text = Player.speedBonus + " / " + player.spdCap;
                value1 = Player.speedBonus;
                price1 = speedPrice;
                cap1 = player.spdCap;
                break;
            case 1:
                up1Txt.text = "Health: (" + healthPrice + ")";
                up1CountTxt.text = Player.healthBonus + " / " + player.healthCap;
                value1 = Player.healthBonus;
                price1 = healthPrice;
                cap1 = player.healthCap;
                break;
            case 2:
                up1Txt.text = "Melee Damage: (" + meleePrice + ")";
                up1CountTxt.text = Player.meleeDmgBonus + " / " + player.meleeDmgCap;
                value1 = Player.meleeDmgBonus;
                price1 = meleePrice;
                cap1 = player.meleeDmgCap;
                break;
            case 3:
                up1Txt.text = "Bullet Damage: (" + bulletPrice + ")";
                up1CountTxt.text = Player.bulletDmgBonus + " / " + player.bulletDmgCap;
                value1 = Player.bulletDmgBonus;
                price1 = bulletPrice;
                cap1 = player.bulletDmgCap;
                break;
            case 4:
                up1Txt.text = "Fire Rate: (" + firePrice + ")";
                up1CountTxt.text = Player.fireRateBonus + " / " + player.fireRateCap;
                value1 = Player.fireRateBonus;
                price1 = firePrice;
                cap1 = player.fireRateCap;
                break;
        }
        switch (chosen2)
        {
            case 0:
                up2Txt.text = "Speed: (" + speedPrice + ")";
                up2CountTxt.text = Player.speedBonus + " / " + player.spdCap;
                value2 = Player.speedBonus;
                price2 = speedPrice;
                cap2 = player.spdCap;
                break;
            case 1:
                up2Txt.text = "Health: ("+ healthPrice + ")";
                up2CountTxt.text = Player.healthBonus + " / " + player.healthCap;
                value2 = Player.healthBonus;
                price2 = healthPrice;
                cap2 = player.healthCap;
                break;
            case 2:
                up2Txt.text = "Melee Damage: (" + meleePrice + ")";
                up2CountTxt.text = Player.meleeDmgBonus + " / " + player.meleeDmgCap;
                value2 = Player.meleeDmgBonus;
                price2 = meleePrice;
                cap2 = player.meleeDmgCap;
                break;
            case 3:
                up2Txt.text = "Bullet Damage: (" + bulletPrice + ")";
                up2CountTxt.text = Player.bulletDmgBonus + " / " + player.bulletDmgCap;
                value2 = Player.bulletDmgBonus;
                price2 = bulletPrice;
                cap2 = player.bulletDmgCap;
                break;
            case 4:
                up2Txt.text = "Fire Rate: (" + firePrice + ")";
                up2CountTxt.text = Player.fireRateBonus + " / " + player.fireRateCap;
                value2 = Player.fireRateBonus;
                price2 = firePrice;
                cap2 = player.fireRateCap;
                break;
        }
        floor1 = value1;
        floor2 = value2;

        if (Player.ResourceCount < price1 || value1 == cap1)
        {
            up1Incr.interactable = false;
        }
        if (Player.ResourceCount < price2 || value2 == cap2)
        {
            up2Incr.interactable = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void incrUpgrade(int choice)
    {
        if (choice == 0)
        {
            value1++;
            Player.ResourceCount -= price1;
            up1CountTxt.text = value1 + " / " + cap1;
            if (Player.ResourceCount < price1 || value1 == cap1)
            {
                up1Incr.interactable = false;
            }
            if (Player.ResourceCount < price2)
            {
                up2Incr.interactable = false;
            }
            up1Decr.interactable = true;
        }
        else
        {
            value2++;
            Player.ResourceCount -= price2;
            up2CountTxt.text = value2 + " / " + cap2;
            if (Player.ResourceCount < price2 || value2 == cap2)
            {
                up2Incr.interactable = false;
            }
            if (Player.ResourceCount < price1)
            {
                up1Incr.interactable = false;
            }
            up2Decr.interactable = true;
        }
        resourceTxt.text = "Resource Count: " + Player.ResourceCount;
    }

    public void decrUpgrade(int choice)
    {
        if (choice == 0)
        {
            value1--;
            Player.ResourceCount += price1;
            up1CountTxt.text = value1 + " / " + cap1;
            if (value1 == floor1)
            {
                up1Decr.interactable = false;
            }
            up1Incr.interactable = true;
            if (Player.ResourceCount >= price2)
            {
                up2Incr.interactable = true;
            }
        }
        else
        {
            value2--;
            Player.ResourceCount += price2;
            up2CountTxt.text = value2 + " / " + cap2;
            if (value2 == floor2)
            {
                up2Decr.interactable = false;
            }
            up2Incr.interactable = true;
            if (Player.ResourceCount >= price1)
            {
                up1Incr.interactable = true;
            }
        }
        resourceTxt.text = "Resource Count: " + Player.ResourceCount;
    }

    public void applyUprades()
    {
        switch (lastChosen1)
        {
            case 0:
                Player.speedBonus = value1;
                break;
            case 1:
                Player.healthBonus = value1;
                break;
            case 2:
                Player.meleeDmgBonus = value1;
                break;
            case 3:
                Player.bulletDmgBonus = value1;
                break;
            case 4:
                Player.fireRateBonus = value1;
                break;
        }
        switch (lastChosen2)
        {
            case 0:
                Player.speedBonus = value2;
                break;
            case 1:
                Player.healthBonus = value2;
                break;
            case 2:
                Player.meleeDmgBonus = value2;
                break;
            case 3:
                Player.bulletDmgBonus = value2;
                break;
            case 4:
                Player.fireRateBonus = value2;
                break;
        }
    }
}
