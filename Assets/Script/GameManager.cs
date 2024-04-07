using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool isStartGame;  //Whether to start executing the command
    public static bool isShowOptimalSolution = false;  //Whether to demonstrate the optimal command
    public bool isWin = false;  //Whether the level is successfully completed  
    public CommandQueue commandQueue;  //Command queue management class
    public ScoreCondition scoreCondition;  //Score management

    private void Awake()
    {
        //Initialization
        commandQueue = GetComponent<CommandQueue>();
        scoreCondition = GetComponent<ScoreCondition>();

        isStartGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        //If the level does not need to reach the destination,
        //kill all the enemies to pass the level, then the victory is judged here
        if (isWin == false)
        {
            //A true value of isJustEnemy means that the level is cleared by killing all enemies.
            if (scoreCondition.isJustEnemy)
            {
                //Get the number of enemies remaining, if it is zero, the game is won
                if (GetEnemyAmount()  == 0)
                {
                    commandQueue.isStop = true;
                    Win();
                }
            }
        }
    }

    //Level win
    public void Win() {
        if (isWin) return;
        isWin = true;
        int highestLevel = PlayerPrefs.GetInt("HighestLevel", 0); //Get the highest level completed
        //Determine if the level is the highest level completed and save
        if (highestLevel < SceneManager.GetActiveScene().buildIndex - 1)
            PlayerPrefs.SetInt("HighestLevel", SceneManager.GetActiveScene().buildIndex - 1);

        //Show victory panel
        GameObject canvas = GameObject.Find("Canvas");
        WinPanel winPanel = canvas.transform.Find("WinPanel").GetComponent<WinPanel>();
        winPanel.SetScore(scoreCondition.GetScore(this));
        winPanel.gameObject.SetActive(true);
        commandQueue.isStop = true;
    }

    //Get the number of enemies
    public int GetEnemyAmount()
    {
        GameObject[] enemys = GameObject.FindGameObjectsWithTag("Enemy");
        return enemys.Length; 
    }

    //Game failed
    public void GameOver()
    {
        //Because something in the code prematurely determines that the game has failed and calls the method
        //So here's how to determine if the game has really failed
        if (GetEnemyAmount() == 0 && scoreCondition.isJustEnemy) {
            Win();
        }
        if (isWin) return;

        //Show game failure panel
        GameObject canvas = GameObject.Find("Canvas");
        canvas.transform.Find("GameOverPanel").gameObject.SetActive(true);
        commandQueue.isStop = true;
        isStartGame = false;
    }
}
