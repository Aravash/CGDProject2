using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    const float THROW_SPEED_MODIFIER = 75f; // Should match the ratio of camera clipping plane to prop depth
    
    bool grounded;

    Transform hand;

    GameObject head;

    int playerId = 2;

    // Start is called before the first frame update
    void Start()
    {
        head = GameObject.FindGameObjectWithTag("HeadTarget");
        // If grounded, spawn low, update velocity with each change of cart speed
        // If not grounded, spawn above
    }

    private void Update()
    {
    }
    
    private void FixedUpdate()
    {
        if(grounded)
        {
            //float vel = GetComponent<Rigidbody>().velocity;
            //vel.x = TODO: fetch cart speed
            //GetComponent<Rigidbody>().velocity = vel;
        }
        if (hand != null)
        {
            //var projection = ;//new Vector3(
                //hand.GetComponent<RectTransform>().position.x,
                //hand.GetComponent<RectTransform>().position.y,
                //gameObject.transform.position.z - Camera.main.transform.position.z);
                //-Camera.main.transform.position.z);
            Vector3 diff = hand.GetComponent<RectTransform>().position - gameObject.transform.position;
            GetComponent<Rigidbody>().AddForce(diff, ForceMode.Impulse);

            Debug.DrawRay(gameObject.transform.position, diff, Color.white, Time.fixedDeltaTime);
        }
    }

    public void grab(Transform grabber, int id)
    {
        playerId = id;
        hand = grabber;

        Debug.Log(id);
    }
    public void release()
    {
        hand = null;
    }

    private void OnCollisionEnter(Collision collision)
    {

        //TODO change this to be cleaner unless implementation changes
        if (collision.collider.tag == "P1Head" && playerId == 1 || collision.collider.tag == "P2Head" && playerId == 0)
        {
            head.GetComponent<Head>().bonk(collision.impulse.magnitude, playerId);
            StartCoroutine("DeathDelay");
        }

        // if(collision.collider.tag == "conveyor")
        //  grounded = true;
    }

    IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(.1f);
        Destroy(gameObject);
    }
}
