using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandUI : MonoBehaviour
{
    public Text commandText;

    public void SetText(string content)
    {
        commandText.text = content;
    }
}
