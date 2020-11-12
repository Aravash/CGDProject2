using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleCarPosManager : MonoBehaviour
{
    [SerializeField] private BackdropSlider backDrop;
    private Vector3 oldpos;

    // Start is called before the first frame update
    void Start()
    {
        oldpos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (oldpos != transform.position)
        {
            Debug.Log("pos changed");
            backDrop.scrollBackDrop(
                Vector2.Distance(oldpos, transform.position));
            
            oldpos = Vector3.Lerp(oldpos, transform.position, Time.deltaTime);
        }
    }
}
