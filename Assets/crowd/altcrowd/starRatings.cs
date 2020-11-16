using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class starRatings : MonoBehaviour
{
    [SerializeField] private int maxActiveStars = 5;
    [SerializeField] private int activeStars = 2;
    [SerializeField] private OutcomeManager outcomes;
    [SerializeField]private GameObject[] star = new GameObject[5];

    private GameObject warning;

    // Start is called before the first frame update
    void Start()
    {
        warning = transform.GetChild(0).gameObject;
        
        int i = activeStars;
        foreach (GameObject ster in star)
        {
            //Debug.Log("checking star, i is " + i);
            if (i > 0)
            {
                ster.SetActive(true);
                i--;
            }
            else ster.SetActive(false);
        }
    }

    public void changeActiveStars(int change)
    {
        activeStars += change;
        if (activeStars > maxActiveStars) activeStars = maxActiveStars;
        
        int i = activeStars;
        foreach (GameObject ster in star)
        {
            if (i > 0)
            {
                ster.SetActive(true);
                i--;
            }
            else ster.SetActive(false);
        }

        if (activeStars <= 0)
        {
            outcomes.callLose();
        }
    }

    public void showWarning()
    {
        warning.SetActive(true);
    }

    public void hideWarning()
    {
        warning.SetActive(false);
    }
}
