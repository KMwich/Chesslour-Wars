using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour {

    Vector3 dist;
    float posx;
    float posy;

    void OnMouseDown()
    {
        Debug.Log("Hello");
        dist = Camera.main.WorldToScreenPoint(transform.position);
        posx = Input.mousePosition.x - dist.x;
        posy = Input.mousePosition.y - dist.y;
    }

    private void OnMouseDrag()
    {
        Vector3 curPos = new Vector3(Input.mousePosition.x - posx, Input.mousePosition.y - posy, dist.z);
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
        transform.position = worldPos;
    }
}
