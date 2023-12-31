using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject mainUI;
    public GameObject controlsUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadLevel(int scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Controls()
    {
        mainUI.SetActive(false);
        controlsUI.SetActive(true);
    }

    public void Menu()
    {
        mainUI.SetActive(true);
        controlsUI.SetActive(false);
    }
}
