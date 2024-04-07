using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelMenuPanel : MonoBehaviour
{
    public Button[] levelButtonArray;
    // Start is called before the first frame update
    void Start()
    {
        //Opening the level

        int highestLevel = PlayerPrefs.GetInt("HighestLevel", 0); //Get the highest level completed

        //Iterate through all level buttons
        for (int i = 0; i < levelButtonArray.Length; i++)
        {
            //If the index is less than or equal to the highest level completed,
            //the level button is turned on
            if (i <= highestLevel)
            {
                levelButtonArray[i].interactable = true;
                int level = i + 2;
                levelButtonArray[i].onClick.AddListener(()=> {
                    WinPanel.totalScore = 0;
                    SceneManager.LoadScene(level);
                });
            }
            else
            {
                //Otherwise close the button
                levelButtonArray[i].interactable = false;
            }
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }
}
