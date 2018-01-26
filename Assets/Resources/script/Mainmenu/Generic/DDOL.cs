using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOL : MonoBehaviour {

    public static int mapIndex = 0;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

}
