using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackdropSlider : MonoBehaviour
{
    const float ACCEL_MULTIPLIER = 0.6f;
    const float FRICTION = 0.4f;
    private new Renderer renderer;
    float speed = 0;

    [SerializeField]private Vector2 SpawnTimerRange = new Vector2(.5f, 1.5f);
    private float spawnTimer;
    [SerializeField] private GameObject hazardObj;
    private Vector3 hazardSpawnPos;
    private HazardMove hazard = null;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();
        hazardSpawnPos = transform.GetChild(0).transform.position;
        spawnTimer = Random.Range(SpawnTimerRange.x, SpawnTimerRange.y);
    }

    private void Update()
    {
        applyFriction();
        float tempx = renderer.material.mainTextureOffset.x;
        renderer.material.mainTextureOffset = new Vector2(tempx += speed * Time.deltaTime, 0);

        hazardBusiness();
    }

    void hazardBusiness()
    {
        if (!hazard)
        {
            if (spawnTimer <= 0)
            {
                hazard = Instantiate(hazardObj, hazardSpawnPos, Quaternion.identity).GetComponent<HazardMove>();
                spawnTimer = Random.Range(SpawnTimerRange.x, SpawnTimerRange.y);
            }
            else spawnTimer -= speed * Time.deltaTime;
        }
        else hazard.Move(speed * Time.deltaTime);
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
