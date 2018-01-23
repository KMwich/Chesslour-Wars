using System.IO;
using UnityEngine;

public class unit_database : MonoBehaviour {

    public static unit units;

    private void Awake()
    {
        string data = File.ReadAllText("Assets/Resources/database/unit_data.json");
        units = JsonUtility.FromJson<unit>(data);
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
