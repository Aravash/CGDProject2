using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdMember : MonoBehaviour
{
    const float SPAWN_POS_X = 3f;

    //const float CROWD_SIZE = 0.25f;
    const float CROWD_SIZE_VARI = 0.05f;
    //Vector3 CROWD_POS = new Vector3(0, 0.2f, -3);
    const float CROWD_POS_OFFSET = 1.5f;
    const float CROWD_POS_VARI = 0.5f;
    const float CROWD_POS_Y = 0.15f;
    const float CROWD_BOB = 0.05f;

    const float TRAVEL_POS_Y = 0.1f;
    const float TRAVEL_SPEED = 1f;

    const float COLOUR_LO = 0.1f;
    const float COLOUR_HI = 0.3f;

    float destination_x;
    bool walk_in_dir_right;

    float arrival_time;

    enum State
    {
        ENTERING,
        WATCHING,
        LEAVING
    }
    State state = State.ENTERING;

    // Start is called before the first frame update
    void Start()
    {
        arrival_time = Time.time;
        // DESTINATION
        bool right = Random.value < 0.5f;
        float offset = right ? CROWD_POS_OFFSET : -CROWD_POS_OFFSET;
        offset += Random.Range(-CROWD_POS_VARI, CROWD_POS_VARI);
        destination_x = offset;
        walk_in_dir_right = right;

        // move offscreen
        Vector3 pos = transform.position;
        pos.x = right ? SPAWN_POS_X : -SPAWN_POS_X;
        transform.position = pos;

        // SCALE
        Vector3 scale = transform.localScale;
        float scale_ex = Random.Range(0, CROWD_SIZE_VARI);
        scale.x += scale_ex;
        scale.y += scale_ex;
        transform.localScale = scale;

        // COLOUR
        float r = Random.Range(COLOUR_LO, COLOUR_HI);
        float g = Random.Range(COLOUR_LO, COLOUR_HI);
        float b = Random.Range(COLOUR_LO, COLOUR_HI);
        GetComponent<SpriteRenderer>().color = new Color(r, g, b);
    }

    // Update is called once per frame
    void Update()
    {
        bob();
        if(state == State.ENTERING || state == State.LEAVING)
        {
            walkIn();
        }
    }

    void bob()
    {
        float y = CROWD_POS_Y;
        y += CROWD_BOB * Mathf.Sin(Time.time - arrival_time);

        Vector3 pos = transform.position;
        pos.y = y;
        transform.position = pos;
    }

    void walkIn()
    {
        Vector3 pos = transform.position;
        float dist = walk_in_dir_right ? -TRAVEL_SPEED : TRAVEL_SPEED;
        dist *= Time.deltaTime;
        pos.x += dist;
        transform.position = pos;
        // Check if I have arrived
        if (walk_in_dir_right ? (pos.x <= destination_x) : (pos.x >= destination_x))
        {
            // If we are walking offscreen, we are now out of the scene
            if(state == State.LEAVING)
            {
                CrowdManager._i.decreaseCount(gameObject);
                return;
            }
            state = State.WATCHING;
            //arrival_time = Time.time;
        }
    }

    public void leave()
    {
        state = State.LEAVING;
        destination_x = walk_in_dir_right ? -SPAWN_POS_X : SPAWN_POS_X;
    }
}
