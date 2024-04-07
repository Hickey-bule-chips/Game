using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Forward Command
public class ForwardCommand : Command
{
    protected float direction = 1;  //Direction of movement 1 for forward, -1 for backward
    protected float moveDistant = 2;  //Distance moved
    protected float moveTime = 0.5f;  //Movement time
    protected float timer;  //Timing variables

    public override void Enter()
    {
        base.Enter();
        direction = 1;
        timer = 0;
    }

    public override bool StopCondition()
    {
        //End the move command when the time reaches the set moveTime
        timer += Time.deltaTime;
        if (timer >= moveTime)
            return true;
        return false;
    }

    public override void Playing()
    {
        player.inputX = direction;//Setting the direction of movement
        player.speed = moveDistant / moveTime;//Distance/time = speed
    }

    public override void Pausing()
    {
        player.inputX = 0;
    }

    public override void Quit()
    {
        player.inputX = 0;
    }
}
