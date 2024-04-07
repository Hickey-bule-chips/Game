using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCondition : MonoBehaviour
{
    public int bestCommandAmount;  //Minimum number of commands required for the level
    public int maxScore = 20;  //Maximum score
    public int minScore = 10;  //Minimum score
    public bool isJustEnemy = false;  //Is the level only enemy, no destination

    //Calculating the score
    //Rules: If you kill all the enemies in the level and the number of commands used is less than or equal to the minimum number of commands required,
    //you will score the highest score.
    //One point is deducted for one less kill or one more order than the set number of orders
    //The lowest mark is awarded when the mark is reduced to less than the minimum mark
    public int GetScore(GameManager manager)
    {
        int count = manager.commandQueue.commandList.Count;
        int enemyAmout = GameObject.Find("GameManager").GetComponent<GameManager>().GetEnemyAmount();
        if (count <= bestCommandAmount && enemyAmout == 0)
        {
            return maxScore;
        }

        return Mathf.Max(minScore, maxScore - count - enemyAmout);
    }
}
