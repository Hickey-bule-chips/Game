using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//Command queues, control of switching between commands, etc.
public class CommandQueue : MonoBehaviour
{
    public string[] optimalCommandList;  //List of set optimal commands
    [HideInInspector]
    public List<Command> commandList;  //List of entered commands
    [HideInInspector]
    public int currentCommandIndex;  //Index of the currently executed command
    [HideInInspector]
    public bool isPlay = false;  //Whether to start execution
    [HideInInspector]
    public bool isPause = false;  //Whether to pause
    [HideInInspector]
    public bool isStop = false;  //Whether to end or not
    [HideInInspector]
    public bool isLoop = false;  //Whether to execute cyclically

    public event Action stopEvent;  //Callback event when the list of commands has been executed

    void Awake()
    {
        commandList = new List<Command>();
    }

    //Get the last entered command list string, example format: Forward;Back;Attack;......
    public string GetCommandString()
    {
        string str = PlayerPrefs.GetString("Commands" + SceneManager.GetActiveScene().buildIndex, "");
        return str;
    }

    //Convert command list string to command string list
    public string[] StringToList(string commandString)
    {
        string[] commandStrArr = commandString.Split(';');  //Splitting strings into lists of command strings.
        int limit = Mathf.Min(100, commandStrArr.Length);  //Set the maximum number of commands
        List<string> commandStringList = new List<string>();  

        for (int i = 0; i < limit; i++)
        {
            //Empty strings are not added to the array
            if (commandStrArr[i].Length <= 0) continue;
            commandStringList.Add(commandStrArr[i]);
        }
        
        return commandStringList.ToArray();
    }

    //Instantiate the corresponding command from the command string and add it to the commandList
    public void AddCommand(string command)
    {
        //Generate command examples from command strings
        string className = command + "Command";
        System.Type t = System.Type.GetType(className);
        Command c = Activator.CreateInstance(t) as Command;

        if (c == null) return;

        AddCommand(c);//Add commandList
    }

    //Add the instantiated command to the commandList
    public void AddCommand(Command command)
    {
        commandList.Add(command);
        command.commandQueue = this;
    }

    //Start execution
    public void Play()
    {
        if (commandList.Count == 0) return;
        if (isPlay) return;
        isPlay = true;
        StartCoroutine(Playing());
    }

    //Stop execution
    public void Stop()
    {
        commandList[currentCommandIndex].isFinish = true;
        isStop = true;
    }

    //Command execution control logic
    public IEnumerator Playing()
    {
        currentCommandIndex = 0;
        while (true)
        {
            if (currentCommandIndex >= commandList.Count) break;  //Subscript >= list length, i.e. the list of commands is executed and the loop is exited
            commandList[currentCommandIndex].Start();  //execute command
            //Waiting in a loop for the command to finish before the current command is executed
            while (true)
            {
                yield return new WaitForEndOfFrame();
                if (!isPause)
                {
                    //When isFinish is true, it means the current command is finished and the loop is exited
                    if (commandList[currentCommandIndex].isFinish)
                    {
                        break;
                    }
                }
            }
            //isStop is true to end the list of executed commands directly
            if (isStop)
            {
                break;
            }
            //isLoop is true for cyclic execution, otherwise execute currentCommandIndex++ for the next command
            if (!isLoop)
            {
                currentCommandIndex++;
            }
            else
            {
                Loop();
            }
            isLoop = false;
        }
        isPlay = false;
        //The 1.5 second wait here is to delay the stopEvent call.
        //The stopEvent call is mainly to display the game failure panel at the end of the command list execution if the destination is not reached
        //Waiting 1.5 seconds prevents the stopEvent from being called too quickly and displaying a game failure panel
        //when you have already reached your destination.
        yield return new WaitForSeconds(1.5f); 
        stopEvent?.Invoke();
        //Whether or not to demonstrate the optimal command is determined by the value of isShowOptimalSolution,
        //which should be reset to false when the demonstration is finished.
        GameManager.isShowOptimalSolution = false;
    }

    //loop
    public void Loop()
    {
        currentCommandIndex = 0;
        for(int i = 0; i < commandList.Count; i++)
        {
            commandList[i].isFinish = false;
        }
    }
}
