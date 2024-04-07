using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckClick : MonoBehaviour
{
    public event Action pressButton;  //Callback event after clicking on the UI
    private void Update()
    {
        //If the left mouse button is pressed
        if (Input.GetMouseButtonDown(0))
        {
            //If the UI has a component BoxCollider2D
            if (GetComponentInChildren<BoxCollider2D>() != null)
            {
                //Call BoxCollider2D.OverlapPoint to determine if the mouse has clicked on the UI
                if (GetComponentInChildren<BoxCollider2D>().OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
                {
                    pressButton?.Invoke();  //Callbacks
                }
            }
        }
    }
}
