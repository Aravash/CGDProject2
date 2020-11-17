using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartHandle : MonoBehaviour
{
    const float HANDLE_RADIUS = 0.65f;
    const float MAX_PULL = 1;
    const float PULL_MULTIPLIER = 50;
    Transform hand_L;
    Transform hand_R;
    float old_rot;
    BackdropSlider backdrop;
    
    void Start()
    {
        old_rot = transform.rotation.x;
        backdrop = FindObjectOfType<BackdropSlider>();
    }

    [SerializeField][Range(0.01f, 0.2f)]
    public float MX_DIST_TO_HAND = .1f;
    [SerializeField] private Transform handle_L;
    [SerializeField] private Transform handle_R;
    void Update()
    {
	if(hand_L){
        // Leash Target back to hand
	var delta = hand_L.position-handle_L.position;
	if(delta.magnitude > MX_DIST_TO_HAND)
	{hand_L.position = handle_L.position + delta.normalized * MX_DIST_TO_HAND;}
	}
	if(hand_R){
        // Leash Target back to hand
	var delta = hand_R.position-handle_R.position;
	if(delta.magnitude > MX_DIST_TO_HAND)
	{hand_R.position = handle_R.position + delta.normalized * MX_DIST_TO_HAND;}
	}
    }

    private void FixedUpdate()
    {
        if (hand_L != null)
        {
            pull(false);
        }
        if (hand_R != null)
        {
            pull(true);
        }
        // Control the background
        float delta = Mathf.Abs(transform.rotation.x - old_rot);
        backdrop.accelerate(delta);
        old_rot = transform.rotation.x;
    }

    Vector3 debugA;
    Vector3 debugB;
    
    private void pull(bool right_hand)
    {
	
	var hand = right_hand ? hand_R : hand_L;

        Vector3 projection = new Vector3(
            hand.GetComponent<RectTransform>().position.x,
            hand.GetComponent<RectTransform>().position.y,
            gameObject.transform.position.z);

        Vector3 handle_root = calculateHandleRoot(right_hand);
        Vector3 diff = projection - handle_root;

        if (diff.magnitude > MAX_PULL)
            diff *= MAX_PULL / diff.magnitude;
        diff *= PULL_MULTIPLIER;

        debugA = handle_root;
        debugB = handle_root + diff;

        Debug.DrawRay(handle_root, diff, Color.green, 1);
        diff *= Time.deltaTime;
        Debug.DrawRay(handle_root, diff, Color.yellow, 1);

        GetComponent<Rigidbody>().AddForceAtPosition(diff, handle_root, ForceMode.Impulse);
    }

    private Vector3 calculateHandleRoot(bool right_hand)
    {
        float mod = 1;
        if (!right_hand)
            mod *= -1;
        Vector3 offset = new Vector3(mod * HANDLE_RADIUS, 0, 0);
        offset = Quaternion.AngleAxis(-transform.localRotation.eulerAngles.x, Vector3.forward) * offset;
        return transform.position + offset;
    }

    // Interface
    public void grab(Transform grabber, int hand_id)
    {
        switch (hand_id)
        {
            case 0:
                hand_L = grabber;
                break;
            case 1:
                hand_R = grabber;
                break;
        }
    }
    public void release(int hand_id)
    {
        switch (hand_id)
        {
            case 0:
                hand_L = null;
                break;
            case 1:
                hand_R = null;
                break;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.yellow;
    //    Gizmos.DrawSphere(debugA, 0.2f);
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawSphere(debugB, 0.2f);
    //}
}
