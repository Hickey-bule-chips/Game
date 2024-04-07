using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base class for commands
public class Command
{
    public CommandQueue commandQueue;  //Command Queue
    protected Player player;  
    public bool isFinish = false;  //Indicates whether the command has finished executing

    //The command starts execution
    public void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        Enter();

        commandQueue.StartCoroutine(IEPlaying());
    }

    //Command execution framework
    private IEnumerator IEPlaying()
    {
        while(isFinish == false)
        {
            //Determining whether a command has finished executing
            if (StopCondition()) break;
            //Determining whether a command is paused
            if (commandQueue.isPause)
            {
                //Logic when executing a command pause
                Pausing();
            }
            else
            {
                //Executing the main logic of the command
                Playing();
            }
            //Waiting for the next frame
            yield return new WaitForEndOfFrame();
        }
        isFinish = true;
        Quit();
    }

    //End of command condition
    public virtual bool StopCondition()
    {
        return false;
    }

    //Handling some logic before the command is executed
    public virtual void Enter()
    {

    }

    //Main logic for command execution
    public virtual void Playing()
    {

    }

    //Logic executed when the command is paused
    public virtual void Pausing()
    {

    }

    //Logic executed when the command exits
    public virtual void Quit()
    {

    }
}
