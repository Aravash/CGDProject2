using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMove : MonoBehaviour
{
    [SerializeField] float speedMultiplier = 5.0f;
    
    public void Move(float change)
    {
        transform.position -= new Vector3(change * speedMultiplier, 0, 0);
        
        if (transform.position.x < -4.0f) 
                            Destroy(gameObject);
    }
}
