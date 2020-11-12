using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    const float THROW_SPEED_MODIFIER = 75f; // Should match the ratio of camera clipping plane to prop depth

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
    }
    
    private void FixedUpdate()
    {
        if(hand != null)
        {
            Vector3 projection = new Vector3(
                hand.GetComponent<RectTransform>().position.x,
                hand.GetComponent<RectTransform>().position.y,
                //gameObject.transform.position.z - Camera.main.transform.position.z);
                -Camera.main.transform.position.z);
            Vector3 diff = Camera.main.ScreenToWorldPoint(projection) - gameObject.transform.position;
            GetComponent<Rigidbody>().AddForce(diff, ForceMode.Impulse);

            Debug.DrawRay(gameObject.transform.position, diff, Color.white, Time.fixedDeltaTime);
        }
    }

    public void grab(Transform grabber)
    {
        hand = grabber;

    }
    public void release()
    {
        hand = null;
        untouched = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // if(collision.collider.GetComponent<Head>() != null)
        //  collision.collider.GetComponent<Head>().bonk(dmg)
    }
}
