using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class printCoin : MonoBehaviour {

	// Use this for initialization
	void Start () {
        gameObject.GetComponent<Text>().text = "" + GameStatus.coin + "/18";
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
