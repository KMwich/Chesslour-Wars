using UnityEngine;

public class mouse_icon : MonoBehaviour {

    public Texture2D mouse_img;
	// Use this for initialization
	void Start () {
        Cursor.SetCursor(mouse_img,Vector2.zero,CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
