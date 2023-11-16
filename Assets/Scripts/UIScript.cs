using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    bool paused = false;
    public GameObject PauseMenu;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && (Time.timeScale != 0 || paused))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        paused = !paused;
        Time.timeScale = paused ? 0 : 1;
        PauseMenu.SetActive(paused);
    }

    public void LoadLevel(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Restart()
    {
        Player.ResourceCount = 0;
        Player.Level = 0;
        Player.speedBonus = 0;
        Player.healthBonus = 0;
        Player.meleeDmgBonus = 0;
        Player.bulletDmgBonus = 0;
        Player.fireRateBonus = 0;

        Player.poisonBonus = 0;
        Player.AoEBonus = 0;
        Player.lifeStealBonus = 0;
        Player.knockBackBonus = 0;
        Player.jugBonus = 0;
        Player.glassBonus = 0;
        Player.antiGravBonus = 0;
        Player.lifeSupportBonus = 0;
        Player.richMatsBonus = 0;
        Player.armorPierceBonus = 0;
        Player.gasLeakBonus = 0;
        Player.reflectBonus = 0;
        Player.resourceSpdBonus = 0;
        Player.rockStealBonus = 0;
        Player.rockShatterBonus = 0;
        Player.invincBonus = 0;
    }
}
