using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private Text score;
    private void Awake()
    {
        score = transform.Find("ScoreText").GetComponent<Text>();
    }
    private void Update()
    {
        score.text = "Score:\n" + GameManger.GetInstance().GetScore().ToString();
    }
}
