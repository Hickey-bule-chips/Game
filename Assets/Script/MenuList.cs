using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuList : MonoBehaviour
{
    public GameObject menuList;//Esc control panel

    [SerializeField] private bool menuKeys = true;
    void Start()
    {
        
    }

    void Update()
    {
        if (menuKeys)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuList.SetActive(true);
                menuKeys = false;
                Time.timeScale = 0;//time stop
            }
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            menuList.SetActive(false);
            menuKeys = true;
            Time.timeScale = 1;//time begin
        }

    }
    public void LevelMenu()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void Exit()// exit the game
    {
        Application.Quit();
    }
}
