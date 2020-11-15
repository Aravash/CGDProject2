using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OutcomeManager : MonoBehaviour
{
    private void Update()
    {
        /*test input
         if (Input.GetKeyDown(KeyCode.D))
        {
            callWin("joseph joestar");
        }*/
    }

    /*
     * slow down time and show the winner of the match
     */
    public void callWin(string playername)
    {
        Time.timeScale = 0.4f;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(0).GetChild(0).GetComponent<Text>().text =
            "Hoo Doggy!\n" + playername + " gets to keep their job!";
        transform.GetChild(2).gameObject.SetActive(true);
    }

    public void callLose()
    {
        Time.timeScale = 0.4f;
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
    }

    public void returnToMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("Menu");
    }
}
