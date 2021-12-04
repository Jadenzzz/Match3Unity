using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : GameWindow
{
    private Text score;
    
    private void Start()
    {
        GameManger.GetInstance().over += Over;
        Hide();
    }
   
    private void Over(object sender, System.EventArgs e)
    {
        
        Show();
    }
    
   
}
