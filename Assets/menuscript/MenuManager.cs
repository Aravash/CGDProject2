using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void loadGame()
    {
        SceneManager.LoadScene("Game");
    }
    
    public void exitGame()
    {
        Application.Quit();
    }
}
