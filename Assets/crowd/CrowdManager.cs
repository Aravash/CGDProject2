using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdManager : MonoBehaviour
{
    // Needs to be a MonoBehaviour singleton, because it utilizes unity's Update()
    static CrowdManager _i;
    const int MAX_CROWD = 6;

    int num_onlookers;
    float event_cooldown;

    float suspicion = 0;
    const float SUSP_DECAY = 0.2f;
    const float SUSP_LIMIT = 0.2f;
    const float SUSP_RATE = 1f;
    const float SUSP_THRESHOLD = 1f;

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
        
    }
    
    void Update()
    {

        event_cooldown -= Time.deltaTime;
        if(event_cooldown < 0)
        {
            float intensity = Mathf.Sin(Time.time * 0.1f);
            Debug.Log("Intensity: " + intensity);
            float rng = gaussDistribution();
            Debug.Log("rng: " + rng);
            rng += intensity;
            if (rng > 0 )
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
            Debug.Log("CD: " + event_cooldown);
        }
    }

    void increment()
    {
        num_onlookers++;
        if (num_onlookers > MAX_CROWD)
            num_onlookers = MAX_CROWD;
        Debug.Log("SPAWN: " + num_onlookers);
    }
    void decrement()
    {
        if (num_onlookers > 0)
            num_onlookers--;
        Debug.Log("DSPWN: " + num_onlookers);
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
        float pumpspeed = 1;
        if (pumpspeed < SUSP_THRESHOLD)
        {
            suspicion -= SUSP_DECAY * Time.deltaTime;
            return;
        }

        // increase suspicion (inversely proportional to pumpspeed)
        float susp_multiplier = pumpspeed == 0 ? 1 : SUSP_THRESHOLD / pumpspeed;
        suspicion += SUSP_RATE * Time.deltaTime * susp_multiplier;

        if(suspicion > SUSP_LIMIT)
        {
            // TODO: GAME OVER
        }
    }
}
