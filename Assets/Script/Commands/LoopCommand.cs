using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Loop Command
public class LoopCommand : Command
{
    public override void Enter()
    {
        base.Enter();
        commandQueue.isLoop = true;
    }

    public override bool StopCondition()
    {
        return true;
    }
}
