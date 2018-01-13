using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class printText4 : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Text>().text = "" + GameStatus.trapType + "/5";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
