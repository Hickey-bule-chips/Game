using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controlling character attacks
public class PlayerAttack : MonoBehaviour
{
    public bool isAttackState = false;  //Whether the attack is on status
    public bool isAttack = false;  //Whether the attack plays to an animation frame that can do damage
    public bool isPlayAttackAnimation = false;  //Whether or not the attack animation starts
    private Player player;
    private AttackSensor attackSensor;  //Detection of enemies in range of attack

    private void Start()
    {
        //Initialisation
        player = GetComponent<Player>();
        attackSensor = GetComponentInChildren<AttackSensor>();
        attackSensor.type = "Enemy";  //Target of attack is Enemy
    }

    private void Update()
    {
        //If in ready to attack state
        if (isAttackState)
        {
            //If the attack animation is not playing
            if (isPlayAttackAnimation == false)
            {
                //If an enemy is detected in the attack range
                if (attackSensor.enemyList.Count > 0)
                {
                    //Start the attack animation
                    isPlayAttackAnimation = true;
                    player.Attack();
                   // isAttackState = false;
                }
            }
        }

        //If an animation frame is played that can do damage
        if (isAttack)
        {
            //If an enemy is detected in the attack range
            if (attackSensor.enemyList.Count > 0)
            {
                //Invoke Hurt to deal damage to the enemy
                for (int i = 0; i < attackSensor.enemyList.Count; i++)
                {
                    attackSensor.enemyList[i].GetComponent<Enemy>().Hurt(gameObject);
                }
                isAttack = false;
            }
        }
    }

    //Start attacking
    public void StartAttack()
    {
        isAttackState = true;
    }
    //Start the attack, animation triggers the event
    public void Attacking()
    {
        isAttack = true;
    }

    //Ends the attack, animation fires the event
    public void CompleteAttack()
    {
        isAttack = false;
        isPlayAttackAnimation = false;
        isAttackState = false;
    }
}
