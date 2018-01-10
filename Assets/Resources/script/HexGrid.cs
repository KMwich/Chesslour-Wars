using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

    public int width = 6;
    public int height = 6;

    public Color defaultColor = Color.clear;
    public Color touchedColor = Color.magenta;

    public HexCell cellPrefab;

    int hexFilterSize = 0;
    HexCoordinates hexFilterCoordinates;
    

    HexCell[] cells;

    HexMesh hexMesh;

    Sprite[] maps;

    void Awake(){
        
        hexMesh = GetComponentInChildren<HexMesh>();
        maps = Resources.LoadAll<Sprite>("sprite/map");

        cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < height; z++)
        {
            for (int x = 0; x < width; x++)
            {
                CreateCell(x, z, i++);
            }
        }

    }

    void Start(){
        hexMesh.Triangulate(cells);
    }

    void CreateCell(int x, int z, int i){

        if (x == z) return;

        Vector3 position;
        position.x = x * (HexMetrics.outerRadius * 1.5f);
        position.y = - z - ((float) i % 4 / 4f );
        position.z = (z + x * 0.5f - x / 2) * (HexMetrics.innerRadius * 2f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;
        cell.setType(i % 4);
        cell.setMap(maps[cell.mapType]);
    }

    void Update () {
        if (Input.GetMouseButtonUp(0)) {
            HandleInput();
        }
    }

    void HandleInput () {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            TouchCell(hit.point);
        }
    }

    void TouchCell (Vector3 position) {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        setHexFilter(coordinates, 2);
        hexMesh.Triangulate(cells);
    }

    void setHexFilter(HexCoordinates coordinate, int size) {
        if (!coordinate.Equals(hexFilterCoordinates)) clearHexFilter();
        hexFilterCoordinates = coordinate;
        hexFilterSize = size;

        for (int x = -size; x <= size; x++) {
            int iX = x + coordinate.X;

            if ((iX < 0 || iX >= width)) continue;
            for (int y = Mathf.Max(-size ,-x - size) ; y <= Mathf.Min(size, -x + size); y++) {
                int iY = y + coordinate.Y;

                if (((iY + iX / 2) < 0 || (iY + iX / 2) >= height)) continue;

                int index = iX + (iY + iX / 2) * width;

                if (cells[index] != null) cells[index].setMap(maps[4]);
            }
        }
    }

    void clearHexFilter() {
        for (int x = -hexFilterSize; x <= hexFilterSize; x++) {
            int iX = x + hexFilterCoordinates.X;

            if ((iX < 0 || iX >= width)) continue;
            for (int y = Mathf.Max(-hexFilterSize, -x - hexFilterSize); y <= Mathf.Min(hexFilterSize, -x + hexFilterSize); y++) {
                int iY = y + hexFilterCoordinates.Y;

                if (((iY + iX / 2) < 0 || (iY + iX / 2) >= height)) continue;

                int index = iX + (iY + iX / 2) * width;
                if (cells[index] != null) cells[index].setMap(maps[cells[index].mapType]);
            }
        }
    }

}
