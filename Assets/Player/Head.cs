using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Head : MonoBehaviour
{
    const float MV_ACCEL = 10f;
    const float MV_FRICTION = 1f;

    Slider player1_HPBar;
    Slider player2_HPBar;
    [SerializeField] Gradient gradient;

    float maxHP = 100f;
    float currentP1HP;
    float currentP2HP;

    float damageMulti = 3f;

    [SerializeField] int id = 0; // 0 = left, 1 = right

    Transform head;

    private void Start()
    {
        currentP1HP = maxHP;
        currentP2HP = maxHP;
        setHPBar();
        //setHealth();
    }
    private void FixedUpdate()
    {
        applyFriction();
        //Test();
        // Fetch user directional input
        Vector2 wish_dir = new Vector2(0, 0);
        /*
         if (Input.GetKey("right"))
            wish_dir.x++;
         if (Input.GetKey("left"))
            wish_dir.x--;
        if (Input.GetKey("up"))
            wish_dir.y++;
        if (Input.GetKey("down"))
            wish_dir.y--;
        wish_dir.Normalize();
        */
        
        wish_dir.x += Input.GetAxis("RHorizontal" + id);
        wish_dir.y += Input.GetAxis("RVertical" + id);

        // Convert input to movement
        Vector2 acceleration = wish_dir;
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


    private void setHPBar()
    {
        player1_HPBar = GameObject.Find("Player1_HPBar").GetComponent<Slider>();
        player1_HPBar.maxValue = maxHP;
        player1_HPBar.value = maxHP;

        player2_HPBar = GameObject.Find("Player2_HPBar").GetComponent<Slider>();
        player2_HPBar.maxValue = maxHP;
        player2_HPBar.value = maxHP;

        //TODO set up gradient 
    }
    
    //iS THIS BEST WAY? LOOK INTO
    private void setHealthBarP1(float damage)
    {
        damage *= damageMulti;
        player1_HPBar.value -= damage;
        currentP1HP = player1_HPBar.value;
    }

    private void setHealthBarP2(float damage)
    {
        damage *= damageMulti;
        player2_HPBar.value -= damage;
        currentP2HP = player2_HPBar.value;
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

    public void bonk(float impluse, int id)
    {
        //TODO sort out damage taken and only to target hit
        if (id == 0)
        {
            setHealthBarP2(Mathf.Floor(impluse));
        }
        else
        {
            setHealthBarP1(Mathf.Floor(impluse));
        }

        Debug.Log("currentSpeed: " + Mathf.Floor(impluse));
    }
}
