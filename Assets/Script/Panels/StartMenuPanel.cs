using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Clear level completion data
    public void ClearData()
    {
        PlayerPrefs.SetInt("HighestLevel", 0);
        WinPanel.totalScore = 0;
    }

    public void StartGame()
    {
        SceneManager.LoadScene(2);
       // WinPanel.totalScore = 0;
    }

    public void SelectLevel()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()// exit the game
    {
        Application.Quit();
    }
}
