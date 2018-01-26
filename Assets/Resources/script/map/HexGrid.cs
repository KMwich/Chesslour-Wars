using UnityEngine;
using System.IO;

public class HexGrid : MonoBehaviour {

    public HexCell cellPrefab;
    HexCell[] cells;

    public int width;
    public int height;
    
    int hexFilterSize = 0;
    HexCoordinates hexFilterCoordinate;
    public HexFilter hexFilterPrefab;
    HexFilter hexFilter;

    HexMesh hexMesh;

    public static JsonMap mapDetail;
    Sprite[] maps;

    void Awake(){

        hexFilter = null;
        hexMesh = GetComponentInChildren<HexMesh>();
        maps = Resources.LoadAll<Sprite>("sprite/map");

        string json = File.ReadAllText("Assets/Resources/database/stage1.json");
        mapDetail = JsonUtility.FromJson<JsonMap>(json);
        width = mapDetail.width;
        height = mapDetail.height;

        cells = new HexCell[height * width];

        for (int z = 0, i = 0; z < mapDetail.height; z++)
        {
            for (int x = 0; x < mapDetail.width; x++)
            {
                CreateCell(x, z, i++);
            }
        }

    }

    void Start(){
        hexMesh.Triangulate(cells);
    }

    void CreateCell(int x, int z, int i){

        if (mapDetail.detail[i] == 0) return;

        Vector3 position;
        position.x = x * (HexMetrics.outerRadius * 1.5f);
        position.y = - z - ((float) i % 4 / 4f );
        position.z = (z + x * 0.5f - x / 2) * (HexMetrics.innerRadius * 2f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab); //cell is referent to cells[]
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.setType(mapDetail.detail[i] % 4);
        cell.setMap(maps[cell.mapType]);
    }

    //use send position cross function
    public HexCoordinates getTouchCoordinate() {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        Physics.Raycast(inputRay, out hit);
        return HexCoordinates.FromPosition(hit.point);
    }

    public void setHexFilter(HexCoordinates coordinate, int size) {
        if (hexFilter != null) clearHexFilter();
        hexFilter = Instantiate<HexFilter>(hexFilterPrefab);
        hexFilter.transform.SetParent(transform, false);
        hexFilter.setHexFilterSize(size);
        

        for (int i = 0, x = -size; x <= size; x++) {
            int iX = x + coordinate.X;

            if ((iX < 0 || iX >= width)) continue;
            for (int y = Mathf.Max(-size, -x - size); y <= Mathf.Min(size, -x + size); y++) {
                int iY = y + coordinate.Y;
                
                if (((iY + iX / 2) < 0 || (iY + iX / 2) >= height)) continue;

                int index = iX + (iY + iX / 2) * width;

                if (cells[index] != null)
                    hexFilter.setCoordinate(HexCoordinates.FromOffsetCoordinates(iX, iY + iX/ 2),i);
                
                i++;
            }
        }
    }

    public void clearHexFilter() {
        Destroy(hexFilter.gameObject);
        hexFilterSize = 0;
    }

}
