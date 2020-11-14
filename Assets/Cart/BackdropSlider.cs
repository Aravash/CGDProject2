using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackdropSlider : MonoBehaviour
{
    const float ACCEL_MULTIPLIER = 0.6f;
    const float FRICTION = 0.4f;
    private new Renderer renderer;
    float speed = 0;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        applyFriction();
        float tempx = renderer.material.mainTextureOffset.x;
        renderer.material.mainTextureOffset = new Vector2(tempx += speed * Time.deltaTime, 0);
        //Debug.Log("SPEED: " + speed);
    }

    // Accel value is based on the cart handle's rotation delta
    float debug_high = 0;
    public void accelerate(float accel)
    {
        if (Mathf.Abs(accel) <= 0)
            return;
        speed += accel * ACCEL_MULTIPLIER;
    }

    private void applyFriction()
    {
        if (speed < 0.0001)
        {
            speed = 0;
            return;
        }

        float drop = speed * FRICTION * Time.deltaTime;

        float newspeed = speed - drop;
        if (newspeed < 0)
            newspeed = 0;
        newspeed /= speed;

        speed *= newspeed;
    }

    public float getSpeed()
    {
        return speed;
    }
}
