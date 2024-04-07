using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHP : MonoBehaviour
{
    Text hpText; //Blood level display Text

    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        hpText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        hpText.text = "HP:" + player.hp;  //Updating blood levels
    }
}
