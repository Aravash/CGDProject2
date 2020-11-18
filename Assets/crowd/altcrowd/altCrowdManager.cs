﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class altCrowdManager : MonoBehaviour
{
    [SerializeField] private Vector2 crowdPresentDuration = new Vector2(5f, 10f);
    [SerializeField] private Vector2 crowdAbsentDuration = new Vector2(15f, 20f);
    [SerializeField] private float crowdMoveSpeed = 2f;
    [SerializeField] private float warningTimer = 5f;
    private Transform[] crowdSprite = new Transform[2];
    [SerializeField] private float acceptableAverage = 0.00015f;
    private bool crowdAbsent = false;
    [SerializeField]private float timer;

    [SerializeField] private starRatings stars;
    [SerializeField] private BackdropSlider slider;
    private float[] backdropSpeeds = new float[0];

    public AudioClip boo;
    public AudioClip applause;
    public AudioClip hmm;
    
    // Start is called before the first frame update
    void Start()
    {
        timer = Random.Range(crowdPresentDuration.x, crowdPresentDuration.y);
        crowdSprite[0] = transform.GetChild(0);
        crowdSprite[1] = transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer <= warningTimer && timer > 0 && !crowdAbsent)
        {
            stars.showWarning();
        }
        
        if (timer <= 0)
        {
            crowdAbsent = !crowdAbsent;
            if (crowdAbsent)
            {
                timer = Random.Range(crowdPresentDuration.x, crowdPresentDuration.y);
                GetComponent<AudioSource>().PlayOneShot(hmm, 0.5f);
            }
            else
            {
                timer = Random.Range(crowdAbsentDuration.x, crowdAbsentDuration.y);
                if (speedAcceptable())
                {
                    Debug.Log("the speed was acceptable this wave");
                    GetComponent<AudioSource>().PlayOneShot(applause, 0.5f);
                    stars.changeActiveStars(1);
                }
                else
                {
                    Debug.Log("the speed was UNACCEPTABLE!!! this wave");
                    GetComponent<AudioSource>().PlayOneShot(boo, 0.5f);
                    stars.changeActiveStars(-1);
                }
                Array.Clear(backdropSpeeds, 0, backdropSpeeds.Length);
                backdropSpeeds = new float[0];
            }

            stars.hideWarning();
        }
        else timer -= Time.deltaTime;

        // move crowd in or out of scene depending on the bool
        if (crowdAbsent)
        {
            moveCrowd(0, -1.3f);
            moveCrowd(1, 1.3f);
        }
        else
        {
            moveCrowd(0, -4f);
            moveCrowd(1, 4f);
            
            backdropSpeeds = new float[backdropSpeeds.Length+1];
            backdropSpeeds[backdropSpeeds.Length - 1] = slider.getSpeed();

        }
    }

    /*
     * returns whether the average speed while the audience was
     * present is above the acceptable average
     */
    private bool speedAcceptable()
    {
        float cumulative = 0f;
        foreach (float speed in backdropSpeeds)
        {
            cumulative += speed;
        }
        cumulative *= 1000; //prevent fucky business because speed is so low by default
        Debug.Log("average speed was " + cumulative / backdropSpeeds.Length + ", acceptable average is " + acceptableAverage);
        return (cumulative/ backdropSpeeds.Length > acceptableAverage);
    }

    /*
     * lerps the crowd sprites to the new position on the X
     */
    private void moveCrowd(int arraySpot, float newXPos)
    {
        crowdSprite[arraySpot].position =
            Vector3.Lerp(crowdSprite[arraySpot].position,
                new Vector3(newXPos, crowdSprite[arraySpot].position.y, crowdSprite[arraySpot].position.z),
                crowdMoveSpeed * Time.deltaTime);
    }
}
