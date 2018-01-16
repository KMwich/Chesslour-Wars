using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatus : MonoBehaviour {

    public static int damageType;
    public static int supportType;
    public static int objectType;
    public static int trapType;
    public static int coin;

	// Use this for initialization
	void Awake () {
        damageType = 2;
        supportType = 6;
        objectType = 7;
        trapType = 1;
        coin = 15;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
