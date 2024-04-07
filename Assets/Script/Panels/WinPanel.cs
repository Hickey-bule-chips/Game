using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WinPanel : MonoBehaviour
{
    public static int totalScore;
    public Text scoreText;
    public Text totalText;
    public void SetScore(int score)
    {
        scoreText.text = "Score:" + score.ToString();
        Debug.Log(totalScore);
        totalScore += score;
        SetTotalText(totalScore);
    }
    public void SetTotalText(int score) {
        totalText.text ="TotalScore:" + totalScore.ToString();
    }
}
