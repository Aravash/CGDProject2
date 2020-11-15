using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    private EventSystem eventsys;

    [SerializeField] private GameObject[] Button;
    private int selectedButton = 0;

    private void Start()
    {
        eventsys = EventSystem.current.GetComponent<EventSystem>();
        eventsys.SetSelectedGameObject(Button[selectedButton]);
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("LT0") > 0.9 || Input.GetAxis("LT1") > 0.9)
        {
            if (selectedButton == 0)
            {
                selectedButton = Button.Length - 1;
            }
            else selectedButton--;
            eventsys.SetSelectedGameObject(Button[selectedButton]);
        }

        if (Input.GetAxis("LT0") < -0.9 || Input.GetAxis("LT1") < -0.9)
        {
            if (selectedButton == Button.Length - 1)
            {
                selectedButton = 0;
            }
            else selectedButton++;
            eventsys.SetSelectedGameObject(Button[selectedButton]);
        }
    }

    public void loadGame()
    {
        SceneManager.LoadScene("Game");
    }
    
    public void exitGame()
    {
        Application.Quit();
    }
}
