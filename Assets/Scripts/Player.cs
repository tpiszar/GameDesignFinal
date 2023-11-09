using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.VisualScripting;

public class Player : MonoBehaviour
{
    public static int ResourceCount = 0;
    public static int Level = 1;
    public TextMeshProUGUI resourceTxt;
    public GameObject deathScreen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        resourceTxt.text = ResourceCount.ToString();
    }

    public void Dead()
    {
        Time.timeScale = 0;
        deathScreen.SetActive(true);
    }
}
