using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Determine if the character moves to the edge of the terrain or touches a wall
public class EnemyFlipSensor : MonoBehaviour
{
    public bool isGround = false;
    public bool isWall = false;
    //If the collider touches a wall, it turns.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            isWall = true;
        }
    }
    //If the collider touches the edge of the ground, it turns, Box Collider 2D
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            isGround = false;
        }
    }
}
