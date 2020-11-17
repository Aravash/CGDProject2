using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    const float MV_MAX_SPEED = 5f;
    const float MV_ACCEL = 150f;
    const float MV_FRICTION = 1f;
    const float RADIUS = 0.8f;

    [SerializeField][Range(0.01f, 0.2f)]
    public float MX_DIST_TO_HAND = .1f;
    [SerializeField] private Transform leash;
    public bool isLeashed = true;
    [SerializeField] private bool IOwnAController = true;

    [SerializeField] int id = 0; // 0 = left, 1 = right

    bool grabbed_something = false;
    GameObject held_prop;
    GameObject held_bar;

    // Update is called once per frame
    void Update()
    {
        if (IOwnAController)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetAxis("LT" + id) > 0.1 && !grabbed_something)
            {
                grab();
            }

            if (Input.GetMouseButtonUp(0) || Input.GetAxis("LT" + id) < 0.1 && grabbed_something)
            {
                release();
            }
        }
        else
        {
            
            if (Input.GetMouseButtonDown(0))
            {
                grab();
            }

            if (Input.GetMouseButtonUp(0))
            {
                release();
            }
        }
    }

    private void FixedUpdate()
    {
        applyFriction();

        // Fetch user directional input
        Vector2 wish_dir = new Vector2(0, 0);
        if (IOwnAController)
        {
            wish_dir.x += Input.GetAxis("LHorizontal" + id);
            wish_dir.y += Input.GetAxis("LVertical" + id);
        }
        else
        {
            if (Input.GetKey("d"))
                wish_dir.x++;
            if (Input.GetKey("a"))
                wish_dir.x--;
            if (Input.GetKey("w"))
                wish_dir.y++;
            if (Input.GetKey("s"))
                wish_dir.y--;

            wish_dir.Normalize();
        }

        // Convert input to movement
        Vector2 acceleration = wish_dir * Time.deltaTime;
        acceleration.x *= MV_ACCEL;
        acceleration.y *= MV_ACCEL;
        gameObject.GetComponent<Rigidbody2D>().velocity += acceleration;

        // TODO: Update the IK to match new position
	if(isLeashed){
        // Leash Target back to hand
	var delta = transform.position-leash.position;
	if(delta.magnitude > MX_DIST_TO_HAND)
	{transform.position = leash.position + delta.normalized * MX_DIST_TO_HAND;}
	}
    }


    private void applyFriction()
    {
        Vector2 vel = gameObject.GetComponent<Rigidbody2D>().velocity;
        float speed = vel.magnitude;
        if (speed < 0.01)
        {
            vel.x = 0;
            vel.y = 0;
            gameObject.GetComponent<Rigidbody2D>().velocity = vel;
            return;
        }

        float drop = speed * MV_FRICTION;

        float newspeed = speed - drop;
        if (newspeed < 0)
            newspeed = 0;
        newspeed /= speed;

        vel.x *= newspeed;
        vel.y *= newspeed;
        gameObject.GetComponent<Rigidbody2D>().velocity = vel;
    }

    void grab()
    {
        RaycastHit hit;
		
		var pos = transform.position;
		pos.z = -999;
        var ray = new Ray(pos, Vector3.forward);//Camera.main.ScreenPointToRay(gameObject.GetComponent<RectTransform>().position);
		
        LayerMask lm = 0;
        //lm |= 1 << 9;
        lm = LayerMask.GetMask("grabbable");
        Debug.DrawRay(Camera.main.transform.position, ray.direction * 30, Color.white, 3);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, lm))
        {
            grabbed_something = true;
            GameObject other = hit.transform.gameObject;
            Debug.Log(other.gameObject.name);
            //if(other.GetComponent<Head>())
            //{
            //    // You have just grabbed your own head
            //}
            if (other.GetComponent<Prop>() != null)
            {
                held_prop = other;
                other.GetComponent<Prop>().grab(gameObject.transform, id);
            }
            if (other.GetComponent<CartHandle>())
            {
                held_bar = other;
                // gameObject.transform.parent = other.transform;
                other.GetComponent<CartHandle>().grab(gameObject.transform, id);
            }
        }

	}
    void release()
    {
        if (held_prop != null)
        {
            held_prop.GetComponent<Prop>().release();
            held_prop = null;
        }
        if (held_bar != null)
        {
            held_bar.GetComponent<CartHandle>().release(id);
            held_bar = null;
        }
        grabbed_something = false;
    }
}
