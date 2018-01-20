using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    //start phase value
    public static int costToUse;
    public static int countTrap;
    public static int[] choiseIndex;//send index to map screen to create object
    public static int countIndex;

    //play phase value
    public static List<chess> ChessInGame;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Use this for initialization
    void Start () {
        ChessInGame = new List<chess>();
        countTrap = 3;
        costToUse = 18;
        if(countIndex != 0)
        {
            for(int i = 0;i < countIndex;i++)
            {
                choiseIndex[i] = 0;
            }
            countIndex = 0;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
