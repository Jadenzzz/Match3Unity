using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePauseWindow : GameWindow
{
    // Start is called before the first frame update
    

    void Start()
    {
       
        Hide();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        Show();
    }
    
    public void Resume()
    {
        Hide();
        Time.timeScale = 1f;
    }

}
