using UnityEngine;

public class HexCell : MonoBehaviour {

    public HexCoordinates coordinates;

    public int mapType;

    public void setType(int type) {
        mapType = type;
    }

    public void setMap(Sprite map) {
        GetComponent<SpriteRenderer>().sprite = map;
    }
}