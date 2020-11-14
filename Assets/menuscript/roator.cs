using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roator : MonoBehaviour
{
    [SerializeField] private float speed = 1.5f;

    // Update is called once per frame
    void Update()
    {
        transform.rotation *= Quaternion.Euler(0f, speed* Time.deltaTime, 0f);
    }
}
