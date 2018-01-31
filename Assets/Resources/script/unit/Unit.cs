using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Unit : Photon.MonoBehaviour
{
    HexCoordinates coordinate;
    //   public HexGrid map;
    int attackRange = 1;
    bool move = false;
    private Vector3 TargetPosition;
    private Quaternion TargetRotation;
    private string TargetSprite;
    private Vector3 TargetScale;
    private PhotonView PhotonView;

    private string _spritePath;
    public string SpritePath
    {
        get { return _spritePath;}
        set { _spritePath = value; }
    }
    
    private Unit Instance;

    // Use this for initialization
    void Awake()
    {
        Instance = this;
        PhotonView = GetComponent<PhotonView>();
        Vector3 S = gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size;
        gameObject.GetComponent<BoxCollider2D>().size = S;
        gameObject.GetComponent<BoxCollider2D>().offset = new Vector3((S.x / 2), 0);

        if (!PhotonView.isMine)
        {
            //this.gameObject.GetComponent<RectTransform>().localScale = new Vector3(0.0f, 8.0f, 1.0f);
            //this.setUnitSprite(TargetSprite);
        }

    }

    // Update is called once per frame
    void Update()
    {
        Scene scene = SceneManager.GetActiveScene();
        if ( scene.name == "map")
        {
            if (PhotonView.isMine)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    Physics.Raycast(inputRay, out hit);
                    Debug.Log(hit.point);
                    if (move)
                    {
                        int dist = HexCoordinates.cubeDeistance(coordinate, UnitManager.Instance.HexGrid.getTouchCoordinate());
                        if (dist != 0 && dist <= attackRange)
                            setUnitPosition(HexCoordinates.cubeToOffset(UnitManager.Instance.HexGrid.getTouchCoordinate()));
                    }
                    if (coordinate.Equals(UnitManager.Instance.HexGrid.getTouchCoordinate()))
                    {
                        UnitManager.Instance.HexGrid.setHexFilter(coordinate, 1);
                        move = true;
                    }
                }
                if (Input.GetMouseButtonUp(1))
                {
                    UnitManager.Instance.HexGrid.clearHexFilter();
                    move = false;
                }
            }
            else
            {
                transform.position = TargetPosition;
                transform.rotation = TargetRotation;
                this.setUnitSprite(TargetSprite);
                //this.gameObject.GetComponent<RectTransform>().localScale = TargetScale;

            }
        }

    }

    public void setUnitPosition(Vector3 position)
    {
        coordinate = HexCoordinates.FromOffsetCoordinates((int)position.x, (int)position.z);
        position.x = position.x * (HexMetrics.outerRadius * 1.5f);
        position.y = (position.z + position.x * 0.5f - (int)position.x / 2) * (HexMetrics.innerRadius * 2f); 
        position.z = transform.localPosition.z;
        transform.localPosition = position;
    }

    private void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
            stream.SendNext(SpritePath);
            stream.SendNext(this.gameObject.GetComponent<RectTransform>().localScale);
        }
        else
        {
            TargetPosition = (Vector3)stream.ReceiveNext();
            TargetRotation = (Quaternion)stream.ReceiveNext();
            TargetSprite = (string)stream.ReceiveNext();
            //TargetScale = (Vector3)stream.ReceiveNext();
        }
    }

    private void OnMouseDrag() {
        if (PhotonView.isMine)
        {
            Vector3 dist = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 curPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, dist.z);
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(curPos);
            transform.position = worldPos;
        }
    }

    private void OnMouseUp() {
        if (PhotonView.isMine)
        {
            HexCoordinates hexPosition = GetComponentInParent<UnitBar>()._hexGrid.getTouchCoordinate();
            Vector3 position = HexCoordinates.cubeToOffset(hexPosition);
            setUnitPosition(position);
        }
    }

    public void setUnitSprite(string path) {
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>(path);
    }

}
