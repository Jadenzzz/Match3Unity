using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text time;
    private void Awake()
    {
        time = transform.Find("TimeText").GetComponent<Text>();
    }
    private void Update()
    {
        time.text = "Time:\n" + GameManger.GetInstance().GetTime().ToString();
    }
}
