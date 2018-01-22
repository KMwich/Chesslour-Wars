using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
    HexCoordinates coordinate;
    public HexGrid map;
    int attackRange = 1;
    bool move = false;

	// Use this for initialization
	void Awake () {
        int x = Random.Range(0, 6);
        int z = Random.Range(0, 6);

        Vector3 position;
        position.x = x;
        position.y = 0;
        position.z = z;

        setUnitPosition(position);
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButtonUp(0)) {
            if (move) {
                int dist = HexCoordinates.cubeDeistance(coordinate, map.getTouchCoordinate());
                if (dist != 0 && dist <= attackRange )
                    setUnitPosition(HexCoordinates.cubeToOffset(map.getTouchCoordinate()));
            }
            if (coordinate.Equals(map.getTouchCoordinate())) {
                map.setHexFilter(coordinate, 1);
                move = true;
            }
        }
        if (Input.GetMouseButtonUp(1)) {
            map.clearHexFilter();
            move = false;
        }
        
    }

    void setUnitPosition(Vector3 position) {
        coordinate = HexCoordinates.FromOffsetCoordinates((int)position.x, (int)position.z);
        position.x = position.x * (HexMetrics.outerRadius * 1.5f);
        position.y = 2;
        position.z = (position.z + position.x * 0.5f - position.x / 2) * (HexMetrics.innerRadius * 2f);
        transform.localPosition = position;
    }
}
