using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class switchScroll : MonoBehaviour {

    private List<GameObject> selectPlanels;
    private int viewIndex;

	// Use this for initialization
	void Awake () {
        selectPlanels = new List<GameObject>();
        foreach(Transform t in transform)
        {
            selectPlanels.Add(t.gameObject);
            t.gameObject.SetActive(false);
        }
        selectPlanels[0].SetActive(true);
        viewIndex = 0;
    }
	
	// Update is called once per frame
	void Update () {
		
    }

    public void select(int index)
    {
        if (index == viewIndex)
            return;
        if (index < 0 || index >= selectPlanels.Count)
            return;

        selectPlanels[viewIndex].SetActive(false);
        viewIndex = index;
        selectPlanels[viewIndex].SetActive(true);
    }
}
