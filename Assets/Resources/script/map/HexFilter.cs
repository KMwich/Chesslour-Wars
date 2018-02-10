using System.Collections.Generic;
using UnityEngine;

public class HexFilter : MonoBehaviour {
    List<HexCell> cells = new List<HexCell>();
    public HexCell cellPrefab;
    Sprite bluefilter,redfilter;

    private void Awake() {
        bluefilter = Resources.Load<Sprite>("sprite/mapfilter/CBzone");
        redfilter = Resources.Load<Sprite>("sprite/mapfilter/CRzone");
    }

    public void setFilter(HexCoordinates coordinate, int action) {
        Vector3 position = HexCoordinates.cubeToOffset(coordinate);
        position.x = position.x * HexMetrics.outerRadius * 1.5f;
        position.y = 0.1f;
        position.z = (position.z + position.x * 0.5f - (int) position.x / 2) * (HexMetrics.innerRadius * 2f);

        HexCell cell = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        if (action == 1) {
            cell.GetComponent<SpriteRenderer>().sprite = bluefilter;
            cell.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0.5f);
        } else {
            cell.GetComponent<SpriteRenderer>().sprite = redfilter;
            cell.GetComponent<SpriteRenderer>().color = new Vector4(1, 1, 1, 0.7f);
        }
            
        
    }
}
