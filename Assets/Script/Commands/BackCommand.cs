using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Back Command
public class BackCommand : ForwardCommand
{
    
    public override void Enter()
    {
        base.Enter();
        //Since it is inherited from the forward command and differs from the forward command only in direction,
        //it is sufficient to change the value of the direction
        direction = -1;
    }
}
