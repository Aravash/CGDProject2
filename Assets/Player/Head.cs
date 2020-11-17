﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Head : MonoBehaviour
{
    const float MV_ACCEL = 120f;
    const float MV_FRICTION = 1f;
    
    [SerializeField] private bool IOwnAController = true;

    [SerializeField][Range(0.01f, 0.2f)]
    public float MX_DIST_TO_HEAD = .1f;
    [SerializeField] private Transform leash;
    public bool is_leashed = true;

    float damageMulti = 3f;

    [SerializeField] int id = 0; // 0 = left, 1 = right

    Transform head;

    GameObject HealthM; //Opposite player ref

    private void Start()
    {
        HealthM = GameObject.FindGameObjectWithTag("HealthManager");

 
        //setHealth();
    }
    public void Update()
    {
	if(is_leashed){
        // Leash Target back to hand
	var delta = transform.position-leash.position;
	if(delta.magnitude > MX_DIST_TO_HEAD )
	{transform.position = leash.position + delta.normalized * MX_DIST_TO_HEAD ;}
	}
    }
    private void FixedUpdate()
    {
        applyFriction();
        //Test();
        // Fetch user directional input
        Vector2 wish_dir = new Vector2(0, 0);
        if (IOwnAController)
        {
            wish_dir.x += Input.GetAxis("RHorizontal" + id);
            wish_dir.y += Input.GetAxis("RVertical" + id);
        }
        else
        {
            if (Input.GetKey("right"))
                wish_dir.x++;
            if (Input.GetKey("left"))
                wish_dir.x--;
            if (Input.GetKey("up"))
                wish_dir.y++;
            if (Input.GetKey("down"))
                wish_dir.y--;
            wish_dir.Normalize();
        }

        // Convert input to movement
        Vector2 acceleration = wish_dir * Time.deltaTime;
        acceleration.x *= MV_ACCEL;
        acceleration.y *= MV_ACCEL;
        gameObject.GetComponent<Rigidbody2D>().velocity += acceleration;

    }

    private void Test()
    {
        Vector3 projection = new Vector3(GetComponent<RectTransform>().position.x, GetComponent<RectTransform>().position.y, -Camera.main.transform.position.z);
        Vector3 diff = Camera.main.ScreenToWorldPoint(projection);
        
        Gizmos.DrawWireSphere(diff, 0.1f);

        //Debug.Log(diff);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Test();
    }
    
    //iS THIS BEST WAY? LOOK INTO
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

    public void bonk(float impluse, int input_id)
    {
        float newDamage;
        newDamage = impluse * impluse;

        if(newDamage > 30)
        {
            newDamage = 30;
        }
        HealthM.GetComponent<HealthManager>().ChangeHP(Mathf.Floor(newDamage), input_id);
        //Debug.Log(newDamage);
    }
}


