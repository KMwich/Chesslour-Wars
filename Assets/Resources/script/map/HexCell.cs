using UnityEngine;

public class HexCell : MonoBehaviour {

    public HexCoordinates coordinates;

    public int mapType;

    //set type to detect draw and game play
    public void setType(int type) {
        mapType = type;
    }

    //draw sprite in this object
    public void setMap(Sprite map) {
        GetComponent<SpriteRenderer>().sprite = map;
    }
}

/*Note!!!
    1.i don't to create database map because all value in this can to use in game
    2.index type map is forest=0 mountain=1 water=2 yard=3 
 */