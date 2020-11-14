using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HazardMove : MonoBehaviour
{
    public void Move(float change)
    {
        Debug.Log("moving");
        transform.position -= new Vector3(change, 0, 0);
    }
}
