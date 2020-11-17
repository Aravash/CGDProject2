using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prop : MonoBehaviour
{
    const float THROW_SPEED_MODIFIER = 75f; // Should match the ratio of camera clipping plane to prop depth
    
    bool grounded;

    Transform hand;

    GameObject head;
    GameObject Spawner;
    int playerId = 2;

    private float currentTime;
    private float maxTime = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        head = GameObject.FindGameObjectWithTag("HeadTarget");
        Spawner = GameObject.Find("PropSpawner");
        currentTime = 0.0f;
        // If grounded, spawn low, update velocity with each change of cart speed
        // If not grounded, spawn above
    }

    private void Update()
    {
        if(!grounded)
        {
            currentTime = 5.0f;
        }
        else
        {
            currentTime += Time.deltaTime;
        }

        Timer();
    }
    
    private void FixedUpdate()
    {
        if(hand == null)
        {
            grounded = true;
        }
        else if (hand != null)
        {
            grounded = false;
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

    private void Timer()
    {
        if (currentTime >= maxTime)
        {
            StartCoroutine("DeathDelay");
            currentTime = 0.0f;
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
            Spawner.GetComponent<PropSpawner>().changeSpawned(1);
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
