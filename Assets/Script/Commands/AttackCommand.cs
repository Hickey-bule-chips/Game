using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Attack Command
public class AttackCommand : Command
{
    public override void Enter()
    {
        base.Enter();
        player.CombatIdle();  
        player.GetComponent<PlayerAttack>().StartAttack();
    }

    public override bool StopCondition()
    {
        //If isAttackState is false, which means the attack is over, the command ends
        return !player.GetComponent<PlayerAttack>().isAttackState;
    }
}
