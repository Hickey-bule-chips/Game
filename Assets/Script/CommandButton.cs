using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

//Manage command buttons
public class CommandButton : MonoBehaviour
{
    public Text contentText;  //Text on UI

    [HideInInspector]
    public GamePanel gamePanel;

    //Set button text
    public void SetText(string content)
    {
        contentText.text = content;
    }
    //Binding click events
    public void BindButtonEvent()
    {
        //GetComponent<Button>().onClick.AddListener(OnClickButton);
        GetComponent<CheckClick>().pressButton += OnClickButton;
    }

    public void OnClickButton()
    {
        //Clicking a button instantiates a dragging UI that follows the mouse
        GameObject dragUIPrefab = Resources.Load("DragUI") as GameObject;
        GameObject dragUI = Instantiate(dragUIPrefab,transform.parent.parent);
        dragUI.transform.position = transform.position;
        dragUI.GetComponent<DragUIMove>().SetText(contentText.text);
        dragUI.GetComponent<DragUIMove>().StartMove();
    }
}
