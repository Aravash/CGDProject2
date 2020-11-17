using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthManager : MonoBehaviour
{



    Slider P1HPBar;
    Slider P2HPBar;
    [SerializeField] Gradient gradient;

    Image imageP1;
    Image imageP2;

    Image P1Medic;
    Image P2Medic;

    float MaxHP = 100;
    float currentP1HP;
    float currentP2HP;

    GameObject outcome;
    GameObject starRating;


    void Start()
    {
        currentP1HP = MaxHP;
        currentP2HP = MaxHP;

        P1HPBar = GameObject.FindGameObjectWithTag("Player1_HPBar").GetComponent<Slider>();
        P2HPBar = GameObject.FindGameObjectWithTag("Player2_HPBar").GetComponent<Slider>();

        //Need to clean this up
        imageP1 = GameObject.Find("FillP1").GetComponent<Image>();
        imageP2 = GameObject.Find("FillP2").GetComponent<Image>();
        P1Medic = GameObject.Find("P1").GetComponent<Image>();
        P2Medic = GameObject.Find("P2").GetComponent<Image>();

        outcome = GameObject.Find("Outcomes");
        starRating = GameObject.Find("StarRatings");
        SetHPBars();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SetHPBars()
    {
        P1HPBar.maxValue = MaxHP;
        P1HPBar.value = currentP1HP;
        P2HPBar.maxValue = MaxHP;
        P2HPBar.value = currentP2HP;

        imageP1.color = gradient.Evaluate(1f);
        P1Medic.color = imageP1.color;
        imageP2.color = gradient.Evaluate(1f);
        P2Medic.color = imageP2.color;

    }

    public void ChangeHP(float damage, int player_id)
    {
        checkEndGame();
        if (player_id == 0)
        {
            currentP2HP -= damage;
            P2HPBar.value = currentP2HP;
            imageP2.color = gradient.Evaluate(P2HPBar.normalizedValue);
            P2Medic.color = imageP2.color;
            if (currentP2HP <= 0)
            {
                outcome.GetComponent<OutcomeManager>().callWin("Player 1");
            }
        }
        if (player_id == 1)
        {
            currentP1HP -= damage;
            P1HPBar.value = currentP1HP;
            imageP1.color = gradient.Evaluate(P1HPBar.normalizedValue);
            P1Medic.color = imageP1.color;
            if (currentP1HP <= 0)
            {
                outcome.GetComponent<OutcomeManager>().callWin("Player 2");
            }
        }
    }

    public void checkEndGame()
    {
        if (currentP1HP <= 0 || currentP2HP <= 0)
        {
            starRating.GetComponent<starRatings>().hideWarning();
        }
        return;
    }

    public float GetCurrentP1HP()
    {
        return currentP1HP;
    }

    public float GetCurrentP2HP()
    {
        return currentP2HP;
    }
}