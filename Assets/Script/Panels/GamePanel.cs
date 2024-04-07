using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//Manage Game Panel
public class GamePanel : MonoBehaviour
{
    public bool isShowTipPanel = false; //Whether or not to display the tips panel
    public GameObject tipPanel;
    public ScrollRect scrollRect;  //Scrolling component for command lists
    public Transform commandQueueUITrans;  //Command list parent object
    public CommandQueue commandQueue;  //Command list management class
    public List<Image> commandUIInsList;  //Command UI list

    private void Start()
    {
        //Initialisation
        commandQueue = GameObject.Find("GameManager").GetComponent<CommandQueue>();
        commandUIInsList = new List<Image>();
        tipPanel = transform.Find("TipPanel").gameObject;

        //Display tips panel
        if (isShowTipPanel)
        {
            tipPanel.SetActive(true);
        }

        //Read the list of last saved commands
        string commandStr = commandQueue.GetCommandString();//Get command list string

        string[] commandList;
        //If isShowOptimalSolution is true, then get the list of optimal commands
        if (GameManager.isShowOptimalSolution)
            commandList = commandQueue.optimalCommandList;
        else
            //Otherwise normal conversion of command list strings to command string lists
            commandList = commandQueue.StringToList(commandStr);

        //Loop through and add commands to the list
        for (int i = 0; i < commandList.Length; i++)
        {
            AddCommand(commandList[i]);
        }
        Debug.Log("total command：" + commandList.Length);
        //Scroll the list of UI commands to the last page
        scrollRect.normalizedPosition = new Vector2(1, 1);

        //Set the commandQueue.stopEvent callback to determine if the game has failed after the command list has been executed
        commandQueue.stopEvent += () =>{
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            gm.GameOver();
        };

        //If isShowOptimalSolution is true, the optimal command list is demonstrated directly
        if (GameManager.isShowOptimalSolution)
        {
            Play();
        }
    }

    private void Update()
    {
        if (commandQueue.isPlay)
        {
            //When executing the command list, set the UI of the currently executed command to black
            for (int i = 0; i < commandUIInsList.Count; i++)
            {
                if (i == commandQueue.currentCommandIndex)
                {
                    commandUIInsList[i].color = new Color(0.65f, 0.65f, 0.65f);
                }
                else
                {
                    commandUIInsList[i].color = new Color(1, 1, 1);
                }
            }
        }
        else
        {
            //Returns the command UI colours when the command list is not executed
            for (int i = 0; i < commandUIInsList.Count; i++)
            {
                commandUIInsList[i].color = new Color(1, 1, 1);
            }
        }

        if (commandQueue.isPlay)
        {
            //Automatic scrolling of the UI command list when executing a command list
            if (commandQueue.commandList.Count > 0)
            {
                float x = (commandQueue.currentCommandIndex /10) * 10 * 1.0f / (commandQueue.commandList.Count - 10);
                x = Mathf.Min(1, x);
                scrollRect.normalizedPosition = new Vector2(x, x);
            }
        }
    }

    //CommandButton click event
    public void OnClickCommandButton(string command)
    {
        AddCommand(command);
        SaveCommand(command);
        StartCoroutine(TopScrollRect());
    }

    //Top command scroll box
    public IEnumerator TopScrollRect() {
        yield return new WaitForEndOfFrame();
        scrollRect.normalizedPosition = new Vector2(1, 1);
    }

    //OptimalSolutionButton click event
    public void OnClickOptimalSolutionButton()
    {
        GameManager.isShowOptimalSolution = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    //Display command UI based on command string
    public void AddCommand(string command)
    {
        commandQueue.AddCommand(command);
        GameObject prefab = Resources.Load<GameObject>("CommandUI");
        GameObject ins = Instantiate(prefab, commandQueueUITrans);
        ins.GetComponent<CommandUI>().SetText(command);
        commandUIInsList.Add(ins.GetComponent<Image>());
    }

    //Save the currently entered command
    public void SaveCommand(string command)
    {
        string str = PlayerPrefs.GetString("Commands" + SceneManager.GetActiveScene().buildIndex, "");
        str += command + ";";
        PlayerPrefs.SetString("Commands" + SceneManager.GetActiveScene().buildIndex, str);
    }

    //Start execution of the command
    public void Play()
    {
        GameManager.isStartGame = true;
        commandQueue.isPause = false;
        commandQueue.Play();
        GameManager.isShowOptimalSolution = false;
    }

    //Pause
    public void Pause()
    {
        commandQueue.isPause = true;
    }

    //Replay
    public void RePlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        GameManager.isShowOptimalSolution = false;
    }
    //Clear the entered command
    public void ClearCommand()
    {
        GameManager.isShowOptimalSolution = false;
        PlayerPrefs.SetString("Commands" + SceneManager.GetActiveScene().buildIndex, "");
        RePlay();
    }

    //Next level
    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
