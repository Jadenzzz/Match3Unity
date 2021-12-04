using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameWindow : MonoBehaviour
{
    // Start is called before the first frame update
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    

   public void Show()
    {
        gameObject.SetActive(true);

    }
    public void Restart()
    {
        SceneManager.LoadScene("Playing");
        Time.timeScale = 1f;
    }
}
