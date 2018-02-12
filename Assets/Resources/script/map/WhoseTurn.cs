using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhoseTurn : MonoBehaviour {

    void Update()
    {
        if (StatusControl.Instance.active == true)
        {
            this.transform.Find("Your Turn").localScale = new Vector3(1, 1, 1);
            this.transform.Find("Opponent's Turn").localScale = new Vector3(0, 1, 1);
        }
        else
        {
            this.transform.Find("Opponent's Turn").localScale = new Vector3(1, 1, 1);
            this.transform.Find("Your Turn").localScale = new Vector3(0, 1, 1);
        }
    }
}
