using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    const float THROW_SPEED_MODIFIER = 1; // Should match the ratio of camera clipping plane to prop depth

    bool untouched = true;
    bool grounded;

    Transform hand;


    // Start is called before the first frame update
    void Start()
    {
        // If grounded, spawn low, update velocity with each change of cart speed
        // If not grounded, spawn above
    }

    private void Update()
    {
        if(hand != null)
        {
            Vector3 projection = new Vector3(
                hand.GetComponent<RectTransform>().position.x,
                hand.GetComponent<RectTransform>().position.y,
                gameObject.transform.position.z - Camera.main.transform.position.z);
            
            gameObject.transform.position = Camera.main.ScreenToWorldPoint(projection); ;
        }
        //gameObject.transform.position = Camera.main.ScreenToWorldPoint(projection);
    }

    public void grab(Transform grabber)
    {
        hand = grabber;
    }
    public void release()
    {
        GetComponent<Rigidbody>().velocity = transform.parent.GetComponent<Rigidbody2D>().velocity;
        hand = null;
        untouched = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if(collision.collider.GetComponent<Head>() != null)
        //  collision.collider.GetComponent<Head>().bonk(dmg)
    }
}
