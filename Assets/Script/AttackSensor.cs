using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Determine if there is an enemy in range of the attack
public class AttackSensor : MonoBehaviour
{
    public List<GameObject> enemyList;  //Save for enemies in range of attack
    public string type; //The tag of the object to be attacked
    private void Start()
    {
        enemyList = new List<GameObject>();
    }
    //If the player's or enemy's Circle Collider 2D detects the opponent, the enemyList will increase the number of detected enemies
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == type)
        {
            enemyList.Add(collision.gameObject);
        }
    }
    //If the player's or enemy's Circle Collider 2D does not detect or kills the opponent,
    //the enemyList subtracts the number of detected enemies or does not show them
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == type)
        {
            enemyList.Remove(collision.gameObject);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        
    }
}
