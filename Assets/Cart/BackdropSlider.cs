using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackdropSlider : MonoBehaviour
{
    const float ACCEL_MULTIPLIER = 0.6f;
    const float FRICTION = 0.4f;
    private new Renderer renderer;
    float speed = 0;

    [SerializeField] private GameObject hazardObj;
    private Vector3 hazardSpawnPos;
    private HazardMove hazard = null;
    private float maxSpawnTimer = 3.0f;
    private float spawnTimer = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        hazardSpawnPos = transform.GetChild(0).transform.position;
    }

    private void Update()
    {
        applyFriction();
        float tempx = renderer.material.mainTextureOffset.x;
        float change = tempx += speed * Time.deltaTime;
        renderer.material.mainTextureOffset = new Vector2(change, 0);
        if (!hazard)
        {
            if (spawnTimer <= 0)
            {
            spawnTimer = maxSpawnTimer;
            }
            else spawnTimer -= change;
        }
        else hazard.Move(change);
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
