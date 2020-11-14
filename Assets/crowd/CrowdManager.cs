using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrowdManager : MonoBehaviour
{
    [SerializeField] bool DEBUG_MESSAGES = false;
    // Needs to be a MonoBehaviour singleton, because it utilizes unity's Update()
    static CrowdManager _i;

    const int MAX_CROWD = 6;
    int num_onlookers = 0;
    float event_cooldown;

    const float SUSP_DECAY = 1f;
    const float SUSP_LIMIT = 100f;
    const float SUSP_RATE = 5f;
    const float SUSP_SPEED_THRESHOLD = 0.2f;
    BackdropSlider backdrop;
    float suspicion = 0;
    Slider susp_bar;

    private void Awake()
    {
        if(_i != null)
        {
            Destroy(this);
            return;
        }
        _i = this;
    }
    
    void Start()
    {
        backdrop = FindObjectOfType<BackdropSlider>();
        susp_bar = GameObject.Find("Susp_bar").GetComponent<Slider>();
        susp_bar.maxValue = SUSP_LIMIT;
    }
    
    void Update()
    {
        event_cooldown -= Time.deltaTime;
        if(event_cooldown < 0)
        {
            crowdSpawnEvent();
        }
        suspicionCheck();
    }

    /// Crowd population control
    void crowdSpawnEvent()
    {
        float intensity = Mathf.Sin(Time.time * 0.1f);
        float rng = gaussDistribution();
        rng += intensity;
        if (rng > 0)
        {
            increment();
        }
        else if (rng < 0)
        {
            decrement();
        }

        // How long to wait before another spawn/despawn event
        float new_cd = 2 * gaussDistribution() + 2.5f;
        // flatten values exceeding 3sigma
        if (new_cd > 5 || new_cd < 0.3f)
        {
            new_cd = Random.Range(0.3f, 5);
        }
        event_cooldown = new_cd;
        if(DEBUG_MESSAGES)
        {
            Debug.Log("Intensity: " + intensity);
            Debug.Log("rng: " + rng);
            Debug.Log("CD: " + event_cooldown);
        }
    }
    void increment()
    {
        num_onlookers++;
        if (num_onlookers > MAX_CROWD)
            num_onlookers = MAX_CROWD;
        if (DEBUG_MESSAGES)
        {
            Debug.Log("SPAWN: " + num_onlookers);
        }
    }
    void decrement()
    {
        if (num_onlookers > 0)
            num_onlookers--;
        if (DEBUG_MESSAGES)
        {
            Debug.Log("DSPWN: " + num_onlookers);
        }
    }
    // Random number in a normal distribution -1 to 1
    float gaussDistribution()
    {
        float u, v, S;

        do
        {
            u = 2.0f * Random.value - 1.0f;
            v = 2.0f * Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0);

        float fac = Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);
        return u * fac;
    }


    void suspicionCheck()
    {
        // TODO: fetch cart pseudovelocity
        float pumpspeed = backdrop.getSpeed();
        if (pumpspeed > SUSP_SPEED_THRESHOLD)
        {
            suspicion -= SUSP_DECAY * Time.deltaTime;
            /// update GUI
            susp_bar.value = suspicion;
            return;
        }

        // increase suspicion (inversely proportional to pumpspeed)
        float susp_multiplier = 1 - pumpspeed / SUSP_SPEED_THRESHOLD;
        suspicion += SUSP_RATE * Time.deltaTime * susp_multiplier;
        /// update GUI
        susp_bar.value = suspicion;

        if (suspicion >= SUSP_LIMIT)
        {
            // TODO: GAME OVER
            //Debug.Log("EXHIBIT CLOSED");
            //Debug.Log("GAME OVER, EVERYBODY LOSES!");
        }
    }
}
