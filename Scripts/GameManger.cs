using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    private static GameManger instance;
    private int _score = 0;
    public bool _gameover = false;
    private float timer = 100f;
    public event EventHandler over;


    private void Awake()
    {
        instance = this;
    }

    public static GameManger GetInstance()
    {
        return instance;
    }

    public int Score
    {
        get { return _score; }
        set { _score = value; } 
    }

    public int GetScore()
    {
        return this._score;
    }

    public float GetTime()
    {
        return this.timer;
    }
   

    void Start()
    {
            
    }
    void Update()
    {
       
        if (timer > 0f && _gameover == false)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0;
            if (over != null) over(this, EventArgs.Empty);
        }
    }

    public void Dead()
    {

        if (over != null) over(this, EventArgs.Empty);

    }
}
