using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchScroll : MonoBehaviour {

    private List<GameObject> planels;
    private int viewIndex;

	// Use this for initialization
	void Start () {
        planels = new List<GameObject>();
        foreach(Transform t in transform)
        {
            planels.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }
        planels[0].SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		
    }

    public void select(int index)
    {
        if (index == viewIndex)
            return;
        if (index < 0 || index >= planels.Count)
            return;
        planels[viewIndex].SetActive(false);
        viewIndex = index;
        planels[viewIndex].SetActive(true);
    }
}
