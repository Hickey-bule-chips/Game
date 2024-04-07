using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Controls the display of input command buttons
public class ActiveCommandButton : MonoBehaviour
{
    //Control the show/hide of individual buttons in the Inspect panel
    public bool forwardCommandBtn;
    public bool backCommandBtn;
    public bool jumpCommandBtn;
    public bool loopCommandBtn;
    public bool attackCommandBtn;
    public bool pickCommandBtn;

    private GamePanel gamePanel;
    private Transform commandList;
    private GameObject btnPrefab;
    // Start is called before the first frame update
    void Start()
    {
        gamePanel = GameObject.Find("Canvas").GetComponent<GamePanel>();
        commandList = GameObject.Find("CommandList").transform;
        btnPrefab = Resources.Load<GameObject>("CommandButton");

        InsAndBindBtnEvent("Forward", forwardCommandBtn);
        InsAndBindBtnEvent("Back", backCommandBtn);
        InsAndBindBtnEvent("Jump", jumpCommandBtn);
        InsAndBindBtnEvent("Loop", loopCommandBtn);
        InsAndBindBtnEvent("Attack", attackCommandBtn);
      //  InsAndBindBtnEvent("Pick", pickCommandBtn);
    }

    public void InsAndBindBtnEvent(string command,bool isActive)
    {
        //If isActive is true then the command button is displayed
        if (isActive)
        {
            //Instantiate button
            CommandButton ins = Instantiate(btnPrefab, commandList).GetComponent<CommandButton>();
            //Set command text
            ins.SetText(command);
            ins.gamePanel = gamePanel;
            //Binding click events
            ins.BindButtonEvent();
        }
    }
}
