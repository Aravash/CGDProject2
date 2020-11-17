using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    public RectTransform canvasRect;
    public Camera cam;

    public float propMinSpawnDelay = 3f;
    public float propMaxSpawnDelay = 6f;
    public float indicatorHeightPos;
    public float propHeightDelay;

    private BoxCollider spawnPlaneCol;
    private float timer;
    private float nextPropDelay;

    private int currentSpawned;
    private int maxSpawned = 6;
    
    void Start()
    {
        spawnPlaneCol = GetComponent<BoxCollider>();
        timer = 0;
        nextPropDelay = NewRndDelay();
    }
    
    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= nextPropDelay)
        {
            timer = 0;
            nextPropDelay = NewRndDelay();

            // Generate new prop position
            float spawnPlaneLPos = spawnPlaneCol.bounds.min.x;
            float spawnPlaneRPos = spawnPlaneCol.bounds.max.x;
            float spawnPlaneFPos = spawnPlaneCol.bounds.min.z;
            float spawnPlaneBPos = spawnPlaneCol.bounds.max.z;
            Vector3 newPropPos = new Vector3(Random.Range(spawnPlaneLPos, spawnPlaneRPos),
                                             transform.position.y + propHeightDelay,
                                             Random.Range(spawnPlaneFPos, spawnPlaneBPos));

            SpawnIncomingIndicator(newPropPos);
            SpawnProp(newPropPos);
        }
    }

    float NewRndDelay()
    {
        return Random.Range(propMinSpawnDelay, propMaxSpawnDelay);
    }

    void SpawnProp(Vector3 newPropPos)
    {
        GameObject newProp;
        if (currentSpawned <= maxSpawned)
        {
            newProp = Instantiate(Resources.Load("props/ExampleProp")) as GameObject;
            newProp.transform.position = newPropPos;
            newProp.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            currentSpawned++;
        }
    }

    void SpawnIncomingIndicator(Vector3 newPropPos)
    {
        GameObject newIndicator = Instantiate(Resources.Load("props/Prop Incoming Indicator")) as GameObject;
        newIndicator.transform.SetParent(canvasRect.transform, false);

        RectTransform newIndicatorRect = newIndicator.GetComponent<RectTransform>();

        Vector2 viewportPosition = cam.WorldToViewportPoint(newPropPos);
        Vector2 onScreenPosition = new Vector2(
            (viewportPosition.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f),
            (viewportPosition.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f) + indicatorHeightPos);

        newIndicatorRect.anchoredPosition = onScreenPosition;
    }

    public void changeSpawned(int remove)
    {
        currentSpawned -= remove;
    }
}
