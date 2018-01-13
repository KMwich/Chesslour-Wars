using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class printText : MonoBehaviour {


	// Use this for initialization
	void Start () {
        GetComponent<Text>().text = "" + GameStatus.damageType;
    }
	
	// Update is called once per frame
	void Update () {
    }
}
