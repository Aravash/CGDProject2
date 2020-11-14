﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    const float MV_MAX_SPEED = .1f;
    const float MV_ACCEL = 10f;
    const float MV_FRICTION = 1f;
    const float RADIUS = 0.8f;

    [SerializeField] int id = 0; // 0 = left, 1 = right

    GameObject held_prop;
    GameObject held_bar;

    // Update is called once per frame
    void Update()
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

    private void FixedUpdate()
    {
       //if(held_bar)
       //{
       //    gameObject.GetComponent<Rigidbody2D>().velocity *= 0;
       //    return;
       //}

        applyFriction();

        // Fetch user directional input
        Vector2 wish_dir = new Vector2(0, 0);
        if (Input.GetKey("d") || Input.GetKey("right"))
            wish_dir.x++;
        if (Input.GetKey("a") || Input.GetKey("left"))
            wish_dir.x--;
        if (Input.GetKey("w") || Input.GetKey("up"))
            wish_dir.y++;
        if (Input.GetKey("s") || Input.GetKey("down"))
            wish_dir.y--;
        wish_dir.Normalize();

        // Convert input to movement
        Vector2 acceleration = wish_dir;
        acceleration.x *= MV_ACCEL;
        acceleration.y *= MV_ACCEL;
        gameObject.GetComponent<Rigidbody2D>().velocity += acceleration;

        // TODO: Update the IK to match new position
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
		
		var back = transform.TransformDirection(Vector3.back);
        var ray = new Ray(transform.position, Vector3.forward);//Camera.main.ScreenPointToRay(gameObject.GetComponent<RectTransform>().position);
		
        LayerMask lm = 0;
        //lm |= 1 << 9;
        lm = LayerMask.GetMask("grabbable");
        Debug.DrawRay(transform.position, ray.direction * 30, Color.white, 3);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, lm))
        {
            GameObject other = hit.transform.gameObject;
            Debug.Log(other.gameObject.name);
            //if(other.GetComponent<Head>())
            //{
            //    // You have just grabbed your own head
            //}
            if (other.GetComponent<Prop>() != null)
            {
                held_prop = other;
                other.GetComponent<Prop>().grab(gameObject.transform);
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
    }
}
