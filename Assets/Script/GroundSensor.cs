using UnityEngine;
using System.Collections;

//In the jump logic, detects if the player is jumpable
public class GroundSensor : MonoBehaviour {

    private int m_ColCount = 0;  //0 means no further jumps

    private float m_DisableTimer;  //Disable jump time to prevent frequent calls to the jump logic within a short period of time,
                                   //which could lead to errors in judgement and could be interpreted as a cooldown time

    private bool isJump = false;  //Whether or not to be in a jump state

    private void OnEnable()
    {
        //Initialization
        m_ColCount = 0;
        isJump = false;
    }

    //Determines whether a jump is possible or not
    public bool State()
    {
        //If m_DisableTimer>0, then jumping is disabled
        if (m_DisableTimer > 0)
            return false;
        //if isJump is false, then jumping is allowed
        if (isJump == false) return true;
        //if m_ColCount>0, then jumping is allowed
        return m_ColCount > 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(isJump){
            m_ColCount++;
            isJump = false;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if(isJump){
            m_ColCount--;
        }
    }

    void Update()
    {
        m_DisableTimer -= Time.deltaTime;
    }

    public void Disable(float duration)
    {
        //After a player has made a jump, jumping is prohibited for a short period of time to prevent errors in judgement
        isJump = true;
        m_DisableTimer = duration;
    }
}
