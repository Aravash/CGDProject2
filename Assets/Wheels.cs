using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheels : MonoBehaviour
{
    const float ACCEL_MULTIPLIER = 4f;
    const float FRICTION = 1f;
    float speed = 0;

	public List<Transform> kids = new List<Transform>();

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in transform)
{
	kids.Add(child);
}
    }

    // Update is called once per frame
    void Update()
    {
	foreach (Transform child in kids)
{
	child.Rotate(speed,0,0);
}

        applyFriction();
    }

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
}
