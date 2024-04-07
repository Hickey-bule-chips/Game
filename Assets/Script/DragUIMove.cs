using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragUIMove : MonoBehaviour
{
    private bool UICanMove = false; //Does the UI start to move
    private Text text;  //Text on the dragged UI
    private bool isOverlapCommListUI;  //Whether to move to the position of the command list

    private void Update() {
        //Mouse release
        if (Input.GetMouseButtonUp(0)) {
            UICanMove = false;  //End of move
            EndMove();  //Invoke the logic after the end of the move
        }

        //UI starts to move
        if (UICanMove) {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);  //Get the position of the mouse in the world coordinate system
            pos.z = 0;  

            GetComponent<RectTransform>().position = pos; //Make the UI follow the mouse

            Collider2D[] col = Physics2D.OverlapPointAll(Camera.main.ScreenToWorldPoint(Input.mousePosition));  //Get all the UI at the mouse position

            if (col.Length > 0)//Array length is not empty
            {
                //Set isOverlapCommListUI to true if the mouse position is a command list
                foreach (Collider2D c in col) {
                    if (c.gameObject.name == "Scroll View") {
                        isOverlapCommListUI = true;
                        return;
                    }
                }
                isOverlapCommListUI = false;
            }
        }
    }

    //Set the text on the UI
    public void SetText(string content) {
        if (text == null) text = GetComponentInChildren<Text>();
        text.text = content;
    }

    //Start moving
    public void StartMove() {
        UICanMove = true;
        isOverlapCommListUI = false;
    }

    //Logic to process when ending a move
    private void EndMove() {
        //If isOverlapCommListUI is true the button has been moved to the command list position, add the command to the list
        if (isOverlapCommListUI) {
            GameObject.Find("Canvas").GetComponent<GamePanel>().OnClickCommandButton(text.text);
        }
        
        Destroy(gameObject);
    }
}
