using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class printText2 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Text>().text = "" + GameStatus.supportType;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
