using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Jump Command
public class JumpCommand : Command
{
    bool isEnd = false;
    public override void Enter()
    {
        base.Enter();
        isEnd = false;
    }

    public override bool StopCondition()
    {
        return isEnd;
    }

    public override void Playing()
    {
        base.Playing();
        Player player = GameObject.Find("Player").GetComponent<Player>();
        //If player.GetGrounded() == true, the player is currently in the jumping state
        //if the player is in a previous jump when the current jump command is executed,
        //wait until the previous jump is completed before executing the current jump
        if (player.GetGrounded())
        {
            player.Jump();
            isEnd = true;
        }
    }
}
