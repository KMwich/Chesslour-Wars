using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

    public int width = 6;
    public int height = 6;

    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    public HexCell cellPrefab;

    HexCell[] cells;

    public Text cellLabelPrefab;

    HexMesh hexMesh;

    public Sprite[] maps;

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

        Vector3 position;
        position.x = x * (HexMetrics.outerRadius * 1.5f);
        position.y = - z - ((float) i % 4 / 4f );
        position.z = (z + x * 0.5f - x / 2) * (HexMetrics.innerRadius * 2f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;
        cell.GetComponent<SpriteRenderer>().sprite = maps[i % 4];
        DontDestroyOnLoad(this.transform);
    }

    void Update() {
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
        int index = coordinates.X + (coordinates.Y + coordinates.X / 2) * width;
        HexCell cell = cells[index];
        cell.color = touchedColor;
        hexMesh.Triangulate(cells);
    }

}
